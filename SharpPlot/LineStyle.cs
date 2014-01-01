using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPlot
{
	public class LineStyle
	{
		public LineStyle()
		{

		}
		public LineStyle(int lt, double width, Color color,int pointtype,double pointsize)
		{
			LineType = lt;
			Width = width;
			Color = color;
			PointType = pointtype;
			PointSize = pointsize;
		}
		public int LineType { get; set; }
		public double Width { get; set; }
		public Color Color { get; set; }
		public int PointType { get; set; }
		public double PointSize { get; set; }
	}
}
