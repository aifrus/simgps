﻿using System;
using System.Collections.Concurrent;
using System.Drawing;

namespace Aifrus.SimGPS
{
    internal class NMEAEncoder
    {
        public double UTCTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public double TrueCourse { get; set; }
        public double MagCourse { get; set; }
        public double Speed { get; set; }
        public double GeoidalSeparation { get; set; }

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
            DateTime time = DateTime.UtcNow;
            return time.ToString("HHmmss.ff");
        }


        private string FormatDate()
        {
            return DateTime.UtcNow.ToString("ddMMyy");
        }

        private string FormatSpeed()
        {
            return Speed.ToString("0.0");
        }

        private string FormatTrueCourse()
        {
            return TrueCourse.ToString("0.0");
        }

        private string FormatMagVar()
        {
            double MagVar = TrueCourse - MagCourse;
            return $"{MagVar.ToString("0.0")},W";
        }

        private string FormatMagCourse()
        {
            return MagCourse.ToString("0.0");
        }

        private string FormatAltitude()
        {
            return Altitude.ToString("0.0");
        }

        private string FormatGeoidalSeparation()
        {
            return GeoidalSeparation.ToString("0.0");
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
            if (altitude < -1000.0 || altitude > 1000000.0)
                throw new ArgumentOutOfRangeException(nameof(altitude), "Altitude must be between -1000 and 1000000 meters.");
            Altitude = altitude;
        }

        public void SetTrueCourse(double course)
        {
            if (course < 0.0 || course > 360.0)
                throw new ArgumentOutOfRangeException(nameof(course), "Course must be between 0 and 360 degrees.");
            TrueCourse = course;
        }

        public void SetMagCourse(double course)
        {
            if (course < 0.0 || course > 360.0)
                throw new ArgumentOutOfRangeException(nameof(course), "Course must be between 0 and 360 degrees.");
            MagCourse = course;
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
            return EncodeGPGGA() + "\r\n" +
                EncodeGPGLL() + "\r\n" +
                EncodeGPRMC() + "\r\n" +
                EncodeGPVTG();
        }

        public string EncodeGPGGA()
        {
            string data = $"$GPGGA,{FormatTime()},{FormatLatitude()},{FormatLongitude()},8,12,1.0,{FormatAltitude()},M,{FormatGeoidalSeparation()},M,,";
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
            string data = $"$GPRMC,{FormatTime()},A,{FormatLatitude()},{FormatLongitude()},{FormatSpeed()},{FormatTrueCourse()},{FormatDate()},{FormatMagVar()},";
            string checksum = CalculateChecksum(data);
            return data + "*" + checksum;
        }

        public string EncodeGPVTG()
        {
            string data = $"$GPVTG,{FormatTrueCourse()},T,{FormatMagCourse()},M,{FormatSpeed()},N,,K,A";
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
    }
}


