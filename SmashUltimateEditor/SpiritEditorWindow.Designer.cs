using System;
using System.Collections.Generic;
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabFighter = new System.Windows.Forms.TabPage();
            this.labelFighterFighterKind = new System.Windows.Forms.Label();
            this.dropdownFighterFighterKind = new System.Windows.Forms.ComboBox();
            this.labelFighterHp = new System.Windows.Forms.Label();
            this.labelFighterEntryType = new System.Windows.Forms.Label();
            this.textBoxFighterHp = new System.Windows.Forms.TextBox();
            this.dropdownFighterEntryType = new System.Windows.Forms.ComboBox();
            this.tabSpirit = new System.Windows.Forms.TabPage();
            this.dropdownSpiritData = new System.Windows.Forms.ComboBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonLoadData = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabFighter.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dropdownFighterData
            // 
            this.dropdownFighterData.FormattingEnabled = true;
            this.dropdownFighterData.Location = new System.Drawing.Point(3, 3);
            this.dropdownFighterData.Name = "dropdownFighterData";
            this.dropdownFighterData.Size = new System.Drawing.Size(767, 23);
            this.dropdownFighterData.TabIndex = 0;
            this.dropdownFighterData.SelectedIndexChanged += new System.EventHandler(this.dropdownFighterData_SelectedIndexChanged_1);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabFighter);
            this.tabControl1.Controls.Add(this.tabSpirit);
            this.tabControl1.Location = new System.Drawing.Point(10, 238);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(779, 176);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabFighter
            // 
            this.tabFighter.Controls.Add(this.labelFighterFighterKind);
            this.tabFighter.Controls.Add(this.dropdownFighterFighterKind);
            this.tabFighter.Controls.Add(this.labelFighterHp);
            this.tabFighter.Controls.Add(this.labelFighterEntryType);
            this.tabFighter.Controls.Add(this.textBoxFighterHp);
            this.tabFighter.Controls.Add(this.dropdownFighterEntryType);
            this.tabFighter.Controls.Add(this.dropdownFighterData);
            this.tabFighter.Location = new System.Drawing.Point(4, 24);
            this.tabFighter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabFighter.Name = "tabFighter";
            this.tabFighter.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabFighter.Size = new System.Drawing.Size(771, 148);
            this.tabFighter.TabIndex = 0;
            this.tabFighter.Text = "Fighter Data";
            this.tabFighter.UseVisualStyleBackColor = true;
            // 
            // labelFighterFighterKind
            // 
            this.labelFighterFighterKind.AutoSize = true;
            this.labelFighterFighterKind.Location = new System.Drawing.Point(4, 106);
            this.labelFighterFighterKind.Name = "labelFighterFighterKind";
            this.labelFighterFighterKind.Size = new System.Drawing.Size(71, 15);
            this.labelFighterFighterKind.TabIndex = 4;
            this.labelFighterFighterKind.Text = "Fighter Kind";
            // 
            // dropdownFighterFighterKind
            // 
            this.dropdownFighterFighterKind.FormattingEnabled = true;
            this.dropdownFighterFighterKind.Location = new System.Drawing.Point(4, 124);
            this.dropdownFighterFighterKind.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dropdownFighterFighterKind.Name = "dropdownFighterFighterKind";
            this.dropdownFighterFighterKind.Size = new System.Drawing.Size(306, 23);
            this.dropdownFighterFighterKind.TabIndex = 2;
            this.dropdownFighterFighterKind.SelectedIndexChanged += new System.EventHandler(this.dropdownFighterFighterKind_SelectedIndexChanged);
            // 
            // labelFighterHp
            // 
            this.labelFighterHp.AutoSize = true;
            this.labelFighterHp.Location = new System.Drawing.Point(4, 67);
            this.labelFighterHp.Name = "labelFighterHp";
            this.labelFighterHp.Size = new System.Drawing.Size(23, 15);
            this.labelFighterHp.TabIndex = 4;
            this.labelFighterHp.Text = "HP";
            // 
            // labelFighterEntryType
            // 
            this.labelFighterEntryType.AutoSize = true;
            this.labelFighterEntryType.Location = new System.Drawing.Point(4, 26);
            this.labelFighterEntryType.Name = "labelFighterEntryType";
            this.labelFighterEntryType.Size = new System.Drawing.Size(61, 15);
            this.labelFighterEntryType.TabIndex = 4;
            this.labelFighterEntryType.Text = "Entry Type";
            // 
            // textBoxFighterHp
            // 
            this.textBoxFighterHp.Location = new System.Drawing.Point(4, 84);
            this.textBoxFighterHp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxFighterHp.Name = "textBoxFighterHp";
            this.textBoxFighterHp.Size = new System.Drawing.Size(110, 23);
            this.textBoxFighterHp.TabIndex = 3;
            // 
            // dropdownFighterEntryType
            // 
            this.dropdownFighterEntryType.FormattingEnabled = true;
            this.dropdownFighterEntryType.Location = new System.Drawing.Point(4, 44);
            this.dropdownFighterEntryType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dropdownFighterEntryType.Name = "dropdownFighterEntryType";
            this.dropdownFighterEntryType.Size = new System.Drawing.Size(306, 23);
            this.dropdownFighterEntryType.TabIndex = 2;
            // 
            // tabSpirit
            // 
            this.tabSpirit.Location = new System.Drawing.Point(4, 24);
            this.tabSpirit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabSpirit.Name = "tabSpirit";
            this.tabSpirit.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabSpirit.Size = new System.Drawing.Size(771, 148);
            this.tabSpirit.TabIndex = 1;
            this.tabSpirit.Text = "Spirit Data";
            this.tabSpirit.UseVisualStyleBackColor = true;
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
            // SpiritEditorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dropdownSpiritData);
            this.Controls.Add(this.buttonLoadData);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SpiritEditorWindow";
            this.Text = "Smash Spirits Editor";
            this.tabControl1.ResumeLayout(false);
            this.tabFighter.ResumeLayout(false);
            this.tabFighter.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabFighter;
        private System.Windows.Forms.TabPage tabSpirit;
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
    }
}

