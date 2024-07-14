using System.Runtime.InteropServices;
using System.Text;

namespace Neodymium
{
    partial class Main
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            notifyIcon1 = new NotifyIcon(components);
            appStatusLabel = new Label();
            openConfig = new Button();
            keyReference = new LinkLabel();
            gitHub = new LinkLabel();
            SuspendLayout();
            // 
            // notifyIcon1
            // 
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "Open Neodymium";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // appStatusLabel
            // 
            appStatusLabel.AutoSize = true;
            appStatusLabel.Location = new Point(11, 9);
            appStatusLabel.Margin = new Padding(2, 0, 2, 0);
            appStatusLabel.Name = "appStatusLabel";
            appStatusLabel.Size = new Size(135, 15);
            appStatusLabel.TabIndex = 7;
            appStatusLabel.Text = "Minimizing into taskbar!";
            // 
            // openConfig
            // 
            openConfig.Location = new Point(11, 53);
            openConfig.Margin = new Padding(2);
            openConfig.Name = "openConfig";
            openConfig.Size = new Size(128, 28);
            openConfig.TabIndex = 8;
            openConfig.Text = "Open config path";
            openConfig.UseVisualStyleBackColor = true;
            openConfig.Click += openConfig_Click;
            // 
            // keyReference
            // 
            keyReference.AutoSize = true;
            keyReference.Location = new Point(194, 66);
            keyReference.Margin = new Padding(2, 0, 2, 0);
            keyReference.Name = "keyReference";
            keyReference.Size = new Size(83, 15);
            keyReference.TabIndex = 10;
            keyReference.TabStop = true;
            keyReference.Text = "Keys reference";
            keyReference.LinkClicked += keyReference_LinkClicked;
            // 
            // gitHub
            // 
            gitHub.AutoSize = true;
            gitHub.Location = new Point(232, 51);
            gitHub.Margin = new Padding(2, 0, 2, 0);
            gitHub.Name = "gitHub";
            gitHub.Size = new Size(45, 15);
            gitHub.TabIndex = 11;
            gitHub.TabStop = true;
            gitHub.Text = "GitHub";
            gitHub.LinkClicked += gitHub_LinkClicked;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(288, 92);
            Controls.Add(gitHub);
            Controls.Add(keyReference);
            Controls.Add(openConfig);
            Controls.Add(appStatusLabel);
            ForeColor = SystemColors.ControlText;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Main";
            Text = "Neodymium v0.3.2";
            Load += Main_Load;
            Resize += Form1_Resize;
            ResumeLayout(false);
            PerformLayout();
        }



        #endregion

        private NotifyIcon notifyIcon1;
        private Label appStatusLabel;
        private Button openConfig;
        private LinkLabel keyReference;
        private LinkLabel gitHub;
        private LinkLabel supportProjectLink;
    }
}
