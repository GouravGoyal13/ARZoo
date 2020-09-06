using System.Xml.Serialization;
using System.IO;
using System.Text;
using System;
using System.Xml;
using System.Collections.Generic;

public class XMLUtil
{
    public static List<T> DeserializeList<T>(string filePath, string type)
    {
        var itemList = new List<T>();

        if (File.Exists(filePath))
        {
            var serializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute(type));
            TextReader reader = new StreamReader(filePath);
            itemList = (List<T>)serializer.Deserialize(reader);
            reader.Close();
        }

        return itemList;
    }
}




