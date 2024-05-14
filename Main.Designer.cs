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
            buttonSupport = new Button();
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
            appStatusLabel.Location = new Point(12, 9);
            appStatusLabel.Name = "appStatusLabel";
            appStatusLabel.Size = new Size(203, 25);
            appStatusLabel.TabIndex = 7;
            appStatusLabel.Text = "Minimizing into taskbar!";
            // 
            // openConfig
            // 
            openConfig.Location = new Point(13, 70);
            openConfig.Name = "openConfig";
            openConfig.Size = new Size(163, 34);
            openConfig.TabIndex = 8;
            openConfig.Text = "Open config path";
            openConfig.UseVisualStyleBackColor = true;
            openConfig.Click += openConfig_Click;
            // 
            // keyReference
            // 
            keyReference.AutoSize = true;
            keyReference.Location = new Point(276, 34);
            keyReference.Name = "keyReference";
            keyReference.Size = new Size(125, 25);
            keyReference.TabIndex = 10;
            keyReference.TabStop = true;
            keyReference.Text = "Keys reference";
            keyReference.LinkClicked += keyReference_LinkClicked;
            // 
            // gitHub
            // 
            gitHub.AutoSize = true;
            gitHub.Location = new Point(333, 9);
            gitHub.Name = "gitHub";
            gitHub.Size = new Size(68, 25);
            gitHub.TabIndex = 11;
            gitHub.TabStop = true;
            gitHub.Text = "GitHub";
            gitHub.LinkClicked += gitHub_LinkClicked;
            // 
            // buttonSupport
            // 
            buttonSupport.BackColor = Color.Gold;
            buttonSupport.Location = new Point(215, 70);
            buttonSupport.Name = "buttonSupport";
            buttonSupport.Size = new Size(186, 34);
            buttonSupport.TabIndex = 13;
            buttonSupport.Text = "Support this project!";
            buttonSupport.UseVisualStyleBackColor = false;
            buttonSupport.Click += buttonSupport_Click;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(413, 117);
            Controls.Add(buttonSupport);
            Controls.Add(gitHub);
            Controls.Add(keyReference);
            Controls.Add(openConfig);
            Controls.Add(appStatusLabel);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            Name = "Main";
            Text = "Neodymium v0.3.0";
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
        private Button buttonSupport;
    }
}
