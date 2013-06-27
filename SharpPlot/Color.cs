using System;

namespace SharpPlot
{
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

}