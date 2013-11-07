namespace SharpPlot
{
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
		postscript,
		epscairo,
		pdfcairo,
		pngcairo
	}
}