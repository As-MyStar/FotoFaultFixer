using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_Tester.YUV
{
	// https://www.programmingalgorithms.com/algorithm/rgb-to-yuv/
	// https://www.programmingalgorithms.com/algorithm/yuv-to-rgb/
	public struct RGB
	{
		private byte _r;
		private byte _g;
		private byte _b;

		public RGB(byte r, byte g, byte b)
		{
			_r = r;
			_g = g;
			_b = b;
		}

		public byte R
		{
			get { return _r; }
			set { _r = value; }
		}

		public byte G
		{
			get { return _g; }
			set { _g = value; }
		}

		public byte B
		{
			get { return _b; }
			set { _b = value; }
		}

		public bool Equals(RGB rgb)
		{
			return (R == rgb.R) && (G == rgb.G) && (B == rgb.B);
		}
		
		public YUV ToYUV()
		{
			double y = R * .299000 + G * .587000 + B * .114000;
			double u = R * -.168736 + G * -.331264 + B * .500000 + 128;
			double v = R * .500000 + G * -.418688 + B * -.081312 + 128;

			return new YUV(y, u, v);
		}
	}

	public struct YUV
	{
		private double _y;
		private double _u;
		private double _v;

		public YUV(double y, double u, double v)
		{
			_y = y;
			_u = u;
			_v = v;
		}

		public double Y
		{
			get { return _y; }
			set { _y = value; }
		}

		public double U
		{
			get { return _u; }
			set { _u = value; }
		}

		public double V
		{
			get { return _v; }
			set { _v = value; }
		}

		public bool Equals(YUV yuv)
		{
			return (Y == yuv.Y) && (U == yuv.U) && (V == yuv.V);
		}

		public RGB ToRGB()
		{
			byte r = (byte)(Y + 1.4075 * (V - 128));
			byte g = (byte)(Y - 0.3455 * (U - 128) - (0.7169 * (V - 128)));
			byte b = (byte)(Y + 1.7790 * (U - 128));

			return new RGB(r, g, b);
		}
	}
}
