using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

using Wave;
using Graph;
using WaveAnalysis;

namespace AppMain
{
	/// <summary>
	/// MainForm の概要の説明です。
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem menuItemFile;
		private System.Windows.Forms.MenuItem menuItemEnd;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.MenuItem menuItemFileOpen;

		#region 手動行進用領域
		AppSettings appSettings = new AppSettings();
		const string settingFileName = @"\setting.xml";

		/// <summary>
		/// アプリの設定をデフォルト設定にする。
		/// 設定ファイルがなかった場合に呼ばれる。
		/// </summary>
		private void SetAppSettings()
		{
			this.appSettings.loadForm.skipLength   = "0";
			this.appSettings.loadForm.readLength   = "1024";
			this.appSettings.loadForm.removeSilent = false;
			this.appSettings.loadForm.threshold    = "0";
			this.appSettings.loadForm.relative     = false;

			this.appSettings.loadForm.useReference = false;
			this.appSettings.loadForm.isNormalized = false;
			this.appSettings.loadForm.type         = 0;

			this.appSettings.loadForm.skipLengthRef   = "0";
			this.appSettings.loadForm.removeSilentRef = false;
			this.appSettings.loadForm.thresholdRef    = "0";
			this.appSettings.loadForm.relativeRef     = false;
		}//SetAppSettings

		/// <summary>
		/// アプリの設定を設定ファイルから読み込む。
		/// </summary>
		private void LoadAppSettings()
		{
			StreamReader reader = null;
			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
				reader = new StreamReader(Application.StartupPath + MainForm.settingFileName);
				this.appSettings = (AppSettings)serializer.Deserialize(reader);
			}
			catch(FileNotFoundException)
			{
				this.SetAppSettings();
				return;
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				if(reader != null) reader.Close();
			}
			this.Left   = this.appSettings.left;
			this.Top    = this.appSettings.top;
			this.Width  = this.appSettings.width;
			this.Height = this.appSettings.height;
		}//LoadAppSettings

		/// <summary>
		/// アプリの設定を設定ファイルに保存する。
		/// </summary>
		private void SaveAppSettings()
		{
			this.appSettings.left   = this.Left;
			this.appSettings.top    = this.Top;
			this.appSettings.width  = this.Width;
			this.appSettings.height = this.Height;

			StreamWriter writer = null;
			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
				writer = new StreamWriter(Application.StartupPath + MainForm.settingFileName);
				serializer.Serialize(writer, this.appSettings);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				if(writer != null) writer.Close();
			}
		}//SaveAppSettings

		/// <summary>
		/// 初期化
		/// </summary>
		private void Initialize()
		{
			this.LoadAppSettings();
		}

		public void CreateWaveForm(string title, WaveData wave)
		{
			WaveGraphForm form = new WaveGraphForm(wave);
			form.Text = title;
			form.MdiParent = this;
			form.Show();
		}
		#endregion

		public MainForm()
		{
			InitializeComponent();

			Initialize();
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.menuItemFile = new System.Windows.Forms.MenuItem();
			this.menuItemFileOpen = new System.Windows.Forms.MenuItem();
			this.menuItemEnd = new System.Windows.Forms.MenuItem();
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																						 this.menuItemFile});
			// 
			// menuItemFile
			// 
			this.menuItemFile.Index = 0;
			this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																								 this.menuItemFileOpen,
																																								 this.menuItemEnd});
			this.menuItemFile.Text = "ファイル";
			// 
			// menuItemFileOpen
			// 
			this.menuItemFileOpen.Index = 0;
			this.menuItemFileOpen.Text = "開く";
			this.menuItemFileOpen.Click += new System.EventHandler(this.menuItemFileOpen_Click);
			// 
			// menuItemEnd
			// 
			this.menuItemEnd.Index = 1;
			this.menuItemEnd.Text = "終了";
			this.menuItemEnd.Click += new System.EventHandler(this.menuItemEnd_Click);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(632, 433);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.IsMdiContainer = true;
			this.Menu = this.mainMenu;
			this.Name = "MainForm";
			this.Text = "Wave Analyzer";
			this.Closed += new System.EventHandler(this.MainForm_Closed);

		}
		#endregion

		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
//		[STAThread]
		static void Main(string[] args)
		{
//!			AppTest.Hoge(args);
			Application.Run(new MainForm());
		}

		private void menuItemEnd_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void menuItemFileOpen_Click(object sender, System.EventArgs e)
		{
			WaveLoadForm dlg = new WaveLoadForm();
			dlg.Setting = this.appSettings.loadForm;
			if(dlg.ShowDialog() != DialogResult.OK) return;
			this.appSettings.loadForm = dlg.Setting;

			WaveData wave;
			string dataName;
			try
			{
				wave = dlg.CreateWave();
				dataName = dlg.DataFileName;
			}
			catch(Exception exc)
			{
				MessageBox.Show("ファイルの読み込み中にエラーが発生しました。\n" + exc.Message);
				return;
			}
			CreateWaveForm(dataName, wave);
		}

		private void MainForm_Closed(object sender, System.EventArgs e)
		{
			this.SaveAppSettings();
		}
	}//class MainForm
}//namespace AppMain
