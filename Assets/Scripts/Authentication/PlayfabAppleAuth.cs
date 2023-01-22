using System.Text;
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Extensions;
using AppleAuth.Interfaces;
using AppleAuth.Native;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayfabAppleAuth : MonoBehaviour
{
    private IAppleAuthManager appleAuthManager;


    // Chaves/Keys para os dados salvos do usuario no PlayerPrefs.
    private string APPLE_USER_ID_KEY = "userID";
    private string APPLE_USER_EMAIL_KEY = "userEmail";
    private string APPLE_USER_FULLNAME_KEY = "userName";
    private string APPLE_USER_AUTHORIZATION_CODE_KEY = "APPLE_USER_AUTHORIZATION_CODE_KEY";
    private string APPLE_USER_IDENTITY_TOKEN_KEY = "APPLE_USER_IDENTITY_TOKEN_KEY";

    void Start()
    {
    
    // If the current platform is supported
    if (AppleAuthManager.IsCurrentPlatformSupported)
    {
        // Creates a default JSON deserializer, to transform JSON Native responses to C# instances
        var deserializer = new PayloadDeserializer();
        // Creates an Apple Authentication manager with the deserializer
        this.appleAuthManager = new AppleAuthManager(deserializer);    
    }

    }

    void Update()
    {
        // Updates the AppleAuthManager instance to execute
        // pending callbacks inside Unity's execution loop
        if (this.appleAuthManager != null)
        {
            this.appleAuthManager.Update();
        }
    }

    public void signInApple(){

        Debug.Log("Loggin into apple account...");

        if(!AppleAuthManager.IsCurrentPlatformSupported){
            Debug.Log("Loggin into apple is not supported...");
        }

        var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName);

        this.appleAuthManager.LoginWithAppleId(
        loginArgs,
        credential =>
        {
            // Obtained credential, cast it to IAppleIDCredential
            var appleIdCredential = credential as IAppleIDCredential;
            if (appleIdCredential != null)
            {   
                // Obtem e sava em PlayerPrefs dados do usuario.

                // Apple User ID
                var userId = appleIdCredential.User;
                PlayerPrefs.SetString(APPLE_USER_ID_KEY, userId);

                // Email (Received ONLY in the first login)
                var email = appleIdCredential.Email;
                PlayerPrefs.SetString(APPLE_USER_EMAIL_KEY, email);

                // Full name (Received ONLY in the first login)
                // SAVE JUST DE GIVEN NAME OF USER
                var fullName = appleIdCredential.FullName.GivenName;
                PlayerPrefs.SetString(APPLE_USER_FULLNAME_KEY, fullName);

                // Identity token
                var identityToken = Encoding.UTF8.GetString(
                    appleIdCredential.IdentityToken,
                    0,
                    appleIdCredential.IdentityToken.Length);
                PlayerPrefs.SetString(APPLE_USER_IDENTITY_TOKEN_KEY, identityToken);

                // Authorization code
                var authorizationCode = Encoding.UTF8.GetString(
                    appleIdCredential.AuthorizationCode,
                    0,
                    appleIdCredential.AuthorizationCode.Length);
                PlayerPrefs.SetString(APPLE_USER_AUTHORIZATION_CODE_KEY, authorizationCode);

                Debug.Log("Logged apple success");

                AuthPlayfabWithApple(identityToken);
            }
        },
        error =>
        {
            // Something went wrong
            var authorizationErrorCode = error.GetAuthorizationErrorCode();
            Debug.Log("Something went wrong when log with apple");

        });

    }


    private void AuthPlayfabWithApple(string UserIdentityToken){

        Debug.Log("Log into playfab using apple auth...");
        
        var request = new LoginWithAppleRequest {
            
            CreateAccount = true,
            IdentityToken = UserIdentityToken

        };

        PlayFabClientAPI.LoginWithApple(request, OnPlayfabAppleAuthComplete, OnPlayfabAppleAuthFailed);

    }

    // Inicia a sessão do usuario
    private void OnPlayfabAppleAuthComplete(LoginResult result){
        Debug.Log("PlayFab Apple Auth Complete. Session ticket: " + result.SessionTicket);
        SceneManager.LoadScene("SessionScene");
    }

    private void OnPlayfabAppleAuthFailed(PlayFabError error){
        Debug.Log("PlayFab Apple Auth Failed: " + error.GenerateErrorReport());
    }

}
