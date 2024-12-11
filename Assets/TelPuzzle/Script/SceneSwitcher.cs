using System.Collections;           
using UnityEngine;                  
using UnityEngine.SceneManagement;  
using TMPro;                     

public class SceneSwitcher : MonoBehaviour
{
    public TMP_InputField userInputField; 
    public WebSocketClientScript socketScript; 

    public void SwitchToPlayerScene()
    {
        if (string.IsNullOrWhiteSpace(userInputField.text))
        {
            Debug.Log("Le champ doit être complété avant de changer de scène.");
            return;
        }

        string playerName = userInputField.text;
        Debug.Log("Nom du joueur envoyé : " + playerName);

        StartCoroutine(WaitForSceneName(playerName));
    }

    private IEnumerator WaitForSceneName(string playerName)
    {
        bool sceneReceived = false;
        string sceneName = null;

        socketScript.SendPlayerData(playerName, receivedSceneName =>
        {
            sceneName = receivedSceneName;
            sceneReceived = true;
        });

        while (!sceneReceived)
        {
            yield return null;  // Attente du prochain frame
        }

        if (!string.IsNullOrEmpty(sceneName))
        {
            Debug.Log("Chargement de la scène : " + sceneName);
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Nom de la scène invalide.");
        }
    }
}
