using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPlot
{
	public class Utility
	{
		/// <summary>
		/// グラフ表示に最適な軸レンジを求める．
		/// ただし，軸の最小値が0のグラフに限定する．
		/// </summary>
		/// <param name="x">軸の最大値</param>
		/// <returns>軸レンジ</returns>
		public static double OptimizeRange(double x)
		{
			string raw_value = x.ToString("e4");
			string[] div_value = raw_value.Split('e');
			double fractional_part = double.Parse(div_value[0]);
			int exponent_part = int.Parse(div_value[1]);

			if ((1.0 <= fractional_part) && (fractional_part < 1.5))
			{
				fractional_part = 1.5;
			}
			else if ((1.5 <= fractional_part) && (fractional_part < 2.0))
			{
				fractional_part = 2.0;
			}
			else if ((2.0 <= fractional_part) && (fractional_part < 3.0))
			{
				fractional_part = 3.0;
			}
			else if ((3.0 <= fractional_part) && (fractional_part < 5.0))
			{
				fractional_part = 5.0;
			}
			else if ((5.0 <= fractional_part) && (fractional_part < 8.0))
			{
				fractional_part = 8.0;
			}
			else if ((8.0 <= fractional_part) && (fractional_part < 10))
			{
				fractional_part = 1.0;
				exponent_part++;
			}
			else
			{

			}
			return fractional_part * Math.Pow(10, exponent_part);
		}
	}
}
