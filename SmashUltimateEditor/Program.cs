using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SmashUltimateEditor.Enums;
using static SmashUltimateEditor.Extensions;

namespace SmashUltimateEditor
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //EnumUtil<sub_rule_opt>.Contains("metal_rule");
            //string xml = @"F:\Tools\Switch\Smash Ultimate Modding\PRCEditor\files\out_ui_spirits_battle_db.prc";
            //FileManager.ReadXML(xml);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SpiritEditorWindow());
        }
    }
}
