using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;

namespace LineArt
{
	/// <summary>
	/// ひたすらラインアートを描写し続けるだけのフォーム。
	/// </summary>
	public class LineArtForm : System.Windows.Forms.Form
	{
		public const int default_lines  = 4;
		public const int default_vertex = 4;
		public const int default_width  = 640;
		public const int default_height = 480;
		public const int default_wait = 0;
		const string parameter_file = "parameter.xml";

		int vertex;
		int lines;
		int wait_time;

		public Point[][] p;
		public Size[] v;
		Thread thread;
		bool loop_flag=true;

		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public LineArtForm()
		{
			InitializeComponent();

			MenuItem item = new MenuItem("設定");
			item.Click += new EventHandler(ParameterSetting);
			System.Windows.Forms.ContextMenu menu = new ContextMenu();
			menu.MenuItems.Add(item);
			this.ContextMenu = menu;

			ReadParameter();

			InitializeLineObject();

			thread = new Thread(new ThreadStart(ThreadProc));
			thread.Start();
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(LineArtForm));
			// 
			// LineArtForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(632, 453);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimizeBox = false;
			this.Name = "LineArtForm";
			this.Text = "Line Art";
			this.Closed += new System.EventHandler(this.LineArtForm_Closed);

		}
		#endregion

		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main(string[] args) 
		{
			try
			{
				Application.Run(new LineArtForm());
			}
			catch(Exception e)
			{
				MessageBox.Show(e.GetType() + " " + e.Message + "\n");
			}
		}//Main

		/// <summary>
		/// ラインオブジェクトの初期化。
		/// 頂点の座標および速度をランダムに初期化する。
		/// </summary>
		private void InitializeLineObject()
		{
			Random rnd = new Random();

			this.p = new Point[lines][];
			this.p[0] = new Point[vertex];
			this.v = new Size[vertex];
			for(int j=0; j<vertex; ++j)
			{
				this.p[0][j].X = (rnd.Next()>>3) % this.ClientSize.Width;
				this.p[0][j].Y = (rnd.Next()>>3) % this.ClientSize.Height;
				double theta = rnd.NextDouble() * 2*Math.PI;
				double r = rnd.NextDouble() * 5+2;
				this.v[j].Width = (int)(r * Math.Cos(theta));
				this.v[j].Height = (int)(r * Math.Sin(theta));
			}
			for(int i=1; i<lines; ++i)
			{
				this.p[i] = new Point[vertex];
				for(int j=0; j<vertex; ++j)
				{
					this.p[i][j] = this.p[i-1][j];
				}
			}
		}//InitializeLineObject

		/// <summary>
		/// 画面背景描写イベントハンドラ。
		/// </summary>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			Bitmap bmp = new Bitmap(this.Width, this.Height);
			Graphics g = Graphics.FromImage(bmp);

			g.FillRectangle(new SolidBrush(Color.White), 0, 0, this.Width, this.Height);
			for(int i=0; i<this.lines; ++i)
			{
				g.DrawPolygon(new Pen(Color.Black), this.p[i]);
			}
			e.Graphics.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height);
		}//OnPaintBackground

		/// <summary>
		/// 画面更新用のスレッド。
		/// ラインオブジェクトの頂点位置および速度を更新する。
		/// </summary>
		void ThreadProc()
		{
			for(;;)
			{
				lock(this)
				{
					for(int j=0; j<this.vertex; ++j)
					{
						//残像を作る
						for(int i=this.lines-1; i>0; --i)
						{
							this.p[i][j] = this.p[i-1][j];
						}
						//点の位置の更新
						this.p[0][j] += this.v[j];
						//画面からはみ出したときの処理
						if(this.p[0][j].X >= this.ClientSize.Width)
						{
							this.p[0][j].X = this.ClientSize.Width * 2 - this.p[0][j].X;
							this.v[j].Width = -this.v[j].Width;
						}
						else if(this.p[0][j].X < 0)
						{
							this.p[0][j].X = -this.p[0][j].X;
							this.v[j].Width = -this.v[j].Width;
						}
						if(this.p[0][j].Y >= this.ClientSize.Height)
						{
							this.p[0][j].Y = this.ClientSize.Height * 2 - this.p[0][j].Y;
							this.v[j].Height = -this.v[j].Height;
						}
						else if(this.p[0][j].Y < 0)
						{
							this.p[0][j].Y = -this.p[0][j].Y;
							this.v[j].Height = -this.v[j].Height;
						}
					}//for
					//終了すべきかどうか判断
					if(!this.loop_flag)
						return;
				}
				//画面の更新
				this.Refresh();
				//GCの強制
				GC.Collect();

				Thread.Sleep(this.wait_time);
			}
		}//ThreadProc

		/// <summary>
		/// プログラム終了時の処理。
		/// 設定をファイルに保存する。
		/// </summary>
		private void LineArtForm_Closed(object sender, System.EventArgs e)
		{
			lock(this)
			{
				this.loop_flag = false;
			}
			try
			{
				thread.Join();
			}
			catch(Exception)
			{
			}
			WriteParameter();
		}//LineArtForm_Closed

		/// <summary>
		/// コンテキストメニューの「設定」が押されたときのイベントハンドラ。
		/// 設定用ダイアログを表示し、設定値の更新を行う。
		/// </summary>
		private void ParameterSetting(object sender, System.EventArgs e)
		{
			SettingForm form = new SettingForm();
			form.Vertex   = this.p[0].Length;
			form.Lines    = this.p.Length;
			form.WaitTime = this.wait_time;
			DialogResult res = form.ShowDialog();
			if(res == DialogResult.OK)
			{
				lock(this)
				{
					this.lines     = form.Lines;
					this.vertex    = form.Vertex;
					this.wait_time = form.WaitTime;
					InitializeLineObject();
				}
			}
		}//ParameterSetting

		/// <summary>
		/// 設定値をXMLファイルから読み出す。
		/// </summary>
		private void ReadParameter()
		{
			try
			{
				System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
				doc.Load(parameter_file);

				System.Xml.XmlElement root = doc.DocumentElement;

				this.Width     = Int32.Parse(root.GetElementsByTagName("Width").Item(0).InnerText);
				this.Height    = Int32.Parse(root.GetElementsByTagName("Height").Item(0).InnerText);
				this.lines     = Int32.Parse(root.GetElementsByTagName("Lines").Item(0).InnerText);
				this.vertex    = Int32.Parse(root.GetElementsByTagName("Vertex").Item(0).InnerText);
				this.wait_time = Int32.Parse(root.GetElementsByTagName("WaitTime").Item(0).InnerText);
			}
			catch(Exception)
			{
				this.Width    = LineArtForm.default_width;
				this.Height   = LineArtForm.default_height;
				this.lines    = LineArtForm.default_lines;
				this.vertex   = LineArtForm.default_vertex;
				this.wait_time= LineArtForm.default_wait;
			}
		}//ReadParameter

		/// <summary>
		/// 設定値をXMLファイルに書き出す。
		/// </summary>
		private void WriteParameter()
		{
			try
			{
				System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
				System.Xml.XmlElement root = doc.CreateElement(parameter_file);
				System.Xml.XmlElement elem;

				doc.AppendChild(root);

				elem = doc.CreateElement("Width");
				elem.InnerText = this.Width.ToString();
				root.AppendChild(elem);

				elem = doc.CreateElement("Height");
				elem.InnerText = this.Height.ToString();
				root.AppendChild(elem);

				elem = doc.CreateElement("Lines");
				elem.InnerText = this.lines.ToString();
				root.AppendChild(elem);

				elem = doc.CreateElement("Vertex");
				elem.InnerText = this.vertex.ToString();
				root.AppendChild(elem);

				elem = doc.CreateElement("WaitTime");
				elem.InnerText = this.wait_time.ToString();
				root.AppendChild(elem);

				doc.Save("parameter.xml");
			}
			catch(Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}//WriteParameter
	}//class LineArtForm
}//namespace LineArt
