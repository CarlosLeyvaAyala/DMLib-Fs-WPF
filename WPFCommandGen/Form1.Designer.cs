namespace WPFCommandGen
{
    partial class Form1
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
        private void InitializeComponent() {
            label1 = new Label();
            tbFsClassName = new TextBox();
            cbWPFClassType = new ComboBox();
            label2 = new Label();
            tbCommands = new TextBox();
            label3 = new Label();
            btnGen = new Button();
            groupBox1 = new GroupBox();
            tbFsOutput = new TextBox();
            groupBox2 = new GroupBox();
            tbCsOutput = new TextBox();
            groupBox3 = new GroupBox();
            tbWpfOutput = new TextBox();
            btnDeclFromClpb = new Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(83, 20);
            label1.TabIndex = 0;
            label1.Text = "Class name";
            // 
            // tbFsClassName
            // 
            tbFsClassName.Font = new Font("Fira Code", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tbFsClassName.ForeColor = Color.Green;
            tbFsClassName.Location = new Point(12, 32);
            tbFsClassName.Name = "tbFsClassName";
            tbFsClassName.Size = new Size(176, 26);
            tbFsClassName.TabIndex = 1;
            // 
            // cbWPFClassType
            // 
            cbWPFClassType.Font = new Font("Fira Code", 9F);
            cbWPFClassType.FormattingEnabled = true;
            cbWPFClassType.Items.AddRange(new object[] { "Page", "Window" });
            cbWPFClassType.Location = new Point(12, 496);
            cbWPFClassType.Name = "cbWPFClassType";
            cbWPFClassType.Size = new Size(176, 27);
            cbWPFClassType.TabIndex = 4;
            cbWPFClassType.Text = "Window";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 473);
            label2.Name = "label2";
            label2.Size = new Size(139, 20);
            label2.TabIndex = 3;
            label2.Text = "WPF container class";
            // 
            // tbCommands
            // 
            tbCommands.AcceptsReturn = true;
            tbCommands.Font = new Font("Fira Code", 9F);
            tbCommands.Location = new Point(12, 98);
            tbCommands.Multiline = true;
            tbCommands.Name = "tbCommands";
            tbCommands.Size = new Size(176, 358);
            tbCommands.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 75);
            label3.Name = "label3";
            label3.Size = new Size(84, 20);
            label3.TabIndex = 5;
            label3.Text = "Commands";
            // 
            // btnGen
            // 
            btnGen.Location = new Point(12, 542);
            btnGen.Name = "btnGen";
            btnGen.Size = new Size(176, 29);
            btnGen.TabIndex = 6;
            btnGen.Text = "&Generate";
            btnGen.UseVisualStyleBackColor = true;
            btnGen.Click += BtnGen_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tbFsOutput);
            groupBox1.Location = new Point(220, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(517, 323);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            groupBox1.Text = "F#";
            // 
            // tbFsOutput
            // 
            tbFsOutput.AcceptsReturn = true;
            tbFsOutput.AcceptsTab = true;
            tbFsOutput.Font = new Font("Fira Code", 9F);
            tbFsOutput.Location = new Point(16, 26);
            tbFsOutput.Multiline = true;
            tbFsOutput.Name = "tbFsOutput";
            tbFsOutput.ReadOnly = true;
            tbFsOutput.ScrollBars = ScrollBars.Both;
            tbFsOutput.Size = new Size(484, 275);
            tbFsOutput.TabIndex = 0;
            tbFsOutput.WordWrap = false;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(tbCsOutput);
            groupBox2.Location = new Point(220, 341);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(517, 388);
            groupBox2.TabIndex = 8;
            groupBox2.TabStop = false;
            groupBox2.Text = "C#";
            // 
            // tbCsOutput
            // 
            tbCsOutput.AcceptsReturn = true;
            tbCsOutput.AcceptsTab = true;
            tbCsOutput.Font = new Font("Fira Code", 9F);
            tbCsOutput.Location = new Point(16, 26);
            tbCsOutput.Multiline = true;
            tbCsOutput.Name = "tbCsOutput";
            tbCsOutput.ReadOnly = true;
            tbCsOutput.ScrollBars = ScrollBars.Both;
            tbCsOutput.Size = new Size(484, 347);
            tbCsOutput.TabIndex = 0;
            tbCsOutput.WordWrap = false;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(tbWpfOutput);
            groupBox3.Location = new Point(760, 12);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(620, 717);
            groupBox3.TabIndex = 8;
            groupBox3.TabStop = false;
            groupBox3.Text = "WPF";
            // 
            // tbWpfOutput
            // 
            tbWpfOutput.AcceptsReturn = true;
            tbWpfOutput.AcceptsTab = true;
            tbWpfOutput.Font = new Font("Fira Code", 9F);
            tbWpfOutput.Location = new Point(15, 26);
            tbWpfOutput.Multiline = true;
            tbWpfOutput.Name = "tbWpfOutput";
            tbWpfOutput.ReadOnly = true;
            tbWpfOutput.ScrollBars = ScrollBars.Both;
            tbWpfOutput.Size = new Size(586, 676);
            tbWpfOutput.TabIndex = 0;
            tbWpfOutput.WordWrap = false;
            // 
            // btnDeclFromClpb
            // 
            btnDeclFromClpb.Location = new Point(12, 577);
            btnDeclFromClpb.Name = "btnDeclFromClpb";
            btnDeclFromClpb.Size = new Size(176, 29);
            btnDeclFromClpb.TabIndex = 9;
            btnDeclFromClpb.Text = "Get from &clipboard";
            btnDeclFromClpb.UseVisualStyleBackColor = true;
            btnDeclFromClpb.Click += BtnDeclFromClpb_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1380, 772);
            Controls.Add(btnDeclFromClpb);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(btnGen);
            Controls.Add(label3);
            Controls.Add(tbCommands);
            Controls.Add(label2);
            Controls.Add(cbWPFClassType);
            Controls.Add(tbFsClassName);
            Controls.Add(label1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "WPF Command Generator";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox tbFsClassName;
        private ComboBox cbWPFClassType;
        private Label label2;
        private TextBox tbCommands;
        private Label label3;
        private Button btnGen;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private TextBox tbFsOutput;
        private TextBox tbCsOutput;
        private TextBox tbWpfOutput;
        private Button btnDeclFromClpb;
    }
}
