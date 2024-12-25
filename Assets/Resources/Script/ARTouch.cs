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
    private Dictionary<string, List<string>> roomNeighbors = new Dictionary<string, List<string>>();

    void Start()
    {
        if (canvas != null)
        {
            canvas.SetActive(false);
        }

        InitializeRoomNeighbors();

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

            List<Student> students = CSVDataReader.Instance.GetStudents();
            bool studentFound = false;
            message += $"\nÉtudiants : ";
            foreach (Student student in students)
            {
                if (IsStudentInRoom(student, objectName))
                {
                    studentFound = true;
                    message += $"{student.name}, ";
                }
            }

            if (!studentFound)
            {
                message += "\nAucun étudiant trouvé dans cette salle.\n";
            }

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
            dropdown.AddOptions(new List<string>(roomNeighbors.Keys));
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
        if (!roomNeighbors.ContainsKey(start) || !roomNeighbors.ContainsKey(end))
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

            foreach (string neighbor in roomNeighbors[currentRoom])
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

    bool IsStudentInRoom(Student student, string roomName)
    {
        foreach (string location in student.locations)
        {
            if (location == roomName)
            {
                return true;
            }
        }
        return false;
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

    void InitializeRoomNeighbors()
    {
        roomNeighbors["Amphitheater"] = new List<string> { "Room42", "DirectorOffice" };
        roomNeighbors["Cafeteria"] = new List<string> { "Room07", "ImmersiveRoom", "DirectorOffice" };
        roomNeighbors["DirectorOffice"] = new List<string> { "Amphitheater", "Cafeteria" };
        roomNeighbors["Garden"] = new List<string> { "SwimmingPool", "Room01" };
        roomNeighbors["Gym"] = new List<string> { "RestRoom", "Room260", "MusicClass" };
        roomNeighbors["ImmersiveRoom"] = new List<string> { "Room507", "Room682", "Room02", "Room03", "Room07", "Cafeteria" };
        roomNeighbors["Infirmery"] = new List<string> { "RestRoom", "Library" };
        roomNeighbors["Library"] = new List<string> { "Infirmery", "RestRoom", "Room260", "Room420" };
        roomNeighbors["MusicClass"] = new List<string> { "Gym", "Room260", "Playground" };
        roomNeighbors["Playground"] = new List<string> { "MusicClass", "SwimmingPool" };
        roomNeighbors["Secretariat"] = new List<string> { "Room25" };
        roomNeighbors["SwimmingPool"] = new List<string> { "Playground", "Garden" };
        roomNeighbors["RestRoom"] = new List<string> { "Infirmery", "Library", "Gym", "Room260" };
        roomNeighbors["Room01"] = new List<string> { "Garden", "Room02", "Room03" };
        roomNeighbors["Room02"] = new List<string> { "Room01", "ImmersiveRoom" };
        roomNeighbors["Room03"] = new List<string> { "Room01", "ImmersiveRoom", "Room07" };
        roomNeighbors["Room07"] = new List<string> { "Room03", "ImmersiveRoom", "Cafeteria" };
        roomNeighbors["Room25"] = new List<string> { "Secretariat", "Room42" };
        roomNeighbors["Room260"] = new List<string> { "MusicClass", "Gym", "RestRoom", "Library", "Room420" };
        roomNeighbors["Room42"] = new List<string> { "Room25", "Amphitheater" };
        roomNeighbors["Room420"] = new List<string> { "Room260", "Library", "Room507" };
        roomNeighbors["Room507"] = new List<string> { "Room420", "Room682", "ImmersiveRoom" };
        roomNeighbors["Room682"] = new List<string> { "Room507", "ImmersiveRoom" };
    }

    void CloseCanvas()
    {
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
    }

}
