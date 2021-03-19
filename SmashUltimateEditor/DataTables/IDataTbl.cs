using System;
using System.Xml;

namespace YesWeDo.DataTables
{
    public interface IDataTbl
    {
        abstract void BuildFromXml(XmlReader reader);
        abstract public Tuple<float, float> GetRangeFromName(string name);
    }
}
