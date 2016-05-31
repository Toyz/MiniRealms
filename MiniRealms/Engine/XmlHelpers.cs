using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MiniRealms.Engine
{
    public static class XmlHelpers
    {
        public static string Serialize(object o)
        {
            var sw = new StringWriter();
            try
            {
                var serializer = new XmlSerializer(o.GetType());
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                var tw = new XmlTextWriter(sw);
                serializer.Serialize(tw, o, ns);
                return sw.ToString();
            }
            catch (Exception)
            {
                //Handle Exception Code
            }

            return string.Empty;
        }

        public static object Deserialize(string xmlOfAnObject, Type objectType)
        {
            var strReader = new StringReader(xmlOfAnObject);
            var serializer = new XmlSerializer(objectType);
            var xmlReader = new XmlTextReader(strReader);
            try
            {
                var anObject = serializer.Deserialize(xmlReader);
                return anObject;
            }
            catch (Exception)
            {
                //Handle Exception Code
            }
            finally
            {
                xmlReader.Close();
                strReader.Close();
            }

            return null;
        }
    }
}
