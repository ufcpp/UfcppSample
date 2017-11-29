using System;

namespace Reversi
{
	/// <summary>
	/// リバー利の駒の状態
	/// </summary>
	public enum ReversiColor
	{
		Wall = 0, //盤面の淵
		Black,    //黒い駒
		White,    //白い駒
		None      //マス目に何も置かれていない状態
	}

	/// <summary>
	/// リバーシの盤面
	/// </summary>
	public class ReversiBoard
	{
		private readonly int width;
		private readonly int height;
		private ReversiColor[,] board;

		/// <summary>
		/// 盤面の初期化
		/// 盤面の幅は自由に決めれる
		/// </summary>
		/// <param name="width">盤面の幅</param>
		/// <param name="height">盤面の高さ</param>
		public ReversiBoard(int width, int height)
		{
			this.width = width;
			this.height = height;
			//盤面用のメモリ確保
			this.board = new ReversiColor[width+2, height+2];//周りに番兵を置くので+2
			//盤面の淵以外をNoneにクリア
			for(int x=1; x<=width; ++x)
				for(int y=1; y<=height; ++y)
				{
					this.board[x, y] = ReversiColor.None;
				}
			//盤面の淵に番兵を置く
			for(int i=0; i<width+2; i++)
			{
				this.board[i, 0] = ReversiColor.Wall;
				this.board[i, height+1] = ReversiColor.Wall;
			}
			for(int i=1; i<height+1; i++)
			{
				this.board[0, i] = ReversiColor.Wall;
				this.board[width+1, i] = ReversiColor.Wall;
			}
			//盤面の中央に最初の駒を置く
			this.board[width/2  , height/2  ] = ReversiColor.Black;
			this.board[width/2  , height/2+1] = ReversiColor.White;
			this.board[width/2+1, height/2  ] = ReversiColor.White;
			this.board[width/2+1, height/2+1] = ReversiColor.Black;
		}

		/// <summary>
		/// 盤面のコピーを作成するコンストラクタ
		/// </summary>
		/// <param name="b">コピー元</param>
		protected ReversiBoard(ReversiBoard b)
		{
			this.width = b.width;
			this.height = b.height;
			//盤面用のメモリ確保
			this.board = new ReversiColor[b.width+2, b.height+2];//周りに番兵を置くので+2
			//盤面の淵以外をNoneにクリア
			for(int x=0; x<b.width+2; ++x)
				for(int y=0; y<b.height+2; ++y)
				{
					this.board[x, y] = b.board[x, y];
				}
		}

		/// <summary>
		/// 自分自身のコピーを生成
		/// </summary>
		public ReversiBoard Clone()
		{
			return new ReversiBoard(this);
		}

		/// <summary>
		/// 盤面の初期化
		/// 盤面のサイズはデフォルトでは8×8
		/// </summary>
		public ReversiBoard():this(8, 8){}

		//=========================================================
		// public methods

		/// <summary>
		/// 座標(x,y)にコマをおけるかどうかを調べる
		/// <param name="x">調べる場所のx座標 0～width-1</param>
		/// <param name="y">調べる場所のy座標 0～width-1</param>
		/// <returns>置けるかどうか</returns>
		/// </summary>
		public bool Check(int x, int y, ReversiColor color)
		{
			++x; ++y;
			return this.board[x,y] == ReversiColor.None &&
				( CheckLine(x, y, -1,  0, color)  //左
				|| CheckLine(x, y,  1,  0, color)  //右
				|| CheckLine(x, y,  0, -1, color)  //上
				|| CheckLine(x, y,  0,  1, color)  //下
				|| CheckLine(x, y, -1, -1, color)  //左上
				|| CheckLine(x, y,  1, -1, color)  //右上
				|| CheckLine(x, y, -1,  1, color)  //左下
				|| CheckLine(x, y,  1,  1, color));//右上
		}
		
		/// <summary>
		/// 盤面に置けるますがあるかどうかを調べる
		/// </summary>
		/// <returns>置けるますが存在するかどうか</returns>
		public bool CheckAll(ReversiColor color)
		{
			for(int x=1; x<=this.width; ++x)
				for(int y=1; y<=this.height; ++y)
					if(Check(x,y, color)) return true;

			return false;
		}

		/// <summary>
		/// set 盤面に新たに駒を置き、盤面の更新を行う
		/// get 盤面に置かれた駒の色を返す
		/// x : 0～width-1
		/// y : 0～height-1
		/// </summary>
		public ReversiColor this[int x, int y]
		{
			set
			{
				++x; ++y;
				this.board[x,y] = value;//そのマス目にこまを置く
				UpdateLine(x, y, -1,  0, value);//左
				UpdateLine(x, y,  1,  0, value);//右
				UpdateLine(x, y,  0, -1, value);//上
				UpdateLine(x, y,  0,  1, value);//下
				UpdateLine(x, y, -1, -1, value);//左上
				UpdateLine(x, y,  1, -1, value);//右上
				UpdateLine(x, y, -1,  1, value);//左下
				UpdateLine(x, y,  1,  1, value);//右上
			}
			get
			{
				if(x >= this.width || x < 0 || y >= this.height || y < 0)
					return ReversiColor.Wall;
				return this.board[x+1, y+1];
			}
		}

		public int Width{get{return width;}}
		public int Height{get{return height;}}

		/// <summary>
		/// 駒の数を数える
		/// </summary>
		/// <param name="black_num">黒い駒の数を返す</param>
		/// <param name="white_num">白い駒の数を返す</param>
		public void CountUp(out int black_num, out int white_num)
		{
			black_num = 0;
			white_num = 0;
			for(int x=1; x<=width; ++x)
				for(int y=1; y<=height; ++y)
				{
					if(board[x, y] == ReversiColor.Black) black_num++;
					if(board[x, y] == ReversiColor.White) white_num++;
				}
		}

		public int GetScore(ReversiColor color)
		{
			int black, white;
			this.CountUp(out black, out white);
			if(color == ReversiColor.Black) return black;
			if(color == ReversiColor.White) return white;
			return 0;
		}

		//=========================================================
		// private methods

		/// <summary>
		/// 盤面の更新を1ラインずつ行う
		/// <param name="x">置く場所のx座標</param>
		/// <param name="y">置く場所のy座標</param>
		/// <param name="color">置く駒の色</param>
		/// </summary>
		private void UpdateLine(int x, int y, int dx, int dy, ReversiColor color)
		{
			int i, j;
			ReversiColor inverse_color = InverseColor(color);
			for(i=x+dx, j=y+dy; this.board[i,j] == inverse_color; i+=dx, j+=dy);
			if(!(i==x+dx && j==y+dy) && this.board[i,j]==color)
				for(i-=dx, j-=dy; !(i==x && j==y); i-=dx, j-=dy)
					this.board[i,j] = color;
		}

		/// <summary>
		/// 駒の色と逆の色を返す
		/// </summary>
		/// <param name="color">駒の色</param>
		/// <returns>逆の色</returns>
		static public ReversiColor InverseColor(ReversiColor color)
		{
			if(color == ReversiColor.Black) return ReversiColor.White;
			if(color == ReversiColor.White) return ReversiColor.Black;
			return color;
		}

		/// <summary>
		/// 座標(x,y)にコマをおけるかどうか、1ライン分調べる
		/// (Checkメソッドで利用する)
		/// <param name="x">調べる場所のx座標</param>
		/// <param name="y">調べる場所のy座標</param>
		/// <returns>置けるかどうか</returns>
		/// </summary>
		private bool CheckLine(int x, int y, int dx, int dy, ReversiColor color)
		{
			int i, j;
			ReversiColor inverse_color = InverseColor(color);
			for(i=x+dx, j=y+dy; this.board[i,j] == inverse_color; i+=dx, j+=dy);
			return !(i==x+dx && j==y+dy) && this.board[i,j]==color;
		}

	}//class ReversiBoard
}
