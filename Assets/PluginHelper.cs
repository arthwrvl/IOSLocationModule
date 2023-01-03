
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PluginHelper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textResult;

    [DllImport("__Internal")]
    private static extern int _addTwoNumberInIOS(int a, int b);

    void Start()
    {
        AddTwoNumber();
    }

    public void AddTwoNumber()
    {
        int result = _addTwoNumberInIOS(10, 5);
        textResult.text = "10 + 5  is : " + result;
    }
}
