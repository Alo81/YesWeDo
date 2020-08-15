using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static SmashUltimateEditor.Enums;
using static SmashUltimateEditor.Extensions;

namespace SmashUltimateEditor
{
    partial class SpiritEditorWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dropdownFighterData = new System.Windows.Forms.ComboBox();
            this.labelFighterFighterKind = new System.Windows.Forms.Label();
            this.dropdownFighterFighterKind = new System.Windows.Forms.ComboBox();
            this.labelFighterHp = new System.Windows.Forms.Label();
            this.labelFighterEntryType = new System.Windows.Forms.Label();
            this.textBoxFighterHp = new System.Windows.Forms.TextBox();
            this.dropdownFighterEntryType = new System.Windows.Forms.ComboBox();
            this.dropdownSpiritData = new System.Windows.Forms.ComboBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonLoadData = new System.Windows.Forms.Button();
            this.listBoxFighterSpirit = new System.Windows.Forms.ListBox();
            this.tabControlData = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.menuStrip1.SuspendLayout();
            this.tabControlData.SuspendLayout();
            this.SuspendLayout();
            // 
            // dropdownFighterData
            // 
            this.dropdownFighterData.FormattingEnabled = true;
            this.dropdownFighterData.Location = new System.Drawing.Point(10, 78);
            this.dropdownFighterData.Name = "dropdownFighterData";
            this.dropdownFighterData.Size = new System.Drawing.Size(382, 23);
            this.dropdownFighterData.TabIndex = 0;
            this.dropdownFighterData.SelectedIndexChanged += new System.EventHandler(this.dropdownFighterData_SelectedIndexChanged);
            // 
            // labelFighterFighterKind
            // 
            this.labelFighterFighterKind.AutoSize = true;
            this.labelFighterFighterKind.Location = new System.Drawing.Point(10, 398);
            this.labelFighterFighterKind.Name = "labelFighterFighterKind";
            this.labelFighterFighterKind.Size = new System.Drawing.Size(71, 15);
            this.labelFighterFighterKind.TabIndex = 4;
            this.labelFighterFighterKind.Text = "Fighter Kind";
            // 
            // dropdownFighterFighterKind
            // 
            this.dropdownFighterFighterKind.FormattingEnabled = true;
            this.dropdownFighterFighterKind.Location = new System.Drawing.Point(10, 415);
            this.dropdownFighterFighterKind.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dropdownFighterFighterKind.Name = "dropdownFighterFighterKind";
            this.dropdownFighterFighterKind.Size = new System.Drawing.Size(306, 23);
            this.dropdownFighterFighterKind.TabIndex = 2;
            // 
            // labelFighterHp
            // 
            this.labelFighterHp.AutoSize = true;
            this.labelFighterHp.Location = new System.Drawing.Point(10, 356);
            this.labelFighterHp.Name = "labelFighterHp";
            this.labelFighterHp.Size = new System.Drawing.Size(23, 15);
            this.labelFighterHp.TabIndex = 4;
            this.labelFighterHp.Text = "HP";
            // 
            // labelFighterEntryType
            // 
            this.labelFighterEntryType.AutoSize = true;
            this.labelFighterEntryType.Location = new System.Drawing.Point(10, 315);
            this.labelFighterEntryType.Name = "labelFighterEntryType";
            this.labelFighterEntryType.Size = new System.Drawing.Size(61, 15);
            this.labelFighterEntryType.TabIndex = 4;
            this.labelFighterEntryType.Text = "Entry Type";
            // 
            // textBoxFighterHp
            // 
            this.textBoxFighterHp.Location = new System.Drawing.Point(10, 373);
            this.textBoxFighterHp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxFighterHp.Name = "textBoxFighterHp";
            this.textBoxFighterHp.Size = new System.Drawing.Size(110, 23);
            this.textBoxFighterHp.TabIndex = 3;
            // 
            // dropdownFighterEntryType
            // 
            this.dropdownFighterEntryType.FormattingEnabled = true;
            this.dropdownFighterEntryType.Location = new System.Drawing.Point(10, 333);
            this.dropdownFighterEntryType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dropdownFighterEntryType.Name = "dropdownFighterEntryType";
            this.dropdownFighterEntryType.Size = new System.Drawing.Size(306, 23);
            this.dropdownFighterEntryType.TabIndex = 2;
            // 
            // dropdownSpiritData
            // 
            this.dropdownSpiritData.FormattingEnabled = true;
            this.dropdownSpiritData.Location = new System.Drawing.Point(10, 50);
            this.dropdownSpiritData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dropdownSpiritData.Name = "dropdownSpiritData";
            this.dropdownSpiritData.Size = new System.Drawing.Size(767, 23);
            this.dropdownSpiritData.TabIndex = 4;
            this.dropdownSpiritData.SelectedIndexChanged += new System.EventHandler(this.dropdownSpiritData_SelectedIndexChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileMenu
            // 
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(37, 20);
            this.fileMenu.Text = "File";
            // 
            // buttonLoadData
            // 
            this.buttonLoadData.Location = new System.Drawing.Point(11, 24);
            this.buttonLoadData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonLoadData.Name = "buttonLoadData";
            this.buttonLoadData.Size = new System.Drawing.Size(82, 22);
            this.buttonLoadData.TabIndex = 3;
            this.buttonLoadData.Text = "L O A D";
            this.buttonLoadData.UseVisualStyleBackColor = true;
            this.buttonLoadData.Click += new System.EventHandler(this.buttonLoadData_Click);
            // 
            // listBoxFighterSpirit
            // 
            this.listBoxFighterSpirit.FormattingEnabled = true;
            this.listBoxFighterSpirit.ItemHeight = 15;
            this.listBoxFighterSpirit.Location = new System.Drawing.Point(13, 108);
            this.listBoxFighterSpirit.MultiColumn = true;
            this.listBoxFighterSpirit.Name = "listBoxFighterSpirit";
            this.listBoxFighterSpirit.Size = new System.Drawing.Size(776, 19);
            this.listBoxFighterSpirit.TabIndex = 5;
            this.listBoxFighterSpirit.SelectedIndexChanged += new System.EventHandler(this.listBoxFighterSpirit_SelectedIndexChanged);
            // 
            // tabControlData
            // 
            this.tabControlData.Controls.Add(this.tabPage1);
            this.tabControlData.Controls.Add(this.tabPage2);
            this.tabControlData.Location = new System.Drawing.Point(13, 134);
            this.tabControlData.Name = "tabControlData";
            this.tabControlData.SelectedIndex = 0;
            this.tabControlData.Size = new System.Drawing.Size(776, 178);
            this.tabControlData.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(768, 150);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(768, 150);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            // 
            // SpiritEditorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControlData);
            this.Controls.Add(this.labelFighterFighterKind);
            this.Controls.Add(this.listBoxFighterSpirit);
            this.Controls.Add(this.dropdownFighterFighterKind);
            this.Controls.Add(this.dropdownSpiritData);
            this.Controls.Add(this.labelFighterHp);
            this.Controls.Add(this.buttonLoadData);
            this.Controls.Add(this.labelFighterEntryType);
            this.Controls.Add(this.textBoxFighterHp);
            this.Controls.Add(this.dropdownFighterEntryType);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.dropdownFighterData);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SpiritEditorWindow";
            this.Text = "Smash Spirits Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControlData.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.Button buttonLoadData;
        private System.Windows.Forms.ComboBox dropdownFighterData;
        private System.Windows.Forms.ComboBox dropdownSpiritData;
        DataTbls dataTbls;
        private System.Windows.Forms.Label labelFighterEntryType;
        private System.Windows.Forms.TextBox textBoxFighterHp;
        private System.Windows.Forms.ComboBox dropdownFighterEntryType;
        private System.Windows.Forms.Label labelFighterHp;
        private System.Windows.Forms.Label labelFighterFighterKind;
        private System.Windows.Forms.ComboBox dropdownFighterFighterKind;
        private System.Windows.Forms.ListBox listBoxFighterSpirit;
        private List<TabPage> tabPages = new List<TabPage>(8);
        private System.Windows.Forms.TabPage tabPage1;
        private TabPage tabPage2;
        public System.Windows.Forms.TabControl tabControlData;
    }
}

