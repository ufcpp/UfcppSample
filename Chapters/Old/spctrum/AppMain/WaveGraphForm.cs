using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

using Graph;
using SpectrumAnalysis;
using WaveAnalysis;

namespace AppMain
{
	/// <summary>
	/// WaveForm の概要の説明です。
	/// </summary>
	public class WaveGraphForm : System.Windows.Forms.Form
	{
		#region 手動更新領域
		WaveData wave;

		static readonly Brush brush = new SolidBrush(SystemColors.WindowText);
		static readonly Font  font  = new Font(FontFamily.GenericSansSerif, 8);
		static readonly Pen[] pens  = new Pen[]{
																						 new Pen(Color.Black),
																						 new Pen(Color.Crimson),
																						 new Pen(Color.DarkBlue),
																						 new Pen(Color.DarkGreen),
																						 new Pen(Color.Brown),
																						 new Pen(Color.DarkViolet)
																					 };

		#endregion

		private System.Windows.Forms.Label labelChannel;
		private System.Windows.Forms.Label labelType;
		private System.Windows.Forms.ComboBox comboType;
		private System.Windows.Forms.Label labelXAxis;
		private System.Windows.Forms.ComboBox comboXAxis;
		private System.Windows.Forms.GroupBox groupXAxis;
		private System.Windows.Forms.CheckBox checkXLog;
		private System.Windows.Forms.GroupBox groupYAxis;
		private System.Windows.Forms.Label labelYMax;
		private System.Windows.Forms.Label labelYMin;
		private System.Windows.Forms.CheckBox checkYAxisAuto;
		private System.Windows.Forms.TextBox textYMax;
		private System.Windows.Forms.TextBox textYMin;
		private System.Windows.Forms.CheckBox checkYLog;
		private System.Windows.Forms.Button buttonShowGraph;
		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.ComboBox comboChannel1;
		private System.Windows.Forms.CheckBox checkOneGraph;
		private System.Windows.Forms.ComboBox comboChannel2;
		private System.Windows.Forms.Button buttonSave;

		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public WaveGraphForm(WaveData wave)
		{
			InitializeComponent();

			this.wave = wave;

			this.comboChannel1.SelectedIndex = 0;
			this.comboChannel2.SelectedIndex = 1;
			this.comboType.SelectedIndex = 0;
			this.comboXAxis.SelectedIndex = 0;
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
			this.labelType = new System.Windows.Forms.Label();
			this.comboChannel1 = new System.Windows.Forms.ComboBox();
			this.comboType = new System.Windows.Forms.ComboBox();
			this.labelXAxis = new System.Windows.Forms.Label();
			this.comboXAxis = new System.Windows.Forms.ComboBox();
			this.groupXAxis = new System.Windows.Forms.GroupBox();
			this.checkXLog = new System.Windows.Forms.CheckBox();
			this.groupYAxis = new System.Windows.Forms.GroupBox();
			this.textYMax = new System.Windows.Forms.TextBox();
			this.checkYAxisAuto = new System.Windows.Forms.CheckBox();
			this.labelYMax = new System.Windows.Forms.Label();
			this.labelYMin = new System.Windows.Forms.Label();
			this.textYMin = new System.Windows.Forms.TextBox();
			this.checkYLog = new System.Windows.Forms.CheckBox();
			this.buttonShowGraph = new System.Windows.Forms.Button();
			this.buttonClose = new System.Windows.Forms.Button();
			this.comboChannel2 = new System.Windows.Forms.ComboBox();
			this.checkOneGraph = new System.Windows.Forms.CheckBox();
			this.buttonSave = new System.Windows.Forms.Button();
			this.groupXAxis.SuspendLayout();
			this.groupYAxis.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelChannel
			// 
			this.labelChannel.Location = new System.Drawing.Point(8, 8);
			this.labelChannel.Name = "labelChannel";
			this.labelChannel.Size = new System.Drawing.Size(48, 16);
			this.labelChannel.TabIndex = 2;
			this.labelChannel.Text = "チャネル";
			this.labelChannel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelType
			// 
			this.labelType.Location = new System.Drawing.Point(8, 56);
			this.labelType.Name = "labelType";
			this.labelType.Size = new System.Drawing.Size(48, 16);
			this.labelType.TabIndex = 3;
			this.labelType.Text = "特性";
			this.labelType.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// comboChannel1
			// 
			this.comboChannel1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboChannel1.Items.AddRange(new object[] {
																											 "Left",
																											 "Right",
																											 "L/R",
																											 "R/L",
																											 "Middle",
																											 "Side",
																											 "M/S",
																											 "S/M"});
			this.comboChannel1.Location = new System.Drawing.Point(56, 8);
			this.comboChannel1.Name = "comboChannel1";
			this.comboChannel1.Size = new System.Drawing.Size(80, 20);
			this.comboChannel1.TabIndex = 4;
			// 
			// comboType
			// 
			this.comboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboType.Items.AddRange(new object[] {
																									 "振幅特性",
																									 "位相特性",
																									 "位相遅延特性",
																									 "群遅延特性",
																									 "最小位相",
																									 "オールパス位相",
																									 "時系列"});
			this.comboType.Location = new System.Drawing.Point(56, 56);
			this.comboType.Name = "comboType";
			this.comboType.Size = new System.Drawing.Size(96, 20);
			this.comboType.TabIndex = 5;
			// 
			// labelXAxis
			// 
			this.labelXAxis.Location = new System.Drawing.Point(8, 16);
			this.labelXAxis.Name = "labelXAxis";
			this.labelXAxis.Size = new System.Drawing.Size(56, 16);
			this.labelXAxis.TabIndex = 6;
			this.labelXAxis.Text = "表示項目";
			this.labelXAxis.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// comboXAxis
			// 
			this.comboXAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboXAxis.Items.AddRange(new object[] {
																										"周波数",
																										"角周波数",
																										"正規化周波数",
																										"正規化角周波数"});
			this.comboXAxis.Location = new System.Drawing.Point(64, 16);
			this.comboXAxis.Name = "comboXAxis";
			this.comboXAxis.Size = new System.Drawing.Size(121, 20);
			this.comboXAxis.TabIndex = 7;
			// 
			// groupXAxis
			// 
			this.groupXAxis.Controls.AddRange(new System.Windows.Forms.Control[] {
																																						 this.checkXLog,
																																						 this.comboXAxis,
																																						 this.labelXAxis});
			this.groupXAxis.Location = new System.Drawing.Point(8, 88);
			this.groupXAxis.Name = "groupXAxis";
			this.groupXAxis.Size = new System.Drawing.Size(192, 64);
			this.groupXAxis.TabIndex = 8;
			this.groupXAxis.TabStop = false;
			this.groupXAxis.Text = "X軸";
			// 
			// checkXLog
			// 
			this.checkXLog.Enabled = false;
			this.checkXLog.Location = new System.Drawing.Point(64, 40);
			this.checkXLog.Name = "checkXLog";
			this.checkXLog.Size = new System.Drawing.Size(88, 16);
			this.checkXLog.TabIndex = 8;
			this.checkXLog.Text = "対数";
			// 
			// groupYAxis
			// 
			this.groupYAxis.Controls.AddRange(new System.Windows.Forms.Control[] {
																																						 this.textYMax,
																																						 this.checkYAxisAuto,
																																						 this.labelYMax,
																																						 this.labelYMin,
																																						 this.textYMin,
																																						 this.checkYLog});
			this.groupYAxis.Location = new System.Drawing.Point(8, 152);
			this.groupYAxis.Name = "groupYAxis";
			this.groupYAxis.Size = new System.Drawing.Size(192, 112);
			this.groupYAxis.TabIndex = 9;
			this.groupYAxis.TabStop = false;
			this.groupYAxis.Text = "Y軸";
			// 
			// textYMax
			// 
			this.textYMax.Enabled = false;
			this.textYMax.Location = new System.Drawing.Point(64, 40);
			this.textYMax.Name = "textYMax";
			this.textYMax.TabIndex = 2;
			this.textYMax.Text = "0";
			this.textYMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// checkYAxisAuto
			// 
			this.checkYAxisAuto.Checked = true;
			this.checkYAxisAuto.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkYAxisAuto.Location = new System.Drawing.Point(8, 16);
			this.checkYAxisAuto.Name = "checkYAxisAuto";
			this.checkYAxisAuto.Size = new System.Drawing.Size(168, 24);
			this.checkYAxisAuto.TabIndex = 1;
			this.checkYAxisAuto.Text = "表示範囲を自動で設定する";
			this.checkYAxisAuto.CheckedChanged += new System.EventHandler(this.checkYAxisAuto_CheckedChanged);
			// 
			// labelYMax
			// 
			this.labelYMax.Enabled = false;
			this.labelYMax.Location = new System.Drawing.Point(16, 40);
			this.labelYMax.Name = "labelYMax";
			this.labelYMax.Size = new System.Drawing.Size(48, 16);
			this.labelYMax.TabIndex = 0;
			this.labelYMax.Text = "最大値";
			this.labelYMax.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelYMin
			// 
			this.labelYMin.Enabled = false;
			this.labelYMin.Location = new System.Drawing.Point(16, 64);
			this.labelYMin.Name = "labelYMin";
			this.labelYMin.Size = new System.Drawing.Size(48, 16);
			this.labelYMin.TabIndex = 0;
			this.labelYMin.Text = "最小値";
			this.labelYMin.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textYMin
			// 
			this.textYMin.Enabled = false;
			this.textYMin.Location = new System.Drawing.Point(64, 64);
			this.textYMin.Name = "textYMin";
			this.textYMin.TabIndex = 2;
			this.textYMin.Text = "0";
			this.textYMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// checkYLog
			// 
			this.checkYLog.Enabled = false;
			this.checkYLog.Location = new System.Drawing.Point(64, 88);
			this.checkYLog.Name = "checkYLog";
			this.checkYLog.Size = new System.Drawing.Size(88, 16);
			this.checkYLog.TabIndex = 8;
			this.checkYLog.Text = "対数";
			// 
			// buttonShowGraph
			// 
			this.buttonShowGraph.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonShowGraph.Location = new System.Drawing.Point(208, 210);
			this.buttonShowGraph.Name = "buttonShowGraph";
			this.buttonShowGraph.TabIndex = 10;
			this.buttonShowGraph.Text = "グラフ表示";
			this.buttonShowGraph.Click += new System.EventHandler(this.buttonShowGraph_Click);
			// 
			// buttonClose
			// 
			this.buttonClose.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonClose.Location = new System.Drawing.Point(208, 242);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.TabIndex = 11;
			this.buttonClose.Text = "閉じる";
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// comboChannel2
			// 
			this.comboChannel2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboChannel2.Items.AddRange(new object[] {
																											 "Left",
																											 "Right",
																											 "L/R",
																											 "R/L",
																											 "Middle",
																											 "Side",
																											 "M/S",
																											 "S/M",
																											 "なし"});
			this.comboChannel2.Location = new System.Drawing.Point(144, 8);
			this.comboChannel2.Name = "comboChannel2";
			this.comboChannel2.Size = new System.Drawing.Size(80, 20);
			this.comboChannel2.TabIndex = 4;
			// 
			// checkOneGraph
			// 
			this.checkOneGraph.Location = new System.Drawing.Point(16, 32);
			this.checkOneGraph.Name = "checkOneGraph";
			this.checkOneGraph.Size = new System.Drawing.Size(112, 16);
			this.checkOneGraph.TabIndex = 12;
			this.checkOneGraph.Text = "同一グラフに表示";
			// 
			// buttonSave
			// 
			this.buttonSave.Location = new System.Drawing.Point(208, 176);
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.TabIndex = 13;
			this.buttonSave.Text = "保存";
			this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
			// 
			// WaveGraphForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(288, 271);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																																	this.buttonSave,
																																	this.checkOneGraph,
																																	this.buttonClose,
																																	this.buttonShowGraph,
																																	this.groupYAxis,
																																	this.groupXAxis,
																																	this.comboType,
																																	this.comboChannel1,
																																	this.labelType,
																																	this.labelChannel,
																																	this.comboChannel2});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "WaveGraphForm";
			this.Text = "WaveGraphForm";
			this.groupXAxis.ResumeLayout(false);
			this.groupYAxis.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private double[] GetLabel(out double labelMax)
		{
			if(this.comboType.SelectedIndex >= 6)
			{
				int len = this.wave.Left.TimeLength;
				double[] tmp = new double[len];
				for(int i=0; i<len ;++i) tmp[i] = i;
				labelMax = len;
				return tmp;
			}
			else
			{
				int len = this.wave.Left.Count - 1;
				double[] tmp = new double[len];
				double df;

				switch(this.comboXAxis.SelectedIndex)
				{
					case 0:
						df = 0.5 * this.wave.Header.sampleRate / len;
						labelMax = 0.5 * this.wave.Header.sampleRate;
						break;
					case 1:
						df = Math.PI * this.wave.Header.sampleRate / len;
						labelMax = Math.PI * this.wave.Header.sampleRate;
						break;
					case 2:
						df = 0.5 / len;
						labelMax = 0.5;
						break;
					case 3:
						df = Math.PI / len;
						labelMax = 4;
						break;
					default:
						df = 0;
						labelMax = 0;
						break;
				}
				for(int i=0; i<len; ++i)
					tmp[i] = df * i;
				return tmp;
			}
		}

		private void ShowSingleGraph(params double[][] data)
		{
			GraphForm form = new GraphForm();

			double labelMax;
			double[] label = GetLabel(out labelMax);

			for(int i=0; i<data.Length; ++i)
			{
				int k = i % pens.Length;
				form.Graph.AddEntry(label, data[i], pens[k]);
			}

			form.Graph.SetXAxis(0, labelMax, 4, font, brush);
			if(this.checkYAxisAuto.Checked)
			{
				form.Graph.SetYAxis(0, 0, 5, font, brush);
				form.Graph.AutoScaleY();
			}
			else
			{
				double min = double.Parse(this.textYMin.Text);
				double max = double.Parse(this.textYMax.Text);
				form.Graph.SetYAxis(min, max, 5, font, brush);
			}

			form.Text = this.Text;
			form.MdiParent = this.MdiParent;
			form.Show();
		}

		private void ShowTwinGraph(double[] data1, double[] data2)
		{
			GraphForm2 form = new GraphForm2();

			double labelMax;
			double[] label = GetLabel(out labelMax);

			form.GraphL.AddEntry(label, data1, pens[0]);
			form.GraphR.AddEntry(label, data2, pens[1]);

			form.GraphL.SetXAxis(0, labelMax, 4, font, brush);
			form.GraphR.SetXAxis(0, labelMax, 4, font, brush);
			if(this.checkYAxisAuto.Checked)
			{
				form.GraphL.SetYAxis(0, 0, 5, font, brush);
				form.GraphL.AutoScaleY();
				form.GraphR.SetYAxis(0, 0, 5, font, brush);
				form.GraphR.AutoScaleY();
			}
			else
			{
				double min = double.Parse(this.textYMin.Text);
				double max = double.Parse(this.textYMax.Text);
				form.GraphL.SetYAxis(min, max, 5, font, brush);
				form.GraphR.SetYAxis(min, max, 5, font, brush);
			}

			form.Text = this.Text;
			form.MdiParent = this.MdiParent;
			form.Show();
		}

		private void buttonShowGraph_Click(object sender, System.EventArgs e)
		{
			double[] data1 = this.wave.GetData((Channel)this.comboChannel1.SelectedIndex, (Property)this.comboType.SelectedIndex);
			double[] data2 = this.wave.GetData((Channel)this.comboChannel2.SelectedIndex, (Property)this.comboType.SelectedIndex);

			if(data2 == null)
				this.ShowSingleGraph(data1);
			else if(this.checkOneGraph.Checked)
				this.ShowSingleGraph(data1, data2);
			else
				this.ShowTwinGraph(data1, data2);
		}

		private void checkYAxisAuto_CheckedChanged(object sender, System.EventArgs e)
		{
			this.labelYMin.Enabled = !this.checkYAxisAuto.Checked;
			this.labelYMax.Enabled = !this.checkYAxisAuto.Checked;
			this.textYMin.Enabled = !this.checkYAxisAuto.Checked;
			this.textYMax.Enabled = !this.checkYAxisAuto.Checked;
		}

		private void buttonSave_Click(object sender, System.EventArgs e)
		{
			WaveSaveForm form = new WaveSaveForm(this.wave, new FileInfo(this.Text));
			form.Text = this.Text;
			form.MdiParent = this.MdiParent;
			form.Show();
		}
	}
}
