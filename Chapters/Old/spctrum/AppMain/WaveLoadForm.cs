using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using WaveAnalysis;

namespace AppMain
{
	/// <summary>
	/// WaveLoadForm の設定項目
	/// </summary>
	public class WaveLoadFormSettings
	{
		public string skipLength;
		public string readLength;
		public bool removeSilent;
		public string threshold;
		public bool relative;

		public bool useReference;
		public bool isNormalized;
		public int type;

		public string skipLengthRef;
		public bool removeSilentRef;
		public string thresholdRef;
		public bool relativeRef;
	}

	/// <summary>
	/// Wave ファイル読込み用のダイアログ。
	/// </summary>
	public class WaveLoadForm : System.Windows.Forms.Form
	{
		#region 手動更新領域
		/// <summary>
		/// Wave データ作成。
		/// </summary>
		/// <returns>作成した Wave</returns>
		public WaveData CreateWave()
		{
			WaveAnalyzer analyzer = new WaveAnalyzer();

			// データ読込み
		{
			int skip = int.Parse(this.textSkipLength.Text);
			int length = int.Parse(this.textReadLength.Text);
			if(this.checkRemoveSilent.Checked)
			{
				double threshold = double.Parse(this.textThreshold.Text);
				bool relative = this.checkRelativeThreshold.Checked;

				analyzer.ReadData(this.textDataName.Text, skip, length, threshold, relative);
			}
			else
			{
				analyzer.ReadData(this.textDataName.Text, skip, length);
			}
		}

			// リファレンス逆畳み込み
			if(this.checkUseReference.Checked)
			{
				int skip = int.Parse(this.textSkipLengthRef.Text);
				int length = int.Parse(this.textReadLength.Text);
				ReferenceType type = (ReferenceType)this.comboReferenceType.SelectedIndex;
				bool isNormalized = this.checkNormalize.Checked;

				if(this.checkRemoveSilent.Checked)
				{
					double threshold = double.Parse(this.textThresholdRef.Text);
					bool relative = this.checkRelativeThresholdRef.Checked;

					analyzer.DeconvoluteReference(this.textReferenceName.Text, skip, length, type, isNormalized, threshold, relative);
				}
				else
				{
					analyzer.DeconvoluteReference(this.textReferenceName.Text, skip, length, type, isNormalized);
				}
			}

			return analyzer.Data;
		}//CreateWave

		/// <summary>
		/// データのファイル名
		/// </summary>
		public string DataFileName
		{
			get{return this.textDataName.Text;}
		}

		/// <summary>
		/// 設定の取得・更新
		/// </summary>
		public WaveLoadFormSettings Setting
		{
			set
			{
				this.textSkipLength.Text            = value.skipLength;
				this.textReadLength.Text            = value.readLength;
				this.checkRemoveSilent.Checked      = value.removeSilent;
				this.textThreshold.Text             = value.threshold;
				this.checkRelativeThreshold.Checked = value.relative;

				this.checkUseReference.Checked = value.useReference;
				this.checkNormalize.Checked    = value.isNormalized;
				this.comboReferenceType.SelectedIndex = value.type;

				this.textSkipLengthRef.Text            = value.skipLengthRef;
				this.checkRemoveSilentRef.Checked      = value.removeSilentRef;
				this.textThresholdRef.Text             = value.thresholdRef;
				this.checkRelativeThresholdRef.Checked = value.relativeRef;
			}
			get
			{
				WaveLoadFormSettings setting = new WaveLoadFormSettings();

				setting.skipLength   = this.textSkipLength.Text;
				setting.readLength   = this.textReadLength.Text;
				setting.removeSilent = this.checkRemoveSilent.Checked;
				setting.threshold    = this.textThreshold.Text;
				setting.relative     = this.checkRelativeThreshold.Checked;

				setting.useReference = this.checkUseReference.Checked;
				setting.isNormalized = this.checkNormalize.Checked;
				setting.type         = this.comboReferenceType.SelectedIndex;

				setting.skipLengthRef   = this.textSkipLengthRef.Text;
				setting.removeSilentRef = this.checkRemoveSilentRef.Checked;
				setting.thresholdRef    = this.textThresholdRef.Text;
				setting.relativeRef     = this.checkRelativeThresholdRef.Checked;

				return setting;
			}
		}//Setting
		#endregion

		private System.Windows.Forms.TextBox textDataName;
		private System.Windows.Forms.TextBox textReferenceName;
		private System.Windows.Forms.Button buttonData;
		private System.Windows.Forms.Button buttonReference;
		private System.Windows.Forms.CheckBox checkRemoveSilent;
		private System.Windows.Forms.TextBox textThreshold;
		private System.Windows.Forms.Label labelThreshold;
		private System.Windows.Forms.CheckBox checkRelativeThreshold;
		private System.Windows.Forms.GroupBox groupRemoveSilent;
		private System.Windows.Forms.Label labelDataName;
		private System.Windows.Forms.GroupBox groupData;
		private System.Windows.Forms.GroupBox groupReference;
		private System.Windows.Forms.CheckBox checkUseReference;
		private System.Windows.Forms.Label labelReferenceName;
		private System.Windows.Forms.CheckBox checkNormalize;
		private System.Windows.Forms.GroupBox groupRange;
		private System.Windows.Forms.Label labelSkipLength;
		private System.Windows.Forms.Label labelReadLength;
		private System.Windows.Forms.TextBox textSkipLength;
		private System.Windows.Forms.TextBox textReadLength;
		private System.Windows.Forms.GroupBox groupRemoveSilentRef;
		private System.Windows.Forms.TextBox textThresholdRef;
		private System.Windows.Forms.Label labelThresholdRef;
		private System.Windows.Forms.CheckBox checkRelativeThresholdRef;
		private System.Windows.Forms.CheckBox checkRemoveSilentRef;
		private System.Windows.Forms.GroupBox groupRangeRef;
		private System.Windows.Forms.TextBox textSkipLengthRef;
		private System.Windows.Forms.Label labelSkipLengthRef;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label labelReferenceType;
		private System.Windows.Forms.ComboBox comboReferenceType;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public WaveLoadForm()
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
			this.textDataName = new System.Windows.Forms.TextBox();
			this.textReferenceName = new System.Windows.Forms.TextBox();
			this.buttonData = new System.Windows.Forms.Button();
			this.buttonReference = new System.Windows.Forms.Button();
			this.checkRemoveSilent = new System.Windows.Forms.CheckBox();
			this.textThreshold = new System.Windows.Forms.TextBox();
			this.labelThreshold = new System.Windows.Forms.Label();
			this.checkRelativeThreshold = new System.Windows.Forms.CheckBox();
			this.groupRemoveSilent = new System.Windows.Forms.GroupBox();
			this.labelDataName = new System.Windows.Forms.Label();
			this.groupData = new System.Windows.Forms.GroupBox();
			this.groupRange = new System.Windows.Forms.GroupBox();
			this.textSkipLength = new System.Windows.Forms.TextBox();
			this.labelReadLength = new System.Windows.Forms.Label();
			this.labelSkipLength = new System.Windows.Forms.Label();
			this.textReadLength = new System.Windows.Forms.TextBox();
			this.checkUseReference = new System.Windows.Forms.CheckBox();
			this.groupReference = new System.Windows.Forms.GroupBox();
			this.comboReferenceType = new System.Windows.Forms.ComboBox();
			this.labelReferenceType = new System.Windows.Forms.Label();
			this.checkNormalize = new System.Windows.Forms.CheckBox();
			this.labelReferenceName = new System.Windows.Forms.Label();
			this.groupRemoveSilentRef = new System.Windows.Forms.GroupBox();
			this.textThresholdRef = new System.Windows.Forms.TextBox();
			this.labelThresholdRef = new System.Windows.Forms.Label();
			this.checkRelativeThresholdRef = new System.Windows.Forms.CheckBox();
			this.checkRemoveSilentRef = new System.Windows.Forms.CheckBox();
			this.groupRangeRef = new System.Windows.Forms.GroupBox();
			this.textSkipLengthRef = new System.Windows.Forms.TextBox();
			this.labelSkipLengthRef = new System.Windows.Forms.Label();
			this.buttonOk = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.groupRemoveSilent.SuspendLayout();
			this.groupData.SuspendLayout();
			this.groupRange.SuspendLayout();
			this.groupReference.SuspendLayout();
			this.groupRemoveSilentRef.SuspendLayout();
			this.groupRangeRef.SuspendLayout();
			this.SuspendLayout();
			// 
			// textDataName
			// 
			this.textDataName.Location = new System.Drawing.Point(64, 16);
			this.textDataName.Name = "textDataName";
			this.textDataName.Size = new System.Drawing.Size(192, 19);
			this.textDataName.TabIndex = 0;
			this.textDataName.Text = "";
			// 
			// textReferenceName
			// 
			this.textReferenceName.Location = new System.Drawing.Point(64, 64);
			this.textReferenceName.Name = "textReferenceName";
			this.textReferenceName.Size = new System.Drawing.Size(192, 19);
			this.textReferenceName.TabIndex = 12;
			this.textReferenceName.Text = "";
			// 
			// buttonData
			// 
			this.buttonData.Location = new System.Drawing.Point(264, 16);
			this.buttonData.Name = "buttonData";
			this.buttonData.Size = new System.Drawing.Size(40, 23);
			this.buttonData.TabIndex = 1;
			this.buttonData.Text = "参照";
			this.buttonData.Click += new System.EventHandler(this.buttonData_Click);
			// 
			// buttonReference
			// 
			this.buttonReference.Location = new System.Drawing.Point(264, 64);
			this.buttonReference.Name = "buttonReference";
			this.buttonReference.Size = new System.Drawing.Size(40, 23);
			this.buttonReference.TabIndex = 13;
			this.buttonReference.Text = "参照";
			this.buttonReference.Click += new System.EventHandler(this.buttonReference_Click);
			// 
			// checkRemoveSilent
			// 
			this.checkRemoveSilent.Location = new System.Drawing.Point(8, 16);
			this.checkRemoveSilent.Name = "checkRemoveSilent";
			this.checkRemoveSilent.Size = new System.Drawing.Size(128, 24);
			this.checkRemoveSilent.TabIndex = 4;
			this.checkRemoveSilent.Text = "無音区間を除去する";
			this.checkRemoveSilent.CheckedChanged += new System.EventHandler(this.checkRemoveSilent_CheckedChanged);
			// 
			// textThreshold
			// 
			this.textThreshold.Enabled = false;
			this.textThreshold.Location = new System.Drawing.Point(144, 40);
			this.textThreshold.Name = "textThreshold";
			this.textThreshold.Size = new System.Drawing.Size(56, 19);
			this.textThreshold.TabIndex = 6;
			this.textThreshold.Text = "0";
			this.textThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelThreshold
			// 
			this.labelThreshold.Location = new System.Drawing.Point(144, 24);
			this.labelThreshold.Name = "labelThreshold";
			this.labelThreshold.Size = new System.Drawing.Size(32, 16);
			this.labelThreshold.TabIndex = 1;
			this.labelThreshold.Text = "閾値";
			this.labelThreshold.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// checkRelativeThreshold
			// 
			this.checkRelativeThreshold.Enabled = false;
			this.checkRelativeThreshold.Location = new System.Drawing.Point(8, 40);
			this.checkRelativeThreshold.Name = "checkRelativeThreshold";
			this.checkRelativeThreshold.Size = new System.Drawing.Size(128, 24);
			this.checkRelativeThreshold.TabIndex = 5;
			this.checkRelativeThreshold.Text = "閾値に相対値を使う";
			// 
			// groupRemoveSilent
			// 
			this.groupRemoveSilent.Controls.AddRange(new System.Windows.Forms.Control[] {
																																										this.textThreshold,
																																										this.labelThreshold,
																																										this.checkRelativeThreshold,
																																										this.checkRemoveSilent});
			this.groupRemoveSilent.Location = new System.Drawing.Point(176, 56);
			this.groupRemoveSilent.Name = "groupRemoveSilent";
			this.groupRemoveSilent.Size = new System.Drawing.Size(208, 72);
			this.groupRemoveSilent.TabIndex = 5;
			this.groupRemoveSilent.TabStop = false;
			this.groupRemoveSilent.Text = "無音区間の除去";
			// 
			// labelDataName
			// 
			this.labelDataName.Location = new System.Drawing.Point(8, 16);
			this.labelDataName.Name = "labelDataName";
			this.labelDataName.Size = new System.Drawing.Size(56, 16);
			this.labelDataName.TabIndex = 18;
			this.labelDataName.Text = "ファイル名";
			this.labelDataName.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// groupData
			// 
			this.groupData.Controls.AddRange(new System.Windows.Forms.Control[] {
																																						this.labelDataName,
																																						this.textDataName,
																																						this.buttonData,
																																						this.groupRange,
																																						this.groupRemoveSilent,
																																						this.checkUseReference});
			this.groupData.Location = new System.Drawing.Point(8, 8);
			this.groupData.Name = "groupData";
			this.groupData.Size = new System.Drawing.Size(392, 160);
			this.groupData.TabIndex = 7;
			this.groupData.TabStop = false;
			this.groupData.Text = "データファイル";
			// 
			// groupRange
			// 
			this.groupRange.Controls.AddRange(new System.Windows.Forms.Control[] {
																																						 this.textSkipLength,
																																						 this.labelReadLength,
																																						 this.labelSkipLength,
																																						 this.textReadLength});
			this.groupRange.Location = new System.Drawing.Point(8, 56);
			this.groupRange.Name = "groupRange";
			this.groupRange.Size = new System.Drawing.Size(160, 72);
			this.groupRange.TabIndex = 9;
			this.groupRange.TabStop = false;
			this.groupRange.Text = "読込み区間";
			// 
			// textSkipLength
			// 
			this.textSkipLength.Location = new System.Drawing.Point(72, 16);
			this.textSkipLength.Name = "textSkipLength";
			this.textSkipLength.Size = new System.Drawing.Size(80, 19);
			this.textSkipLength.TabIndex = 2;
			this.textSkipLength.Text = "0";
			this.textSkipLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelReadLength
			// 
			this.labelReadLength.Location = new System.Drawing.Point(8, 40);
			this.labelReadLength.Name = "labelReadLength";
			this.labelReadLength.Size = new System.Drawing.Size(64, 16);
			this.labelReadLength.TabIndex = 1;
			this.labelReadLength.Text = "読込み長";
			this.labelReadLength.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelSkipLength
			// 
			this.labelSkipLength.Location = new System.Drawing.Point(8, 16);
			this.labelSkipLength.Name = "labelSkipLength";
			this.labelSkipLength.Size = new System.Drawing.Size(64, 16);
			this.labelSkipLength.TabIndex = 0;
			this.labelSkipLength.Text = "読飛ばし長";
			this.labelSkipLength.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textReadLength
			// 
			this.textReadLength.Location = new System.Drawing.Point(72, 40);
			this.textReadLength.Name = "textReadLength";
			this.textReadLength.Size = new System.Drawing.Size(80, 19);
			this.textReadLength.TabIndex = 3;
			this.textReadLength.Text = "1024";
			this.textReadLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// checkUseReference
			// 
			this.checkUseReference.Location = new System.Drawing.Point(8, 128);
			this.checkUseReference.Name = "checkUseReference";
			this.checkUseReference.Size = new System.Drawing.Size(160, 24);
			this.checkUseReference.TabIndex = 7;
			this.checkUseReference.Text = "リファレンスデータを使用する";
			this.checkUseReference.CheckedChanged += new System.EventHandler(this.checkUseReference_CheckedChanged);
			// 
			// groupReference
			// 
			this.groupReference.Controls.AddRange(new System.Windows.Forms.Control[] {
																																								 this.comboReferenceType,
																																								 this.labelReferenceType,
																																								 this.checkNormalize,
																																								 this.labelReferenceName,
																																								 this.buttonReference,
																																								 this.textReferenceName,
																																								 this.groupRemoveSilentRef,
																																								 this.groupRangeRef});
			this.groupReference.Enabled = false;
			this.groupReference.Location = new System.Drawing.Point(8, 168);
			this.groupReference.Name = "groupReference";
			this.groupReference.Size = new System.Drawing.Size(392, 176);
			this.groupReference.TabIndex = 8;
			this.groupReference.TabStop = false;
			this.groupReference.Text = "リファレンスデータ";
			// 
			// comboReferenceType
			// 
			this.comboReferenceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboReferenceType.Items.AddRange(new object[] {
																														"両側チャネル",
																														"左チャネル",
																														"右チャネル",
																														"逆側チャネル",
																														"クロストーク"});
			this.comboReferenceType.Location = new System.Drawing.Point(96, 40);
			this.comboReferenceType.Name = "comboReferenceType";
			this.comboReferenceType.Size = new System.Drawing.Size(121, 20);
			this.comboReferenceType.TabIndex = 15;
			// 
			// labelReferenceType
			// 
			this.labelReferenceType.Location = new System.Drawing.Point(16, 40);
			this.labelReferenceType.Name = "labelReferenceType";
			this.labelReferenceType.Size = new System.Drawing.Size(88, 16);
			this.labelReferenceType.TabIndex = 14;
			this.labelReferenceType.Text = "リファレンスタイプ";
			this.labelReferenceType.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// checkNormalize
			// 
			this.checkNormalize.Location = new System.Drawing.Point(8, 16);
			this.checkNormalize.Name = "checkNormalize";
			this.checkNormalize.Size = new System.Drawing.Size(176, 24);
			this.checkNormalize.TabIndex = 8;
			this.checkNormalize.Text = "リファレンスデータを正規化する";
			// 
			// labelReferenceName
			// 
			this.labelReferenceName.Location = new System.Drawing.Point(8, 64);
			this.labelReferenceName.Name = "labelReferenceName";
			this.labelReferenceName.Size = new System.Drawing.Size(56, 16);
			this.labelReferenceName.TabIndex = 7;
			this.labelReferenceName.Text = "ファイル名";
			this.labelReferenceName.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// groupRemoveSilentRef
			// 
			this.groupRemoveSilentRef.Controls.AddRange(new System.Windows.Forms.Control[] {
																																											 this.textThresholdRef,
																																											 this.labelThresholdRef,
																																											 this.checkRelativeThresholdRef,
																																											 this.checkRemoveSilentRef});
			this.groupRemoveSilentRef.Location = new System.Drawing.Point(176, 96);
			this.groupRemoveSilentRef.Name = "groupRemoveSilentRef";
			this.groupRemoveSilentRef.Size = new System.Drawing.Size(208, 72);
			this.groupRemoveSilentRef.TabIndex = 10;
			this.groupRemoveSilentRef.TabStop = false;
			this.groupRemoveSilentRef.Text = "無音区間の除去";
			// 
			// textThresholdRef
			// 
			this.textThresholdRef.Enabled = false;
			this.textThresholdRef.Location = new System.Drawing.Point(144, 40);
			this.textThresholdRef.Name = "textThresholdRef";
			this.textThresholdRef.Size = new System.Drawing.Size(56, 19);
			this.textThresholdRef.TabIndex = 17;
			this.textThresholdRef.Text = "0";
			this.textThresholdRef.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelThresholdRef
			// 
			this.labelThresholdRef.Location = new System.Drawing.Point(144, 24);
			this.labelThresholdRef.Name = "labelThresholdRef";
			this.labelThresholdRef.Size = new System.Drawing.Size(32, 16);
			this.labelThresholdRef.TabIndex = 1;
			this.labelThresholdRef.Text = "閾値";
			this.labelThresholdRef.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// checkRelativeThresholdRef
			// 
			this.checkRelativeThresholdRef.Enabled = false;
			this.checkRelativeThresholdRef.Location = new System.Drawing.Point(8, 40);
			this.checkRelativeThresholdRef.Name = "checkRelativeThresholdRef";
			this.checkRelativeThresholdRef.Size = new System.Drawing.Size(128, 24);
			this.checkRelativeThresholdRef.TabIndex = 16;
			this.checkRelativeThresholdRef.Text = "閾値に相対値を使う";
			// 
			// checkRemoveSilentRef
			// 
			this.checkRemoveSilentRef.Location = new System.Drawing.Point(8, 16);
			this.checkRemoveSilentRef.Name = "checkRemoveSilentRef";
			this.checkRemoveSilentRef.Size = new System.Drawing.Size(128, 24);
			this.checkRemoveSilentRef.TabIndex = 15;
			this.checkRemoveSilentRef.Text = "無音区間を除去する";
			this.checkRemoveSilentRef.CheckedChanged += new System.EventHandler(this.checkRemoveSilentRef_CheckedChanged);
			// 
			// groupRangeRef
			// 
			this.groupRangeRef.Controls.AddRange(new System.Windows.Forms.Control[] {
																																								this.textSkipLengthRef,
																																								this.labelSkipLengthRef});
			this.groupRangeRef.Location = new System.Drawing.Point(8, 96);
			this.groupRangeRef.Name = "groupRangeRef";
			this.groupRangeRef.Size = new System.Drawing.Size(160, 72);
			this.groupRangeRef.TabIndex = 11;
			this.groupRangeRef.TabStop = false;
			this.groupRangeRef.Text = "読込み区間";
			// 
			// textSkipLengthRef
			// 
			this.textSkipLengthRef.Location = new System.Drawing.Point(72, 16);
			this.textSkipLengthRef.Name = "textSkipLengthRef";
			this.textSkipLengthRef.Size = new System.Drawing.Size(80, 19);
			this.textSkipLengthRef.TabIndex = 14;
			this.textSkipLengthRef.Text = "0";
			this.textSkipLengthRef.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelSkipLengthRef
			// 
			this.labelSkipLengthRef.Location = new System.Drawing.Point(8, 16);
			this.labelSkipLengthRef.Name = "labelSkipLengthRef";
			this.labelSkipLengthRef.Size = new System.Drawing.Size(64, 16);
			this.labelSkipLengthRef.TabIndex = 0;
			this.labelSkipLengthRef.Text = "読飛ばし長";
			this.labelSkipLengthRef.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// buttonOk
			// 
			this.buttonOk.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonOk.Location = new System.Drawing.Point(248, 352);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.TabIndex = 9;
			this.buttonOk.Text = "OK";
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(328, 352);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.TabIndex = 10;
			this.buttonCancel.Text = "cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// WaveLoadForm
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(410, 383);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																																	this.buttonCancel,
																																	this.buttonOk,
																																	this.groupReference,
																																	this.groupData});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "WaveLoadForm";
			this.Text = "Wave ファイル読込み設定";
			this.groupRemoveSilent.ResumeLayout(false);
			this.groupData.ResumeLayout(false);
			this.groupRange.ResumeLayout(false);
			this.groupReference.ResumeLayout(false);
			this.groupRemoveSilentRef.ResumeLayout(false);
			this.groupRangeRef.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonData_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "wave file(*.wav)|*.wav";

			dlg.Title = "データファイルの選択";
			if(dlg.ShowDialog() != DialogResult.OK) return;
			this.textDataName.Text = dlg.FileName;
		}

		private void buttonReference_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "wave file(*.wav)|*.wav";

			dlg.Title = "リファレンスファイルの選択";
			if(dlg.ShowDialog() != DialogResult.OK) return;
			this.textReferenceName.Text = dlg.FileName;
		}

		private void buttonOk_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void checkRemoveSilent_CheckedChanged(object sender, System.EventArgs e)
		{
			this.checkRelativeThreshold.Enabled = this.checkRemoveSilent.Checked;
			this.textThreshold.Enabled = this.checkRemoveSilent.Checked;
		}

		private void checkRemoveSilentRef_CheckedChanged(object sender, System.EventArgs e)
		{
			this.checkRelativeThresholdRef.Enabled = this.checkRemoveSilentRef.Checked;
			this.textThresholdRef.Enabled = this.checkRemoveSilentRef.Checked;
		}

		private void checkUseReference_CheckedChanged(object sender, System.EventArgs e)
		{
			this.groupReference.Enabled = this.checkUseReference.Checked;
		}

	}
}
