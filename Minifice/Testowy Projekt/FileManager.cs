#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
#endregion

namespace Testowy_Projekt
{
    class FileManager
    {
        #region Pola
        XmlSerializer serializer;
        TextWriter textWriter;
        TextReader textReader;
        #endregion

        #region Inicjalizacja
        /// <summary>
        /// Konstruktor,
        /// chyba nie ma potrzeby żeby coś robił
        /// </summary>
        public FileManager()
        {
        } 
        #endregion

        #region Metody Publiczne

        public T Deserialize<T>(string File)
        {

            try
            {
                serializer = new XmlSerializer(typeof(T));
                textReader = new StreamReader(File + ".xml");
                T Obj = (T)serializer.Deserialize(textReader);
                textReader.Close();
                return Obj;
            }
            catch (Exception)
            {
                // TODO: Handle Errors
                return default(T);
            }
        }

        public void Serialize<T>(string File, T Obj)
        {
            try
            {
                serializer = new XmlSerializer(typeof(T));
                textWriter = new StreamWriter(File + ".xml");
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                serializer.Serialize(textWriter, Obj, ns);
                textWriter.Close();
            }
            catch(Exception)
            {
                // TODO: Handle Errors
            }
        }


        #endregion


    }
}
