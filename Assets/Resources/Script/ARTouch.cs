using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.XR.CoreUtils;
using System.Linq;

public class ARTouch : MonoBehaviour
{
    public GameObject panelRoom;
    public XROrigin xrOrigin;

    public TMP_Text nameLabel;
    public GameObject studentsContentArea;
    public GameObject studentsNamePrefab;
    public TMP_Text objectResultLabel;
    public TMP_Dropdown locationDropdown; 
    public TMP_Text resultLabel;


    private bool init = false;
    private string currentRoom;

    void Start()
    {
        PopulateDropdown();
    }

    void Update()
    {
        if(!init) {
            locationDropdown.value = 0;
            locationDropdown.captionText.text = "Sélectionnez un lieu";
            init = true;
        }

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
        
        DisableXRComponents();
        panelRoom.SetActive(true);
        ClearView();

        if (CSVDataReader.Instance != null)
        {
            nameLabel.SetText(objectName);

            List<ObjectOfInterest> objects = CSVDataReader.Instance.GetObjects();
            ObjectOfInterest targetObject = objects.Find(obj => obj.sourceLoc == objectName);

            if (targetObject != null) {
                objectResultLabel.SetText(targetObject.name);
            } else {
                objectResultLabel.SetText("Aucun objet trouvé");
            }

            List<Student> students = CSVDataReader.Instance.GetStudents(objectName);
            if(students.Count() == 0) {
                CreateStudentEntry("Aucun étudiant trouvé");
            } else {
                foreach(Student student in students) {
                    CreateStudentEntry(student.name);
                }
            }
            
            PopulateDropdown();
        }
        else
        {
            Debug.LogWarning("CSVDataReader n'est pas initialisé.");
        }
    }

    private void DisableXRComponents()
    {
        if (xrOrigin != null)
        {
            Debug.Log("XROrigin trouv� : " + xrOrigin.name);
            Component[] components = xrOrigin.GetComponents<Component>();
            foreach (Component component in components)
            {
                if (component is MonoBehaviour script)
                {
                    script.enabled = false;
                    Debug.Log("Script d�sactiv� : " + script.GetType().Name);
                }
                else
                {
                    Debug.Log("Composant non script trouv� : " + component.GetType().Name);
                }
            }
        }
        else
        {
            Debug.LogWarning("Aucun XROrigin trouv� dans la sc�ne !");
        }
    }

    void PopulateDropdown()
    {
        if (locationDropdown != null)
        {
            locationDropdown.ClearOptions();
            List<string> locations = new List<string>(){""};
            foreach(string value in CSVDataReader.Instance.GetLocationNames()) {
                locations.Add(value);
            }
            locationDropdown.AddOptions(locations);
            locationDropdown.value = 0;
            locationDropdown.captionText.text = "Sélectionnez un lieu";
        }
    }

    public void CalculateTravelTime()
    {
        if (locationDropdown != null && !string.IsNullOrEmpty(currentRoom))
        {
            string destination = locationDropdown.options[locationDropdown.value].text;
            int travelTime = GetTravelTime(currentRoom, destination)*2;

            if (travelTime >= 0)
            {
                resultLabel.SetText($"Temps de trajet entre {currentRoom} et {destination} : \n{travelTime} minutes.");
                LoggingService.Instance.LogInfo($"(Trajet) Calcul du temps de trajet entre {currentRoom} et {destination} : {travelTime} minutes.");
            }
            else
            {
                resultLabel.SetText($"Aucun chemin trouvé entre {currentRoom} et {destination}.");
                LoggingService.Instance.LogInfo($"(Trajet) Recherche de trajet. Aucun chemin trouvé.");
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

    private void CreateStudentEntry(string entryText)
    {
        GameObject newLogObject = Instantiate(studentsNamePrefab, studentsContentArea.transform);

        TMPro.TextMeshProUGUI tmpText = newLogObject.GetComponent<TMPro.TextMeshProUGUI>();
        if (tmpText != null) {
            tmpText.text = entryText;
        }
    }

    private void ClearView() {
        Transform firstChild = studentsContentArea.transform.GetChild(0);
        foreach(Transform child in studentsContentArea.transform) {
            if (child != firstChild) {
                Destroy(child.gameObject);
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(studentsContentArea.GetComponent<RectTransform>());
        resultLabel.SetText("");
    }

}
