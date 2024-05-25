namespace UT;

public class PersonTest
{
  [Test]
    public void StructVsClassTest()
    {
        // Struct usage
        var personStruct = new PersonStruct("John", 30);
        var personStruct2 = personStruct;
        personStruct.Age = 31; // This is allowed because structs are value types
        Assert.AreEqual("John", personStruct.Name);
        Assert.AreEqual(31, personStruct.Age);
        Assert.AreEqual(30, personStruct2.Age);

        // Class usage
        var personClass = new PersonClass("Jane", 25);
        var personClass2 = personClass;
        personClass.Age = 26; // This is allowed because classes are reference types
        Assert.AreEqual("Jane", personClass.Name);
        Assert.AreEqual(26, personClass.Age);
        Assert.AreEqual(26, personClass2.Age);
    }
}

public class PersonClass
{
    public string Name { get; set; }
    public int Age { get; set; }

    public PersonClass(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public string GetInfo()
    {
        return $"Name: {Name}, Age: {Age}";
    }
}

public struct PersonStruct
{
    public string Name { get; set; }
    public int Age { get; set; }

    public PersonStruct(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public string GetInfo()
    {
        return $"Name: {Name}, Age: {Age}";
    }
}