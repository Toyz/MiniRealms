using System.Collections.Generic;
using System.Xml.Serialization;

namespace MiniRealms.Engine
{
    public class XmlDictionary<T, TV> : Dictionary<T, TV>, IXmlSerializable
    {
        [XmlRoot("Tiles")]
        [XmlType("Entry")]
        public struct Entry
        {
            public Entry(T key, TV value) : this() { Key = key; Value = value; }
            [XmlElement("Key")]
            public T Key { get; set; }
            [XmlElement("Value")]
            public TV Value { get; set; }
        }

        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
        {
            Clear();
            var serializer = new XmlSerializer(typeof(List<Entry>));
            reader.Read();  // Why is this necessary?
            var list = (List<Entry>)serializer.Deserialize(reader);
            foreach (var entry in list) Add(entry.Key, entry.Value);
            reader.ReadEndElement();
        }

        void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
        {
            var list = new List<Entry>(Count);
            foreach (var entry in this) list.Add(new Entry(entry.Key, entry.Value));
            XmlSerializer serializer = new XmlSerializer(list.GetType());
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            serializer.Serialize(writer, list, ns);
        }
    }
}
