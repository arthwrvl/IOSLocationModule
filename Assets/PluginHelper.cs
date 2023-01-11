
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PluginHelper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textResult;

    [DllImport("__Internal")]
    private static extern string _addTwoNumberInIOS(int a, int b);
    [DllImport("__Internal")]
    private static extern void _start();


    public void AddTwoNumber()
    {
        string result = _addTwoNumberInIOS(10, 5);
        textResult.text = "10 + 5  is : " + result;
    }
    public void Start() {
        _start();
    }
}
