using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPlot
{
	/// <summary>
	/// TODO : Startメソッドを呼び出す前にコマンドを設定して，Startメソッド呼び出し時にパイプstream確保→コマンド流し込み→パイプstream開放に変更
	/// </summary>
	public class PlotStream : IDisposable
	{
		static PlotStream()
		{
			DefaultLabelFont = "Times-Roman";

		}

		public PlotStream()
		{
			disposed = false;
			LineStyle = new LineStyle();
		}

		/// <summary>
		/// This destructor will run only if the Dispose method does not get called.
		/// It gives your base class the opportunity to finalize.
		/// Do not provide destructors in types derived from this class.
		/// </summary>
		~PlotStream()
		{
			// Do not re-create Dispose clean-up code here.
			// Calling Dispose(false) is optimal in terms of readability and maintainability.
			Dispose(false);
		}

		#region ForImplimentIDisporsable

		/// <summary>
		/// A derived class should not be able to override this method.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			// This object will be cleaned up by the Dispose method.
			// Therefore, you should call GC.SupressFinalize to take this object off the finalization queue
			// and prevent finalization code for this object from executing a second time.
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// If disposing equals true, the method has been called directly or indirectly by a user's code.
		/// Managed and unmanaged resources can be disposed.
		/// If disposing equals false, the method has been called by the runtime from inside the finalizer and you should not reference other objects.
		/// Only unmanaged resources can be disposed.
		/// </summary>
		/// <param name="disposing"></param>
		private void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if (!this.disposed)
			{
				// If disposing equals true, dispose all managed and unmanaged resources.
				if (disposing)
				{
					// Dispose managed resources.
					if (this.stream != null) { this.stream.Dispose(); }
					this.stream = null;
					if (gnuplotProcess != null) { gnuplotProcess.Dispose(); }
					gnuplotProcess = null;
				}

				// Call the appropriate methods to clean up unmanaged resources here.
				// If disposing is false, only the following code is executed.

				/*
				if (this.m_pUnmanaged != IntPtr.Zero)
					free(this.m_pUnmanaged);
				this.m_pUnmanaged = IntPtr.Zero;
				*/
			}
			disposed = true;
		}

		/// <summary>
		/// Dispose済みの場合ObjectDisposedExceptionをthrow
		/// </summary>
		private void ThrowExceptionIfDisposed()
		{
			if (disposed)
			{
				throw new ObjectDisposedException(this.GetType().ToString());
			}
		}

		// Track whether Dispose has been called.
		private bool disposed = false;

		#endregion


		public StreamWriter Stream
		{
			get
			{
				ThrowExceptionIfDisposed();
				return stream;
			}
			private set { stream = value; }
		}


		public void Start()
		{
			gnuplotProcess = new Process();
			gnuplotProcess.StartInfo.FileName = "pgnuplot.exe";
			gnuplotProcess.StartInfo.UseShellExecute = false;
			gnuplotProcess.StartInfo.RedirectStandardInput = true;
			gnuplotProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
			gnuplotProcess.Start();
			Stream = gnuplotProcess.StandardInput;

			SetDefaultLineStyle();
		}

		private void SetDefaultLineStyle()
		{
			SetLineStyle(1, new LineStyle(1, 2, Color.Red));
			SetLineStyle(2, new LineStyle(1, 2, Color.Green));
			SetLineStyle(3, new LineStyle(1, 2, Color.Blue));
			SetLineStyle(4, new LineStyle(1, 2, Color.Purple));
			SetLineStyle(5, new LineStyle(1, 2, Color.Cyan));
			SetLineStyle(6, new LineStyle(1, 2, Color.Orange));
			SetLineStyle(7, new LineStyle(1, 2, Color.Black));
			SetLineStyle(8, new LineStyle(2, 2, Color.Red));
			SetLineStyle(9, new LineStyle(2, 2, Color.Green));
			SetLineStyle(10, new LineStyle(2, 2, Color.Blue));
		}

		public LineStyle LineStyle { get; set; }



		public class PlotTitle
		{

		}

		public class PlotWith
		{

		}

		/// <summary>
		/// 作業ディレクトリをstr文字列で表されたディレクトリに変更する．
		/// </summary>
		/// <param name="str">ディレクトリ</param>
		public void ChangeDirectory(string str)
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("cd '{0}'", str);
		}

		/// <summary>
		/// プロットに用いる描画スタイルを登録する．
		/// </summary>
		/// <param name="style_num">登録番号</param>
		/// <param name="ls">描画スタイル</param>
		public void SetLineStyle(int style_num, LineStyle ls)
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set style line {0} lt {1} lw {2} lc rgbcolor'{3}'", style_num, ls.LineType, ls.Width, "#" + ls.Color.R + ls.Color.G + ls.Color.B);
		}

		public void SetCircleStyle()
		{
			throw new NotImplementedException();
		}
		public void SetRectangleStyle()
		{
			throw new NotImplementedException();
		}
		public void SetEllipseStyle()
		{
			throw new NotImplementedException();
		}

		public void PlotFromFile(string filename, int x_column, int y_column, PlottingStyle ps, int style_num, PlotAxis axis, string title)
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("plot '{0}.txt' using {1}:{2} with {3} ls {4} axis {5} title \"{6}\"", filename, x_column, y_column, ps, style_num, axis, title);
		}

		public void ReplotFromFile(string filename, int x_column, int y_column, PlottingStyle ps, int style_num, PlotAxis axis, string title)
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("replot '{0}.txt' using {1}:{2} with {3} ls {4} axis {5} title \"{6}\"", filename, x_column, y_column, ps, style_num, axis, title);
		}

		public void SplotFromFile(string filename, int x_column, int y_column, int z_colum, PlottingStyle ps)
		{

			//Stream.WriteLine("set palette rgbformulae 30,31,32");
			//	Stream.WriteLine("set palette defined (-1 \"blue\", 0 \"white\", 1 \"red\")");
			//	Stream.WriteLine("set {0} at b", PlottingStyle.pm3d);

			Stream.WriteLine("set cntrparam levels auto 12");
			Stream.WriteLine("set contour base");

			Stream.WriteLine("set hidden3d");
			Stream.WriteLine("splot '{0}.txt' using {1}:{2}:{3} with {4}", filename, x_column, y_column, z_colum, ps, ps.ToString());
		}

		public void ColorContourFromFile(string filename, int x_column, int y_column, int z_colum)
		{
			Stream.WriteLine("set palette defined (-1 \"blue\", 0 \"white\", 1 \"red\")");
			Stream.WriteLine("set pm3d map");
			Stream.WriteLine("splot '{0}.txt' using {1}:{2}:{3}", filename, x_column, y_column, z_colum);
		}

		public void LineContourFromFile(string filename, int x_column, int y_column, int z_colum)
		{
			Stream.WriteLine("set style increment user");
			Stream.WriteLine("set cntrparam levels auto 10");
			Stream.WriteLine("set contour base");
			Stream.WriteLine("set view 0,0");
			Stream.WriteLine("unset surface");
			//	Stream.WriteLine("set palette defined (-1 \"blue\", 0 \"white\", 1 \"red\")");
			Stream.WriteLine("set pm3d map");
			Stream.WriteLine("splot '{0}.txt' using {1}:{2}:{3}", filename, x_column, y_column, z_colum);
		}

		#region label

		public void SetXLabel(string label, int fontsize)
		{
			ThrowExceptionIfDisposed();
			SetXLabel(label, fontsize, DefaultLabelFont);
		}
		public void SetXLabel(string label, int fontsize, string font)
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set xl \"{0}\" font \"{1},{2}\"", label, font, fontsize);
		}

		public void SetYLabel(string label, int fontsize)
		{
			ThrowExceptionIfDisposed();
			SetYLabel(label, fontsize, DefaultLabelFont);
		}
		public void SetYLabel(string label, int fontsize, string font)
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set yl \"{0}\" font \"{1},{2}\"", label, font, fontsize);
		}

		public void SetY2Label(string label, int fontsize)
		{
			ThrowExceptionIfDisposed();
			SetY2Label(label, fontsize, DefaultLabelFont);
		}
		public void SetY2Label(string label, int fontsize, string font)
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set y2l \"{0}\" font \"{1},{2}\"", label, font, fontsize);
		}

		#endregion

		#region range
		public void SetXRange(double start, double end)
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set xr[{0}:{1}]", start, end);
		}
		
		public void SetYRange(double start, double end)
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set yr[{0}:{1}]", start, end);
		}

		public void SetX2Range(double start, double end)
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set x2r[{0}:{1}]", start, end);
		}

		public void SetY2Range(double start, double end)
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set y2r[{0}:{1}]", start, end);
		}

		#endregion

		#region tics

		public void SetXTics()
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set xtics");
			Stream.WriteLine("set xtics nomirror");
		}
		public void SetXTics(double start, double tic)
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set xtics {0},{1}", start, tic);
			Stream.WriteLine("set xtics nomirror");
		}
		public void SetX2Tics()
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set x2tics");
		}
		public void SetX2Tics(double start, double tic)
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set x2tics {0},{1}", start, tic);
		}
		public void SetYTics()
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set ytics");
		}
		public void SetYTics(double start, double tic)
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set ytics {0},{1}", start, tic);
		}
		public void SetY2Tics()
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set y2tics");
		}
		public void SetY2Tics(double start, double tic)
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set ytics nomirror");
			Stream.WriteLine("set y2tics {0},{1}", start, tic);
		}

		#endregion


		public void SetLegendPosition(params LegendPosition[] position)
		{
			ThrowExceptionIfDisposed();
			if (position.Length == 1)
			{
				Stream.WriteLine("set key {0}", position[0]);
			}
			else if (position.Length == 2)
			{
				Stream.WriteLine("set key {0} {1}", position[0], position[1]);
			}
			else
			{
				throw (new ArgumentException());
			}
		}
		public void SetLegendBox()
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set key box");
		}
		public void EraseLegend()
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("unset key");
		}

		public void SetMargin(double rltb)
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set rmargin {0}", rltb);
			Stream.WriteLine("set lmargin {0}", rltb);
			Stream.WriteLine("set tmargin {0}", rltb);
			Stream.WriteLine("set bmargin {0}", rltb);
		}
		public void SetMargin(double rl, double tb)
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set rmargin {0}", rl);
			Stream.WriteLine("set lmargin {0}", rl);
			Stream.WriteLine("set tmargin {0}", tb);
			Stream.WriteLine("set bmargin {0}", tb);
		}
		public void SetMargin(double r, double l, double t, double b)
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set rmargin {0}", r);
			Stream.WriteLine("set lmargin {0}", l);
			Stream.WriteLine("set tmargin {0}", t);
			Stream.WriteLine("set bmargin {0}", b);
		}

		public void SetGrid()
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set grid");
		}

		public void SetSize(double ratio)
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set size ratio {0}", ratio);
		}

		public void SetSize(double x_ratio, double y_ratio)
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set size ratio {0} {1}", x_ratio, y_ratio);
		}

		public void SetSize(SizeOption size)
		{
			ThrowExceptionIfDisposed();
			if (size == SizeOption.Default)
			{
				Stream.WriteLine("set size noratio");
			}
			else if (size == SizeOption.Square)
			{
				Stream.WriteLine("set size square");
			}
		}

		public void SetTerminal()
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set terminal postscript eps enhanced color \"Times-Roman,16\"");
		}

		public void SetOutput(string filename)
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set output \"{0}.eps\"", filename);
		}
		public void SetOutput()
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("set output");
		}

		public void Replot()
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("replot");
		}

		public void Quit()
		{
			ThrowExceptionIfDisposed();
			Stream.WriteLine("quit");
		}

		private static readonly string DefaultLabelFont;

		private StreamWriter stream;

		/// <summary>
		/// pgnuplot.exeを立ち上げるプロセス
		/// </summary>
		private Process gnuplotProcess;

	}

	public class LineStyle
	{
		public LineStyle()
		{

		}
		public LineStyle(int lt, double width, Color color)
		{
			LineType = lt;
			Width = width;
			Color = color;
		}
		public int LineType { get; set; }
		public double Width { get; set; }
		public Color Color { get; set; }
	}

	public struct Color
	{
		private Color(byte r, byte g, byte b)
		{
			this.r = r;
			this.g = g;
			this.b = b;
		}
		/// <summary>
		/// KnownColor列挙体で宣言した内部整数値の16進数表記文字列を用いてColor構造体を初期化する
		/// </summary>
		/// <param name="name">KnownColor列挙体で宣言した内部整数値の16進数表記文字列</param>
		private Color(string name)
		{
			int temp2 = (int)((KnownColor)Enum.Parse(typeof(KnownColor), name));
			string aa = temp2.ToString("X6");	//16進数表記6桁に
			this.r = Convert.ToByte(aa.Substring(0, 2), 16);
			this.g = Convert.ToByte(aa.Substring(2, 2), 16);
			this.b = Convert.ToByte(aa.Substring(4, 2), 16);
		}

		#region PropertyForKnownColor

		public static Color White { get { return new Color(KnownColor.White.ToString()); } }
		public static Color Black { get { return new Color(KnownColor.Black.ToString()); } }

		public static Color Gray10 { get { return new Color(KnownColor.Gray10.ToString()); } }
		public static Color Gray20 { get { return new Color(KnownColor.Gray20.ToString()); } }
		public static Color Gray30 { get { return new Color(KnownColor.Gray30.ToString()); } }
		public static Color Gray40 { get { return new Color(KnownColor.Gray40.ToString()); } }
		public static Color Gray50 { get { return new Color(KnownColor.Gray50.ToString()); } }
		public static Color Gray60 { get { return new Color(KnownColor.Gray60.ToString()); } }
		public static Color Gray70 { get { return new Color(KnownColor.Gray70.ToString()); } }
		public static Color Gray80 { get { return new Color(KnownColor.Gray80.ToString()); } }
		public static Color Gray90 { get { return new Color(KnownColor.Gray90.ToString()); } }
		public static Color Gray100 { get { return new Color(KnownColor.Gray100.ToString()); } }
		public static Color Gray { get { return new Color(KnownColor.Gray.ToString()); } }
		public static Color LightGray { get { return new Color(KnownColor.LightGray.ToString()); } }
		public static Color DarkGray { get { return new Color(KnownColor.DarkGray.ToString()); } }

		public static Color Red { get { return new Color(KnownColor.Red.ToString()); } }
		public static Color LightRed { get { return new Color(KnownColor.LightRed.ToString()); } }
		public static Color DarkRed { get { return new Color(KnownColor.DarkRed.ToString()); } }

		public static Color Yellow { get { return new Color(KnownColor.Yellow.ToString()); } }
		public static Color LightYellow { get { return new Color(KnownColor.LightYellow.ToString()); } }
		public static Color DarkYellow { get { return new Color(KnownColor.DarkYellow.ToString()); } }

		public static Color Green { get { return new Color(KnownColor.Green.ToString()); } }
		public static Color LightGreen { get { return new Color(KnownColor.LightGreen.ToString()); } }
		public static Color DarkGreen { get { return new Color(KnownColor.DarkGreen.ToString()); } }
		public static Color SpringGreen { get { return new Color(KnownColor.SpringGreen.ToString()); } }
		public static Color ForestGreen { get { return new Color(KnownColor.ForestGreen.ToString()); } }
		public static Color SeaGreen { get { return new Color(KnownColor.SeaGreen.ToString()); } }

		public static Color Blue { get { return new Color(KnownColor.Blue.ToString()); } }
		public static Color LightBlue { get { return new Color(KnownColor.LightBlue.ToString()); } }
		public static Color DarkBlue { get { return new Color(KnownColor.DarkBlue.ToString()); } }
		public static Color MidnightBlue { get { return new Color(KnownColor.MidnightBlue.ToString()); } }
		public static Color Navy { get { return new Color(KnownColor.Navy.ToString()); } }
		public static Color MediumBlue { get { return new Color(KnownColor.MediumBlue.ToString()); } }
		public static Color Royalblue { get { return new Color(KnownColor.Royalblue.ToString()); } }
		public static Color Skyblue { get { return new Color(KnownColor.Skyblue.ToString()); } }
		public static Color Cyan { get { return new Color(KnownColor.Cyan.ToString()); } }
		public static Color LightCyan { get { return new Color(KnownColor.LightCyan.ToString()); } }
		public static Color DarkCyan { get { return new Color(KnownColor.DarkCyan.ToString()); } }

		public static Color Magenta { get { return new Color(KnownColor.Magenta.ToString()); } }
		public static Color LightMagenta { get { return new Color(KnownColor.LightMagenta.ToString()); } }
		public static Color DarkMagenta { get { return new Color(KnownColor.DarkMagenta.ToString()); } }

		public static Color Turquoise { get { return new Color(KnownColor.Turquoise.ToString()); } }
		public static Color LightTurquoise { get { return new Color(KnownColor.LightTurquoise.ToString()); } }
		public static Color DarkTurquoise { get { return new Color(KnownColor.DarkTurquoise.ToString()); } }

		public static Color Pink { get { return new Color(KnownColor.Pink.ToString()); } }
		public static Color LightPink { get { return new Color(KnownColor.LightPink.ToString()); } }
		public static Color DarkPink { get { return new Color(KnownColor.DarkPink.ToString()); } }

		public static Color Coral { get { return new Color(KnownColor.Coral.ToString()); } }
		public static Color LightCoral { get { return new Color(KnownColor.LightCoral.ToString()); } }
		public static Color OrangeRed { get { return new Color(KnownColor.OrangeRed.ToString()); } }
		public static Color Salmon { get { return new Color(KnownColor.Salmon.ToString()); } }
		public static Color LightSalmon { get { return new Color(KnownColor.LightSalmon.ToString()); } }
		public static Color DarkSalmon { get { return new Color(KnownColor.DarkSalmon.ToString()); } }

		public static Color Aquamarine { get { return new Color(KnownColor.Aquamarine.ToString()); } }

		public static Color Khaki { get { return new Color(KnownColor.Khaki.ToString()); } }
		public static Color DarkKhaki { get { return new Color(KnownColor.DarkKhaki.ToString()); } }
		public static Color Goldenrod { get { return new Color(KnownColor.Goldenrod.ToString()); } }
		public static Color LightGoldenrod { get { return new Color(KnownColor.LightGoldenrod.ToString()); } }
		public static Color DarkGoldenrod { get { return new Color(KnownColor.DarkGoldenrod.ToString()); } }
		public static Color Gold { get { return new Color(KnownColor.Gold.ToString()); } }
		public static Color Beige { get { return new Color(KnownColor.Beige.ToString()); } }
		public static Color Brown { get { return new Color(KnownColor.Brown.ToString()); } }
		public static Color Orange { get { return new Color(KnownColor.Orange.ToString()); } }
		public static Color DarkOrange { get { return new Color(KnownColor.DarkOrange.ToString()); } }

		public static Color Violet { get { return new Color(KnownColor.Violet.ToString()); } }
		public static Color DarkViolet { get { return new Color(KnownColor.DarkViolet.ToString()); } }
		public static Color Plum { get { return new Color(KnownColor.Plum.ToString()); } }
		public static Color Purple { get { return new Color(KnownColor.Purple.ToString()); } }

		#endregion

		/// <summary>
		/// 定義済みの色を示す名前（文字列）からColor構造体を作成する
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static Color FromName(string name)
		{
			//列挙体で宣言した内部整数値に変換
			int name_number = (int)((KnownColor)Enum.Parse(typeof(KnownColor), name));
			//16進数表記文字列に変換
			string hexadecimal_str = name_number.ToString("X6");

			return new Color(hexadecimal_str);
		}

		/*
		public byte R { get { return r; } }
		public byte G { get { return g; } }
		public byte B { get { return b; } }
		*/
		public string R { get { return r.ToString("X2"); } }
		public string G { get { return g.ToString("X2"); } }
		public string B { get { return b.ToString("X2"); } }

		private byte r;
		private byte g;
		private byte b;
	}

	enum KnownColor : int
	{
		White = 0xffffff,
		Black = 0x000000,
		Gray10 = 0x1a1a1a,
		Gray20 = 0x333333,
		Gray30 = 0x4d4d4d,
		Gray40 = 0x666666,
		Gray50 = 0x7f7f7f,
		Gray60 = 0x999999,
		Gray70 = 0xb3b3b3,
		Gray80 = 0xcccccc,
		Gray90 = 0xe5e5e5,
		Gray100 = 0xffffff,
		Gray = 0xbebebe,
		LightGray = 0xd3d3d3,
		DarkGray = 0xa9a9a9,

		Red = 0xff0000,
		LightRed = 0xf03232,
		DarkRed = 0x8b0000,

		Yellow = 0xffff00,
		LightYellow = 0xffffe0,
		DarkYellow = 0xc8c800,

		Green = 0x00ff00,
		LightGreen = 0x90ee90,
		DarkGreen = 0x006400,
		SpringGreen = 0x00ff7f,
		ForestGreen = 0x228b22,
		SeaGreen = 0x2e8b57,

		Blue = 0x0000ff,
		LightBlue = 0xadd8e6,
		DarkBlue = 0x00008b,
		MidnightBlue = 0x191970,
		Navy = 0x000080,
		MediumBlue = 0x0000cd,
		Royalblue = 0x4169e1,
		Skyblue = 0x87ceeb,
		Cyan = 0x00ffff,
		LightCyan = 0xe0ffff,
		DarkCyan = 0x008b8b,

		Magenta = 0xff00ff,
		LightMagenta = 0xf055f0,
		DarkMagenta = 0x8b008b,

		Turquoise = 0x40e0d0,
		LightTurquoise = 0xafeeee,
		DarkTurquoise = 0x00ced1,

		Pink = 0xffc0cb,
		LightPink = 0xffb6c1,
		DarkPink = 0xff1493,

		Coral = 0xff7f50,
		LightCoral = 0xf08080,
		OrangeRed = 0xff4500,
		Salmon = 0xfa8072,
		LightSalmon = 0xffa07a,
		DarkSalmon = 0xe9967a,

		Aquamarine = 0x7fffd4,

		Khaki = 0xf0e68c,
		DarkKhaki = 0xbdb76b,
		Goldenrod = 0xdaa520,
		LightGoldenrod = 0xeedd82,
		DarkGoldenrod = 0xb8860b,
		Gold = 0xffd700,
		Beige = 0xf5f5dc,
		Brown = 0xa52a2a,
		Orange = 0xffa500,
		DarkOrange = 0xff8c00,

		Violet = 0xee82ee,
		DarkViolet = 0x9400d3,
		Plum = 0xdda0dd,
		Purple = 0xa020f0
	}

	public enum PlottingStyle
	{
		boxerrorbars,
		boxes,
		boxplot,
		boxxyerrorbars,
		candlesticks,
		circles,
		ellipses,
		dots,
		filledcurves,
		financebars,
		fsteps,
		fillsteps,
		histeps,
		histograms,
		image,
		impulses,
		labels,
		lines,
		linespoints,
		points,
		polar,
		steps,
		rgbalpha,
		rgbimage,
		vectors,
		xerrorbars,
		xyerrorbars,
		yerrorbars,
		xerrorlines,
		xyerrorlines,
		yerrorlines,
		pm3d		//3次元グラフを色を用いて2次元平面にプロット
	}

	public enum PlotAxis
	{
		x1y1,
		x1y2,
		x2y1,
		x2y2
	}

	public enum LegendPosition
	{
		right,
		left,
		top,
		bottom,
		outside,
		below
	}

	public enum SizeOption
	{
		Square,
		Default
	}

	public enum Terminal
	{
		MonochromeEPS,
		ColorEPS
	}
}
