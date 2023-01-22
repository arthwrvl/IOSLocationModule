using UnityEngine;
using TMPro;

public class getUserProfile : MonoBehaviour
{

    public TextMeshProUGUI userInfoText;
    

    // Start is called before the first frame update
    void Start()
    {
        getuserInfo();   
    }

   private void getuserInfo(){
        userInfoText.text = PlayerPrefs.GetString("userID", "no name") + "\n" 
        + PlayerPrefs.GetString("userName", "no name") + "\n" 
        + PlayerPrefs.GetString("userEmail", "no email");
   }
}
