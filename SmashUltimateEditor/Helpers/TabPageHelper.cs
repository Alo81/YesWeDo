using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SmashUltimateEditor.Helpers
{
    class TabPageHelper
    {
        int page = 0;
        public static TabPage GetEmptyTabPage(int page)
        {
        
        TabPage tabPage = new TabPage()
        {
            Location = new System.Drawing.Point(4, 24),
            //Name = "tabPage1",
            Padding = new System.Windows.Forms.Padding(3),
            Size = new System.Drawing.Size(768, 150),
            TabIndex = page,
            //Text = "tabPage1",
            UseVisualStyleBackColor = true
        };
            return tabPage;
        }
    }
}
