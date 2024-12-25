using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ARTouch : MonoBehaviour
{
    public TMP_Text messageText; // Texte pour afficher les informations
    public GameObject infoCanvas; // Canvas contenant l'image et le texte

    private bool isCanvasVisible = false; // État du Canvas

    void Start()
    {
        // Assurez-vous que le Canvas est masqué au démarrage
        if (infoCanvas != null)
        {
            infoCanvas.SetActive(false);
        }
        else
        {
            Debug.LogWarning("InfoCanvas n'est pas attribué dans l'inspecteur.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isCanvasVisible)
            {
                // Si le Canvas est visible, on le masque
                HideCanvas();
            }
            else
            {
                // Sinon, on détecte les clics sur les objets
                DetectObjectClick();
            }
        }
    }

    void DetectObjectClick()
    {
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

    void ShowObjectInfo(string objectName)
    {
        if (CSVDataReader.Instance != null)
        {
            string message = $"Salle : {objectName}\n";

            // Recherche des objets liés
            List<ObjectOfInterest> objects = CSVDataReader.Instance.GetObjects();
            ObjectOfInterest targetObject = objects.Find(obj => obj.sourceLoc == objectName);

            if (targetObject != null)
            {
                message += $"Objet lié : {targetObject.name}\n" +
                           $"Heure de vente : {targetObject.sellTime}\n";
            }

            // Recherche des étudiants liés
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
            ShowCanvas();
        }
        else
        {
            Debug.LogWarning("CSVDataReader n'est pas initialisé.");
        }
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

    void ShowCanvas()
    {
        if (infoCanvas != null)
        {
            infoCanvas.SetActive(true);
            isCanvasVisible = true;
        }
    }

    void HideCanvas()
    {
        if (infoCanvas != null)
        {
            infoCanvas.SetActive(false);
            isCanvasVisible = false;
        }
    }
}
