using UnityEngine;
using BestHTTP.SocketIO3;
using System;
using static StaticData;

public class SocketConnection : MonoBehaviour
{
    public static SocketConnection instance;

    [Header("===== Socket Info =====")]
    internal SocketManager socketManager;
    public string socketURL;

    public SocketState socketState;

    public SocketEventReceiver socketEventReceiver;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        socketURL = baseURL;
        Uri uri = new Uri(socketURL);
        socketManager = new SocketManager(uri);
        InvokeRepeating(nameof(CheckConnection), 0f, 2f);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("Game Paused. Handle socket disconnection if needed.");
            OnDisConnectedToServer();
        }
        else
        {
            Debug.Log("Game Resumed. Attempting to reconnect socket.");
            CheckConnection();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Debug.Log("Game in Focus. Reconnecting socket if necessary.");
            CheckConnection();
        }
    }




    public void CheckConnection()
    {
        Debug.Log(" CheckConnection Start ");
        if (IsInternetConnection())
        {
            Debug.Log("SocketConnection || CheckConnection");
            StartSocketConnection();
            // Write code for No internet
        }
        else
        {
            IsInternetConnection();
        }
    }

    public void StartSocketConnection()
    {
        Debug.Log("network recover || " + socketState);
        if (socketState.Equals(SocketState.Disconnect) || socketState.Equals(SocketState.Error) || socketState.Equals(SocketState.None))
        {
            CancelInvoke(nameof(CheckConnection));
            SocketConnectionStart(socketURL);
        }
    }

    public void SocketConnectionStart(string socketUrl)
    {
        try
        {
            socketUrl = socketUrl + "/socket.io/";
            Debug.Log("SocketConnectionStart function socketURL :-> " + socketUrl);
            if (socketManager != null)
            {
                if (socketManager.Socket.IsOpen)
                {
                    socketManager.Socket.Disconnect();
                }
            }
            socketManager = null;
            Debug.Log("SocketConnectionStart 1");

            socketManager = new SocketManager(new Uri(socketUrl), SocketsOption());

            Debug.Log("SocketConnectionStart 2");

            if (socketManager == null) return;

            socketManager.Open();
            socketManager.Socket.On(SocketIOEventTypes.Connect, OnConnected);
            socketManager.Socket.On(SocketIOEventTypes.Disconnect, OnDisConnectedToServer);
            socketManager.Socket.On<Error>(SocketIOEventTypes.Error, OnScoketError);

            RegisterCustomEvents();

            Debug.Log("SocketConnectionStart 8");
        }
        catch (Exception ex)
        {
            Debug.LogError("Socket Connection Start Exception - >" + ex.ToString());
        }
    }

    private void OnScoketError(Error error)
    {
        socketState = SocketState.Error;
        Debug.LogError("Error Message : " + error.message);
        socketManager.Socket.Disconnect();
    }

    public void RegisterCustomEvents()
    {
        Debug.Log(" RegisterCustomEvents ");
        var events = Enum.GetValues(typeof(PuzzleEvent)) as PuzzleEvent[];
        for (int i = 0; i < events.Length; i++)
        {
            string en = events[i].ToString();

            socketManager.Socket.On<Socket, string>(en, (socket, res) =>
            {
                //if (!res.Contains("HEART_BEAT"))
                try
                {
                    Debug.Log("<color=green> Data Recived " + en + "</color>");
                    //JSONNode data = JSONNode.Parse(res);
                    string j = JsonUtility.ToJson(res);
                    //Debug.Log("<color=green> Data Recived " + res + " DONE " + "</color>");
                    socketEventReceiver.HandleEventResponse(en, res);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    throw;
                }
            });
        }
    }

    public void SendDataToServer(string eventName, string jsonData)
    {
        if (!eventName.Contains("HEART_BEAT"))
            Debug.Log("<color=cyan>On Request : </color><color><b>" + eventName + "</b></color><color=red> || On request : </color> " + jsonData);

        //Debug.Log("<color=red>Send Data To Server :- </color>");

        try
        {
            if (socketManager != null)
                if (socketManager.Socket != null)
                    socketManager.Socket.Emit(eventName, jsonData);
            //if (eventName != "HEART_BEAT")
            //    Debug.Log("<color=cyan>On Request : </color>" + jsonData);
        }
        catch (Exception ex)
        {
            Debug.LogError(" Exception SendData Methods -> " + ex.ToString());
        }
    }

    bool IsInternetConnection()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }

    private void OnDisConnectedToServer()
    {
        Debug.LogError(" SocketConnectionStart || Socket Disconnected || Disconnected!" + socketManager.Socket.IsOpen);
        socketManager.Socket.Disconnect();
        socketState = SocketState.Disconnect;
    }

    private void OnConnected()
    {
        Debug.Log("<color=green>Socket Connection Successfully</color>");
        socketState = SocketState.Connect;
    }

    SocketOptions SocketsOption()
    {
        Debug.Log(PlayerPrefs.GetString("token"));
        SocketOptions socketOptions = new SocketOptions();
        socketOptions.AutoConnect = true;
        socketOptions.ConnectWith = BestHTTP.SocketIO3.Transports.TransportTypes.WebSocket;
        socketOptions.RandomizationFactor = 0.5f;
        socketOptions.Reconnection = true;
        socketOptions.ReconnectionAttempts = int.MaxValue;
        socketOptions.ReconnectionDelay = TimeSpan.FromMilliseconds(1000);
        socketOptions.ReconnectionDelayMax = TimeSpan.FromMilliseconds(5000);
        socketOptions.Timeout = TimeSpan.FromMilliseconds(20000);
        socketOptions.QueryParamsOnlyForHandshake = true;
        socketOptions.Auth = (manager, socket) => new { token = PlayerPrefs.GetString("token") };
        //socketOptions.Auth = (manager, socket) => new Dictionary<string, string>
        //{
        //    { "token", PlayerPrefs.GetString("token")}
        //};
        return socketOptions;
    }
}


public enum SocketState
{
    None,
    Connect,
    Running,
    Error,
    Disconnect,
    Connecting
}