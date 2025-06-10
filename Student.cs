public class Student
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public double Gpa { get; set; }

    public Student(string id, string name, string address, double GPA)
    {
        Id = id;
        Name = name;
        Address = address;
        Gpa = GPA;
    }
    public Student()
    {
        Id = "no ID";
        Name = "no name";
        Address = "no address";
        Gpa = 0.0;
    }
    public override string ToString()
    {
        return $"[{Id}] {Name} from {Address}: {Gpa}";
    }
}