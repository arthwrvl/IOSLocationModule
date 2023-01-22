using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using PlayFab;
using PlayFab.ClientModels;
using LoginResult = PlayFab.ClientModels.LoginResult;
using UnityEngine.SceneManagement;

// Realiza o processo de autenticação do usuario usando a plataforma facebook.
public class PlayFabFacebookAuth : MonoBehaviour
{
    public GameObject loadingPanel;
    // Inicia o modulo do facebook, caso ainda não inicado, para comecar a autenticação.
    public void initFacebookLogin(){

        // Ativar painel de loading
        loadingPanel.gameObject.SetActive(true);

        if(!FB.IsInitialized){
            Debug.Log("Initializing Facebook...");
            FB.Init(OnFacebookInitialized);
        }else{
            OnFacebookInitialized();
        }
        
    }


    // Inicia o login usando o facebook
    private void OnFacebookInitialized(){
        
        Debug.Log("Logging into Facebook...");
        
        // Verifica se o usuario ja esta logado.
        if(FB.IsLoggedIn){

            LoginIntoPlayFab(AccessToken.CurrentAccessToken.TokenString);

        }else{

            // Loga usuario com permissoes de leitura ao perfil.
            var perms = new List<string>(){"public_profile", "email"};
            FB.LogInWithReadPermissions(perms, OnFacebookLoggedIn);
        }
    

    }

    // Lida com o resultado da tentativa de login no facebook - Erros ou sucesso.
    private void OnFacebookLoggedIn(ILoginResult result){
        
        if(result.Cancelled || !string.IsNullOrEmpty(result.Error)){

            Debug.Log("Facebook Auth Failed: " + result.Error + "\n" + result.RawResult);
            loadingPanel.gameObject.SetActive(false);
        }

        // Se não há erros, significa que autenticamos no facebook com sucesso
        else if (result == null || string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("Facebook auth complete");
            Debug.Log("Acess Token: " + AccessToken.CurrentAccessToken.TokenString);
            LoginIntoPlayFab(AccessToken.CurrentAccessToken.TokenString);

        }else{
            Debug.Log("Facebook Auth Failed: " + result.Error + "\n" + result.RawResult);
        }
    }

    // Login no playFab usando facebook - AcessToken
    private void LoginIntoPlayFab(string acessToken){
        
        var request = new LoginWithFacebookRequest {
            
            CreateAccount = true,
            AccessToken = acessToken

        };

        PlayFabClientAPI.LoginWithFacebook(request, OnPlayfabFacebookAuthComplete, OnPlayfabFacebookAuthFailed);

    }

    // Inicia a sessão do usuario
    private void OnPlayfabFacebookAuthComplete(LoginResult result){
        Debug.Log("PlayFab Facebook Auth Complete. Session ticket: " + result.SessionTicket);
        GetCurrentProfileFromFacebook();
        SceneManager.LoadScene("SessionScene");
    }

    private void OnPlayfabFacebookAuthFailed(PlayFabError error){
        Debug.Log("PlayFab Facebook Auth Failed: " + error.GenerateErrorReport());
    }


private void GetCurrentProfileFromFacebook()
{   
    Debug.Log("Getting current user profile from facebook ...");
    var profile = FB.Mobile.CurrentProfile();
    if(profile != null) {

        string userId = profile.UserID;
        string userName = profile.Name;
        string userEmail = profile.Email;

        Debug.Log("userId: " + userId);
        Debug.Log("userName: "+ userName);
        Debug.Log("userEmail: " + userEmail);

        saveUserProfile(userId, userName, userEmail);   

     }else{
        Debug.Log("Failed to get user Profile ...");

     }
}

// private void GetCurrentProfileFromPlayFab(){

//     Debug.Log("Getting current user profile from facebook ...");
    

// }

// Salva as informações do usuario no playerPrefs.
private void saveUserProfile(string userId, string name, string email){

    PlayerPrefs.SetString("userName", name);
    PlayerPrefs.SetString("userEmail", email);
    PlayerPrefs.SetString("userID", userId);


}
}
