namespace WindowsFormsApplication2
{
    partial class Monitor
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
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Alarm_mode = new System.Windows.Forms.CheckBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.eneble_page2 = new System.Windows.Forms.CheckBox();
            this.Timebox = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.buttontest = new System.Windows.Forms.Button();
            this.countpages = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Check";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(13, 70);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(309, 173);
            this.textBox1.TabIndex = 1;
            // 
            // Alarm_mode
            // 
            this.Alarm_mode.AutoSize = true;
            this.Alarm_mode.Location = new System.Drawing.Point(95, 14);
            this.Alarm_mode.Name = "Alarm_mode";
            this.Alarm_mode.Size = new System.Drawing.Size(81, 17);
            this.Alarm_mode.TabIndex = 2;
            this.Alarm_mode.Text = "Alarm mode";
            this.Alarm_mode.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(219, 12);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(103, 20);
            this.textBox2.TabIndex = 3;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // eneble_page2
            // 
            this.eneble_page2.AutoSize = true;
            this.eneble_page2.Location = new System.Drawing.Point(13, 39);
            this.eneble_page2.Name = "eneble_page2";
            this.eneble_page2.Size = new System.Drawing.Size(60, 17);
            this.eneble_page2.TabIndex = 4;
            this.eneble_page2.Text = "Page 2";
            this.eneble_page2.UseVisualStyleBackColor = true;
            // 
            // Timebox
            // 
            this.Timebox.Location = new System.Drawing.Point(95, 37);
            this.Timebox.Name = "Timebox";
            this.Timebox.Size = new System.Drawing.Size(33, 20);
            this.Timebox.TabIndex = 5;
            this.Timebox.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(251, 36);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(71, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Scan";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // buttontest
            // 
            this.buttontest.Enabled = false;
            this.buttontest.Location = new System.Drawing.Point(138, 36);
            this.buttontest.Name = "buttontest";
            this.buttontest.Size = new System.Drawing.Size(75, 23);
            this.buttontest.TabIndex = 7;
            this.buttontest.Text = "test button";
            this.buttontest.UseVisualStyleBackColor = true;
            this.buttontest.Click += new System.EventHandler(this.buttontest_Click);
            // 
            // countpages
            // 
            this.countpages.Location = new System.Drawing.Point(182, 12);
            this.countpages.Name = "countpages";
            this.countpages.Size = new System.Drawing.Size(31, 20);
            this.countpages.TabIndex = 8;
            this.countpages.Text = "10";
            this.countpages.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Monitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 255);
            this.Controls.Add(this.countpages);
            this.Controls.Add(this.buttontest);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.Timebox);
            this.Controls.Add(this.eneble_page2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.Alarm_mode);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Name = "Monitor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Monitor";
            this.Load += new System.EventHandler(this.Monitor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox Alarm_mode;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.CheckBox eneble_page2;
        private System.Windows.Forms.TextBox Timebox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button buttontest;
        private System.Windows.Forms.TextBox countpages;
    }
}