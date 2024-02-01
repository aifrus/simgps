using System;

namespace Aifrus.SimGPS
{
    internal class NMEAEncoder
    {
        public double UTCTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public double Course { get; set; }
        public double Speed { get; set; }
        public double GeoidalSeparation { get; set; }


        public string EncodeGPGGA()
        {
            string data = $"$GPGGA,{FormatTime()},{FormatLatitude()},{FormatLongitude()},1,12,0.1,{FormatAltitude()},M,{FormatGeoidalSeparation()},M,,";
            string checksum = CalculateChecksum(data);
            return data + "*" + checksum;
        }

        public string EncodeGPGLL()
        {
            string data = $"$GPGLL,{FormatLatitude()},{FormatLongitude()},{FormatTime()},A";
            string checksum = CalculateChecksum(data);
            return data + "*" + checksum;
        }

        public string EncodeGPRMC()
        {
            string data = $"$GPRMC,{FormatTime()},A,{FormatLatitude()},{FormatLongitude()},{FormatSpeed()},{FormatCourse()},{FormatDate()},,";
            string checksum = CalculateChecksum(data);
            return data + "*" + checksum;
        }

        public string EncodeGPVTG()
        {
            string data = $"$GPVTG,{FormatCourse()},T,,M,{FormatSpeed()},N,,K,A";
            string checksum = CalculateChecksum(data);
            return data + "*" + checksum;
        }

        private string CalculateChecksum(string sentence)
        {
            int checksum = 0;
            for (int i = 1; i < sentence.Length; i++)
            {
                checksum ^= sentence[i];
            }
            return checksum.ToString("X2");
        }

        private string FormatLatitude()
        {
            char hemisphere = Latitude >= 0 ? 'N' : 'S';
            return FormatCoordinate(Math.Abs(Latitude)) + "," + hemisphere;
        }

        private string FormatLongitude()
        {
            char hemisphere = Longitude >= 0 ? 'E' : 'W';
            return FormatCoordinate(Math.Abs(Longitude)) + "," + hemisphere;
        }
        private string FormatCoordinate(double coordinate)
        {
            int degrees = (int)coordinate;
            double minutes = (coordinate - degrees) * 60;
            return $"{degrees.ToString("00")}{minutes.ToString("00.000")}";
        }

        private string FormatTime()
        {
            TimeSpan timeSpan = TimeSpan.FromHours(UTCTime);
            return timeSpan.ToString("hhmmss") + "." + timeSpan.Milliseconds.ToString("000").Substring(0, 2);
        }

        private string FormatSpeed()
        {
            return Speed.ToString("0.0");
        }

        private string FormatCourse()
        {
            return Course.ToString("0.0");
        }

        private string FormatAltitude()
        {
            return Altitude.ToString("0.0");
        }

        private string FormatGeoidalSeparation()
        {
            return GeoidalSeparation.ToString("0.0");
        }

        private string FormatDate()
        {
            return DateTime.UtcNow.ToString("ddMMyy");
        }

        public void SetLatitude(double latitude)
        {
            if (latitude < -90.0 || latitude > 90.0)
                throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90 degrees.");
            Latitude = latitude;
        }

        public void SetLongitude(double longitude)
        {
            if (longitude < -180.0 || longitude > 180.0)
                throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180 degrees.");
            Longitude = longitude;
        }

        public void SetAltitude(double altitude)
        {
            if (altitude < -1000.0 || altitude > 10000.0)
                throw new ArgumentOutOfRangeException(nameof(altitude), "Altitude must be between -1000 and 10000 meters.");
            Altitude = altitude;
        }

        public void SetCourse(double course)
        {
            if (course < 0.0 || course > 360.0)
                throw new ArgumentOutOfRangeException(nameof(course), "Course must be between 0 and 360 degrees.");
            Course = course;
        }

        public void SetSpeed(double speed)
        {
            if (speed < 0.0 || speed > 10000.0)
                throw new ArgumentOutOfRangeException(nameof(speed), "Speed must be between 0 and 10000 knots.");
            Speed = speed;
        }

        public void SetTime(DateTime time)
        {
            UTCTime = time.ToOADate();
        }

        public void SetGeoidalSeparation(double geoidalSeparation)
        {
            if (geoidalSeparation < -1000.0 || geoidalSeparation > 1000.0)
                throw new ArgumentOutOfRangeException(nameof(geoidalSeparation), "Geoidal separation must be between -1000 and 1000 meters.");
            GeoidalSeparation = geoidalSeparation;
        }

        public string Encode()
        {
            return EncodeGPGGA() + "\r\n" + EncodeGPGLL() + "\r\n" + EncodeGPRMC() + "\r\n" + EncodeGPVTG() + "\r\n";
        }
    }
}


