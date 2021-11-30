using System;
using System.IO;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace Geo.Services
{
    public static class XmlSerializerService
    {
        public static void Serialize<T>(string filePath, T data)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                using (var xmlWriter = XmlWriter.Create(fileStream, new XmlWriterSettings() { NewLineOnAttributes = true, Indent = true, }))
                {
                    serializer.Serialize(xmlWriter, data);
                }
                fileStream.Close();
            }
        }


        public static T Deserialize<T>(string filePath)
        {
            T data = default(T);
            try
            {
                if (File.Exists(filePath))
                {

                    var serializer = new XmlSerializer(typeof(T));
                    using (var fileStream = new FileStream(filePath, FileMode.Open))
                    {
                        if (fileStream.Length == 0) return data; // Если файл пустой, то возвращаем пустой объект.
                        data = (T)serializer.Deserialize(fileStream);
                        fileStream.Close();
                    }
                }
                return data;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return data;
            }
        }
    }
}
