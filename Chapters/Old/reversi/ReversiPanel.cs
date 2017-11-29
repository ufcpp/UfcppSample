using System;
using System.Drawing;

namespace Reversi
{
	/// <summary>
	/// ReversiPanelの位置(x,y)にあるセルがクリックされたときに呼び出されるデリゲート
	/// </summary>
	public delegate void CellClickHandler(int x, int y);

	/// <summary>
	/// ReversiPanle の概要の説明です。
	/// </summary>
	public class ReversiPanel : System.Windows.Forms.Panel
	{
		/// <summary>
		/// オセロのセルクラス
		/// </summary>
		internal class ReversiCell : System.Windows.Forms.PictureBox
		{
			public readonly int x;
			public readonly int y;
			private ReversiPanel parent;

			public void SetColor(ReversiColor color)
			{
				Image img = parent.none_image;
				if(color == ReversiColor.Black) img = parent.black_image;
				if(color == ReversiColor.White) img = parent.white_image;
				if(this.Image != img) this.Image = img;
			}

			public ReversiCell(int x, int y, ReversiPanel parent)
			{
				this.x = x;
				this.y = y;
				this.parent = parent;
			}
		}//class ReversiCell


		//---------------------------------------------------------
		public event CellClickHandler CellClick;//セルがクリックされたときに呼ばれるハンドラ

		protected System.Drawing.Image black_image; //黒い駒の絵
		protected System.Drawing.Image white_image; //白い駒の絵
		protected System.Drawing.Image none_image;  //何もないマスの絵

		private ReversiBoard board;
		private ReversiCell[,] cells;

		public ReversiPanel(ReversiBoard board, string image_name)
		{
			none_image  = System.Drawing.Image.FromFile(@"image\" + image_name + "n.png");
			white_image = System.Drawing.Image.FromFile(@"image\" + image_name + "w.png");
			black_image = System.Drawing.Image.FromFile(@"image\" + image_name + "b.png");

			this.board = board;
			if(board != null)
				CreateCells();
			else
				this.Size = new Size(0, 0);
		}

		public ReversiBoard Board
		{
			set
			{
				if(this.board == null || this.board.Width!=value.Width || this.board.Height!=value.Height)
				{
					this.board = value;
					CreateCells();
				}
				else
				{
					this.board = value;
					UpdateBoard();
				}
			}
			get{return this.board;}
		}

		public ReversiPanel(ReversiBoard board) : this(board, "32"){}

		public ReversiPanel() : this(null){}

		public void UpdateBoard()
		{
			for(int x=0; x<board.Width; ++x)
				for(int y=0; y<board.Height; ++y)
				{
					ReversiCell cell = this.cells[x,y];
					cell.SetColor(board[x, y]);
				}
			Invalidate();
		}

		protected void OnCellClick(object sender, EventArgs args)
		{
			ReversiCell cell = (ReversiCell)sender;
			if(CellClick != null)CellClick(cell.x, cell.y);
		}

		private void CreateCells()
		{
			int image_width  = none_image.Width;
			int image_height = none_image.Height;

			cells = new ReversiCell[board.Width, board.Height];
			this.Controls.Clear();
			for(int x=0; x<board.Width; ++x)
				for(int y=0; y<board.Height; ++y)
				{
					ReversiCell cell = new ReversiCell(x, y, this);
					cell.Location = new Point(image_width*x, image_height*y);
					cell.Size = new Size(image_width, image_height);
					cell.SetColor(board[x, y]);
					cell.Click += new EventHandler(OnCellClick);
					this.Controls.Add(cell);
					this.cells[x,y] = cell;
				}
			this.Width = board.Width * image_width;
			this.Height = board.Height * image_height;
		}
	}//class ReversiPanel
}//namespace Reversi
