﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Window.Control
{

	/// <summary>
	/// 現在値と最大値を視覚的に表現するバー
	/// </summary>
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class StatusBarModule
	{


		public event EventHandler PropertyChanged = delegate { };


		#region Property

		private int _value = 66;
		/// <summary>
		/// 現在値
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(66)]
		[Description("現在値を指定します。")]
		public int Value
		{
			get { return _value; }
			set
			{
				_value = value;
				if (!_usePrevValue)
					_prevValue = _value;
				Refresh();
			}
		}

		private int _prevValue = 88;
		/// <summary>
		/// 直前の現在値
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(88)]
		[Description("直前の値を指定します。")]
		public int PrevValue
		{
			get { return _prevValue; }
			set
			{
				if (_usePrevValue)
					_prevValue = value;
				Refresh();
			}
		}

		private int _maximumValue = 100;
		/// <summary>
		/// 最大値
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(100)]
		[Description("最大値を指定します。")]
		public int MaximumValue
		{
			get { return _maximumValue; }
			set
			{
				_maximumValue = value;
				Refresh();
			}
		}

		private bool _usePrevValue = true;
		/// <summary>
		/// 直前の値を利用するかどうか
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(true)]
		[Description("直前の値を利用するかを指定します。")]
		public bool UsePrevValue
		{
			get { return _usePrevValue; }
			set
			{
				_usePrevValue = value;
				if (!_usePrevValue)
					_prevValue = _value;
				Refresh();
			}
		}


		private Color _barColor0Begin = FromArgb(0xFFFF0000);
		/// <summary>
		/// バーの色(0%～25% - 始点)
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "255, 0, 0")]
		[Description("0%～25%エリアの始点の色を指定します。")]
		public Color BarColor0Begin
		{
			get { return _barColor0Begin; }
			set
			{
				_barColor0Begin = value;
				Refresh();
			}
		}

		private Color _barColor0End = FromArgb(0xFFFF0000);
		/// <summary>
		/// バーの色(0%～25% - 終点)
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "255, 0, 0")]
		[Description("0%～25%エリアの終点の色を指定します。")]
		public Color BarColor0End
		{
			get { return _barColor0End; }
			set
			{
				_barColor0End = value;
				Refresh();
			}
		}

		private Color _barColor1Begin = FromArgb(0xFFFF8800);
		/// <summary>
		/// バーの色(25%～50% - 始点)
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "255, 136, 0")]
		[Description("25～50%エリアの始点の色を指定します。")]
		public Color BarColor1Begin
		{
			get { return _barColor1Begin; }
			set
			{
				_barColor1Begin = value;
				Refresh();
			}
		}

		private Color _barColor1End = FromArgb(0xFFFF8800);
		/// <summary>
		/// バーの色(25%～50% - 終点)
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "255, 136, 0")]
		[Description("25～50%エリアの終点の色を指定します。")]
		public Color BarColor1End
		{
			get { return _barColor1End; }
			set
			{
				_barColor1End = value;
				Refresh();
			}
		}

		private Color _barColor2Begin = FromArgb(0xFFFFCC00);
		/// <summary>
		/// バーの色(50%～75% - 始点)
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "255, 204, 0")]
		[Description("50～75%エリアの始点の色を指定します。")]
		public Color BarColor2Begin
		{
			get { return _barColor2Begin; }
			set
			{
				_barColor2Begin = value;
				Refresh();
			}
		}

		private Color _barColor2End = FromArgb(0xFFFFCC00);
		/// <summary>
		/// バーの色(50%～75% - 終点)
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "255, 204, 0")]
		[Description("50～75%エリアの終点の色を指定します。")]
		public Color BarColor2End
		{
			get { return _barColor2End; }
			set
			{
				_barColor2End = value;
				Refresh();
			}
		}

		private Color _barColor3Begin = FromArgb(0xFF00CC00);
		/// <summary>
		/// バーの色(75%～100% - 始点)
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "0, 204, 0")]
		[Description("75～100%エリアの始点の色を指定します。")]
		public Color BarColor3Begin
		{
			get { return _barColor3Begin; }
			set
			{
				_barColor3Begin = value;
				Refresh();
			}
		}

		private Color _barColor3End = FromArgb(0xFF00CC00);
		/// <summary>
		/// バーの色(75%～100% - 終点)
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "0, 204, 0")]
		[Description("75～100%エリアの終点の色を指定します。")]
		public Color BarColor3End
		{
			get { return _barColor3End; }
			set
			{
				_barColor3End = value;
				Refresh();
			}
		}

		private Color _barColor4 = FromArgb(0xFF0044CC);
		/// <summary>
		/// バーの色(100%)
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "0, 68, 204")]
		[Description("100%の時のバーの色を指定します。")]
		public Color BarColor4
		{
			get { return _barColor4; }
			set
			{
				_barColor4 = value;
				Refresh();
			}
		}

		private Color _barColorIncrement = FromArgb(0xFF44FF00);
		/// <summary>
		/// バーの色(増加分)
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "68, 255, 0")]
		[Description("現在値が増加した時のバーの色を指定します。")]
		public Color BarColorIncrement
		{
			get { return _barColorIncrement; }
			set
			{
				_barColorIncrement = value;
				Refresh();
			}
		}

		private Color _barColorDecrement = Utility.Configuration.Config.UI.SubBackColor;
		/// <summary>
		/// バーの色(減少分)
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "136, 34, 34")]
		[Description("現在値が減少した時のバーの色を指定します。")]
		public Color BarColorDecrement
		{
			get { return _barColorDecrement; }
			set
			{
				_barColorDecrement = value;
				Refresh();
			}
		}

		private Color _barColorBackground = FromArgb(0xFF888888);
		/// <summary>
		/// バーの背景色
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "136, 136, 136")]
		[Description("バーの背景色を指定します。")]
		public Color BarColorBackground
		{
			get { return _barColorBackground; }
			set
			{
				_barColorBackground = value;
				Refresh();
			}
		}


		private int _barThickness = 4;
		/// <summary>
		/// バーの太さ
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(4)]
		[Description("バーの太さ(高さ)を指定します。")]
		public int BarThickness
		{
			get { return _barThickness; }
			set
			{
				_barThickness = value;
				Refresh();
			}
		}

		private int _barBackgroundOffset = 1;
		/// <summary>
		/// バーの前景と背景とのずれの大きさ
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(1)]
		[Description("バーの前景と背景のずれの大きさを指定します。影のような表現に利用します。")]
		public int BarBackgroundOffset
		{
			get { return _barBackgroundOffset; }
			set
			{
				_barBackgroundOffset = value;
				Refresh();
			}
		}

		private bool _colorMorphing = false;
		[Browsable(true), Category("Appearance"), DefaultValue(false)]
		[Description("バーの色を割合に応じて滑らかに変化させるかを指定します。")]
		/// <summary>
		/// 色を滑らかに変化させるか
		/// </summary>
		public bool ColorMorphing
		{
			get { return _colorMorphing; }
			set
			{
				_colorMorphing = value;
				Refresh();
			}
		}


		#endregion



		public StatusBarModule()
		{

		}



		public Color[] GetBarColorScheme()
		{
			return new Color[] {
				_barColor0Begin,
				_barColor0End,
				_barColor1Begin,
				_barColor1End,
				_barColor2Begin,
				_barColor2End,
				_barColor3Begin,
				_barColor3End,
				_barColor4,
				_barColorIncrement,
				_barColorDecrement,
				_barColorBackground,
			};
		}

		public void SetBarColorScheme(Color[] colors)
		{

			if (colors.Length < 12)
				throw new ArgumentOutOfRangeException("colors の配列長が足りません。");


			_barColor0Begin = colors[0];
			_barColor0End = colors[1];
			_barColor1Begin = colors[2];
			_barColor1End = colors[3];
			_barColor2Begin = colors[4];
			_barColor2End = colors[5];
			_barColor3Begin = colors[6];
			_barColor3End = colors[7];
			_barColor4 = colors[8];
			_barColorIncrement = colors[9];
			_barColorDecrement = colors[10];
			_barColorBackground = colors[11];


			Refresh();
		}


		private static double GetPercentage(int value, int max)
		{
			if (max <= 0 || value <= 0)
				return 0.0;
			else if (value > max)
				return 1.0;
			else
				return (double)value / max;
		}


		private static Color FromArgb(uint color)
		{
			return Color.FromArgb(unchecked((int)color));
		}

		private static Color BlendColor(Color a, Color b, double weight)
		{

			if (weight < 0.0 || 1.0 < weight)
				throw new ArgumentOutOfRangeException("weight は 0.0 - 1.0 の範囲内でなければなりません。指定値: " + weight);

			return Color.FromArgb(
				(int)(a.A * (1 - weight) + b.A * weight),
				(int)(a.R * (1 - weight) + b.R * weight),
				(int)(a.G * (1 - weight) + b.G * weight),
				(int)(a.B * (1 - weight) + b.B * weight));
		}


		/// <summary>
		/// バーを描画します。
		/// </summary>
		/// <param name="g">描画するための Graphics。</param>
		/// <param name="rect">描画する領域。</param>
		public void Paint(Graphics g, Rectangle rect)
		{

			using (var b = new SolidBrush(BarColorBackground))
			{
				g.FillRectangle(b, new Rectangle(rect.X + BarBackgroundOffset, rect.Bottom - BarThickness, rect.Width - BarBackgroundOffset, BarThickness));
			}
			using (var b = new SolidBrush(Value > PrevValue ? BarColorIncrement : BarColorDecrement))
			{
				g.FillRectangle(b, new Rectangle(rect.X, rect.Bottom - BarThickness - BarBackgroundOffset,
					(int)Math.Ceiling((rect.Width - BarBackgroundOffset) * GetPercentage(Math.Max(Value, PrevValue), MaximumValue)), BarThickness));
			}

			Color barColor;
			double p = GetPercentage(Value, MaximumValue);

			if (!ColorMorphing)
			{

				if (p <= 0.25)
					barColor = BarColor0Begin;
				else if (p <= 0.50)
					barColor = BarColor1Begin;
				else if (p <= 0.75)
					barColor = BarColor2Begin;
				else if (p < 1.00)
					barColor = BarColor3Begin;
				else
					barColor = BarColor4;

			}
			else
			{

				if (p <= 0.25)
					barColor = BlendColor(BarColor0Begin, BarColor0End, p * 4.0);
				else if (p <= 0.50)
					barColor = BlendColor(BarColor1Begin, BarColor1End, (p - 0.25) * 4.0);
				else if (p <= 0.75)
					barColor = BlendColor(BarColor2Begin, BarColor2End, (p - 0.50) * 4.0);
				else if (p < 1.00)
					barColor = BlendColor(BarColor3Begin, BarColor3End, (p - 0.75) * 4.0);
				else
					barColor = BarColor4;

			}

			using (var b = new SolidBrush(barColor))
			{
				g.FillRectangle(b, new Rectangle(rect.X, rect.Bottom - BarThickness - BarBackgroundOffset,
					(int)Math.Ceiling((rect.Width - BarBackgroundOffset) * GetPercentage(Math.Min(Value, PrevValue), MaximumValue)), BarThickness));
			}

		}



		/// <summary>
		/// このモジュールを描画するために適切なサイズを取得します。
		/// </summary>
		public Size GetPreferredSize(Size proposedSize)
		{
			return new Size(Math.Max(proposedSize.Width, BarThickness + BarBackgroundOffset),
				Math.Max(proposedSize.Height, BarThickness + BarBackgroundOffset));
		}

		/// <summary>
		/// このモジュールを描画するために適切なサイズを取得します。
		/// </summary>
		public Size GetPreferredSize()
		{
			return GetPreferredSize(Size.Empty);
		}


		private void Refresh()
		{
			PropertyChanged(this, new EventArgs());
		}

	}

}
