//using Firebase.Extensions;
//using Google;
//using System.Collections;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using UnityEngine;

//public class googelAuth : MonoBehaviour
//{
//    public string GoogleAPI = "YOUR_API_KEY";

//    private GoogleSignInConfiguration configuration;

//    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
//    Firebase.Auth.FirebaseAuth auth;
//    Firebase.Auth.FirebaseUser user;


//    private void Awake()
//    {
//        configuration = new GoogleSignInConfiguration
//        {
//            WebClientId = GoogleAPI,
//            RequestIdToken = true,
//        };
//    }
//    private void Start()
//    {
//        InitFirebase();
//    }

//    void InitFirebase()
//    {
//        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

//    }

//    public void GoogleSignInClick()
//    {
//        GoogleSignIn.Configuration = configuration;
//        GoogleSignIn.Configuration.UseGameSignIn = false;
//        GoogleSignIn.Configuration.RequestIdToken = true;
//        GoogleSignIn.Configuration.RequestEmail = true;

//        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthenticatedFinished);
//    }
//    void OnGoogleAuthenticatedFinished(Task<GoogleSignInUser> task)
//    {
//        if (task.IsFaulted)
//        {
//            Debug.LogError("Faulted");
//        }
//        else if (task.IsCanceled)
//        {
//            Debug.LogError("Cancelled");
//        }
//        else
//        {
//            Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);

//            auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
//            {
//                if (task.IsCanceled)
//                {
//                    return;
//                }

//                if (task.IsFaulted)
//                {
//                    Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
//                    return;
//                }

//                user = auth.CurrentUser;

//                //Username.text = user.DisplayName;
//                //UserEmail.text = user.Email;
//                Debug.Log(user.DisplayName);
//                Debug.Log(user.Email);
//                SliderScript.Instance.LetsPlayClick();
//                SliderScript.Instance.VerifyClick();
//                // LoginScreen.SetActive(false);
//                //ProfileScreen.SetActive(true);

//                // StartCoroutine(LoadImage(CheckImageUrl(user.PhotoUrl.ToString())));
//            });
//        }
//    }
//}
