using System;
using System.Collections.Generic;
using System.Drawing;
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
        }

        public void SetLabel(string name, Point pos)
        {
            SetLabelValue(String.Format("{0}", name));
            label.Width = Defs.LABEL_WIDTH;
            label.Height = Defs.LABEL_HEIGHT;
            label.Location = pos;
        }
        public void SetTextBox(string value, Point pos)
        {
            SetTextBoxValue(value);
            text.Width = Defs.BOX_WIDTH;
            text.Height = Defs.BOX_HEIGHT;
            text.Location = pos;
        }
        public void SetComboBox(string value, List<string> opts, Point pos)
        {
            SetComboBoxDataSource(opts);
            SetComboBoxValue(value);
            combo.Width = Defs.BOX_WIDTH;
            combo.Height = Defs.BOX_HEIGHT;
            combo.Location = pos;
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
            combo.SelectedIndex = combo.FindStringExact(newValue.ToString());
        }
        public void SetComboBoxDataSource(List<string>newValue)
        {
            if (!IsComboSet())
            {
                combo = new ComboBox();
            }
            combo.DataSource = newValue.Where(x => x != null).ToList();
            combo.BindingContext = new BindingContext();
        }
        public void SetComboBoxDataSource(object newValue)
        {
            if (!IsComboSet())
            {
                combo = new ComboBox();
            }
            this.combo.DataSource = newValue;
        }
        public void AddComboBoxValue(object[] newValue)
        {
            if (!IsComboSet())
            {
                combo = new ComboBox();
            }
            this.combo.Items.AddRange(newValue);
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
