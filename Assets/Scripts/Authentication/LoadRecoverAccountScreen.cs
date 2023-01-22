using UnityEngine;
using UnityEngine.UI;

// Inicia o menu de recuperação de senha.
public class LoadRecoverAccountScreen : MonoBehaviour
{

    public GameObject AccountRecoverMenu;
    public GameObject LoginMenu;
    public Button ButtonRecoveryAccount;


    void Start()
    {
        ButtonRecoveryAccount.onClick.AddListener(OnAccountRecoveryButtonClicked);
    }

    private void OnAccountRecoveryButtonClicked(){

        AccountRecoverMenu.gameObject.SetActive(true);
        LoginMenu.gameObject.SetActive(false);
        

    }


}
