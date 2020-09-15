using SmashUltimateEditor.Helpers;
using SmashUltimateEditor.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SmashUltimateEditor
{
    public interface IDataTbl
    {
        abstract void BuildFromXml(XmlReader reader);
        abstract public Tuple<float, float> GetRangeFromName(string name);
    }
}
