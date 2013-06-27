﻿using System;
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

		private static readonly string DefaultLabelFont;

		private StreamWriter stream;

		/// <summary>
		/// pgnuplot.exeを立ち上げるプロセス
		/// </summary>
		private Process gnuplotProcess;

	}
}