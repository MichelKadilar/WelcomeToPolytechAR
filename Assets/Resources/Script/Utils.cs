using System.Collections.Generic;
using System.Linq;

public static class Utils
{
    public static string NiceStudentsDisplay(List<Student> students, string location) {
        if(location == null || students == null || students.Count() == 0) return "\nAucun étudiant trouvé.\n";
        string message = $"\nÉtudiants : ";
        string onePM = "\n13h : \n";
        string twoPM = "\n14h : \n";
        string threePM = "\n15h : \n";
        string fourPM = "\n16h : \n";
        string fivePM = "\n17h : \n";
        foreach (Student student in students) {
            if(student.locations[0] == location)
                onePM += $"{student.name}, ";
            if(student.locations[1] == location)
                twoPM += $"{student.name}, ";
            if(student.locations[2] == location)
                threePM += $"{student.name}, ";
            if(student.locations[3] == location)
                fourPM += $"{student.name}, ";
            if(student.locations[4] == location)
                fivePM += $"{student.name}, ";
        }
        //TODO don't show empty hours
        //TODO remove end ", " of shown hours
        return message + onePM + twoPM + threePM + fourPM + fivePM;
    }
}