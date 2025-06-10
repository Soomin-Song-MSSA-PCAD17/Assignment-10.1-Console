using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

public class StudentSerializer
{
    private Student tempStudent = null;
    public List<Student> studentList = [];
    private string DirectoryPath;
    JsonSerializerOptions jsonOptions;

    public StudentSerializer(string directoryPath)
    {
        tempStudent = null;
        studentList = [];
        DirectoryPath = directoryPath;
        jsonOptions = new()
        {
            IncludeFields = true,
        };
    }

    public void AddStudent(Student newStudent)
    {
        studentList.Add(newStudent);
    }

    public void AddStudent(string id, string name, string address, double GPA)
    {
        studentList.Add(new Student(id, name, address, GPA));
    }

    public void RemoveStudent(int index)
    {
        studentList.RemoveAt(index);
    }

    public void jsonSerialize(int index)
    {
        Student student = studentList[index];
        string filePath = DirectoryPath + $"{student.Id}.json";

        JsonSerializerOptions options = new()
        {
            IncludeFields = true,
            WriteIndented = true,
        };
        string jsonString = JsonSerializer.Serialize(student, options);
        Console.WriteLine(jsonString);

        if (File.Exists(filePath)) { File.Delete(filePath); }
        FileStream filestream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        JsonSerializer.Serialize(filestream, student, options);
        filestream.Close();
        Console.WriteLine($"{filePath} created.");
    }

    public void jsonSerializeMulti(int index1, int index2)
    {
        if (index2 < index1) { return; }
        for (int i = index1; i <= index2; i++)
        {
            jsonSerialize(i);
        }
    }

    public Student jsonDeserialize(string fileName)
    {
        string filePath=DirectoryPath+fileName;
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"{filePath} not found.");
            return null;
        }

        FileStream jsonStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        Student student = null;
        try
        {
            student = JsonSerializer.Deserialize<Student>(jsonStream, jsonOptions);
        }
        catch (JsonException ex) { Console.WriteLine(ex); }
        finally { jsonStream.Close(); }
        Console.WriteLine(student);
        Console.WriteLine($"{filePath} successfully deserialized.");
        return student;
    }

    public List<Student> jsonDeserializeAll()
    {
        List<Student> students = new List<Student>();
        DirectoryInfo directory = new DirectoryInfo(DirectoryPath);
        
        foreach(var fileInfo in directory.GetFiles())
        {
            string fileName = fileInfo.Name;
            if (fileName.EndsWith(".json"))
            {
                students.Add(jsonDeserialize(fileName));
            }
        }
        return students;
    }

    public void xmlSerialize(int index)
    {
        Student student = studentList[index];
        string filePath = DirectoryPath + $"{student.Id}.xml";

        // write XML
        if (File.Exists(filePath)) { File.Delete(filePath); }
        FileStream filestream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(Student));
        xmlSerializer.Serialize(filestream, student);
        filestream.Close();

        // open and read XML
        StringBuilder xmlString = new("");
        filestream = File.OpenRead(filePath);
        byte[] b = new byte[1024];
        UTF8Encoding temp = new UTF8Encoding(true);
        while (filestream.Read(b, 0, b.Length) > 0)
        {
            xmlString.AppendLine(temp.GetString(b));
        }
        filestream.Close();

        Console.WriteLine(xmlString.ToString());
        Console.WriteLine($"{filePath} created.");
    }

    public void xmlSerializeMulti(int index1, int index2)
    {
        if (index2 < index1) { return; }
        for (int i = index1; i <= index2; i++)
        {
            xmlSerialize(i);
        }
    }

    public Student xmlDeserialize(string fileName)
    {
        string filePath = DirectoryPath + fileName;
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"{filePath} not found.");
            return null;
        }

        FileStream xmlStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        Student student = null;
        try
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Student));
            student = (Student)xmlSerializer.Deserialize(xmlStream);
        }
        catch (XmlException ex) { Console.WriteLine(ex); }
        finally { xmlStream.Close(); }
        Console.WriteLine(student);
        Console.WriteLine($"{filePath} successfully deserialized.");
        return student;
    }

    public List<Student> xmlDeserializeAll()
    {
        List<Student> students = new List<Student>();
        DirectoryInfo directory = new DirectoryInfo(DirectoryPath);

        foreach (var fileInfo in directory.GetFiles())
        {
            string fileName = fileInfo.Name;
            if (fileName.EndsWith(".xml"))
            {
                students.Add(xmlDeserialize(fileName));
            }
        }
        return students;
    }

}
