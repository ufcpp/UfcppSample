using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Reversi
{
	/// <summary>
	/// Form1 の概要の説明です。
	/// </summary>
	public class ReversiMainForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// オプション設定値
		/// </summary>
		public class Option
		{
			public int board_width = 8;  //盤面の横幅
			public int board_height = 8; //盤面の高さ
			public int max_lookahead = 5;//CPUの先読み段数(多分、多いほうが強い)
		}

		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		private ReversiBoard board; //リバーシの盤面
		private Option option = new Option();
		private ReversiPanel panel;
		private System.Windows.Forms.StatusBar status_bar;
		private System.Windows.Forms.MenuItem menu_start;
		private System.Windows.Forms.MenuItem menu_option;
		private System.Windows.Forms.MenuItem menu_quit;
		private System.Windows.Forms.MainMenu mainMenu; //盤面を表示するためのパネル
		private ReversiColor phase; //今、白黒どちらの攻撃中か

		public ReversiMainForm()
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			this.panel = new ReversiPanel();
			this.panel.Location = new Point(0, 0);
			this.panel.CellClick += new CellClickHandler(OnCellClick);
			this.Controls.Add(panel);
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
		/// デザイナ サポートに必要です。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ReversiMainForm));
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.menu_start = new System.Windows.Forms.MenuItem();
			this.menu_option = new System.Windows.Forms.MenuItem();
			this.menu_quit = new System.Windows.Forms.MenuItem();
			this.status_bar = new System.Windows.Forms.StatusBar();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																						 this.menu_start,
																																						 this.menu_option,
																																						 this.menu_quit});
			// 
			// menu_start
			// 
			this.menu_start.Index = 0;
			this.menu_start.Text = "ゲーム開始";
			this.menu_start.Click += new System.EventHandler(this.menu_start_Click);
			// 
			// menu_option
			// 
			this.menu_option.Index = 1;
			this.menu_option.Text = "オプション";
			this.menu_option.Click += new System.EventHandler(this.menu_option_Click);
			// 
			// menu_quit
			// 
			this.menu_quit.Index = 2;
			this.menu_quit.Text = "終了";
			this.menu_quit.Click += new System.EventHandler(this.menu_quit_Click);
			// 
			// status_bar
			// 
			this.status_bar.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.status_bar.Dock = System.Windows.Forms.DockStyle.None;
			this.status_bar.Location = new System.Drawing.Point(0, -4);
			this.status_bar.Name = "status_bar";
			this.status_bar.Size = new System.Drawing.Size(296, 20);
			this.status_bar.TabIndex = 0;
			// 
			// ReversiMainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(298, 19);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																																	this.status_bar});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Menu = this.mainMenu;
			this.MinimizeBox = false;
			this.Name = "ReversiMainForm";
			this.Text = "Reversi";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new ReversiMainForm());
		}

		/// <summary>
		/// 対人戦時、セル(x,y)がクリックされたときの処理
		/// </summary>
		protected void OnCellClick(int x, int y)
		{
			if(!this.board.Check(x, y, this.phase))
			{
				return;
			}

			this.board[x, y] = this.phase;
			this.panel.UpdateBoard();
			this.phase = ReversiBoard.InverseColor(this.phase);
			ShowPhaseMessage();
			if(!this.board.CheckAll(this.phase))
			{
				GameSet();
			}
		}

		/// <summary>
		/// ゲーム終了時の処理
		/// </summary>
		private void GameSet()
		{
			int black_num, white_num;
			this.board.CountUp(out black_num, out white_num);
			if(black_num > white_num)
				status_bar.Text = black_num + "対" + white_num + "で黒の勝ち";
			else if(black_num < white_num)
				status_bar.Text = black_num + "対" + white_num + "で白の勝ち";
			else
				status_bar.Text = black_num + "対" + white_num + "で引き分け";
			
		}

		/// <summary>
		/// ゲーム開始の処理
		/// </summary>
		private void GameStart()
		{
			this.board = new ReversiBoard(option.board_width, option.board_height);

			this.panel.Board = this.board;

			this.ClientSize = new Size(this.panel.Width, this.panel.Height + this.status_bar.Height);

			this.phase = ReversiColor.Black;
			ShowPhaseMessage();
		}

		private void ShowPhaseMessage()
		{
			if(this.phase == ReversiColor.Black) status_bar.Text = "黒の番です";
			if(this.phase == ReversiColor.White) status_bar.Text = "白の番です";
		}

		private void ShowErrorMessage()
		{
			status_bar.Text = "そこには置けません";
		}

		private void menu_start_Click(object sender, System.EventArgs e)
		{
			GameStart();
		}

		private void menu_quit_Click(object sender, System.EventArgs e)
		{
			DialogResult res = MessageBox.Show("終了しますか？", "終了の確認", MessageBoxButtons.OKCancel);
			if(res == DialogResult.OK)
				this.Close();
		}

		private void menu_option_Click(object sender, System.EventArgs e)
		{
			OptionForm opt_form = new OptionForm(option);
			opt_form.ShowDialog();
		}
	}
}
