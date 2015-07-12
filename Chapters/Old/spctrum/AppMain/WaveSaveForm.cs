using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Text;

using SpectrumAnalysis;
using WaveAnalysis;

namespace AppMain
{
	/// <summary>
	/// 特性を CSV 形式でファイルに書き出す。
	/// </summary>
	public class WaveSaveForm : System.Windows.Forms.Form
	{
		#region 手動更新領域
		WaveData wave;
		FileInfo file;
		#endregion

		private System.Windows.Forms.Label labelChannel;
		private System.Windows.Forms.CheckedListBox checkedListType;
		private System.Windows.Forms.Label labelType;
		private System.Windows.Forms.CheckBox checkUnrap;
		private System.Windows.Forms.TextBox textFolder;
		private System.Windows.Forms.Label labelFolder;
		private System.Windows.Forms.Button buttonFolderDefault;
		private System.Windows.Forms.Label labelName;
		private System.Windows.Forms.TextBox textName;
		private System.Windows.Forms.Button buttonNameDefault;
		private System.Windows.Forms.Button buttonSave;
		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.Label labelDelim;
		private System.Windows.Forms.ComboBox comboDelim;
		private System.Windows.Forms.ComboBox comboChannel;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public WaveSaveForm(WaveData wave, FileInfo file)
		{
			InitializeComponent();

			this.wave = wave;
			this.file = file;

			this.comboChannel.SelectedIndex = 0;
			this.comboDelim.SelectedIndex = 0;
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
			this.labelChannel = new System.Windows.Forms.Label();
			this.checkedListType = new System.Windows.Forms.CheckedListBox();
			this.labelType = new System.Windows.Forms.Label();
			this.checkUnrap = new System.Windows.Forms.CheckBox();
			this.textFolder = new System.Windows.Forms.TextBox();
			this.labelFolder = new System.Windows.Forms.Label();
			this.buttonFolderDefault = new System.Windows.Forms.Button();
			this.labelName = new System.Windows.Forms.Label();
			this.textName = new System.Windows.Forms.TextBox();
			this.buttonNameDefault = new System.Windows.Forms.Button();
			this.buttonSave = new System.Windows.Forms.Button();
			this.buttonClose = new System.Windows.Forms.Button();
			this.labelDelim = new System.Windows.Forms.Label();
			this.comboDelim = new System.Windows.Forms.ComboBox();
			this.comboChannel = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// labelChannel
			// 
			this.labelChannel.Location = new System.Drawing.Point(8, 8);
			this.labelChannel.Name = "labelChannel";
			this.labelChannel.Size = new System.Drawing.Size(48, 16);
			this.labelChannel.TabIndex = 5;
			this.labelChannel.Text = "チャネル";
			this.labelChannel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// checkedListType
			// 
			this.checkedListType.Items.AddRange(new object[] {
																												 "振幅特性",
																												 "位相特性",
																												 "位相遅延特性",
																												 "群遅延特性",
																												 "最小位相",
																												 "オールパス位相"});
			this.checkedListType.Location = new System.Drawing.Point(176, 8);
			this.checkedListType.Name = "checkedListType";
			this.checkedListType.Size = new System.Drawing.Size(112, 46);
			this.checkedListType.TabIndex = 9;
			// 
			// labelType
			// 
			this.labelType.Location = new System.Drawing.Point(144, 8);
			this.labelType.Name = "labelType";
			this.labelType.Size = new System.Drawing.Size(32, 16);
			this.labelType.TabIndex = 5;
			this.labelType.Text = "特性";
			this.labelType.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// checkUnrap
			// 
			this.checkUnrap.Checked = true;
			this.checkUnrap.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkUnrap.Location = new System.Drawing.Point(184, 56);
			this.checkUnrap.Name = "checkUnrap";
			this.checkUnrap.TabIndex = 10;
			this.checkUnrap.Text = "位相をアンラップ";
			// 
			// textFolder
			// 
			this.textFolder.Location = new System.Drawing.Point(64, 88);
			this.textFolder.Name = "textFolder";
			this.textFolder.Size = new System.Drawing.Size(136, 19);
			this.textFolder.TabIndex = 11;
			this.textFolder.Text = "";
			// 
			// labelFolder
			// 
			this.labelFolder.Location = new System.Drawing.Point(8, 88);
			this.labelFolder.Name = "labelFolder";
			this.labelFolder.Size = new System.Drawing.Size(48, 16);
			this.labelFolder.TabIndex = 5;
			this.labelFolder.Text = "フォルダ";
			this.labelFolder.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// buttonFolderDefault
			// 
			this.buttonFolderDefault.Location = new System.Drawing.Point(208, 88);
			this.buttonFolderDefault.Name = "buttonFolderDefault";
			this.buttonFolderDefault.Size = new System.Drawing.Size(80, 23);
			this.buttonFolderDefault.TabIndex = 13;
			this.buttonFolderDefault.Text = "元と同じ";
			this.buttonFolderDefault.Click += new System.EventHandler(this.buttonFolderDefault_Click);
			// 
			// labelName
			// 
			this.labelName.Location = new System.Drawing.Point(8, 120);
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size(56, 16);
			this.labelName.TabIndex = 5;
			this.labelName.Text = "ファイル名";
			this.labelName.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textName
			// 
			this.textName.Location = new System.Drawing.Point(64, 120);
			this.textName.Name = "textName";
			this.textName.Size = new System.Drawing.Size(136, 19);
			this.textName.TabIndex = 11;
			this.textName.Text = "";
			// 
			// buttonNameDefault
			// 
			this.buttonNameDefault.Location = new System.Drawing.Point(208, 120);
			this.buttonNameDefault.Name = "buttonNameDefault";
			this.buttonNameDefault.Size = new System.Drawing.Size(80, 23);
			this.buttonNameDefault.TabIndex = 13;
			this.buttonNameDefault.Text = "元と同じ";
			this.buttonNameDefault.Click += new System.EventHandler(this.buttonNameDefault_Click);
			// 
			// buttonSave
			// 
			this.buttonSave.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonSave.Location = new System.Drawing.Point(136, 184);
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.TabIndex = 14;
			this.buttonSave.Text = "保存";
			this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
			// 
			// buttonClose
			// 
			this.buttonClose.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonClose.Location = new System.Drawing.Point(218, 184);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.TabIndex = 15;
			this.buttonClose.Text = "閉じる";
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// labelDelim
			// 
			this.labelDelim.Location = new System.Drawing.Point(8, 152);
			this.labelDelim.Name = "labelDelim";
			this.labelDelim.Size = new System.Drawing.Size(64, 16);
			this.labelDelim.TabIndex = 5;
			this.labelDelim.Text = "区切り文字";
			this.labelDelim.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// comboDelim
			// 
			this.comboDelim.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDelim.Items.AddRange(new object[] {
																										"コンマ",
																										"タブ",
																										"スペース"});
			this.comboDelim.Location = new System.Drawing.Point(80, 152);
			this.comboDelim.Name = "comboDelim";
			this.comboDelim.Size = new System.Drawing.Size(72, 20);
			this.comboDelim.TabIndex = 16;
			// 
			// comboChannel
			// 
			this.comboChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboChannel.Items.AddRange(new object[] {
																											"Left",
																											"Right",
																											"L/R",
																											"R/L",
																											"Middle",
																											"Side",
																											"M/S",
																											"S/M"});
			this.comboChannel.Location = new System.Drawing.Point(56, 8);
			this.comboChannel.Name = "comboChannel";
			this.comboChannel.Size = new System.Drawing.Size(80, 20);
			this.comboChannel.TabIndex = 17;
			// 
			// WaveSaveForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(298, 215);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																																	this.comboChannel,
																																	this.comboDelim,
																																	this.buttonClose,
																																	this.buttonSave,
																																	this.buttonFolderDefault,
																																	this.textFolder,
																																	this.checkUnrap,
																																	this.checkedListType,
																																	this.labelChannel,
																																	this.labelType,
																																	this.labelFolder,
																																	this.labelName,
																																	this.textName,
																																	this.buttonNameDefault,
																																	this.labelDelim});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "WaveSaveForm";
			this.Text = "WaveSaveForm";
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonFolderDefault_Click(object sender, System.EventArgs e)
		{
			this.textFolder.Text = file.DirectoryName;
		}

		private void buttonNameDefault_Click(object sender, System.EventArgs e)
		{
			this.textName.Text = (file.Name.Split('.'))[0];
		}

		private void buttonClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void buttonSave_Click(object sender, System.EventArgs e)
		{
			string filename = this.textFolder.Text + '\\';
			filename += this.textName.Text + ".csv";

			if(File.Exists(filename))
			{
				if(MessageBox.Show("ファイルがすでに存在しています。\n上書きしますか？", "上書き確認",
					MessageBoxButtons.YesNo) != DialogResult.Yes)
				{
					return;
				}
			}

			DataList list = new DataList();

			Channel channel = (Channel)this.comboChannel.SelectedIndex;
			Spectrum spectrum = this.wave.GetSpectrum(channel);
			int m = this.checkedListType.CheckedIndices.Count;

			int length = this.wave.Count;
			double fs = this.wave.Header.sampleRate;
			double[] label = new double[length];
			for(int i=0; i<length; ++i)
			{
				label[i] = fs / length * i;
			}
			list.Add("周波数", label);

			for(int i=0; i<m; ++i)
			{
				Property type = (Property)this.checkedListType.CheckedIndices[i];
				double[] data = WaveData.GetData(spectrum, type);
				string title = (string)this.checkedListType.CheckedItems[i];
				list.Add(title, data);
			}

			char delim;
			switch(this.comboDelim.SelectedIndex)
			{
				case 0: delim = ','; break;
				case 1: delim = '\t'; break;
				default: delim = ' '; break;
			}

			try
			{
				list.Save(filename, delim, false);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
	}//class WaveSaveForm
}//namespace AppMain
