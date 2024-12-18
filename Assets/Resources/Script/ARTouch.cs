using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class ARTouch : MonoBehaviour
{
    public TMP_Text messageText;  

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Pressed primary button.");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.Log("hit");
                Debug.Log(hit.transform.name + " : " + hit.transform.tag);

                if (hit.transform.tag == "ZX81")
                {
                    ShowMessage(hit.transform.name);
                }
            }
        }
    }

    void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
        }
        else
        {
            Debug.LogWarning("Message Text non attribué dans l'inspecteur.");
        }
    }
}
