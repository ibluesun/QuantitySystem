namespace QuantitySimpleTest
{
    partial class TestingTorqueForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.numVelocity = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numDensity = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numDiameter = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numViscosity = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.numLength = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblReynolds = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblHead = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.numAngle = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.numTime = new System.Windows.Forms.NumericUpDown();
            this.button2 = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.lblAngularSpeed = new System.Windows.Forms.Label();
            this.lblPower = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.numForce = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.numRL = new System.Windows.Forms.NumericUpDown();
            this.lblTorque = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.lblWork = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numVelocity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDensity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDiameter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numViscosity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numForce)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRL)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 159);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(176, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Calculate Head";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Velocity";
            // 
            // numVelocity
            // 
            this.numVelocity.DecimalPlaces = 3;
            this.numVelocity.Location = new System.Drawing.Point(64, 13);
            this.numVelocity.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numVelocity.Name = "numVelocity";
            this.numVelocity.Size = new System.Drawing.Size(77, 20);
            this.numVelocity.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(147, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "m/s";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Density";
            // 
            // numDensity
            // 
            this.numDensity.DecimalPlaces = 3;
            this.numDensity.Location = new System.Drawing.Point(65, 42);
            this.numDensity.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numDensity.Name = "numDensity";
            this.numDensity.Size = new System.Drawing.Size(77, 20);
            this.numDensity.TabIndex = 5;
            this.numDensity.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(149, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "kg/m^3";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(188, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Diameter";
            // 
            // numDiameter
            // 
            this.numDiameter.DecimalPlaces = 3;
            this.numDiameter.Location = new System.Drawing.Point(240, 81);
            this.numDiameter.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numDiameter.Name = "numDiameter";
            this.numDiameter.Size = new System.Drawing.Size(77, 20);
            this.numDiameter.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(323, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "m";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 118);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Viscosity";
            // 
            // numViscosity
            // 
            this.numViscosity.DecimalPlaces = 3;
            this.numViscosity.Location = new System.Drawing.Point(64, 116);
            this.numViscosity.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numViscosity.Name = "numViscosity";
            this.numViscosity.Size = new System.Drawing.Size(77, 20);
            this.numViscosity.TabIndex = 11;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 81);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Length";
            // 
            // numLength
            // 
            this.numLength.DecimalPlaces = 3;
            this.numLength.Location = new System.Drawing.Point(65, 81);
            this.numLength.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numLength.Name = "numLength";
            this.numLength.Size = new System.Drawing.Size(77, 20);
            this.numLength.TabIndex = 13;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(148, 83);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(15, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "m";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 200);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(151, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Reynolds := rho * v * d / mue";
            // 
            // lblReynolds
            // 
            this.lblReynolds.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblReynolds.Location = new System.Drawing.Point(12, 219);
            this.lblReynolds.Name = "lblReynolds";
            this.lblReynolds.Size = new System.Drawing.Size(252, 23);
            this.lblReynolds.TabIndex = 16;
            this.lblReynolds.Text = "0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 256);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(134, 13);
            this.label11.TabIndex = 17;
            this.label11.Text = "Head := (f*l/d) * v^2/2*g";
            // 
            // lblHead
            // 
            this.lblHead.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblHead.Location = new System.Drawing.Point(12, 272);
            this.lblHead.Name = "lblHead";
            this.lblHead.Size = new System.Drawing.Size(252, 23);
            this.lblHead.TabIndex = 18;
            this.lblHead.Text = "0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(410, 9);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(70, 13);
            this.label12.TabIndex = 19;
            this.label12.Text = "Angle Radian";
            // 
            // numAngle
            // 
            this.numAngle.DecimalPlaces = 3;
            this.numAngle.Location = new System.Drawing.Point(494, 7);
            this.numAngle.Name = "numAngle";
            this.numAngle.Size = new System.Drawing.Size(76, 20);
            this.numAngle.TabIndex = 20;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(413, 282);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(168, 13);
            this.label13.TabIndex = 21;
            this.label13.Text = "Power:= Toruqe * Angular Speed";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(410, 123);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(145, 13);
            this.label14.TabIndex = 22;
            this.label14.Text = "Torque := F * Radius Length";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(410, 40);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(29, 13);
            this.label15.TabIndex = 23;
            this.label15.Text = "Time";
            // 
            // numTime
            // 
            this.numTime.DecimalPlaces = 3;
            this.numTime.Location = new System.Drawing.Point(494, 33);
            this.numTime.Name = "numTime";
            this.numTime.Size = new System.Drawing.Size(76, 20);
            this.numTime.TabIndex = 24;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(413, 217);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(157, 23);
            this.button2.TabIndex = 26;
            this.button2.Text = "Calculate Power";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(413, 243);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(151, 13);
            this.label16.TabIndex = 27;
            this.label16.Text = "Angular Speed:= Angle / Time";
            // 
            // lblAngularSpeed
            // 
            this.lblAngularSpeed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblAngularSpeed.Location = new System.Drawing.Point(413, 260);
            this.lblAngularSpeed.Name = "lblAngularSpeed";
            this.lblAngularSpeed.Size = new System.Drawing.Size(194, 23);
            this.lblAngularSpeed.TabIndex = 28;
            this.lblAngularSpeed.Text = "0";
            // 
            // lblPower
            // 
            this.lblPower.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPower.Location = new System.Drawing.Point(413, 299);
            this.lblPower.Name = "lblPower";
            this.lblPower.Size = new System.Drawing.Size(194, 23);
            this.lblPower.TabIndex = 29;
            this.lblPower.Text = "0";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(413, 67);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(34, 13);
            this.label17.TabIndex = 30;
            this.label17.Text = "Force";
            // 
            // numForce
            // 
            this.numForce.DecimalPlaces = 3;
            this.numForce.Location = new System.Drawing.Point(494, 60);
            this.numForce.Name = "numForce";
            this.numForce.Size = new System.Drawing.Size(76, 20);
            this.numForce.TabIndex = 31;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(413, 89);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(40, 13);
            this.label18.TabIndex = 32;
            this.label18.Text = "Length";
            // 
            // numRL
            // 
            this.numRL.DecimalPlaces = 3;
            this.numRL.Location = new System.Drawing.Point(494, 86);
            this.numRL.Name = "numRL";
            this.numRL.Size = new System.Drawing.Size(76, 20);
            this.numRL.TabIndex = 33;
            // 
            // lblTorque
            // 
            this.lblTorque.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTorque.Location = new System.Drawing.Point(416, 136);
            this.lblTorque.Name = "lblTorque";
            this.lblTorque.Size = new System.Drawing.Size(191, 23);
            this.lblTorque.TabIndex = 34;
            this.lblTorque.Text = "0";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(367, 169);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(240, 13);
            this.label19.TabIndex = 35;
            this.label19.Text = "Work := F * Length ( to illustrate the difference)";
            // 
            // lblWork
            // 
            this.lblWork.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblWork.Location = new System.Drawing.Point(379, 182);
            this.lblWork.Name = "lblWork";
            this.lblWork.Size = new System.Drawing.Size(191, 23);
            this.lblWork.TabIndex = 36;
            // 
            // TestingTorqueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 335);
            this.Controls.Add(this.lblWork);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.lblTorque);
            this.Controls.Add(this.numRL);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.numForce);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.lblPower);
            this.Controls.Add(this.lblAngularSpeed);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.numTime);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.numAngle);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.lblHead);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.lblReynolds);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.numLength);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.numViscosity);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numDiameter);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numDensity);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numVelocity);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "TestingTorqueForm";
            this.Text = "Testing Torque  ( All input units in SI )";
            ((System.ComponentModel.ISupportInitialize)(this.numVelocity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDensity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDiameter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numViscosity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numForce)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRL)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numVelocity;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numDensity;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numDiameter;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numViscosity;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numLength;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblReynolds;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblHead;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown numAngle;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown numTime;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label lblAngularSpeed;
        private System.Windows.Forms.Label lblPower;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown numForce;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.NumericUpDown numRL;
        private System.Windows.Forms.Label lblTorque;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label lblWork;
    }
}