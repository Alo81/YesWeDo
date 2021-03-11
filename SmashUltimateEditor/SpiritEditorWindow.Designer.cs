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
            this.dropdownSpiritData = new System.Windows.Forms.ComboBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabControlData = new System.Windows.Forms.TabControl();
            this.btnAddFighter = new System.Windows.Forms.Button();
            this.btnRandomizeAll = new System.Windows.Forms.Button();
            this.textboxSeed = new System.Windows.Forms.TextBox();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenDbFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.SaveFile = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExportBattleFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.importBattleOverFile = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportBattles = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportBattlesFromFolderFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RandomizeAllTool = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GetParamLabels = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.randomizeProgress = new System.Windows.Forms.ProgressBar();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelSeed = new System.Windows.Forms.Label();
            this.labelInformative = new System.Windows.Forms.Label();
            this.copyFighter = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dropdownSpiritData
            // 
            this.dropdownSpiritData.FormattingEnabled = true;
            this.dropdownSpiritData.Location = new System.Drawing.Point(11, 35);
            this.dropdownSpiritData.Name = "dropdownSpiritData";
            this.dropdownSpiritData.Size = new System.Drawing.Size(457, 28);
            this.dropdownSpiritData.TabIndex = 4;
            this.dropdownSpiritData.SelectedIndexChanged += new System.EventHandler(this.dropdownSpiritData_SelectedIndexChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // tabControlData
            // 
            this.tabControlData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlData.Location = new System.Drawing.Point(11, 109);
            this.tabControlData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControlData.MinimumSize = new System.Drawing.Size(600, 749);
            this.tabControlData.Name = "tabControlData";
            this.tabControlData.SelectedIndex = 0;
            this.tabControlData.Size = new System.Drawing.Size(854, 808);
            this.tabControlData.TabIndex = 5;
            // 
            // btnAddFighter
            // 
            this.btnAddFighter.Location = new System.Drawing.Point(475, 35);
            this.btnAddFighter.Name = "btnAddFighter";
            this.btnAddFighter.Size = new System.Drawing.Size(107, 29);
            this.btnAddFighter.TabIndex = 6;
            this.btnAddFighter.Text = "Add Fighter";
            this.btnAddFighter.UseVisualStyleBackColor = true;
            this.btnAddFighter.Click += new System.EventHandler(this.btnAddFighter_Click);
            // 
            // btnRandomizeAll
            // 
            this.btnRandomizeAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRandomizeAll.Location = new System.Drawing.Point(746, 35);
            this.btnRandomizeAll.Name = "btnRandomizeAll";
            this.btnRandomizeAll.Size = new System.Drawing.Size(121, 29);
            this.btnRandomizeAll.TabIndex = 8;
            this.btnRandomizeAll.Text = "Randomize Page";
            this.btnRandomizeAll.UseVisualStyleBackColor = true;
            this.btnRandomizeAll.Click += new System.EventHandler(this.btnRandomize_Click);
            // 
            // textboxSeed
            // 
            this.textboxSeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxSeed.Location = new System.Drawing.Point(746, 71);
            this.textboxSeed.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textboxSeed.Name = "textboxSeed";
            this.textboxSeed.Size = new System.Drawing.Size(121, 27);
            this.textboxSeed.TabIndex = 9;
            this.textboxSeed.Text = "-1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenDbFile,
            this.toolStripSeparator,
            this.SaveFile,
            this.SaveAsFile,
            this.toolStripSeparator1,
            this.ExportBattleFile,
            this.toolStripSeparator5,
            this.importBattleOverFile,
            this.ImportBattles,
            this.ImportBattlesFromFolderFile,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // OpenDbFile
            // 
            this.OpenDbFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenDbFile.Name = "OpenDbFile";
            this.OpenDbFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.OpenDbFile.Size = new System.Drawing.Size(270, 26);
            this.OpenDbFile.Text = "&Open Db";
            this.OpenDbFile.Click += new System.EventHandler(this.OpenDbFile_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(267, 6);
            // 
            // SaveFile
            // 
            this.SaveFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveFile.Name = "SaveFile";
            this.SaveFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.SaveFile.Size = new System.Drawing.Size(270, 26);
            this.SaveFile.Text = "&Save";
            this.SaveFile.Click += new System.EventHandler(this.SaveFile_Click);
            // 
            // SaveAsFile
            // 
            this.SaveAsFile.Name = "SaveAsFile";
            this.SaveAsFile.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.SaveAsFile.Size = new System.Drawing.Size(270, 26);
            this.SaveAsFile.Text = "Save &As";
            this.SaveAsFile.Click += new System.EventHandler(this.SaveAsFile_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(267, 6);
            // 
            // ExportBattleFile
            // 
            this.ExportBattleFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExportBattleFile.Name = "ExportBattleFile";
            this.ExportBattleFile.Size = new System.Drawing.Size(270, 26);
            this.ExportBattleFile.Text = "&Export";
            this.ExportBattleFile.Click += new System.EventHandler(this.ExportModForRelease_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(267, 6);
            // 
            // importBattleOverFile
            // 
            this.importBattleOverFile.Name = "importBattleOverFile";
            this.importBattleOverFile.Size = new System.Drawing.Size(270, 26);
            this.importBattleOverFile.Text = "Import Battle Over Current";
            this.importBattleOverFile.Click += new System.EventHandler(this.ImportBattleOverFile_Click);
            // 
            // ImportBattles
            // 
            this.ImportBattles.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ImportBattles.Name = "ImportBattles";
            this.ImportBattles.Size = new System.Drawing.Size(270, 26);
            this.ImportBattles.Text = "Import Battle(s)";
            this.ImportBattles.Click += new System.EventHandler(this.ImportBattle_Click);
            // 
            // ImportBattlesFromFolderFile
            // 
            this.ImportBattlesFromFolderFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ImportBattlesFromFolderFile.Name = "ImportBattlesFromFolderFile";
            this.ImportBattlesFromFolderFile.Size = new System.Drawing.Size(270, 26);
            this.ImportBattlesFromFolderFile.Text = "Import Battles From Folder";
            this.ImportBattlesFromFolderFile.Click += new System.EventHandler(this.ImportFolderFile_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(267, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(270, 26);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.CloseApplication_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RandomizeAllTool,
            this.optionsToolStripMenuItem,
            this.GetParamLabels});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(58, 24);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // RandomizeAllTool
            // 
            this.RandomizeAllTool.Name = "RandomizeAllTool";
            this.RandomizeAllTool.Size = new System.Drawing.Size(245, 26);
            this.RandomizeAllTool.Text = "&Randomize All";
            this.RandomizeAllTool.Click += new System.EventHandler(this.RandomizeAllTool_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(245, 26);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // GetParamLabels
            // 
            this.GetParamLabels.Name = "GetParamLabels";
            this.GetParamLabels.Size = new System.Drawing.Size(245, 26);
            this.GetParamLabels.Text = "Get Latest ParamLabels";
            this.GetParamLabels.Click += new System.EventHandler(this.GetParamLabels_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(6, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(882, 30);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // randomizeProgress
            // 
            this.randomizeProgress.Location = new System.Drawing.Point(11, 79);
            this.randomizeProgress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.randomizeProgress.Name = "randomizeProgress";
            this.randomizeProgress.Size = new System.Drawing.Size(418, 23);
            this.randomizeProgress.TabIndex = 10;
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(141, 6);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.pasteToolStripMenuItem.Text = "&Paste";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.cutToolStripMenuItem.Text = "Cu&t";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(141, 6);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.redoToolStripMenuItem.Text = "&Redo";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.undoToolStripMenuItem.Text = "&Undo";
            // 
            // labelSeed
            // 
            this.labelSeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSeed.AutoSize = true;
            this.labelSeed.Location = new System.Drawing.Point(648, 75);
            this.labelSeed.Name = "labelSeed";
            this.labelSeed.Size = new System.Drawing.Size(102, 20);
            this.labelSeed.TabIndex = 11;
            this.labelSeed.Text = "Random Seed";
            // 
            // labelInformative
            // 
            this.labelInformative.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelInformative.AutoSize = true;
            this.labelInformative.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.labelInformative.Location = new System.Drawing.Point(14, 921);
            this.labelInformative.Name = "labelInformative";
            this.labelInformative.Size = new System.Drawing.Size(0, 20);
            this.labelInformative.TabIndex = 14;
            // 
            // copyFighter
            // 
            this.copyFighter.Location = new System.Drawing.Point(475, 73);
            this.copyFighter.Name = "copyFighter";
            this.copyFighter.Size = new System.Drawing.Size(107, 29);
            this.copyFighter.TabIndex = 15;
            this.copyFighter.Text = "Copy Fighter";
            this.copyFighter.UseVisualStyleBackColor = true;
            this.copyFighter.Click += new System.EventHandler(this.btnCopyFighter_Click);
            // 
            // SpiritEditorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 953);
            this.Controls.Add(this.copyFighter);
            this.Controls.Add(this.labelInformative);
            this.Controls.Add(this.labelSeed);
            this.Controls.Add(this.randomizeProgress);
            this.Controls.Add(this.textboxSeed);
            this.Controls.Add(this.btnRandomizeAll);
            this.Controls.Add(this.btnAddFighter);
            this.Controls.Add(this.tabControlData);
            this.Controls.Add(this.dropdownSpiritData);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(749, 947);
            this.Name = "SpiritEditorWindow";
            this.Text = "YesWeDo!";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox dropdownSpiritData;
        DataTbls dataTbls;
        private TabControl tabControlData;
        private Button btnAddFighter;
        private Button btnRandomizeAll;
        private TextBox textboxSeed;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem ImportBattlesFromFolderFile;
        private ToolStripMenuItem OpenDbFileItem;
        private ToolStripSeparator toolStripSeparator;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem ExportBattleFileMenu;
        private ToolStripMenuItem printPreviewToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem RandomizeAllTool;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem OpenDbFile;
        private ToolStripMenuItem SaveFile;
        private ToolStripMenuItem SaveAsFile;
        private ToolStripMenuItem ExportBattleFile;
        private ToolStripMenuItem ImportBattles;
        private ProgressBar randomizeProgress;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem cutToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem redoToolStripMenuItem;
        private ToolStripMenuItem undoToolStripMenuItem;
        private Label labelSeed;
        private Label labelInformative;
        private ToolStripMenuItem importBattleOverFile;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem GetParamLabels;
        private Button copyFighter;
    }
}

