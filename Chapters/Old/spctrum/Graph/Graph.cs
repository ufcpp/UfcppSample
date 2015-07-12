using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Graph
{
	/// <summary>
	/// グラフを表示するコントロールクラス。
	/// </summary>
	public class Graph : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Panel plotArea;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region 手動更新用領域
		/// <summary>
		/// グラフの項目。
		/// </summary>
		public class Entry
		{
			public double[] x; // x 方向データ
			public double[] y; // y 方向データ
			public Pen pen;    // グラフ描写用のペン

			public Entry(double[] x, double[] y, Pen pen)
			{
				this.x = x;
				this.y = y;
				this.pen = pen;
			}

			public bool IsInvalid()
			{
				return this.x == null
					|| this.y == null
					|| this.x.Length != this.y.Length
					|| this.pen == null;
			}
		}

		/// <summary>
		/// 軸の設定。
		/// </summary>
		public class AxisSetting
		{
			public double min;  // 最小値
			public double max;  // 最大値
			public int split;   // 分割数
			public Font font;   // フォント
			public Brush brush; // ブラシ

			public AxisSetting() : this(0, 0, 0, null, null){}

			public AxisSetting(double min, double max, int split, Font font, Brush brush)
			{
				this.min = min;
				this.max = max;
				this.split = split;
				this.font = font;
				this.brush = brush;
			}

			public bool IsInvalid()
			{
				return this.min >= this.max
					|| this.split == 0
					|| this.font == null
					|| this.brush == null;
			}
		}

		private System.Windows.Forms.Panel xAxis;
		private System.Windows.Forms.Panel yAxis;
		ArrayList entries = new ArrayList(); // y 軸データ
		AxisSetting xAxisSetting = new AxisSetting();
		AxisSetting yAxisSetting = new AxisSetting();

		public void AddEntry(double[] x, double[] y, Pen pen)
		{
			if(x.Length != y.Length)
				return;

			this.entries.Add(new Entry(x, y, pen));
		}

		public void SetXAxis(double min, double max, int split, Font font, Brush brush)
		{
			this.xAxisSetting = new AxisSetting(min, max, split, font, brush);
		}

		public void SetYAxis(double min, double max, int split, Font font, Brush brush)
		{
			this.yAxisSetting = new AxisSetting(min, max, split, font, brush);
		}

		public double XMin
		{
			set{this.xAxisSetting.min = value;}
			get{return this.xAxisSetting.min;}
		}

		public double XMax
		{
			set{this.xAxisSetting.max = value;}
			get{return this.xAxisSetting.max;}
		}

		public double YMin
		{
			set{this.yAxisSetting.min = value;}
			get{return this.yAxisSetting.min;}
		}

		public double YMax
		{
			set{this.yAxisSetting.max = value;}
			get{return this.yAxisSetting.max;}
		}

		public void AutoScale()
		{
			double xMin = double.MaxValue;
			double xMax = double.MinValue;
			double yMin = double.MaxValue;
			double yMax = double.MinValue;
			foreach(Entry entry in this.entries)
			{
				if(entry.IsInvalid()) continue;
				MaxMinValue(entry.x, ref xMin, ref xMax);
				MaxMinValue(entry.y, ref yMin, ref yMax);
			}

			int xn = this.xAxisSetting.split;
			int yn = this.yAxisSetting.split;
			this.XMin = Math.Floor(xMin / xn) * xn;
			this.XMax = Math.Ceiling(xMax / xn) * xn;
			this.YMin = Math.Floor(yMin / yn) * yn;
			this.YMax = Math.Ceiling(yMax / yn) * yn;
		}

		public void AutoScaleX()
		{
			double xMin = double.MaxValue;
			double xMax = double.MinValue;
			foreach(Entry entry in this.entries)
			{
				if(entry.IsInvalid()) continue;
				MaxMinValue(entry.x, ref xMin, ref xMax);
			}

			int xn = this.xAxisSetting.split;
			this.XMin = Math.Floor(xMin / xn) * xn;
			this.XMax = Math.Ceiling(xMax / xn) * xn;
		}

		public void AutoScaleY()
		{
			double yMin = double.MaxValue;
			double yMax = double.MinValue;
			foreach(Entry entry in this.entries)
			{
				if(entry.IsInvalid()) continue;
				MaxMinValue(entry.y, ref yMin, ref yMax);
			}

			int yn = this.yAxisSetting.split;
			this.YMin = Math.Floor(yMin / yn) * yn;
			this.YMax = Math.Ceiling(yMax / yn) * yn;
		}

		void MaxMinValue(double[] array, ref double min, ref double max)
		{
			foreach(double val in array)
			{
				if(min > val) min = val;
				if(max < val) max = val;
			}
		}

		protected virtual void DrawGraph(Graphics g, Entry entry)
		{
			if(entry.IsInvalid()) return;

			double[] x = entry.x;
			double[] y = entry.y;
			Pen pen = entry.pen;

			int n = x.Length;
			PointF[] points = new PointF[n];
			double width = this.XMax - this.XMin;
			if(width == 0) width = 1;
			double height = this.YMax - this.YMin;
			if(height == 0) height = 1;
			for(int i=0; i<n; ++i)
			{
				points[i].X = (float)((x[i] - this.XMin) / width ) * this.plotArea.Width;
				points[i].Y = (float)((this.YMax - y[i]) / height) * this.plotArea.Height;
			}
			g.DrawLines(pen, points);
		}
		#endregion

		public Graph()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.plotArea = new System.Windows.Forms.Panel();
			this.xAxis = new System.Windows.Forms.Panel();
			this.yAxis = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// plotArea
			// 
			this.plotArea.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.plotArea.BackColor = System.Drawing.SystemColors.Window;
			this.plotArea.Location = new System.Drawing.Point(48, 8);
			this.plotArea.Name = "plotArea";
			this.plotArea.Size = new System.Drawing.Size(424, 320);
			this.plotArea.TabIndex = 0;
			this.plotArea.Resize += new System.EventHandler(this.plotArea_Resize);
			this.plotArea.Paint += new System.Windows.Forms.PaintEventHandler(this.plotArea_Paint);
			// 
			// xAxis
			// 
			this.xAxis.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.xAxis.Location = new System.Drawing.Point(48, 328);
			this.xAxis.Name = "xAxis";
			this.xAxis.Size = new System.Drawing.Size(424, 24);
			this.xAxis.TabIndex = 1;
			this.xAxis.Resize += new System.EventHandler(this.xAxis_Resize);
			this.xAxis.Paint += new System.Windows.Forms.PaintEventHandler(this.xAxis_Paint);
			// 
			// yAxis
			// 
			this.yAxis.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left);
			this.yAxis.Location = new System.Drawing.Point(8, 8);
			this.yAxis.Name = "yAxis";
			this.yAxis.Size = new System.Drawing.Size(40, 320);
			this.yAxis.TabIndex = 2;
			this.yAxis.Resize += new System.EventHandler(this.yAxis_Resize);
			this.yAxis.Paint += new System.Windows.Forms.PaintEventHandler(this.yAxis_Paint);
			// 
			// Graph
			// 
			this.BackColor = System.Drawing.SystemColors.Window;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																																	this.xAxis,
																																	this.plotArea,
																																	this.yAxis});
			this.Name = "Graph";
			this.Size = new System.Drawing.Size(480, 360);
			this.ResumeLayout(false);

		}
		#endregion

		private void plotArea_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			foreach(Entry entry in this.entries)
			{
				DrawGraph(e.Graphics, entry);
			}
		}

		private void plotArea_Resize(object sender, System.EventArgs e)
		{
			this.plotArea.Refresh();
		}

		private void xAxis_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if(this.xAxisSetting.IsInvalid()) return;

			double min  = this.xAxisSetting.min;
			double max  = this.xAxisSetting.max;
			int n       = this.xAxisSetting.split;
			Font font   = this.xAxisSetting.font;
			Brush brush = this.xAxisSetting.brush;

			for(int i=0; i<n; ++i)
			{
				double val = (max - min) / n * i + min;
				string str = val.ToString();
				float x = (float)(this.xAxis.Width / n * i);
				e.Graphics.DrawString(str, font, brush, x, 0);
			}
		}

		private void xAxis_Resize(object sender, System.EventArgs e)
		{
			this.xAxis.Refresh();
		}

		private void yAxis_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if(this.yAxisSetting.IsInvalid()) return;

			double min  = this.yAxisSetting.min;
			double max  = this.yAxisSetting.max;
			int n       = this.yAxisSetting.split;
			Font font   = this.yAxisSetting.font;
			Brush brush = this.yAxisSetting.brush;

			for(int i=0; i<n; ++i)
			{
				double val = (max - min) / n * i + min;
				string str = val.ToString();
				float y = (float)(this.yAxis.Height / n * (n - i)) - (font.Size + 5);
				e.Graphics.DrawString(str, font, brush, 0, y);
			}
		}

		private void yAxis_Resize(object sender, System.EventArgs e)
		{
			this.yAxis.Refresh();
		}
	}
}
