using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SmashUltimateEditor.UI
{
    class LabelBox
    {
        public ComboBox combo;
        public TextBox text;
        public Label label;

        public LabelBox()
        {
            label = new Label();
            //combo = new ComboBox();
            //text = new TextBox();
        }
        
        public string GetTextBoxValue()
        {
            return this?.text?.Text ?? "";
        }
        public object GetComboBoxValue()
        {
            return this.combo.SelectedItem;
        }

        public string GetLabelValue()
        {
            return this.label.Text;
        }

        public void SetTextBoxValue(string newText)
        {
            if (!IsTextboxSet())
            {
                text = new TextBox();
            }
            this.text.Text = newText;
        }
        public void SetComboBoxValue(object newValue)
        {
            if (!IsComboSet())
            {
                combo = new ComboBox();
            }
            this.combo.SelectedItem = newValue;
        }
        public void SetComboBoxDataSource(List<string>newValue)
        {
            if (!IsComboSet())
            {
                combo = new ComboBox();
            }
            if(newValue.Where(x => x == null).ToList().Count > 0)
            {
                string x = "";
            }
            this.combo.DataSource = newValue.Where(x => x != null).ToList();
        }
        public void SetComboBoxDataSource(object newValue)
        {
            if (!IsComboSet())
            {
                combo = new ComboBox();
            }
            this.combo.DataSource = newValue;
        }
        public void AddComboBoxValue(object newValue)
        {
            if (!IsComboSet())
            {
                combo = new ComboBox();
            }
            this.combo.Items.Add(newValue);
        }

        public void SetLabelValue(string newText)
        {
            this.label.Text = newText;
        }

        public bool IsComboSet()
        {
            return combo != null;
        }
        public bool IsTextboxSet()
        {
            return text != null;
        }
    }
}
