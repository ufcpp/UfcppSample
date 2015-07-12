using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace LineArt
{
	/// <summary>
	/// パラメータ設定用ダイアログ。
	/// </summary>
	public class SettingForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxVertex;
		private System.Windows.Forms.TextBox textBoxLines;
		private System.Windows.Forms.TextBox textBoxWait;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SettingForm()
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

		#region Windows Form Designer generated code
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textBoxVertex = new System.Windows.Forms.TextBox();
			this.textBoxLines = new System.Windows.Forms.TextBox();
			this.textBoxWait = new System.Windows.Forms.TextBox();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "頂点の数";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "線の数";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 64);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "ウェイト";
			// 
			// textBoxVertex
			// 
			this.textBoxVertex.Location = new System.Drawing.Point(64, 8);
			this.textBoxVertex.Name = "textBoxVertex";
			this.textBoxVertex.Size = new System.Drawing.Size(32, 19);
			this.textBoxVertex.TabIndex = 3;
			this.textBoxVertex.Text = "";
			this.textBoxVertex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textBoxLines
			// 
			this.textBoxLines.Location = new System.Drawing.Point(64, 32);
			this.textBoxLines.Name = "textBoxLines";
			this.textBoxLines.Size = new System.Drawing.Size(32, 19);
			this.textBoxLines.TabIndex = 4;
			this.textBoxLines.Text = "";
			this.textBoxLines.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textBoxWait
			// 
			this.textBoxWait.Location = new System.Drawing.Point(64, 56);
			this.textBoxWait.Name = "textBoxWait";
			this.textBoxWait.Size = new System.Drawing.Size(32, 19);
			this.textBoxWait.TabIndex = 5;
			this.textBoxWait.Text = "";
			this.textBoxWait.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// buttonOK
			// 
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(8, 88);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(56, 24);
			this.buttonOK.TabIndex = 6;
			this.buttonOK.Text = "OK";
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(72, 88);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(56, 24);
			this.buttonCancel.TabIndex = 7;
			this.buttonCancel.Text = "Cancel";
			// 
			// SettingForm
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(136, 117);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																																	this.buttonCancel,
																																	this.buttonOK,
																																	this.textBoxWait,
																																	this.textBoxLines,
																																	this.textBoxVertex,
																																	this.label3,
																																	this.label2,
																																	this.label1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SettingForm";
			this.Text = "SettingForm";
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// 頂点の数。
		/// </summary>
		public int Vertex
		{
			set{this.textBoxVertex.Text = value.ToString();}
			get
			{
				int val = LineArtForm.default_vertex;
				try
				{
					val = Int32.Parse(this.textBoxVertex.Text);
				}
				catch(Exception){}
				return val;
			}
		}

		/// <summary>
		/// 線の本数。
		/// </summary>
		public int Lines
		{
			set{this.textBoxLines.Text = value.ToString();}
			get
			{
				int val = LineArtForm.default_lines;
				try
				{
					val = Int32.Parse(this.textBoxLines.Text);
				}
				catch(Exception){}
				return val;
			}
		}

		/// <summary>
		/// 表示速度のウェイト。
		/// </summary>
		public int WaitTime
		{
			set{this.textBoxWait.Text = value.ToString();}
			get
			{
				int val = LineArtForm.default_wait;
				try
				{
					val = Int32.Parse(this.textBoxWait.Text);
				}
				catch(Exception){}
				return val;
			}
		}
	}
}
