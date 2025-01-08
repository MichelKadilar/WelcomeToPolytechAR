using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ARTouch : MonoBehaviour
{
    public TMP_Text messageText; 
    public GameObject canvas; 
    public TMP_Dropdown dropdown; 
    public Button calculateButton; 
    public Button closeButton;  


    private string currentRoom;

    void Start()
    {
        if (canvas != null)
        {
            canvas.SetActive(false);
        }

        if (calculateButton != null)
        {
            calculateButton.onClick.AddListener(CalculateTravelTime);
        }

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseCanvas);
        }
    }

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
                    ShowObjectInfo(hit.transform.name);
                }
            }
        }
    }

    void ShowObjectInfo(string objectName)
    {
        currentRoom = objectName; 

        if (CSVDataReader.Instance != null)
        {
            string message = $"Salle : {objectName}\n";

            List<ObjectOfInterest> objects = CSVDataReader.Instance.GetObjects();
            ObjectOfInterest targetObject = objects.Find(obj => obj.sourceLoc == objectName);

            if (targetObject != null)
            {
                message += $"Objet lié : {targetObject.name}\n" +
                           $"Heure de vente : {targetObject.sellTime}\n";
            }
            else
            {
                message += "Aucun objet lié trouvé.\n";
            }

            message += Utils.NiceStudentsDisplay(CSVDataReader.Instance.GetStudents(), objectName);

            DisplayMessage(message);

            if (canvas != null)
            {
                canvas.SetActive(true);
                PopulateDropdown();
            }
        }
        else
        {
            Debug.LogWarning("CSVDataReader n'est pas initialisé.");
        }
    }

    void PopulateDropdown()
    {
        if (dropdown != null)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(CSVDataReader.Instance.GetLocationNames());
        }
    }

    void CalculateTravelTime()
    {
        if (dropdown != null && !string.IsNullOrEmpty(currentRoom))
        {
            string destination = dropdown.options[dropdown.value].text;
            int travelTime = GetTravelTime(currentRoom, destination)*2;

            if (travelTime >= 0)
            {
                DisplayMessage($"Temps de trajet entre {currentRoom} et {destination} : {travelTime} minutes.");
            }
            else
            {
                DisplayMessage($"Aucun chemin trouvé entre {currentRoom} et {destination}.");
            }
        }
    }

    int GetTravelTime(string start, string end)
    {
        if (!CSVDataReader.Instance.IsExistingLocation(start) || !CSVDataReader.Instance.IsExistingLocation(end))
        {
            return -1;      
        }

        Queue<(string room, int time)> queue = new Queue<(string room, int time)>();
        HashSet<string> visited = new HashSet<string>();

        queue.Enqueue((start, 0));
        visited.Add(start);

        while (queue.Count > 0)
        {
            var (currentRoom, currentTime) = queue.Dequeue();

            if (currentRoom == end)
            {
                return currentTime;
            }

            foreach (string neighbor in CSVDataReader.Instance.GetLocation(currentRoom).neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    queue.Enqueue((neighbor, currentTime + 1));
                    visited.Add(neighbor);
                }
            }
        }

        return -1; 
    }

    void DisplayMessage(string message)
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

    void CloseCanvas()
    {
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
    }

}
