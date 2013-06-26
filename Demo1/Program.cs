using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpPlot;

namespace Demo1
{
	class Program
	{
		static void Main(string[] args)
		{
			double analysis_time = 0.03;
			double time_step = 1.0e-4;
			int element = (int)(analysis_time / time_step) + 1;

			System sys = new System();
			sys.L = 0.0823 / 1000;
			sys.R = 0.299;
			sys.Ke = 1 / (317 * 2 * Math.PI / 60);
			sys.E = 24;
			sys.J = 142e-7 + 9.1e-7;
			sys.C = (0.137 * 30.2 / 1000) / (7580 * 2 * Math.PI / 60);
			sys.Tl = 0;
			sys.Kt = 30.2 / 1000;

			RungeKutta rk = new RungeKutta(time_step, element);
			rk.RegistrateMethod(sys.Eq0_i);
			rk.RegistrateMethod(sys.Eq1_omega);
			rk.RegistrateMethod(sys.Eq2_theta);
			rk.ODE4(new double[] { 0, 0, 0 });		// 初期条件を全て0として解く

			string filename = "motor_simu_sample1";
			using (StreamWriter sw = new StreamWriter(filename + ".txt"))
			{
				WriteFile(sw, rk, " ");
			}
			using (StreamWriter sw = new StreamWriter(filename + ".csv"))
			{
				WriteFile(sw, rk, "\t");
			}

			using (PlotStream gnuplot = new PlotStream())
			{
				gnuplot.Start();
				gnuplot.ChangeDirectory(Directory.GetCurrentDirectory());
				gnuplot.PlotFromFile(filename, 1, 2, PlottingStyle.lines, 1, PlotAxis.x1y1, "Current");
				gnuplot.ReplotFromFile(filename, 1, 3, PlottingStyle.lines, 2, PlotAxis.x1y2, "Rotation frequency");
				gnuplot.SetXLabel("Time [ms]", 20);
				gnuplot.SetYLabel("Current [A]", 20);
				gnuplot.SetY2Label("Rotation frequency [rpm]", 20);
				gnuplot.SetYRange(0, 80);
				gnuplot.SetY2Range(0, 9000);
				gnuplot.SetXTics(0, 2);
				gnuplot.SetYTics(0, 5);
				gnuplot.SetY2Tics(0,500);
				gnuplot.SetGrid();
				gnuplot.SetTerminal();
				gnuplot.SetOutput(filename);
				gnuplot.Replot();
				gnuplot.SetOutput();
				gnuplot.Stream.WriteLine("unset style line");
				gnuplot.Stream.WriteLine("set terminal wxt");
			}
		}

		/// <summary>
		/// 指定されたファイルストリームに解を出力する
		/// </summary>
		/// <param name="sw">ストリーム</param>
		/// <param name="rk">解</param>
		/// <param name="partition">仕切り文字</param>
		private static void WriteFile(StreamWriter sw, RungeKutta rk, string partition)
		{
			for (int i = 0; i < rk.Element; i++)
			{
				double time = rk.GetTime()[i] * 1000;						// 時刻．単位を[ms]に変更．
				double current = rk.GetSolution()[0, i];					// 電流[A]
				double rpm = rk.GetSolution()[1, i] * 60 / 2 / Math.PI;		// 回転数．単位を[rad/s]から[rpm]に変更．
				double theta = rk.GetSolution()[2, i];						// 回転角[rad]
				sw.WriteLine("{1}{0}{2}{0}{3}{0}{4}",partition, time, current, rpm, theta);
			}
		}
	}

	/// <summary>
	/// 解析対象の系を表す
	/// </summary>
	class System
	{
		/// <summary>
		/// 巻線インダクタンス
		/// </summary>
		public double L { get; set; }
		/// <summary>
		/// 巻線抵抗
		/// </summary>
		public double R { get; set; }
		/// <summary>
		/// 逆起電力定数
		/// </summary>
		public double Ke { get; set; }
		/// <summary>
		/// ロータ慣性モーメント
		/// </summary>
		public double J { get; set; }
		/// <summary>
		/// 回転粘性係数
		/// </summary>
		public double C { get; set; }
		/// <summary>
		/// トルク定数
		/// </summary>
		public double Kt { get; set; }

		/// <summary>
		/// 印加電圧
		/// </summary>
		public double E { get; set; }
		/// <summary>
		/// 外部負荷トルク
		/// </summary>
		public double Tl { get; set; }

		/// <summary>
		/// 第1式（電流の時間微分項）
		/// </summary>
		/// <param name="t"></param>
		/// <param name="x"></param>
		/// <returns></returns>
		public double Eq0_i(double t, double[] x)
		{
			return (E - Ke * x[1] - R * x[0]) / L;
		}
		/// <summary>
		/// 第2式（角速度の時間微分項）
		/// </summary>
		/// <param name="t"></param>
		/// <param name="x"></param>
		/// <returns></returns>
		public double Eq1_omega(double t, double[] x)
		{
			return (Kt * x[0] - Tl - C * x[1]) / J;
		}
		/// <summary>
		/// 第3式（変角の時間微分項）
		/// </summary>
		/// <param name="t"></param>
		/// <param name="x"></param>
		/// <returns></returns>
		public double Eq2_theta(double t, double[] x)
		{
			return x[1];
		}
	}

	delegate double SystemEq(double t, double[] x);

	/// <summary>
	/// ルンゲクッタ法
	/// </summary>
	class RungeKutta
	{
		/// <summary>
		/// 時間ステップ幅とステップ数を元にRungeKuttaクラスを初期化する
		/// </summary>
		/// <param name="timestep">ステップ幅</param>
		/// <param name="element">ステップ数</param>
		public RungeKutta(double timestep, int element)
		{
			Element = element;
			TimeStep = timestep;
			Eqnum = 0;
			time = new double[Element];
			for (int i = 0; i < Element; i++)
			{
				time[i] = i * TimeStep;
			}
			Eq = new List<SystemEq>();
		}

		/// <summary>
		/// 微分方程式を登録する
		/// </summary>
		/// <param name="CompEq">微分方程式</param>
		public void RegistrateMethod(SystemEq eq)
		{
			Eq.Add(eq);
			Eqnum++;
		}

		/// <summary>
		/// 微分方程式を消去する
		/// </summary>
		public void ClearRegistratedMethod()
		{
			Eq.Clear();
			Eqnum = 0;
		}

		/// <summary>
		/// セットされた連立常微分方程式を初期条件を元に解く
		/// </summary>
		/// <param name="initial_conditions">初期条件ベクトル</param>
		public void ODE4(double[] initial_conditions)
		{
			x = new double[Eqnum, Element];
			try
			{
				setInitialCondition(initial_conditions);
			}
			catch (ArgumentException) { throw; }
			
			double[] k1 = new double[Eqnum];
			double[] k2 = new double[Eqnum];
			double[] k3 = new double[Eqnum];
			double[] k4 = new double[Eqnum];
			double[] comp_x = new double[Eqnum];

			for (int i = 0; i < (Element - 1); ++i)
			{
				for (int j = 0; j < Eqnum; j++)
					comp_x[j] = x[j, i];
				for (int j = 0; j < Eqnum; j++)
					k1[j] = TimeStep * Eq[j](time[i], comp_x);

				for (int j = 0; j < Eqnum; j++)
					comp_x[j] = x[j, i] + k1[j] * 0.5;
				for (int j = 0; j < Eqnum; j++)
					k2[j] = TimeStep * Eq[j](time[i] + TimeStep * 0.5, comp_x);

				for (int j = 0; j < Eqnum; j++)
					comp_x[j] = x[j, i] + k2[j] * 0.5;
				for (int j = 0; j < Eqnum; j++)
					k3[j] = TimeStep * Eq[j](time[i] + TimeStep * 0.5, comp_x);

				for (int j = 0; j < Eqnum; j++)
					comp_x[j] = x[j, i] + k3[j];
				for (int j = 0; j < Eqnum; j++)
					k4[j] = TimeStep * Eq[j](time[i] + TimeStep, comp_x);

				for (int j = 0; j < Eqnum; j++)
					x[j, i + 1] = x[j, i] + (k1[j] + 2 * (k2[j] + k3[j]) + k4[j]) / 6;
			}
		}

		/// <summary>
		/// 解を取得する
		/// </summary>
		/// <returns>解</returns>
		public double[,] GetSolution()
		{
			return x;
		}

		/// <summary>
		/// 解に対応した時刻列を取得する
		/// </summary>
		/// <returns>時刻列</returns>
		public double[] GetTime()
		{
			return time;
		}

		/// <summary>
		/// 初期条件のセット
		/// </summary>
		/// <param name="ic">初期条件</param>
		private void setInitialCondition(double[] ic)
		{
			if (ic.Length != Eqnum)
			{
				throw new ArgumentException();
			}
			for (int i = 0; i < Eqnum; i++)
			{
				x[i, 0] = ic[i];
			}
		}

		private List<SystemEq> Eq;
		public int Element { get; private set; }	// 時間節点の数
		private double TimeStep { get; set; }			// 時間刻み[s]
		private int Eqnum { get; set; }				// 独立変数の数
		private double[] time;						// 時刻列
		private double[,] x;						// 解
	}
}
