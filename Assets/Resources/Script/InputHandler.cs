using UnityEngine;
using UnityEngine.UI;

public class MyInputHandler : MonoBehaviour
{
    public InputField inputField; 
    public Text displayText;    

    public void OnSubmit()
    {
        string userInput = inputField.text; 
    }
}
