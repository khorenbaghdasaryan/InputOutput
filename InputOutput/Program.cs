using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace InputOutput
{
    public class XmlSerializers
    {
        [Serializable]
        public class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }
        }
        public void Start()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Person));
            string xml;
            using (StringWriter stringWriter = new StringWriter())
            {
                Person person = new Person
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Age = 42
                };
                xmlSerializer.Serialize(stringWriter, person);
                xml = stringWriter.ToString();
            }
            Console.WriteLine(xml);
            using (StringReader reader = new StringReader(xml))
            {
                Person person = (Person)xmlSerializer.Deserialize(reader);
                Console.WriteLine($"{person.FirstName} {person.LastName} {person.Age}");
            }
        }
    }

    public class JSON
    {
        public void Start()
        {
            XDocument xDocument = new XDocument();
            XComment xComment = new XComment("Here is a comment.");
            xDocument.Add(xComment);
            XElement xElement = new XElement("Company",
                new XAttribute("MyAttribute", "MyAttributeValue"),
                new XElement("CompanyName", "AlbDarb"),
                new XElement("CompanyAddress",
                new XElement("Address", "123 Street"),
                new XElement("City", "Yerevan"),
                new XElement("State", "Arabkir"),
                new XElement("Country", "Armenia")));
            xDocument.Add(xElement);
            //Console.WriteLine(xDocument.ToString());
            xDocument.Save("sss.xml");
            XDocument xDocument1 = XDocument.Load("sss.xml");
            //Console.WriteLine(xDocument1);
            string jsonString = JsonConvert.SerializeXNode(xDocument1);
            Console.WriteLine(jsonString);
            xDocument1 = JsonConvert.DeserializeXNode(jsonString);
            //Console.WriteLine(xDocument1);
        }
    }


    [DataContract]
    public class JSONSerializers
    {
        [DataContract]
        public class Person
        {
            [DataMember]
            public string FirstName { get; set; }
            [DataMember]
            public string LastName { get; set; }
            [DataMember]
            public int Age { get; set; }

            public Person(string First, string Last, int Age)
            {
                FirstName = First;
                LastName = Last;
                this.Age = Age;
            }
        }
       public void Start()
       {
            Person person1 = new Person("Ani", "Hovakimyan", 25);
            Person person2 = new Person("Anna", "Baghdasaryan", 27);
            Person person3 = new Person("Armine", "Baghdasaryan", 25);
            Person[] persons = new Person[] { person1, person2, person3 };

            DataContractJsonSerializer jsonSerializer =
                new DataContractJsonSerializer(typeof(Person[]));

            using (FileStream stream = new FileStream("person.json",
                FileMode.OpenOrCreate))
            {
                jsonSerializer.WriteObject(stream, persons);
            }

            using (FileStream fileStream = new FileStream("person.json",
                FileMode.OpenOrCreate))
            {
                Person[] peoples = (Person[])jsonSerializer.ReadObject(fileStream);
                foreach (Person person in peoples)
                {
                    Console.WriteLine($"Name: {person.FirstName} Last: {person.LastName} Age: {person.Age}");
                }
            }

       }
    }
    class Program
    {
        static void Main(string[] args)
        {
            new JSONSerializers().Start();
        }
    }
}
