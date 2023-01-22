using UnityEngine;
using Facebook.Unity;
using PlayFab;
using UnityEngine.SceneManagement;
//using GooglePlayGames;
public class LogOut : MonoBehaviour
{
   
   public void OnSignOutClicked(){


      Debug.Log("SignOut...");

      if(FB.IsLoggedIn){

         Debug.Log("Facebook logging Out...");
         FB.LogOut();
         
            
      }
      else if(Social.localUser.authenticated){
         
         Debug.Log("Google logging Out...");
         //PlayGamesPlatform.Instance.SignOut();
        
      }else{


      }

      PlayFabClientAPI.ForgetAllCredentials();
      Debug.Log("Sucessful, Logged out!");
      SceneManager.LoadScene("LoginScreen");
    
   }
}
