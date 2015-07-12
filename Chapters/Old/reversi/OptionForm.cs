using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Reversi
{
	/// <summary>
	/// OptionForm の概要の説明です。
	/// </summary>
	public class OptionForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button ok_button;
		private System.Windows.Forms.Button cancel_button;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoardHeight;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBoardWidth;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		private ReversiMainForm.Option option;

		public OptionForm(ReversiMainForm.Option option)
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			this.option = option;
			textBoardWidth.Text = option.board_width.ToString();
			textBoardHeight.Text = option.board_height.ToString();
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(OptionForm));
			this.ok_button = new System.Windows.Forms.Button();
			this.cancel_button = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoardHeight = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textBoardWidth = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// ok_button
			// 
			this.ok_button.Location = new System.Drawing.Point(72, 128);
			this.ok_button.Name = "ok_button";
			this.ok_button.Size = new System.Drawing.Size(64, 24);
			this.ok_button.TabIndex = 0;
			this.ok_button.Text = "OK";
			this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
			// 
			// cancel_button
			// 
			this.cancel_button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel_button.Location = new System.Drawing.Point(144, 128);
			this.cancel_button.Name = "cancel_button";
			this.cancel_button.Size = new System.Drawing.Size(64, 24);
			this.cancel_button.TabIndex = 1;
			this.cancel_button.Text = "キャンセル";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(85, 12);
			this.label1.TabIndex = 2;
			this.label1.Text = "盤面のサイズ 縦";
			// 
			// textBoardHeight
			// 
			this.textBoardHeight.Location = new System.Drawing.Point(96, 12);
			this.textBoardHeight.Name = "textBoardHeight";
			this.textBoardHeight.Size = new System.Drawing.Size(32, 19);
			this.textBoardHeight.TabIndex = 3;
			this.textBoardHeight.Text = "8";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(128, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(29, 12);
			this.label2.TabIndex = 4;
			this.label2.Text = "×横";
			// 
			// textBoardWidth
			// 
			this.textBoardWidth.Location = new System.Drawing.Point(160, 12);
			this.textBoardWidth.Name = "textBoardWidth";
			this.textBoardWidth.Size = new System.Drawing.Size(32, 19);
			this.textBoardWidth.TabIndex = 5;
			this.textBoardWidth.Text = "8";
			// 
			// OptionForm
			// 
			this.AcceptButton = this.ok_button;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.CancelButton = this.cancel_button;
			this.ClientSize = new System.Drawing.Size(216, 157);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																																	this.textBoardWidth,
																																	this.label2,
																																	this.textBoardHeight,
																																	this.label1,
																																	this.cancel_button,
																																	this.ok_button});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionForm";
			this.Text = "オプション設定";
			this.ResumeLayout(false);

		}
		#endregion

		private void ok_button_Click(object sender, System.EventArgs e)
		{
			option.board_width = Convert.ToInt32(textBoardWidth.Text);
			option.board_height = Convert.ToInt32(textBoardHeight.Text);
			this.Close();
		}
	}
}
