using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace InputOutput
{
    public class InfoFile
    {
        public void GetInfo()
        {
            FileInfo fileInfo = new FileInfo("c:\\arctic.gif");
            string name = fileInfo.Name;
            string directory = fileInfo.DirectoryName;
            string extension = fileInfo.Extension;
            long length = fileInfo.Length;
            DateTime creationTime = fileInfo.CreationTime;
            DateTime lastTime = fileInfo.LastAccessTime;
            Console.WriteLine(lastTime);
            Console.WriteLine(creationTime);
            Console.WriteLine(length);
            Console.WriteLine(extension);
            Console.WriteLine(directory);
            Console.WriteLine(name);
            fileInfo.CopyTo("c:\\arctic.gif");
            fileInfo.Delete();
        }
    }
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

    class NetworkIO
    {
        public void Start()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.google.com");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream stream = response.GetResponseStream();

            for (int i = 0; ; i++)
            {
                int ch = stream.ReadByte();
                if (ch == -1)
                { 
                    break;
                }
                Console.Write((char)ch);
                if ((i % 400) == 0)
                {
                    Console.WriteLine("\nPress Enter");
                    Console.ReadLine();
                }
            }
            response.Close();
        }
    }
    class AsynchronIO
    {
        Stream stream = File.OpenRead("C:\\alb.txt");
        public byte[] buffer = new byte[256];
        public void Start()
        {
            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(AsyncCall), null);
            for (int i = 0; i < 90; i++)
                Console.Write($"{i} ");

        }

        private void AsyncCall(IAsyncResult ar)
        {
            int bytesRead = stream.EndRead(ar);
            string txt = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine(txt);
            for (long i = 0; i < 1; i++)
            {
                Console.WriteLine("-------");
            }
        }
    }

    class TextIO
    {
        public void Write()
        {
            FileInfo fileInfo = new FileInfo("C:\\alb.txt");
            StreamWriter streamWriter = fileInfo.CreateText();

            streamWriter.WriteLine("aaaaaaaaaaaaaaaaaaaa....");
            streamWriter.WriteLine("bbbbbbbbbbbbbbbbbbbb....");

            for (int i = 0; i < 10; i++)
            {
                streamWriter.Write($"{i} ");
            }
            streamWriter.Close();
        }
        public void Read()
        {
            StreamReader streamReader = File.OpenText("C:\\alb.txt");
            string text = null;
            while ((text = streamReader.ReadLine()) != null)
            {
                Console.WriteLine(text);
            }

            streamReader.Close();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            new InfoFile().GetInfo();
        }
    }
}
