using FountainPensNg.Server.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FountainPensNg.Server.Helpers.ColorHelper;

namespace FountainPensNg.xTests {
	public class ColorHelperTests {
		[Fact]
		public void HexToCieLab() {
			var cielab = ColorHelper.ToCIELAB("#73D13E");
			Assert.Equal(-52, Math.Truncate(cielab.A));
			Assert.Equal(60, Math.Truncate(cielab.B));
			Assert.Equal(75, Math.Truncate(cielab.L));
		}
		[Fact]
		public void XyzToCieLab() {
			ColorHelper.ToCIELAB(new XYZ() { X = 0, Y = 0, Z = 0 });
			var c2 = ColorHelper.ToCIELAB(new XYZ() { X = 95.047, Y = 100, Z = 108.883 });
			ColorHelper.ToCIELAB(new XYZ() { X = -1, Y = -1, Z = -1 });
			Assert.NotEqual(0, c2.A);
		}
		[Fact]
		public void ToCieLch() {
			var cielch = ColorHelper.ToCieLch(new ColorHelper.CIELAB() { A = 1, B = 15, L = 50 });
			Assert.Equal(50, Math.Truncate(cielch.L));
			Assert.Equal(15, Math.Truncate(cielch.C));
			Assert.Equal(86, Math.Truncate(cielch.H));
			ColorHelper.ToCieLch(new ColorHelper.CIELAB() { A = 1, B = -1, L = 50 });
		}
		[Fact]
		public void GetEuclideanDistance() {
			var cielab = new ColorHelper.CIELAB() { A = 1, B = 15, L = 50 };
			Assert.Equal(0, ColorHelper.GetEuclideanDistance(cielab, cielab));
		}
		[Fact]
		public void GetEuclideanDistanceToReferenceCieLch() {
			var cielch = new ColorHelper.CIELCH() { C = 0, H = 0, L = 0 };
			Assert.Equal(0, ColorHelper.GetEuclideanDistanceToReference(cielch));
		}
		[Fact]
		public void GetEuclideanDistanceToReferenceXYZ() {
			var xyz = new XYZ() { X = 0, Y = 0, Z = 0 };
			Assert.Equal(0, ColorHelper.GetEuclideanDistance(xyz, xyz));
		}
		[Fact]
		public void GetEuclideanDistanceToReferenceRGB() {
			var color = Color.AliceBlue;
			Assert.Equal(0, ColorHelper.GetEuclideanDistance(color, color));
			Assert.True(ColorHelper.GetEuclideanDistance(Color.AliceBlue, Color.Red) > 0);
		}
		[Fact]
		public void ToXYZ() {
			var color = Color.FromArgb(255, 10, 20, 30);
			var xyz = ColorHelper.ToXYZ(color);
			Assert.Equal(0.610, Math.Round(xyz.X, 3));
			Assert.Equal(0.659, Math.Round(xyz.Y, 3));
			Assert.Equal(1.323, Math.Round(xyz.Z, 3));
			color = Color.FromArgb(255, 0, 0, 0);
			xyz = ColorHelper.ToXYZ(color);
			Assert.Equal(0, xyz.X);
			Assert.Equal(0, xyz.Y);
			Assert.Equal(0, xyz.Z);
		}
		
	}
}
