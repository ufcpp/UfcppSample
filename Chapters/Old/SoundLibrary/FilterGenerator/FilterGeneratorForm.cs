using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SoundLibrary.Filter
{
	/// <summary>
	/// FilterGeneratorForm の概要の説明です。
	/// </summary>
	public class FilterGeneratorForm : System.Windows.Forms.Form
	{
		#region
		FilterGenerator generator;

		public FilterGeneratorForm(FilterGenerator fg)
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			this.generator = fg;
			//Init();
		}
		#endregion

		private System.Windows.Forms.Button buttonOk;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

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

		#region Windows フォーム デザイナで生成されたコード 
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.buttonOk = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// buttonOk
			// 
			this.buttonOk.Location = new System.Drawing.Point(328, 344);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(56, 24);
			this.buttonOk.TabIndex = 0;
			this.buttonOk.Text = "OK";
			// 
			// FilterGeneratorForm
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(392, 373);
			this.Controls.Add(this.buttonOk);
			this.Name = "FilterGeneratorForm";
			this.Text = "FilterGeneratorForm";
			this.Load += new System.EventHandler(this.c);
			this.ResumeLayout(false);

		}
		#endregion

		private void c(object sender, System.EventArgs e)
		{
		
		}
	}
}
