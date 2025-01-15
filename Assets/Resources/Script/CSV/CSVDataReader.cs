using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CSVDataReader : MonoBehaviour
{
    public static CSVDataReader Instance { get; private set; }

    TextAsset studentsFile;
    TextAsset objectsFile;
    TextAsset locationsFile;

    List<Student> students = new List<Student>();
    List<ObjectOfInterest> objects = new List<ObjectOfInterest>();
    List<Location> locations = new List<Location>();
    List<string> transports = new List<string>();
    List<string> clothings = new List<string>();
    List<string> specializations = new List<string>();
    List<int> years = new List<int>();

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadStudents();
        LoadObjects();
        LoadLocations();
    }
    
    private void LoadStudents() {
        objectsFile = Resources.Load<TextAsset>("CSVFiles/objects");
        studentsFile = Resources.Load<TextAsset>("CSVFiles/students");
        string[] csvStudents = studentsFile.ToString().Split('\n');
        for (int i = 1; i < csvStudents.Length; i++)
        {
            string[] tmp = csvStudents[i].Split(',');
            students.Add(
                new Student(tmp[0], tmp[1], tmp[2], tmp[3], tmp[4], tmp[5], tmp[6], tmp[7], int.Parse(tmp[8]), tmp[9], tmp[10], int.Parse(tmp[11]), tmp[12])
            );
            string transport = tmp[9];
            if(!transports.Contains(transport)) transports.Add(transport);
            string clothing = tmp[10];
            if(!clothings.Contains(clothing)) clothings.Add(clothing);
            string specialization = tmp[12];
            if(!specializations.Contains(specialization)) specializations.Add(specialization);
            int year = int.Parse(tmp[11]);
            if(!years.Contains(year)) years.Add(year);
        }
        years.Sort();
        specializations.Sort();
        transports.Sort();
    }

    private void LoadObjects() {
        objectsFile = Resources.Load<TextAsset>("CSVFiles/objects");
        string[] csvObjects = objectsFile.ToString().Split('\n');
        for (int i = 1; i < csvObjects.Length; i++)
        {
            string[] tmp = csvObjects[i].Split(',');
            objects.Add(new ObjectOfInterest(tmp[0], tmp[1], tmp[2]));
        }
    }

    private void LoadLocations() {
        locationsFile = Resources.Load<TextAsset>("CSVFiles/locations");
        string[] csvLocations = locationsFile.ToString().Split('\n');
        for (int i = 1; i < csvLocations.Length; i++)
        {
            string[] tmp = csvLocations[i].Split(',');
            string[] csvNeighbors = tmp[1].Split('[')[1].Split(']')[0].Split(';');
            locations.Add(new Location(tmp[0], csvNeighbors));
        }
    }

    public List<Location> GetLocations()
    {
        return locations;
    }

    public List<string> GetLocationNames()
    {
        List<string> names = new List<string>();
        foreach(Location location in locations) {
            names.Add(location.name);
        }
        names.Sort();
        return names;
    }

    public Location GetLocation(string locationName) {
        foreach(Location location in locations) {
            if(locationName == location.name) return location;
        }
        return null;
    }

    public Boolean IsExistingLocation(string locationName)
    {
        return GetLocation(locationName) != null;
    }

    public List<string> GetYears()
    {
        List<string> names = new List<string>();
        foreach(int year in years) {
            names.Add(year.ToString());
        }
        return names;
    }

    public List<string> GetTransports()
    {
        return transports;
    }
        
    public List<string> GetClothings()
    {
        return clothings;
    }
        
    public List<string> GetSpecializations()
    {
        return specializations;
    }

    public List<Student> GetStudents()
    {
        return students;
    }

    public List<ObjectOfInterest> GetObjects()
    {
        return objects;
    }

    public List<string> GetObjectNames()
    {
        List<string> names = new List<string>();
        foreach(ObjectOfInterest objectOfInterest in objects) {
            names.Add(objectOfInterest.name);
        }
        names.Sort();
        return names;
    }

    public List<Student> GetStudents(string location, int startTime, int endTime) {
        List<Student> studentsResult = new List<Student>();
        foreach(Student student in students) {
            for(int i = 0; i < 5; i++) {
                if(startTime <= 13+i && endTime >= 13+i && location == student.locations[i]) {
                    studentsResult.Add(student);
                    break;
                }
            }
        }
        return studentsResult;
    }

    public List<Student> GetStudents(int year, string specialization, string location, string objectName, string transport) {
        bool yearParamSetted = year > 0;
        bool specializationParamSetted = specialization != null && specialization.Count() > 0;
        bool locationParamSetted = location != null && location.Count() > 0;
        bool objectNameParamSetted = objectName != null && objectName.Count() > 0;
        bool transportParamSetted = transport != null && transport.Count() > 0;

        //if(!(yearParamSetted && specializationParamSetted && locationParamSetted && objectNameParamSetted && transportParamSetted)) return students;

        List<Student> studentsResult = new List<Student>();
        foreach(Student student in students) {
            if(yearParamSetted && student.year != year) continue;
            if(specializationParamSetted && student.specialization != specialization) continue;
            if(transportParamSetted && student.transport != transport) continue;
            if(locationParamSetted && !student.locations.Contains(location)) continue;
            if(objectNameParamSetted) {
                bool matchObject = false;
                foreach(ObjectOfInterest objectOfInterest in objects) {
                    if(objectOfInterest.name == objectName && student.locations.Contains(objectOfInterest.sourceLoc)) {
                        matchObject = true;
                        break;
                    }
                }
                if(!matchObject) continue;
            }
            studentsResult.Add(student);
        }
        return studentsResult;
    }

    public List<ObjectOfInterest> GetObjects(string location, int startTime, int endTime) {
        List<ObjectOfInterest> objectsOfInterest = new List<ObjectOfInterest>();
        foreach(ObjectOfInterest objectOfInterest in objectsOfInterest) {
            if(objectOfInterest.sellTime == "n/a") continue;
            int time = int.Parse(objectOfInterest.sellTime.Split("h")[0]);
            if(startTime <= time && endTime >= time && location == objectOfInterest.sourceLoc) {
                objectsOfInterest.Add(objectOfInterest);
            }
        }
        return objectsOfInterest;
    }
}
