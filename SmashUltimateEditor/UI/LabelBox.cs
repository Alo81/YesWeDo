using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace YesweDo.UI
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

        public void SetLabel(string name, Point pos, string tooltipText = null)
        {
            SetLabelValue(name);
            label.Width = Defs.LABEL_WIDTH;
            label.Height = Defs.LABEL_HEIGHT;
            label.Location = pos;

            if(! (tooltipText == null))
            {
                var tip = new ToolTip();
                tip.SetToolTip(label, tooltipText);
                label.ForeColor = Defs.labelTooltipTextColor;
            }
        }
        public void SetTextBox(string name, Point pos, string value = "")
        {
            SetTextBoxValue(value);
            text.Name = name;
            text.Width = Defs.BOX_WIDTH;
            // text.Height = Defs.BOX_HEIGHT; // Height cannot be changed for *Windows Reasons*/
            text.Location = pos;
            //text.TextChanged += DataTbls;
        }
        public void SetComboBox(string name, List<string> opts, Point pos, string value = "")
        {
            SetComboBoxDataSource(opts);
            if (!String.IsNullOrEmpty(value))
            {
                SetComboBoxValue(value);
            }
            combo.Name = name;
            combo.Width = Defs.BOX_WIDTH;
            // combo.Height = Defs.BOX_HEIGHT;  // Height cannot be changed for *Windows Reasons*/
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
            combo.DataSource = newValue?.Where(x => x != null)?.ToList();
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
