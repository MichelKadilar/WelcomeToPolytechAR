using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PlayerLoop;
using System.Linq;
public class SearchForm : MonoBehaviour
{
    public TextMeshProUGUI yearLabel;
    public TextMeshProUGUI specializationLabel;
    public TextMeshProUGUI locationLabel;
    public TextMeshProUGUI objectLabel;
    public TextMeshProUGUI transportLabel;
    public TMP_Dropdown yearDropdown;
    public TMP_Dropdown specializationDropdown;
    public TMP_Dropdown locationDropdown;
    public TMP_Dropdown objectDropdown;
    public TMP_Dropdown transportDropdown;
    public Button submitButton;
    public Button resetButton;
    public Button returnButton;
    public GameObject scrollView;
    public GameObject contentArea;
    public GameObject namePrefab;

    private bool init = false;
    private List<string> years = new List<string>(){""};
    private List<string> specializations = new List<string>(){""};
    private List<string> locations = new List<string>(){""};
    private List<string> objects = new List<string>(){""};
    private List<string> transports = new List<string>(){""};

    void Start()
    {
        yearDropdown.ClearOptions();
        foreach(string value in CSVDataReader.Instance.GetYears()) {
            years.Add(value);
        }
        yearDropdown.AddOptions(years);
        specializationDropdown.ClearOptions();
        foreach(string value in CSVDataReader.Instance.GetSpecializations()) {
            specializations.Add(value);
        }
        specializationDropdown.AddOptions(specializations);
        locationDropdown.ClearOptions();
        foreach(string value in CSVDataReader.Instance.GetLocationNames()) {
            locations.Add(value);
        }
        locationDropdown.AddOptions(locations);
        objectDropdown.ClearOptions();
        foreach(string value in CSVDataReader.Instance.GetObjectNames()) {
            objects.Add(value);
        }
        objectDropdown.AddOptions(objects);
        transportDropdown.ClearOptions();
        foreach(string value in CSVDataReader.Instance.GetTransports()) {
            transports.Add(value);
        }
        transportDropdown.AddOptions(transports);

        yearDropdown.onValueChanged.AddListener(OnYearChanged);
        specializationDropdown.onValueChanged.AddListener(OnSpecializationChanged);
        locationDropdown.onValueChanged.AddListener(OnLocationChanged);
        objectDropdown.onValueChanged.AddListener(OnObjectChanged);
        transportDropdown.onValueChanged.AddListener(OnTransportChanged);

        returnButton.gameObject.SetActive(false);
        scrollView.gameObject.SetActive(false);
    }

    void Update()
    {
        if(!init) {
            ClearForm();
            init = true;
        }
    }

    private void OnYearChanged(int value) {
        if(value == 0) {
            yearDropdown.value = 0;
            yearDropdown.captionText.text = "Sélectionnez une année";
        }
    }
    
    private void OnSpecializationChanged(int value) {
        if(value == 0) {
            specializationDropdown.value = 0;
            specializationDropdown.captionText.text = "Sélectionnez une spécialité";
        }
    }
    
    private void OnLocationChanged(int value) {
        if(value == 0) {
            locationDropdown.value = 0;
            locationDropdown.captionText.text = "Sélectionnez un lieu";
        }
    }
    
    private void OnObjectChanged(int value) {
        if(value == 0) {
            objectDropdown.value = 0;
            objectDropdown.captionText.text = "Sélectionnez un objet";
        }
    }
    
    private void OnTransportChanged(int value) {
        if(value == 0) {
            transportDropdown.value = 0;
            transportDropdown.captionText.text = "Sélectionnez un transport";
        }
    }

    public void ClearForm() {
        yearDropdown.value = 0;
        yearDropdown.captionText.text = "Sélectionnez une année";
        specializationDropdown.value = 0;
        specializationDropdown.captionText.text = "Sélectionnez une spécialité";
        locationDropdown.value = 0;
        locationDropdown.captionText.text = "Sélectionnez un lieu";
        objectDropdown.value = 0;
        objectDropdown.captionText.text = "Sélectionnez un objet";
        transportDropdown.value = 0;
        transportDropdown.captionText.text = "Sélectionnez un transport";
    }

    public void SubmitForm() {
        SetFormMode(false);
        int year = years[yearDropdown.value].Count() == 0 ? -1 : int.Parse(years[yearDropdown.value]);
        string specialization = specializations[specializationDropdown.value];
        string location = locations[locationDropdown.value];
        string objectName = objects[objectDropdown.value];
        string transport = transports[transportDropdown.value];
        List<Student> students = CSVDataReader.Instance.GetStudents(year, specialization, location, objectName, transport);
        if(students.Count() == 0) {
            CreateEntry("Aucun étudiant trouvé");
        } else {
            foreach(Student student in students) {
                CreateEntry(student.name);
            }
        }
    }

    public void ReturnToForm() {
        Transform firstChild = contentArea.transform.GetChild(0);
        foreach(Transform child in contentArea.transform) {
            if (child != firstChild) {
                Destroy(child.gameObject);
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentArea.GetComponent<RectTransform>());
        SetFormMode(true);
        ClearForm();
    }

    private void SetFormMode(bool active) {
        yearLabel.gameObject.SetActive(active);
        specializationLabel.gameObject.SetActive(active);
        locationLabel.gameObject.SetActive(active);
        objectLabel.gameObject.SetActive(active);
        transportLabel.gameObject.SetActive(active);
        yearDropdown.gameObject.SetActive(active);
        specializationDropdown.gameObject.SetActive(active);
        locationDropdown.gameObject.SetActive(active);
        objectDropdown.gameObject.SetActive(active);
        transportDropdown.gameObject.SetActive(active);
        submitButton.gameObject.SetActive(active);
        resetButton.gameObject.SetActive(active);
        
        returnButton.gameObject.SetActive(!active);
        scrollView.gameObject.SetActive(!active);
    }

    private void CreateEntry(string entryText)
    {
        GameObject newLogObject = Instantiate(namePrefab, contentArea.transform);

        TMPro.TextMeshProUGUI tmpText = newLogObject.GetComponent<TMPro.TextMeshProUGUI>();
        if (tmpText != null) {
            tmpText.text = entryText;
        }
    }
}
