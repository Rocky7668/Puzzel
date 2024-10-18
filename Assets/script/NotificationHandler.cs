//using Firebase;
//using UnityEngine;
//using Firebase.Messaging;

//public class NotificationHandler : MonoBehaviour
//{
//    void Start()
//    {
//        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
//            FirebaseMessaging.TokenReceived += OnTokenReceived;
//            FirebaseMessaging.MessageReceived += OnMessageReceived;
//        });
//    }

//    public void OnTokenReceived(object sender, TokenReceivedEventArgs token)
//    {
//        Debug.Log("Received Registration Token: " + token.Token);
//    }

//    public void OnMessageReceived(object sender, MessageReceivedEventArgs e)
//    {
//        Debug.Log("Received a new message from: " + e.Message);
//    }
//}
