using System;
using System.IO.Ports;
using System.Windows.Forms;
using Microsoft.FlightSimulator.SimConnect;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Aifrus.SimGPS
{

    public partial class Form1 : Form
    {
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private readonly NMEAEncoder nmeaEncoder = new NMEAEncoder();
        private readonly string[] ports = SerialPort.GetPortNames();
        private SerialPort serialPort = new SerialPort();

        public Form1()
        {
            response = 1;
            output = "\n\n\n\n\n\n\n\n\n\n";
            InitializeComponent();

            this.MouseDown += new MouseEventHandler(Form1_MouseDown);
            this.MouseMove += new MouseEventHandler(Form1_MouseMove);
            this.MouseUp += new MouseEventHandler(Form1_MouseUp);
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            this.Deactivate += Form1_Deactivated;


            // Restore window location
            this.Location = new Point(Properties.Settings.Default.WindowLocationX, Properties.Settings.Default.WindowLocationY);

            foreach (string port in ports)
            {
                //comboBox_Ports.Items.Add(port);
            }
        }

        private void Form1_Deactivated(object sender, EventArgs e)
        {
            // Move the focus to a different control
            label1.Focus();
        }

        void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging) return;
            Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
            Point newLocation = Point.Add(dragFormPoint, new Size(dif));

            Rectangle combinedBounds = Screen.AllScreens[0].Bounds;
            foreach (Screen screen in Screen.AllScreens)
            {
                combinedBounds = Rectangle.Union(combinedBounds, screen.Bounds);
            }

            if (newLocation.X < combinedBounds.Left) newLocation.X = combinedBounds.Left;
            if (newLocation.Y < combinedBounds.Top) newLocation.Y = combinedBounds.Top;
            if (newLocation.X > combinedBounds.Right - this.Width) newLocation.X = combinedBounds.Right - this.Width;
            if (newLocation.Y > combinedBounds.Bottom - this.Height) newLocation.Y = combinedBounds.Bottom - this.Height;

            this.Location = newLocation;
        }



        void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;

            // Save window location
            Properties.Settings.Default.WindowLocationX = this.Location.X;
            Properties.Settings.Default.WindowLocationY = this.Location.Y;
            Properties.Settings.Default.Save();
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save window location
            Properties.Settings.Default.WindowLocationX = this.Location.X;
            Properties.Settings.Default.WindowLocationY = this.Location.Y;
            Properties.Settings.Default.Save();
        }

        private void Button_Connect_Click(object sender, EventArgs e)
        {
            if (my_simconnect == null)
            {
                try
                {
                    //comboBox_Ports.Enabled = false;
                    //serialPort.PortName = comboBox_Ports.SelectedItem.ToString();
                    //serialPort.Open();
                    my_simconnect = new Microsoft.FlightSimulator.SimConnect.SimConnect("Managed Data Request", base.Handle, 0x402, null, 0);
                    initDataRequest();
                    timer1.Enabled = true;
                }
                catch (COMException)
                {
                    label_status.Text = "Unable to connect to sim";
                }
            }
            else
            {
                label_status.Text = "Error - try again";
                closeConnection();
                timer1.Enabled = false;
            }
        }

        private void Button_Disconnect_Click(object sender, EventArgs e)
        {
            closeConnection();
            timer1.Enabled = false;
            /*            textBox_latitude.Text = "";
                        textBox_longitude.Text = "";
                        textBox_course.Text = "";
                        textBox_altitude.Text = "";
                        if (serialPort.IsOpen)
                        {
                            serialPort.Close();
                        }
                        comboBox_Ports.Enabled = true;
            */
        }

        private void closeConnection()
        {
            if (my_simconnect != null)
            {
                my_simconnect.Dispose();
                my_simconnect = null;
                label_status.Text = "Connection closed";
            }
        }

        protected override void DefWndProc(ref Message m)
        {
            if (m.Msg != 0x402)
            {
                base.DefWndProc(ref m);
                return;
            }

            my_simconnect?.ReceiveMessage();
        }


        private void displayText(string s)
        {
            output = output.Substring(output.IndexOf("\n") + 1);
            object obj1 = output;
            output = string.Concat(new object[] { obj1, "\n", response++, ": ", s });
            label_status.Text = output;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            closeConnection();
            timer1.Enabled = false;
        }

        private void initDataRequest()
        {
            try
            {
                my_simconnect.OnRecvOpen += new SimConnect.RecvOpenEventHandler(simconnect_OnRecvOpen);
                my_simconnect.OnRecvQuit += new SimConnect.RecvQuitEventHandler(simconnect_OnRecvQuit);
                my_simconnect.OnRecvException += new SimConnect.RecvExceptionEventHandler(simconnect_OnRecvException);
                my_simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Plane Latitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                my_simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Plane Longitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                my_simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Plane Heading Degrees True", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                my_simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Plane Heading Degrees Magnetic", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                my_simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Plane Altitude", "meters", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                my_simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Ground Velocity", "knots", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                my_simconnect.RegisterDataDefineStruct<Struct1>(DEFINITIONS.Struct1);
                my_simconnect.OnRecvSimobjectDataBytype += new SimConnect.RecvSimobjectDataBytypeEventHandler(simconnect_OnRecvSimobjectDataBytype);
            }
            catch (COMException exception1)
            {
                displayText(exception1.Message);
            }
        }

        private void simconnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
        {
            label_status.Text = "Exception received: " + ((uint)data.dwException);
        }

        private void simconnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            label_status.Text = "CONNECTED TO SIMULATOR";
        }

        private void simconnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            label_status.Text = "SIMULATOR EXITED";
            timer1.Enabled = false;
            closeConnection();
            /*            textBox_altitude.Text = "";
                        textBox_course.Text = "";
                        textBox_latitude.Text = "";
                        textBox_longitude.Text = "";
                        textBox_speed.Text = "";
                        textBox_utcTime.Text = "";
                        textBox_NMEASentences.Text = "";*/
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
            //comboBox_Ports.Enabled = true;
        }

        private void simconnect_OnRecvSimobjectDataBytype(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {
            if (data.dwRequestID == 0)
            {
                Struct1 struct1 = (Struct1)data.dwData[0];
                label_latitude.Text = FormatLatitudeforStatus(struct1.latitude);
                label_longitude.Text = FormatLongitudeforStatus(struct1.longitude);
                label_altitude.Text = FormatAltitudeforStatus(struct1.altitude);
                /*                textBox_latitude.Text = struct1.latitude.ToString();
                                textBox_longitude.Text = struct1.longitude.ToString();
                                textBox_course.Text = struct1.trueCourse.ToString();
                                textBox_altitude.Text = struct1.altitude.ToString();
                                textBox_utcTime.Text = DateTime.UtcNow.ToString("G");
                                textBox_speed.Text = struct1.groundSpeed.ToString();*/
                nmeaEncoder.SetLatitude(struct1.latitude);
                nmeaEncoder.SetLongitude(struct1.longitude);
                nmeaEncoder.SetAltitude(struct1.altitude);
                nmeaEncoder.SetTrueCourse(struct1.trueCourse);
                nmeaEncoder.SetMagCourse(struct1.magCourse);
                nmeaEncoder.SetSpeed(struct1.groundSpeed);
                nmeaEncoder.SetTime(DateTime.UtcNow);
                nmeaEncoder.SetGeoidalSeparation(-34);
                //textBox_NMEASentences.Text = nmeaEncoder.Encode();
                if (serialPort.IsOpen)
                {
                    //serialPort.WriteLine(textBox_NMEASentences.Text);
                }
            }
            else
            {
                label_status.Text = "Unknown request ID: " + ((uint)data.dwRequestID);
                /*                textBox_latitude.Text = "";
                                textBox_longitude.Text = "";
                                textBox_course.Text = "";
                                textBox_altitude.Text = "";
                                textBox_utcTime.Text = "";
                                textBox_speed.Text = "";
                                textBox_NMEASentences.Text = "";*/
            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            my_simconnect.RequestDataOnSimObjectType(DATA_REQUESTS.REQUEST_1, DEFINITIONS.Struct1, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
        }


        private SimConnect my_simconnect;
        private string output;
        private int response;
        private const int WM_USER_SIMCONNECT = 0x402;


        private enum DATA_REQUESTS
        {
            REQUEST_1
        }

        private enum DEFINITIONS
        {
            Struct1
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct Struct1
        {
            public double latitude;
            public double longitude;
            public double trueCourse;
            public double magCourse;
            public double altitude;
            public double groundSpeed;
        }


        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // exit the application completely
            // clean shutdown / disconnect / close
            timer1.Enabled = false;
            closeConnection();
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(button1, 0, button1.Height);
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Button_Connect_Click(sender, e);
        }

        private string FormatLatitudeforStatus(double Latitude)
        {
            // Format the latitude for fixed width display like N 47°38'08.40"
            string lat = "N";
            if (Latitude < 0)
            {
                lat = "S";
                Latitude = -Latitude;
            }
            int degrees = (int)Latitude;
            Latitude -= degrees;
            Latitude *= 60;
            int minutes = (int)Latitude;
            Latitude -= minutes;
            Latitude *= 60;
            return $"{lat} {degrees:00}°{minutes:00}'{Latitude:00.00}\"";
        }

        private string FormatLongitudeforStatus(double Longitude)
        {
            // Format the longitude for fixed width display like W122°18'00.00"
            // same as above except after the W, if the degree is 3 digits, then there is no space.
            string lon = "E";
            if (Longitude < 0)
            {
                lon = "W";
                Longitude = -Longitude;
            }
            int degrees = (int)Longitude;
            Longitude -= degrees;
            Longitude *= 60;
            int minutes = (int)Longitude;
            Longitude -= minutes;
            Longitude *= 60;
            return $"{lon}{degrees:000}°{minutes:00}'{Longitude:00.00}\"";
        }

        private string FormatAltitudeforStatus(double Altitude)
        {
            // convert meters to feet. round to nearest foot.
            // add a comma for thousands separator
            // add a ' FT' at the end
            int feet = (int)(Altitude * 3.28084 + 0.5);
            return $"{feet:#,##0} FT";
        }
    }
}


