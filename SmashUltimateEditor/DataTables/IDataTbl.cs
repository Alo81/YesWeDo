using System;
using System.Xml;

namespace SmashUltimateEditor
{
    public interface IDataTbl
    {
        abstract void BuildFromXml(XmlReader reader);
        abstract public Tuple<float, float> GetRangeFromName(string name);
    }
}
