using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SmashUltimateEditor
{
    public interface IDataTbl
    {
        abstract void BuildFromXml(XmlReader reader);

        abstract string GetValueFromName(string name);
    }
}
