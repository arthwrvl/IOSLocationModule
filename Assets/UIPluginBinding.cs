using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIPluginBinding : MonoBehaviour
{
    [SerializeField]
    private Button showAlertButton;
    // Start is called before the first frame update
    void Start()
    {
        showAlertButton.onClick.AddListener(ShowAlert);
    }

    void ShowAlert() {
        PluginHelper.ShowAlert("Hello", "World");
    }
}
