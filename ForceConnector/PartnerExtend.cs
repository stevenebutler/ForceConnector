using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ForceConnector.Partner
{
    public partial class sObject
    {
        public object getObject()
        {
            var fields = new Dictionary<string, object>();
            foreach (XmlElement field in anyField)
            {
                if (field.Attributes.Count == 0)
                {
                    fields.Add(field.LocalName, field.InnerText);
                }
                else
                {
                    fields.Add(field.LocalName, getObject(field.ChildNodes));
                }
            }

            return fields;
        }

        private object getObject(XmlNodeList childList)
        {
            var childs = new Dictionary<string, object>();
            foreach (XmlNode child in childList)
            {
                if (child.Attributes.Count == 0)
                {
                    childs.Add(child.LocalName, child.InnerText);
                }
                else
                {
                    childs.Add(child.LocalName, getObject(child.ChildNodes));
                }
            }

            return childs;
        }

        public sObject setObject(string objname, Dictionary<string, object> entity)
        {
            var obj = new sObject();
            obj.type = objname;
            var fields = new XmlElement[entity.Count + 1];
            var doc = new XmlDocument();
            for (int i = 0, loopTo = entity.Count; i <= loopTo; i += 1)
            {
                string key = entity.Keys.ElementAtOrDefault(i);
                fields[i] = doc.CreateElement(key);
                fields[i].InnerText = entity[key].ToString();
            }

            obj.anyField = fields;
            return obj;
        }

        public object getField(string fieldName)
        {
            for (int i = 0, loopTo = anyField.Length; i <= loopTo; i += 1)
            {
                var entity = anyField[i];
                if ((entity.LocalName.ToLower() ?? "") == (fieldName.ToLower() ?? ""))
                {
                    if (entity.Attributes.Count == 0)
                    {
                        return entity.InnerText;
                    }
                    else
                    {
                        return convert(entity.ChildNodes);
                    }
                }
            }

            return null;
        }

        private sObject convert(XmlNodeList entity)
        {
            var obj = new sObject();
            obj.type = entity[0].InnerText;
            var fields = new XmlElement[entity.Count];
            var doc = new XmlDocument();
            for (int i = 1, loopTo = entity.Count; i <= loopTo; i += 1)
            {
                fields[i - 1] = doc.CreateElement(entity[i].LocalName);
                fields[i - 1].InnerText = entity[i].InnerText;
                fields[i - 1].Prefix = entity[i].Prefix;
            }

            obj.anyField = fields;
            return obj;
        }
    }
}