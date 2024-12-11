using System.Collections;
using UnityEngine;

public class ARTouch : MonoBehaviour
{
    public WebSocketClientScript socketClient;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Pressed primary button.");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100))
            {
                Debug.Log($"Hit: {hit.transform.name} - Tag: {hit.transform.tag}");

                if (hit.transform.CompareTag("ZX81"))
                {
                    float rotationAmount = 45f; 
                    Debug.Log($"Tag ZX81 détecté. Envoi de la rotation de {rotationAmount}° au serveur.");

                    if (socketClient != null)
                    {
                        socketClient.SendRotationCommand(hit.transform.name, rotationAmount);
                    }
                    else
                    {
                        Debug.LogError("WebSocketClientScript n'est pas attaché !");
                    }
                }
            }
        }
    }
}
