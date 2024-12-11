using System;
using UnityEngine;
using NativeWebSocket;
using System.Threading.Tasks;
using Newtonsoft.Json; 
using System.Collections.Generic;

public class WebSocketClientScript : MonoBehaviour
{
    private WebSocket webSocket;
    //private string serverUrl = "ws://192.168.1.5:8080";
    private string serverUrl = "wss://incredibly-glad-foal.ngrok-free.app"; 

    private string sceneName;

    private Action<string> onSceneNameReceived;

    async void Start()
    {
        // Connexion au serveur WebSocket
        await ConnectToServer();
    }

    private async Task ConnectToServer()
    {
        // Création de l'objet WebSocket
        webSocket = new WebSocket(serverUrl);

        // Gestion de l'événement de connexion
        webSocket.OnOpen += () =>
        {
            Debug.Log("Connecté au serveur !");
            // Emission d'un événement 'joinGame' avec les données spécifiées
            var playerInfo = new
            {
                gameId = 666
            };
            string jsonMessage = JsonConvert.SerializeObject(new { eventType = "joinGame", data = playerInfo });
            webSocket.SendText(jsonMessage);
        };

        // Gestion de la réception des messages
        webSocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            HandleReceivedMessage(message);
        };

        // Gestion des erreurs
        webSocket.OnError += (e) =>
        {
            Debug.LogError("Erreur WebSocket : " + e);
        };

        // Gestion de la fermeture de la connexion
        webSocket.OnClose += (e) =>
        {
            Debug.Log("Connexion WebSocket fermée");
        };

        // Connexion au serveur
        await webSocket.Connect();
    }

    private void HandleReceivedMessage(string message)
    {
        try
        {
            var receivedData = JsonConvert.DeserializeObject<Dictionary<string, object>>(message);

            if (receivedData.ContainsKey("eventType"))
            {
                string eventType = receivedData["eventType"].ToString();

                switch (eventType)
                {
                    case "nameAssigned":
                        string receivedSceneName = receivedData["data"].ToString();
                        Debug.Log("Nom de la scène reçu du serveur : " + receivedSceneName);
                        sceneName = receivedSceneName;
                        onSceneNameReceived?.Invoke(receivedSceneName);
                        break;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Erreur lors du traitement du message : " + e.Message);
        }
    }

    public async void SendPlayerData(string name, Action<string> onSceneReceived)
    {
        onSceneNameReceived = onSceneReceived;
        var playerData = new { playerName = name };
        string jsonData = JsonConvert.SerializeObject(new
        {
            type = "playersCreated",
            data = playerData
        });

        Debug.Log("Données sérialisées : " + jsonData);
        await webSocket.SendText(jsonData);
    }

    public async void SendRotationCommand(string objectName, float rotationAmount)
    {
        Debug.Log($"Envoi de la commande de rotation pour {objectName} : {rotationAmount}°");

        var rotationData = new 
        { 
            objectName, 
            rotation = rotationAmount 
        };
        
        string rotationJson = JsonConvert.SerializeObject(new 
        { 
            type = "rotateObject", 
            data = rotationData 
        });

        Debug.Log("Données sérialisées : " + rotationJson);
        await webSocket.SendText(rotationJson);
    }

    private async void OnApplicationQuit()
    {
        if (webSocket != null && webSocket.State == WebSocketState.Open)
        {
            await webSocket.Close();
        }
    }

    private void Update()
    {
        // Si vous êtes sur WebGL, vous devez appeler DispatchMessageQueue chaque frame
        #if !UNITY_WEBGL || UNITY_EDITOR
                if (webSocket != null)
                {
                    webSocket.DispatchMessageQueue();
                }
        #endif
    }

    private void OnDestroy()
    {
        if (webSocket != null)
        {
            webSocket.Close();
        }
    }
}
