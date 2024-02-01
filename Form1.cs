using System;
using System.Windows.Forms;
using Microsoft.FlightSimulator.SimConnect;
using System.Runtime.InteropServices;

namespace Aifrus.SimGPS
{

    public partial class Form1 : Form
    {
        private readonly NMEAEncoder nmeaEncoder = new NMEAEncoder();

        public Form1()
        {
            response = 1;
            output = "\n\n\n\n\n\n\n\n\n\n";
            InitializeComponent();
            setButtons(true, false);
        }

        private void button_Connect_Click(object sender, EventArgs e)
        {
            if (my_simconnect == null)
            {
                try
                {
                    my_simconnect = new Microsoft.FlightSimulator.SimConnect.SimConnect("Managed Data Request", base.Handle, 0x402, null, 0);
                    setButtons(false, true);
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
                setButtons(true, false);
                timer1.Enabled = false;
            }
        }

        private void button_Disconnect_Click(object sender, EventArgs e)
        {
            closeConnection();
            setButtons(true, false);
            timer1.Enabled = false;
            textBox_latitude.Text = "";
            textBox_longitude.Text = "";
            textBox_course.Text = "";
            textBox_altitude.Text = "";
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
            if (m.Msg == 0x402)
            {
                if (my_simconnect != null)
                {
                    my_simconnect.ReceiveMessage();
                }
            }
            else
            {
                base.DefWndProc(ref m);
            }
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
                my_simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Title", null, SIMCONNECT_DATATYPE.STRING256, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                my_simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Plane Latitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                my_simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Plane Longitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                my_simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Ground Altitude", "meters", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                my_simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Zulu time", "seconds", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                my_simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Ground Velocity", "knots", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                my_simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Plane Heading Degrees True", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);

                my_simconnect.RegisterDataDefineStruct<Struct1>(DEFINITIONS.Struct1);
                my_simconnect.OnRecvSimobjectDataBytype += new SimConnect.RecvSimobjectDataBytypeEventHandler(simconnect_OnRecvSimobjectDataBytype);
            }
            catch (COMException exception1)
            {
                displayText(exception1.Message);
            }
        }

        private void setButtons(bool bConnect, bool bDisconnect)
        {
            button_Connect.Enabled = bConnect;
            button_Disconnect.Enabled = bDisconnect;
        }

        private void simconnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
        {
            label_status.Text = "Exception received: " + ((uint)data.dwException);
        }

        private void simconnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            label_status.Text = "Connected to sim";
        }

        private void simconnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            label_status.Text = "sim has exited";
            closeConnection();
            timer1.Enabled = false;
        }

        private void simconnect_OnRecvSimobjectDataBytype(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {
            if (data.dwRequestID == 0)
            {
                Struct1 struct1 = (Struct1)data.dwData[0];
                label_aircraft.Text = struct1.title.ToString();
                textBox_latitude.Text = struct1.latitude.ToString();
                textBox_longitude.Text = struct1.longitude.ToString();
                textBox_course.Text = struct1.course.ToString();
                textBox_altitude.Text = struct1.altitude.ToString();
                textBox_utcTime.Text = struct1.utcTime.ToString();
                textBox_speed.Text = struct1.speed.ToString();
                nmeaEncoder.SetLatitude(struct1.latitude);
                nmeaEncoder.SetLongitude(struct1.longitude);
                nmeaEncoder.SetAltitude(struct1.altitude);
                nmeaEncoder.SetCourse(struct1.course);
                nmeaEncoder.SetSpeed(struct1.speed);
                nmeaEncoder.SetTime(DateTime.UtcNow);
                nmeaEncoder.SetGeoidalSeparation(-34);
                textBox_NMEASentences.Text = nmeaEncoder.Encode();
            }
            else
            {
                label_status.Text = "Unknown request ID: " + ((uint)data.dwRequestID);
                textBox_latitude.Text = "";
                textBox_longitude.Text = "";
                textBox_course.Text = "";
                textBox_altitude.Text = "";
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
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x100)]
            public string title;
            public double latitude;
            public double longitude;
            public double altitude;
            public double utcTime;
            public double speed;
            public double course;
        }

    }
}


