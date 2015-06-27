﻿namespace Reader_UI
{
    partial class Reader
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Reader));
            this.button1 = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.jumpButton = new System.Windows.Forms.Button();
            this.mrAjax = new System.ComponentModel.BackgroundWorker();
            this.goBack = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.autoSave = new System.Windows.Forms.CheckBox();
            this.exitButton = new System.Windows.Forms.Button();
            this.minimizeButton = new System.Windows.Forms.Button();
            this.flashWarning = new System.Windows.Forms.Label();
            this.helpButton = new System.Windows.Forms.Button();
            this.startOverButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.unMaxButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(709, 286);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(117, 31);
            this.button1.TabIndex = 0;
            this.button1.Text = "Open Archiver";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.openArchiver_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDown1.Location = new System.Drawing.Point(12, 301);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1901,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(110, 20);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.Value = new decimal(new int[] {
            1901,
            0,
            0,
            0});
            // 
            // jumpButton
            // 
            this.jumpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.jumpButton.Location = new System.Drawing.Point(12, 270);
            this.jumpButton.Name = "jumpButton";
            this.jumpButton.Size = new System.Drawing.Size(109, 25);
            this.jumpButton.TabIndex = 2;
            this.jumpButton.Text = "Jump";
            this.jumpButton.UseVisualStyleBackColor = true;
            this.jumpButton.Click += new System.EventHandler(this.jumpPage_Click);
            // 
            // mrAjax
            // 
            this.mrAjax.DoWork += new System.ComponentModel.DoWorkEventHandler(this.mrAjax_DoWork);
            // 
            // goBack
            // 
            this.goBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.goBack.Location = new System.Drawing.Point(128, 270);
            this.goBack.Name = "goBack";
            this.goBack.Size = new System.Drawing.Size(109, 25);
            this.goBack.TabIndex = 5;
            this.goBack.Text = "Go Back";
            this.goBack.UseVisualStyleBackColor = true;
            this.goBack.Click += new System.EventHandler(this.goBack_Click);
            // 
            // loadButton
            // 
            this.loadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.loadButton.Location = new System.Drawing.Point(128, 241);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(109, 25);
            this.loadButton.TabIndex = 6;
            this.loadButton.Text = "Load Game";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveButton.Location = new System.Drawing.Point(12, 241);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(110, 25);
            this.saveButton.TabIndex = 7;
            this.saveButton.Text = "Save Game";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // autoSave
            // 
            this.autoSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.autoSave.AutoSize = true;
            this.autoSave.Location = new System.Drawing.Point(144, 302);
            this.autoSave.Name = "autoSave";
            this.autoSave.Size = new System.Drawing.Size(76, 17);
            this.autoSave.TabIndex = 8;
            this.autoSave.Text = "Auto-Save";
            this.autoSave.UseVisualStyleBackColor = true;
            this.autoSave.CheckedChanged += new System.EventHandler(this.autoSave_CheckedChanged);
            // 
            // exitButton
            // 
            this.exitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exitButton.Location = new System.Drawing.Point(799, 11);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(27, 21);
            this.exitButton.TabIndex = 9;
            this.exitButton.Text = "X";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // minimizeButton
            // 
            this.minimizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.minimizeButton.Location = new System.Drawing.Point(733, 11);
            this.minimizeButton.Name = "minimizeButton";
            this.minimizeButton.Size = new System.Drawing.Size(27, 21);
            this.minimizeButton.TabIndex = 10;
            this.minimizeButton.Text = "_";
            this.minimizeButton.UseVisualStyleBackColor = true;
            this.minimizeButton.Click += new System.EventHandler(this.minimizeButton_Click_1);
            // 
            // flashWarning
            // 
            this.flashWarning.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.flashWarning.AutoSize = true;
            this.flashWarning.ForeColor = System.Drawing.Color.Red;
            this.flashWarning.Location = new System.Drawing.Point(12, 194);
            this.flashWarning.Name = "flashWarning";
            this.flashWarning.Size = new System.Drawing.Size(321, 13);
            this.flashWarning.TabIndex = 11;
            this.flashWarning.Text = "PAGE CONTAINS FLASH. KEYBOARD NAVIGATION DISABLED";
            this.flashWarning.Visible = false;
            // 
            // helpButton
            // 
            this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton.Location = new System.Drawing.Point(586, 286);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(117, 31);
            this.helpButton.TabIndex = 12;
            this.helpButton.Text = "Help and About";
            this.helpButton.UseVisualStyleBackColor = true;
            this.helpButton.Click += new System.EventHandler(this.helpButton_Click);
            // 
            // startOverButton
            // 
            this.startOverButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.startOverButton.Location = new System.Drawing.Point(12, 210);
            this.startOverButton.Name = "startOverButton";
            this.startOverButton.Size = new System.Drawing.Size(110, 25);
            this.startOverButton.TabIndex = 13;
            this.startOverButton.Text = "Start Over";
            this.startOverButton.UseVisualStyleBackColor = true;
            this.startOverButton.Click += new System.EventHandler(this.startOverButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Location = new System.Drawing.Point(128, 210);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(109, 25);
            this.deleteButton.TabIndex = 14;
            this.deleteButton.Text = "Delete Game";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // unMaxButton
            // 
            this.unMaxButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.unMaxButton.Location = new System.Drawing.Point(766, 11);
            this.unMaxButton.Name = "unMaxButton";
            this.unMaxButton.Size = new System.Drawing.Size(27, 21);
            this.unMaxButton.TabIndex = 15;
            this.unMaxButton.Text = "M";
            this.unMaxButton.UseVisualStyleBackColor = true;
            this.unMaxButton.Click += new System.EventHandler(this.unMaxButton_Click);
            // 
            // Reader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(838, 329);
            this.Controls.Add(this.unMaxButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.startOverButton);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.flashWarning);
            this.Controls.Add(this.minimizeButton);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.autoSave);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.goBack);
            this.Controls.Add(this.jumpButton);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Reader";
            this.Text = "MS Paint Adventures";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.ComponentModel.BackgroundWorker mrAjax;
        private System.Windows.Forms.Button jumpButton;
        private System.Windows.Forms.Button goBack;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.CheckBox autoSave;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button minimizeButton;
        private System.Windows.Forms.Label flashWarning;
        private System.Windows.Forms.Button helpButton;
        private System.Windows.Forms.Button startOverButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button unMaxButton;

    }
}

