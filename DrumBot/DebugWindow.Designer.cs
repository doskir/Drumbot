namespace DrumBot
{
    partial class DebugWindow
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
            this.redTrack1PictureBox = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.thresholdRedNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.thresholdGreenNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.thresholdBlueNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.maximumBlueNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.maximumGreenNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.maximumRedNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.yellowTrack1PictureBox = new System.Windows.Forms.PictureBox();
            this.blueTrack1PictureBox = new System.Windows.Forms.PictureBox();
            this.greenTrack1PictureBox = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.greenTrack2PictureBox = new System.Windows.Forms.PictureBox();
            this.blueTrack2PictureBox = new System.Windows.Forms.PictureBox();
            this.yellowTrack2PictureBox = new System.Windows.Forms.PictureBox();
            this.redTrack2PictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.redTrack1PictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdRedNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdGreenNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdBlueNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumBlueNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumGreenNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumRedNumericUpDown)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.yellowTrack1PictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueTrack1PictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenTrack1PictureBox)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.greenTrack2PictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueTrack2PictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yellowTrack2PictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redTrack2PictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // redTrack1PictureBox
            // 
            this.redTrack1PictureBox.Location = new System.Drawing.Point(2, 2);
            this.redTrack1PictureBox.Name = "redTrack1PictureBox";
            this.redTrack1PictureBox.Size = new System.Drawing.Size(96, 171);
            this.redTrack1PictureBox.TabIndex = 0;
            this.redTrack1PictureBox.TabStop = false;
            this.redTrack1PictureBox.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(517, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "DoStuff";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // thresholdRedNumericUpDown
            // 
            this.thresholdRedNumericUpDown.Location = new System.Drawing.Point(502, 111);
            this.thresholdRedNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.thresholdRedNumericUpDown.Name = "thresholdRedNumericUpDown";
            this.thresholdRedNumericUpDown.Size = new System.Drawing.Size(45, 20);
            this.thresholdRedNumericUpDown.TabIndex = 2;
            this.thresholdRedNumericUpDown.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // thresholdGreenNumericUpDown
            // 
            this.thresholdGreenNumericUpDown.Location = new System.Drawing.Point(553, 111);
            this.thresholdGreenNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.thresholdGreenNumericUpDown.Name = "thresholdGreenNumericUpDown";
            this.thresholdGreenNumericUpDown.Size = new System.Drawing.Size(45, 20);
            this.thresholdGreenNumericUpDown.TabIndex = 3;
            this.thresholdGreenNumericUpDown.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // thresholdBlueNumericUpDown
            // 
            this.thresholdBlueNumericUpDown.Location = new System.Drawing.Point(604, 111);
            this.thresholdBlueNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.thresholdBlueNumericUpDown.Name = "thresholdBlueNumericUpDown";
            this.thresholdBlueNumericUpDown.Size = new System.Drawing.Size(45, 20);
            this.thresholdBlueNumericUpDown.TabIndex = 4;
            this.thresholdBlueNumericUpDown.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(499, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Red";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(550, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Green";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(601, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Blue";
            // 
            // maximumBlueNumericUpDown
            // 
            this.maximumBlueNumericUpDown.Location = new System.Drawing.Point(604, 137);
            this.maximumBlueNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.maximumBlueNumericUpDown.Name = "maximumBlueNumericUpDown";
            this.maximumBlueNumericUpDown.Size = new System.Drawing.Size(45, 20);
            this.maximumBlueNumericUpDown.TabIndex = 10;
            this.maximumBlueNumericUpDown.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // maximumGreenNumericUpDown
            // 
            this.maximumGreenNumericUpDown.Location = new System.Drawing.Point(553, 137);
            this.maximumGreenNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.maximumGreenNumericUpDown.Name = "maximumGreenNumericUpDown";
            this.maximumGreenNumericUpDown.Size = new System.Drawing.Size(45, 20);
            this.maximumGreenNumericUpDown.TabIndex = 9;
            this.maximumGreenNumericUpDown.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // maximumRedNumericUpDown
            // 
            this.maximumRedNumericUpDown.Location = new System.Drawing.Point(502, 137);
            this.maximumRedNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.maximumRedNumericUpDown.Name = "maximumRedNumericUpDown";
            this.maximumRedNumericUpDown.Size = new System.Drawing.Size(45, 20);
            this.maximumRedNumericUpDown.TabIndex = 8;
            this.maximumRedNumericUpDown.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(502, 163);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(147, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "Threshold";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.greenTrack1PictureBox);
            this.panel1.Controls.Add(this.blueTrack1PictureBox);
            this.panel1.Controls.Add(this.yellowTrack1PictureBox);
            this.panel1.Controls.Add(this.redTrack1PictureBox);
            this.panel1.Location = new System.Drawing.Point(13, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(355, 175);
            this.panel1.TabIndex = 13;
            // 
            // yellowTrack1PictureBox
            // 
            this.yellowTrack1PictureBox.Location = new System.Drawing.Point(104, 2);
            this.yellowTrack1PictureBox.Name = "yellowTrack1PictureBox";
            this.yellowTrack1PictureBox.Size = new System.Drawing.Size(69, 171);
            this.yellowTrack1PictureBox.TabIndex = 1;
            this.yellowTrack1PictureBox.TabStop = false;
            // 
            // blueTrack1PictureBox
            // 
            this.blueTrack1PictureBox.Location = new System.Drawing.Point(179, 2);
            this.blueTrack1PictureBox.Name = "blueTrack1PictureBox";
            this.blueTrack1PictureBox.Size = new System.Drawing.Size(69, 171);
            this.blueTrack1PictureBox.TabIndex = 2;
            this.blueTrack1PictureBox.TabStop = false;
            // 
            // greenTrack1PictureBox
            // 
            this.greenTrack1PictureBox.Location = new System.Drawing.Point(254, 2);
            this.greenTrack1PictureBox.Name = "greenTrack1PictureBox";
            this.greenTrack1PictureBox.Size = new System.Drawing.Size(96, 171);
            this.greenTrack1PictureBox.TabIndex = 3;
            this.greenTrack1PictureBox.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.greenTrack2PictureBox);
            this.panel2.Controls.Add(this.blueTrack2PictureBox);
            this.panel2.Controls.Add(this.yellowTrack2PictureBox);
            this.panel2.Controls.Add(this.redTrack2PictureBox);
            this.panel2.Location = new System.Drawing.Point(13, 192);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(355, 175);
            this.panel2.TabIndex = 14;
            // 
            // greenTrack2PictureBox
            // 
            this.greenTrack2PictureBox.Location = new System.Drawing.Point(254, 0);
            this.greenTrack2PictureBox.Name = "greenTrack2PictureBox";
            this.greenTrack2PictureBox.Size = new System.Drawing.Size(96, 171);
            this.greenTrack2PictureBox.TabIndex = 3;
            this.greenTrack2PictureBox.TabStop = false;
            // 
            // blueTrack2PictureBox
            // 
            this.blueTrack2PictureBox.Location = new System.Drawing.Point(179, 0);
            this.blueTrack2PictureBox.Name = "blueTrack2PictureBox";
            this.blueTrack2PictureBox.Size = new System.Drawing.Size(69, 171);
            this.blueTrack2PictureBox.TabIndex = 2;
            this.blueTrack2PictureBox.TabStop = false;
            // 
            // yellowTrack2PictureBox
            // 
            this.yellowTrack2PictureBox.Location = new System.Drawing.Point(104, 1);
            this.yellowTrack2PictureBox.Name = "yellowTrack2PictureBox";
            this.yellowTrack2PictureBox.Size = new System.Drawing.Size(69, 171);
            this.yellowTrack2PictureBox.TabIndex = 1;
            this.yellowTrack2PictureBox.TabStop = false;
            // 
            // redTrack2PictureBox
            // 
            this.redTrack2PictureBox.Location = new System.Drawing.Point(2, 2);
            this.redTrack2PictureBox.Name = "redTrack2PictureBox";
            this.redTrack2PictureBox.Size = new System.Drawing.Size(96, 171);
            this.redTrack2PictureBox.TabIndex = 0;
            this.redTrack2PictureBox.TabStop = false;
            // 
            // DebugWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 615);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.maximumBlueNumericUpDown);
            this.Controls.Add(this.maximumGreenNumericUpDown);
            this.Controls.Add(this.maximumRedNumericUpDown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.thresholdBlueNumericUpDown);
            this.Controls.Add(this.thresholdGreenNumericUpDown);
            this.Controls.Add(this.thresholdRedNumericUpDown);
            this.Controls.Add(this.button1);
            this.Name = "DebugWindow";
            this.Text = "DebugWindow";
            ((System.ComponentModel.ISupportInitialize)(this.redTrack1PictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdRedNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdGreenNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdBlueNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumBlueNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumGreenNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumRedNumericUpDown)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.yellowTrack1PictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueTrack1PictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenTrack1PictureBox)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.greenTrack2PictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueTrack2PictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yellowTrack2PictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.redTrack2PictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox redTrack1PictureBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown thresholdRedNumericUpDown;
        private System.Windows.Forms.NumericUpDown thresholdGreenNumericUpDown;
        private System.Windows.Forms.NumericUpDown thresholdBlueNumericUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown maximumBlueNumericUpDown;
        private System.Windows.Forms.NumericUpDown maximumGreenNumericUpDown;
        private System.Windows.Forms.NumericUpDown maximumRedNumericUpDown;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox greenTrack1PictureBox;
        private System.Windows.Forms.PictureBox blueTrack1PictureBox;
        private System.Windows.Forms.PictureBox yellowTrack1PictureBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox greenTrack2PictureBox;
        private System.Windows.Forms.PictureBox blueTrack2PictureBox;
        private System.Windows.Forms.PictureBox yellowTrack2PictureBox;
        private System.Windows.Forms.PictureBox redTrack2PictureBox;
    }
}