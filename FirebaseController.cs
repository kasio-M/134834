using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using System;
using System.Threading.Tasks;
using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class FirebaseController : MonoBehaviour


{
    public GameObject LoginPanel,SignupPanel, ResetPasswordPanel, NotificationPanel;
    public InputField LoginEmail,LoginPassword,SignupEmail,SignupPassword,SignupCPassword,SignUpUserName,SignupAge,forgetPassEmail;

    public Text notif_Title_Text, notif_Message_Text;
    public Toggle rememberMe;
     public string menuScene;

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
    bool IsSignIn = false;
void Start()
{
    FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
    {
        var dependencyStatus = task.Result;
        if (dependencyStatus == DependencyStatus.Available)
        {
            
            InitializeFirebase();
            

          
            openSignupPanel();
        }
        else
        {
            UnityEngine.Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
           
        }
    });
}

 


    public void  openLoginPanel()
    {
        LoginPanel.SetActive(true);
        SignupPanel.SetActive(false);
        ResetPasswordPanel.SetActive(false);

    }
    public void  openSignupPanel()

    {
        LoginPanel.SetActive(false);
        SignupPanel.SetActive(true);
        ResetPasswordPanel.SetActive(false);

        
    }
    public void  openResetPasswordPanel()

    {
        LoginPanel.SetActive(false);
        SignupPanel.SetActive(false);
        ResetPasswordPanel.SetActive(true);

        
    }

    public void LoginUser()
    {
        if(string.IsNullOrEmpty(LoginEmail.text)&&string.IsNullOrEmpty(LoginPassword.text)){
            ShowNotificationMessage("Error","Please input all fields"); 
            
            return;
             
        }
        SignInUser(LoginEmail.text, LoginPassword.text);
        SceneManager.LoadScene(menuScene);
    }
    public void SignUpUser()
    {
        if(string.IsNullOrEmpty(SignupEmail.text)&&string.IsNullOrEmpty(SignupPassword.text)&&string.IsNullOrEmpty(SignupCPassword.text)&&string.IsNullOrEmpty(SignUpUserName.text)&&string.IsNullOrEmpty(SignupAge.text)){
            ShowNotificationMessage("Error","Please input all fields"); 
            return;

            
        }
        CreateUser(SignupEmail.text,SignupPassword.text,SignUpUserName.text);

    }
    public void forgetPass()
    {
         if(string.IsNullOrEmpty(forgetPassEmail.text))
         {
            ShowNotificationMessage("Error","Reset Email Empty");

         return;}
    }
    public void ShowNotificationMessage(string title, string message)
    {
        notif_Title_Text.text=" " +title;
        notif_Message_Text.text=" " +message;

        NotificationPanel.SetActive(true);
    }

    public void CloseNotif_panel(){


        notif_Title_Text.text=" " ;
        notif_Message_Text.text=" " ;

        NotificationPanel.SetActive(false);

    }
    public void CreateUser(string email, string password, string username)
    {

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
  if (task.IsCanceled) {
    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
    return;
  }
  if (task.IsFaulted) {
    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
    return;
  }

  Firebase.Auth.AuthResult result = task.Result;
  Debug.LogFormat("Firebase user created successfully: {0} ({1})",
      result.User.DisplayName, result.User.UserId);

      UpdateUserProfile(username);
});
    }
    public void SignInUser (string email, string password){

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
  if (task.IsCanceled) {
    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
    return;
  }
  if (task.IsFaulted) {
    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
    return;
  }

  Firebase.Auth.AuthResult result = task.Result;
  Debug.LogFormat("User signed in successfully: {0} ({1})",
      result.User.DisplayName, result.User.UserId);
});
    }

    void InitializeFirebase() {
  auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
  auth.StateChanged += AuthStateChanged;
  AuthStateChanged(this, null);
}

void AuthStateChanged(object sender, System.EventArgs eventArgs) {
  if (auth.CurrentUser != user) {
    bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null
        && auth.CurrentUser.IsValid();
    if (!signedIn && user != null) {
      Debug.Log("Signed out " + user.UserId);
    }
    user = auth.CurrentUser;
    if (signedIn) {
      Debug.Log("Signed in " + user.UserId);

      
      

  

      IsSignIn = true;

      
    }
  }
}

void OnDestroy() {
  auth.StateChanged -= AuthStateChanged;
  auth = null;
}
void UpdateUserProfile(string UserName)
{
    Firebase.Auth.FirebaseUser user = auth.CurrentUser;
if (user != null) {
  Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile {
    DisplayName = UserName,
    PhotoUrl = new System.Uri("https://fakeimg.pl/150x150"),
  };
  user.UpdateUserProfileAsync(profile).ContinueWith(task => {
    if (task.IsCanceled) {
      Debug.LogError("UpdateUserProfileAsync was canceled.");
      return;
    }
    if (task.IsFaulted) {
      Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
      return;
    }

    Debug.Log("User profile updated successfully.");
    ShowNotificationMessage("Alert", "Account Created Successfully");
  });
}
}

    }


