﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FITSFormatter.PFSS
{
    class PFSSPoint
    {
        public const double SunRadius = 6.957e8;
        private double l0;
        private double b0;

        internal float x { get; set; }
        internal float y { get; set; }
        internal float z { get; set; }

        internal float origX { get; set; }
        internal float origY { get; set; }
        internal float origZ { get; set; }

        internal short rawR { get; set; }
        internal short rawPhi { get; set; }
        internal short rawTheta { get; set; }

        public PFSSPoint(short rawR, short rawPhi, short rawTheta, double l0, double b0)
        {
            this.l0 = l0;
            this.b0 = b0;

            this.rawR = rawR;
            this.rawPhi = rawPhi;
            this.rawTheta = rawTheta;

            double r = rawR / 8192.0 ;
            r = r < 1 ? SunRadius : r * SunRadius;
            double phi = rawPhi / 32768.0 * 2 * Math.PI;
            double theta = rawTheta / 32768.0 * 2 * Math.PI;

            //current point
            phi -= l0 / 180.0 * Math.PI;
            theta += b0 / 180.0 * Math.PI;
            z = (float)(r * Math.Sin(theta) * Math.Cos(phi));
            x = (float)(r * Math.Sin(theta) * Math.Sin(phi));
            y = (float)(r * Math.Cos(theta));
            this.origX = x;
            this.origY = y;
            this.origZ = z;
        }

        public PFSSPoint(short rawR, short rawPhi, short rawTheta)
        {
            this.rawR = rawR;
            this.rawPhi = rawPhi;
            this.rawTheta = rawTheta;
        }

        public PFSSPoint(PFSSPoint p)
        {
            this.rawPhi = p.rawPhi;
            this.rawR = p.rawR;
            this.rawTheta = p.rawTheta;
            this.x = p.x;
            this.y = p.y;
            this.z = p.z;
            this.origX = p.origX;
            this.origY = p.origY;
            this.origZ = p.origZ;
        }

        public PFSSPoint(float rawR, float rawPhi, float rawTheta, double l0, double b0)
        {
            double r = rawR;
            r = r < 1 ? SunRadius : r * SunRadius;
            double phi = rawPhi * 2 * Math.PI;
            double theta = rawTheta * 2 * Math.PI;

            //current point
            phi -= l0 / 180.0 * Math.PI;
            theta += b0 / 180.0 * Math.PI;
            z = (float)(r * Math.Sin(theta) * Math.Cos(phi));
            x = (float)(r * Math.Sin(theta) * Math.Sin(phi));
            y = (float)(r * Math.Cos(theta));
            this.origX = x;
            this.origY = y;
            this.origZ = z;
        }

        public double AngleTo(PFSSPoint next,PFSSPoint before)
        {
            return calculateAngleBetween2Vecotrs(next.x - x,
                                                    next.y - y, next.z - z, x - before.x, y - before.y, z
                                                                                 - before.z);
        }

        public void ConvertSphericalToXYZ()
        {

            double r = this.x / 8192.0 * SunRadius;
            double phi = this.y / 32768.0 * 2 * Math.PI;
            double theta = this.z / 32768.0 * 2 * Math.PI;

            //current point
            phi -= l0 / 180.0 * Math.PI;
            theta += b0 / 180.0 * Math.PI;
            z = (float)(r * Math.Sin(theta) * Math.Cos(phi));
            x = (float)(r * Math.Sin(theta) * Math.Sin(phi));
            y = (float)(r * Math.Cos(theta));
        }

        private static double calculateAngleBetween2Vecotrs(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            return (x1 * x2 + y1 * y2 + z1 * z2)
                                         / (Math.Sqrt(x1 * x1 + y1 * y1 + z1 * z1) * Math.Sqrt(x2 * x2
                                                                     + y2 * y2 + z2 * z2));
        }
    }
}
