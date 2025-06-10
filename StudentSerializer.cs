using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

public class StudentSerializer
{
    private Student tempStudent = null;
    public List<Student> studentList = [];
    private const string directoryPath = @"C:\MSSA_Output\";
    JsonSerializerOptions jsonOptions;

    public StudentSerializer()
    {
        tempStudent = null;
        studentList = [];
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
        string filePath = directoryPath + $"{student.Id}.json";

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

    public Student jsonDeserialize(string fileName)
    {
        string filePath=directoryPath+fileName;
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

    public void xmlSerialize(int index)
    {
        Student student = studentList[index];
        string filePath = directoryPath + $"{student.Id}.xml";

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

    public Student xmlDeserialize(string fileName)
    {
        string filePath = directoryPath + fileName;
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
}
