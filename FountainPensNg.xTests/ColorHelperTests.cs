using FountainPensNg.Server.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FountainPensNg.Server.Helpers.ColorHelper;

namespace FountainPensNg.xTests {
	public class ColorHelperTests {
		[Fact]
		public void ToCieLab() {
			var cielab = ColorHelper.ToCIELAB("#73D13E");
			Assert.Equal(-52, Math.Truncate(cielab.A));
			Assert.Equal(60, Math.Truncate(cielab.B));
			Assert.Equal(75, Math.Truncate(cielab.L));
		}
		[Fact]
		public void ToCieLch() {
			var cielch = ColorHelper.ToCieLch(new ColorHelper.CIELAB() { A = 1, B = 15, L = 50 });
			Assert.Equal(50, Math.Truncate(cielch.L));
			Assert.Equal(15, Math.Truncate(cielch.C));
			Assert.Equal(86, Math.Truncate(cielch.H));
		}
		[Fact]
		public void GetEuclideanDistance() {
			var cielab = new ColorHelper.CIELAB() { A = 1, B = 15, L = 50 };
			Assert.Equal(0, ColorHelper.GetEuclideanDistance(cielab, cielab));
		}

		[Fact]
		public void GetEuclideanDistanceToReference() {
			var cielab = new ColorHelper.CIELAB() { A = 0, B = 0, L = 0 };
			Assert.Equal(0, ColorHelper.GetEuclideanDistanceToReference(cielab));
		}
	}
}
