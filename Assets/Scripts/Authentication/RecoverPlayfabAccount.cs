using System.Collections;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;

// Lida com o processo de recuperar senha da conta do playFab.
public class RecoverPlayfabAccount : MonoBehaviour
{   

    [Header("Itens do Menu")]
    public GameObject LoginMenu;
    public TextMeshProUGUI WarningText;

    [Header("Itens do Menu de recuperacao de senha")]
    public Button accountRecoverySendEmailButton;
    public TMP_InputField emailFromAccountToBeRecoveredInputField;

    void Start()
    {
        // Configura o botao SendEmail para iniciar a rotina de recuperacao de senha ao ser clicado.
        accountRecoverySendEmailButton.onClick.AddListener(OnAccountRecoverySendEmailButtonClicked);

    }

    // Envia um email de recuperacao de senha do playfab, para um email fornecido pelo usuario.
    private void OnAccountRecoverySendEmailButtonClicked(){

        string emailInformedByUser = emailFromAccountToBeRecoveredInputField.text;

        if(string.IsNullOrEmpty(emailInformedByUser) || emailInformedByUser.Contains("@") == false){
            
            Debug.Log("Enter an valid Email!");
            StartCoroutine(RoutineSendEmailFailure());

        }else{

            Debug.Log("Send Email to recover password...");

            // Desativa a interação com os botoes da tela.
            accountRecoverySendEmailButton.interactable = false;
            emailFromAccountToBeRecoveredInputField.interactable = false;

            // Envia um email para o usuario recuperar a senha.
            string titleId = PlayFabSettings.TitleId;
            var accountRecoveryRequest = new SendAccountRecoveryEmailRequest{
                Email = emailInformedByUser,
                TitleId = titleId
            };

            PlayFabClientAPI.SendAccountRecoveryEmail(
                accountRecoveryRequest, SuccessCallback, FailureCallback

            );
        }

        
    }

    // Chamado se email de recuperacao enviado com sucesso.
    private void SuccessCallback(SendAccountRecoveryEmailResult result){
        Debug.Log("An account recovery email has been sent to the player's email address.");
        StartCoroutine(RoutineSendEmailSuceess());
    }

    // Chamado se acontecer algum erro ao enviar email de recuperacao.
    private void FailureCallback(PlayFabError error){
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());

        StartCoroutine(RoutineSendEmailFailure());
    }


    // Alerta usuario que email foi enviado e reinicia Menu de Login.
    private IEnumerator RoutineSendEmailSuceess(){

        WarningText.gameObject.SetActive(true);
        WarningText.color = Color.green;
        WarningText.text = "Email de recuperação de senha enviado com sucesso. \n Verifique sua caixa de Email.";

        yield return new WaitForSeconds(3.0f);
        accountRecoverySendEmailButton.interactable = true;
        emailFromAccountToBeRecoveredInputField.interactable = true;
        
        WarningText.gameObject.SetActive(false);
        LoginMenu.SetActive(true);
        this.gameObject.SetActive(false);

    }

    // Alerta ao usuario que ocorreu algum erro ao tentar enviar email de recuperacao.
    private IEnumerator RoutineSendEmailFailure(){

        WarningText.gameObject.SetActive(true);
        WarningText.text = "Erro ao enviar email de recuperacao de conta. \n Verifique se email fornecido é valido.";

        yield return new WaitForSeconds(2.5f);
        accountRecoverySendEmailButton.interactable = true;
        emailFromAccountToBeRecoveredInputField.interactable = true;
        
        WarningText.gameObject.SetActive(false);

    }


// -------------------------- Metodos de testes (Apagar depois dos testes) _ Registra um novo usuario no playfab e Login ---------------
    public void Register() {
        Debug.Log("Registing user...");
        var registerRequest = new RegisterPlayFabUserRequest { 
            Email = "jvsc@ic.ufal.br", 
            Password = "123456", 
            DisplayName = "joao", 
            RequireBothUsernameAndEmail = false};
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
    }
   
   
    private void OnRegisterSuccess(RegisterPlayFabUserResult result) {
        Debug.Log("Registrado");

    }
    private void OnRegisterFailure(PlayFabError error) {
        Debug.Log("Erro ao registrar: " + error.GenerateErrorReport());
    }
    public void Login() {

        var request = new LoginWithEmailAddressRequest {
            Email = "jvsc@ic.ufal.br",
            Password = "12345678", 
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams { GetUserAccountInfo = true } 
            };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }
     private void OnLoginSuccess(LoginResult result) {
        Debug.Log("Login Sucess. " + result.PlayFabId + result.SessionTicket);
    }

    private void OnLoginFailure(PlayFabError error) {
        Debug.Log(error.ErrorMessage);
    }

    }