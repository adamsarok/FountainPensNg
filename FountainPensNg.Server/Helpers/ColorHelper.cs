using System.Drawing;

namespace FountainPensNg.Server.Helpers {
    public class ColorHelper {
        public static CIELAB ToCIELAB(string htmlHex) {
            var col = ColorTranslator.FromHtml(htmlHex);
            var xyz = ToXYZ(col);
            return ToCIELAB(xyz);
        }
        public static double GetEuclideanDistance(CIELAB cielab1, CIELAB cielab2) {
            return GetEuclideanDistance(
                cielab1.L, cielab2.L,
                cielab1.A, cielab2.A,
                cielab1.B, cielab2.B
            );
        }

        public static double GetEuclideanDistance(XYZ xyz1, XYZ xyz2) {
            return GetEuclideanDistance(
                xyz1.X, xyz2.X,
                xyz1.Y, xyz2.Y,
                xyz1.Z, xyz2.Z
            );
        }

        public static double GetEuclideanDistance(Color c1, Color c2) {
            return GetEuclideanDistance(
                c1.R, c2.R, c1.G, c2.G, c1.B, c2.B);
        }

        public static double GetEuclideanDistance(double a1, double a2,
            double b1, double b2,
            double c1, double c2) {
            return Math.Sqrt(
                Math.Pow(a1 - a2, 2)
                + Math.Pow(b1 - b2, 2)
                + Math.Pow(c1 - c2, 2)
            );
        }
        public struct CIELAB {
            public double L;
            public double A;
            public double B;
        }

        public struct XYZ {
            public double X;
            public double Y;
            public double Z;
        }

        public static CIELAB ToCIELAB(XYZ xyz) {
            //Reference-X, Y and Z refer to specific illuminants and observers.
            //Common reference values are available below in this same page.

            //Daylight, sRGB, Adobe-RGB - XYZ reference
            //X=94.811	Y=100.000 Z=107.304

            double var_X = xyz.X / 94.811;
            double var_Y = xyz.Y / 100d;
            double var_Z = xyz.Z / 107.304;

            if (var_X > 0.008856) var_X = Math.Pow(var_X, (1d / 3d));
            else var_X = (7.787 * var_X) + (16 / 116);
            if (var_Y > 0.008856) var_Y = Math.Pow(var_Y, (1d / 3d));
            else var_Y = (7.787 * var_Y) + (16 / 116);
            if (var_Z > 0.008856) var_Z = Math.Pow(var_Z, (1d / 3d));
            else var_Z = (7.787 * var_Z) + (16 / 116);

            return new CIELAB() {
                L = (116d * var_Y) - 16,
                A = 500d * (var_X - var_Y),
                B = 200d * (var_Y - var_Z)
            };
        }

        public static XYZ ToXYZ(Color rgb) {
            //sR, sG and sB (Standard RGB) input range = 0 ÷ 255
            //X, Y and Z output refer to a D65/2° standard illuminant.

            var var_R = ((double)rgb.R / 255);
            var var_G = ((double)rgb.G / 255);
            var var_B = ((double)rgb.B / 255);

            if (var_R > 0.04045) var_R = Math.Pow((var_R + 0.055) / 1.055, 2.4);
            else var_R = var_R / 12.92;
            if (var_G > 0.04045) var_G = Math.Pow((var_G + 0.055) / 1.055, 2.4);
            else var_G = var_G / 12.92;
            if (var_B > 0.04045) var_B = Math.Pow((var_B + 0.055) / 1.055, 2.4);
            else var_B = var_B / 12.92;

            var_R = var_R * 100;
            var_G = var_G * 100;
            var_B = var_B * 100;

            return new XYZ() {
                X = var_R * 0.4124 + var_G * 0.3576 + var_B * 0.1805,
                Y = var_R * 0.2126 + var_G * 0.7152 + var_B * 0.0722,
                Z = var_R * 0.0193 + var_G * 0.1192 + var_B * 0.9505
            };
        }
    }
}
