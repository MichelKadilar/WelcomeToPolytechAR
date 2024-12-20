public class Student
{
    public string name, gender, hairColor, transport, clothing, specialization;
    public string[] locations = new string[5];
    public int height, year;

    public Student(string name, string gender, string onePM, string twoPM, string threePM, string fourPM, string fivePM,
        string hairColor, int height, string transport, string clothing, int year, string specialization) {
        this.name = name;
        this.gender = gender;
        locations[0] = onePM;
        locations[1] = twoPM;
        locations[2] = threePM;
        locations[3] = fourPM;
        locations[4] = fivePM;
        this.hairColor = hairColor;
        this.height = height;
        this.transport = transport;
        this.clothing = clothing;
        this.year = year;
        this.specialization = specialization;
    }

}