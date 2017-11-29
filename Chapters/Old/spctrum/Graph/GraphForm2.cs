using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Wave;

namespace Graph
{
	/// <summary>
	/// WaveForm の概要の説明です。
	/// </summary>
	public class GraphForm2 : System.Windows.Forms.Form
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region 手動行進用領域
		Graph graphL = new Graph();
		Graph graphR = new Graph();

		public Graph GraphL
		{
			get{return this.graphL;}
		}

		public Graph GraphR
		{
			get{return this.graphR;}
		}

		public void ResizeGraph()
		{
			this.graphL.Left   = 0;
			this.graphL.Width  = this.ClientSize.Width;
			this.graphL.Top    = 0;
			this.graphL.Height = this.ClientSize.Height / 2;

			this.graphR.Left   = 0;
			this.graphR.Width  = this.ClientSize.Width;
			this.graphR.Top    = this.ClientSize.Height / 2 + 1;
			this.graphR.Height = this.ClientSize.Height / 2;
		}
		#endregion

		public GraphForm2()
		{
			InitializeComponent();

			this.graphL = new Graph();
			this.graphR = new Graph();

			this.ResizeGraph();
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																																	this.graphL,
																																	this.graphR});
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

		#region Windows Form Designer generated code
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(GraphForm2));
			// 
			// GraphForm2
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "GraphForm2";
			this.Resize += new System.EventHandler(this.WaveForm_Resize);

		}
		#endregion

		private void WaveForm_Resize(object sender, System.EventArgs e)
		{
			this.ResizeGraph();
		}
	}
}
