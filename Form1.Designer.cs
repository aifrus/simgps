namespace Aifrus.SimGPS
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.textBox_latitude = new System.Windows.Forms.TextBox();
            this.textBox_longitude = new System.Windows.Forms.TextBox();
            this.textBox_course = new System.Windows.Forms.TextBox();
            this.label_longitude = new System.Windows.Forms.Label();
            this.label_course = new System.Windows.Forms.Label();
            this.label_status = new System.Windows.Forms.Label();
            this.label_altitude = new System.Windows.Forms.Label();
            this.textBox_altitude = new System.Windows.Forms.TextBox();
            this.button_Connect = new System.Windows.Forms.Button();
            this.button_Disconnect = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label_latitude = new System.Windows.Forms.Label();
            this.textBox_NMEASentences = new System.Windows.Forms.TextBox();
            this.label_aircraft = new System.Windows.Forms.Label();
            this.label_utc = new System.Windows.Forms.Label();
            this.textBox_utcTime = new System.Windows.Forms.TextBox();
            this.textBox_speed = new System.Windows.Forms.TextBox();
            this.label_speed = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox_latitude
            // 
            this.textBox_latitude.Location = new System.Drawing.Point(63, 62);
            this.textBox_latitude.Name = "textBox_latitude";
            this.textBox_latitude.ReadOnly = true;
            this.textBox_latitude.Size = new System.Drawing.Size(140, 20);
            this.textBox_latitude.TabIndex = 0;
            // 
            // textBox_longitude
            // 
            this.textBox_longitude.Location = new System.Drawing.Point(63, 88);
            this.textBox_longitude.Name = "textBox_longitude";
            this.textBox_longitude.ReadOnly = true;
            this.textBox_longitude.Size = new System.Drawing.Size(140, 20);
            this.textBox_longitude.TabIndex = 1;
            // 
            // textBox_course
            // 
            this.textBox_course.Location = new System.Drawing.Point(260, 62);
            this.textBox_course.Name = "textBox_course";
            this.textBox_course.ReadOnly = true;
            this.textBox_course.Size = new System.Drawing.Size(140, 20);
            this.textBox_course.TabIndex = 2;
            // 
            // label_longitude
            // 
            this.label_longitude.AutoSize = true;
            this.label_longitude.Location = new System.Drawing.Point(7, 91);
            this.label_longitude.Name = "label_longitude";
            this.label_longitude.Size = new System.Drawing.Size(50, 13);
            this.label_longitude.TabIndex = 4;
            this.label_longitude.Text = "longitude";
            // 
            // label_course
            // 
            this.label_course.AutoSize = true;
            this.label_course.Location = new System.Drawing.Point(215, 65);
            this.label_course.Name = "label_course";
            this.label_course.Size = new System.Drawing.Size(39, 13);
            this.label_course.TabIndex = 5;
            this.label_course.Text = "course";
            // 
            // label_status
            // 
            this.label_status.AutoSize = true;
            this.label_status.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_status.ForeColor = System.Drawing.Color.Red;
            this.label_status.Location = new System.Drawing.Point(12, 31);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(129, 13);
            this.label_status.TabIndex = 6;
            this.label_status.Text = "Not Connected to sim";
            // 
            // label_altitude
            // 
            this.label_altitude.AutoSize = true;
            this.label_altitude.Location = new System.Drawing.Point(213, 91);
            this.label_altitude.Name = "label_altitude";
            this.label_altitude.Size = new System.Drawing.Size(41, 13);
            this.label_altitude.TabIndex = 7;
            this.label_altitude.Text = "altitude";
            // 
            // textBox_altitude
            // 
            this.textBox_altitude.Location = new System.Drawing.Point(260, 88);
            this.textBox_altitude.Name = "textBox_altitude";
            this.textBox_altitude.ReadOnly = true;
            this.textBox_altitude.Size = new System.Drawing.Size(140, 20);
            this.textBox_altitude.TabIndex = 8;
            // 
            // button_Connect
            // 
            this.button_Connect.Location = new System.Drawing.Point(11, 5);
            this.button_Connect.Name = "button_Connect";
            this.button_Connect.Size = new System.Drawing.Size(75, 23);
            this.button_Connect.TabIndex = 9;
            this.button_Connect.Text = "Connect";
            this.button_Connect.UseVisualStyleBackColor = true;
            this.button_Connect.Click += new System.EventHandler(this.button_Connect_Click);
            // 
            // button_Disconnect
            // 
            this.button_Disconnect.Location = new System.Drawing.Point(92, 5);
            this.button_Disconnect.Name = "button_Disconnect";
            this.button_Disconnect.Size = new System.Drawing.Size(75, 23);
            this.button_Disconnect.TabIndex = 10;
            this.button_Disconnect.Text = "Disconnect";
            this.button_Disconnect.UseVisualStyleBackColor = true;
            this.button_Disconnect.Click += new System.EventHandler(this.button_Disconnect_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick_1);
            // 
            // label_latitude
            // 
            this.label_latitude.AutoSize = true;
            this.label_latitude.Location = new System.Drawing.Point(16, 65);
            this.label_latitude.Name = "label_latitude";
            this.label_latitude.Size = new System.Drawing.Size(41, 13);
            this.label_latitude.TabIndex = 3;
            this.label_latitude.Text = "latitude";
            // 
            // textBox_NMEASentences
            // 
            this.textBox_NMEASentences.Location = new System.Drawing.Point(11, 140);
            this.textBox_NMEASentences.Multiline = true;
            this.textBox_NMEASentences.Name = "textBox_NMEASentences";
            this.textBox_NMEASentences.ReadOnly = true;
            this.textBox_NMEASentences.Size = new System.Drawing.Size(389, 61);
            this.textBox_NMEASentences.TabIndex = 11;
            this.textBox_NMEASentences.WordWrap = false;
            // 
            // label_aircraft
            // 
            this.label_aircraft.AutoSize = true;
            this.label_aircraft.Location = new System.Drawing.Point(173, 10);
            this.label_aircraft.Name = "label_aircraft";
            this.label_aircraft.Size = new System.Drawing.Size(0, 13);
            this.label_aircraft.TabIndex = 12;
            // 
            // label_utc
            // 
            this.label_utc.AutoSize = true;
            this.label_utc.Location = new System.Drawing.Point(35, 117);
            this.label_utc.Name = "label_utc";
            this.label_utc.Size = new System.Drawing.Size(22, 13);
            this.label_utc.TabIndex = 14;
            this.label_utc.Text = "utc";
            // 
            // textBox_utcTime
            // 
            this.textBox_utcTime.Location = new System.Drawing.Point(63, 114);
            this.textBox_utcTime.Name = "textBox_utcTime";
            this.textBox_utcTime.ReadOnly = true;
            this.textBox_utcTime.Size = new System.Drawing.Size(140, 20);
            this.textBox_utcTime.TabIndex = 13;
            // 
            // textBox_speed
            // 
            this.textBox_speed.Location = new System.Drawing.Point(260, 114);
            this.textBox_speed.Name = "textBox_speed";
            this.textBox_speed.ReadOnly = true;
            this.textBox_speed.Size = new System.Drawing.Size(140, 20);
            this.textBox_speed.TabIndex = 16;
            // 
            // label_speed
            // 
            this.label_speed.AutoSize = true;
            this.label_speed.Location = new System.Drawing.Point(218, 117);
            this.label_speed.Name = "label_speed";
            this.label_speed.Size = new System.Drawing.Size(36, 13);
            this.label_speed.TabIndex = 15;
            this.label_speed.Text = "speed";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 210);
            this.Controls.Add(this.textBox_speed);
            this.Controls.Add(this.label_speed);
            this.Controls.Add(this.label_utc);
            this.Controls.Add(this.textBox_utcTime);
            this.Controls.Add(this.label_aircraft);
            this.Controls.Add(this.textBox_NMEASentences);
            this.Controls.Add(this.button_Disconnect);
            this.Controls.Add(this.button_Connect);
            this.Controls.Add(this.textBox_altitude);
            this.Controls.Add(this.label_altitude);
            this.Controls.Add(this.label_status);
            this.Controls.Add(this.label_course);
            this.Controls.Add(this.label_longitude);
            this.Controls.Add(this.label_latitude);
            this.Controls.Add(this.textBox_course);
            this.Controls.Add(this.textBox_longitude);
            this.Controls.Add(this.textBox_latitude);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "SimGPS";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_latitude;
        private System.Windows.Forms.TextBox textBox_longitude;
        private System.Windows.Forms.TextBox textBox_course;
        private System.Windows.Forms.Label label_longitude;
        private System.Windows.Forms.Label label_course;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.Label label_altitude;
        private System.Windows.Forms.TextBox textBox_altitude;
        private System.Windows.Forms.Button button_Connect;
        private System.Windows.Forms.Button button_Disconnect;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label_latitude;
        private System.Windows.Forms.TextBox textBox_NMEASentences;
        private System.Windows.Forms.Label label_aircraft;
        private System.Windows.Forms.Label label_utc;
        private System.Windows.Forms.TextBox textBox_utcTime;
        private System.Windows.Forms.TextBox textBox_speed;
        private System.Windows.Forms.Label label_speed;
    }
}

