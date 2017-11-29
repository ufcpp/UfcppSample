using System;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace SoundLibrary.Command
{
	/// <summary>
	/// コマンド処理用のデリゲート。
	/// </summary>
	/// <remarks>
	/// 引数リスト <paramref name="args"/> を受け取って処理を行い、
	/// 出力ストリーム <paramref name="sout"/> に結果を出力する。
	/// コマンド解釈を終了したいときに true を返す。
	/// </remarks>
	public delegate bool CommandHandlar(string[] args, TextWriter sout);

	/// <summary>
	/// テキストストリームから入力されたコマンドの解釈を行うクラス。
	/// </summary>
	public class CommandParser
	{
		#region 内部クラス

		class Tuple
		{
			public CommandHandlar ope;
			public string help;

			public Tuple(CommandHandlar ope, string help)
			{
				this.ope = ope;
				this.help = help;
			}
		}

		#endregion
		#region フィールド

		Hashtable commands;
		CommandHandlar notFound; // コマンドが見つからなかったときの処理

		string prompt;
		Regex delim1; // コマンドと引数の区切り文字
		Regex delim2; // 引数同士の間の区切り文字
		Regex delim3; // リダイレクト用の区切り文字

		#endregion
		#region 初期化

		/// <summary>
		/// 初期化。
		/// 区切り文字に空白文字 ("\s+") を使用。
		/// リダイレクト用に > (\s*>\s*) を使用。
		/// </summary>
		public CommandParser() : this("> ", @"\s+", @"\s+", @"\s*>\s*")
		{
		}

		/// <summary>
		/// 初期化。
		/// コマンド、引数間の区切り文字(正規表現)を指定。
		/// </summary>
		/// <param name="delim1">コマンドと引数の区切り文字</param>
		/// <param name="delim2">引数同士の間の区切り文字</param>
		/// <param name="delim3">リダイレクト用の区切り文字</param>
		public CommandParser(string prompt, string delim1, string delim2, string delim3)
		{
			this.commands = new Hashtable();
			this.commands[HELP_COMMAND] = new Tuple(new CommandHandlar(this.ShowHelp), HELP_MESSAGE);
			this.commands[QUIT_COMMAND] = new Tuple(new CommandHandlar(Quit), QUIT_MESSAGE);
			this.commands[SOURCE_COMMAND] = new Tuple(new CommandHandlar(this.Source), SOURCE_MESSAGE);

			this.notFound = new CommandHandlar(this.DefaultNotFound);

			this.prompt = prompt;
			this.delim1 = new Regex(delim1);
			this.delim2 = new Regex(delim2);
			this.delim3 = new Regex(delim3);
		}

		#endregion
		#region コマンドの解釈

		/// <summary>
		/// 標準入力ストリームからコマンドを読み出して解釈。
		/// </summary>
		/// <returns>コマンド解釈が終わったとき true を返す</returns>
		public bool Parse()
		{
			return this.Parse(Console.In, Console.Out);
		}

		/// <summary>
		/// ストリームからコマンドを読み出して解釈。
		/// 標準出力ストリームに出力。
		/// </summary>
		/// <param name="sin">入力元</param>
		/// <returns>コマンド解釈が終わったとき true を返す</returns>
		public bool Parse(TextReader sin)
		{
			return this.Parse(sin, Console.Out);
		}

		/// <summary>
		/// ストリームからコマンドを読み出して解釈。
		/// </summary>
		/// <param name="sin">入力元</param>
		/// <param name="sout">出力先</param>
		/// <returns>コマンド解釈が終わったとき true を返す</returns>
		public bool Parse(TextReader sin, TextWriter sout)
		{
			while(true)
			{
				if(sin == Console.In && sout == Console.Out)
					sout.Write(this.prompt);

				string line = sin.ReadLine();

				if(line == null)
					break;

				string[] tmp = this.delim3.Split(line);
				if(tmp.Length <= 1)
				{
					if(Parse(tmp[0], sout))
						return true;
				}
				else
				{
					StreamWriter sout1 = null;
					try
					{
						sout1 = new StreamWriter(tmp[1], false, System.Text.Encoding.Default);
						if(Parse(tmp[0], sout1))
							return true;
					}
					catch(Exception exc)
					{
						Console.Error.Write(exc);
					}
					finally
					{
						if(sout1 != null) sout1.Close();
					}
				}
			}

			return false;
		}

		/// <summary>
		/// コマンドを1ライン解釈。
		/// </summary>
		/// <param name="commandLine">コマンドライン</param>
		/// <returns>コマンド解釈が終わったとき true を返す</returns>
		bool Parse(string commandLine, TextWriter sout)
		{
			if(commandLine == null || commandLine.Length == 0)
			{
				ShowHelp(sout);
				return false;
			}

			string[] tmp = this.delim1.Split(commandLine, 2);

			Tuple t = (Tuple)this.commands[tmp[0]];

			CommandHandlar ope;
			string[] args;

			if(t == null)
			{
				ope = this.notFound;
				args = tmp;
			}
			else
			{
				ope = t.ope;
				if(tmp.Length < 2)
					args = null;
				else
					args = this.delim2.Split(tmp[1]);
			}
			
			if(ope == null)
			{
				return true;
			}

			return ope(args, sout);
		}


		#endregion
		#region コマンドの追加・削除

		/// <summary>
		/// コマンドを追加する。
		/// </summary>
		/// <param name="command">コマンド名</param>
		/// <param name="ope">コマンド処理デリゲート</param>
		public void Add(string command, CommandHandlar ope)
		{
			this.Add(command, ope, "");
		}

		/// <summary>
		/// コマンドを追加する。
		/// </summary>
		/// <param name="command">コマンド名</param>
		/// <param name="ope">コマンド処理デリゲート</param>
		/// <param name="help">ヘルプメッセージ</param>
		public void Add(string command, CommandHandlar ope, string help)
		{
			this.commands[command] = new Tuple(ope, help);
		}

		/// <summary>
		/// コマンドを削除する。
		/// </summary>
		/// <param name="command">コマンド名</param>
		public void Remove(string command)
		{
			this.commands.Remove(command);
		}

		#endregion
		#region デフォルトコマンド

		const string QUIT_COMMAND = "quit";
		const string QUIT_MESSAGE = "終了します。\n";

		const string HELP_COMMAND = "help";
		const string HELP_MESSAGE = "ヘルプを表示します。\n";

		const string SOURCE_COMMAND = "source";
		const string SOURCE_MESSAGE = "source [ファイル名]\nテキストファイルからコマンドを読み込みます。\n";

		#region コマンドが見つからなかったとき用

		/// <summary>
		/// コマンド未登録時用デリゲートを登録。
		/// </summary>
		/// <param name="notFound">コマンド未登録時用デリゲート</param>
		void SetNotFound(CommandHandlar notFound)
		{
			if(notFound == null)
				this.notFound = new CommandHandlar(this.DefaultNotFound);
			else
				this.notFound = notFound;
		}

		/// <summary>
		/// コマンド未登録時、デフォルトの動作。
		/// </summary>
		bool DefaultNotFound(string[] args, TextWriter sout)
		{
			if(args.Length >= 1)
				sout.Write("{0} というコマンドはありません。\n", args[0]);
			this.ShowHelp(sout);
			return false;
		}

		#endregion
		#region ヘルプ用

		/// <summary>
		/// ヘルプ表示。
		/// </summary>
		void ShowHelp(TextWriter sout)
		{
			sout.Write("help [コマンド名]\nと入力することで、各コマンドのヘルプを表示できます。\n");

			int i=0;
			foreach(string command in commands.Keys)
			{
				if(i % 4 == 3)
					sout.Write("\n");
				sout.Write("{0,10}", command);
			}
			sout.Write("\n");
		}

		/// <summary>
		/// 各コマンドのヘルプ表示。
		/// </summary>
		/// <param name="command">コマンド名</param>
		/// <param name="sout">出力先。</param>
		bool ShowHelp(string[] command, TextWriter sout)
		{
			if(command == null || command.Length == 0)
				this.ShowHelp(sout);
			else
			{
				Tuple t = (Tuple)this.commands[command[0]];

				if(t == null)
					return this.notFound(command, sout);

				sout.Write(t.help);
			}
			return false;
		}

		#endregion
		#region 終了処理用

		static bool Quit(string[] args, TextWriter sout)
		{
			return true;
		}

		#endregion
		#region 外部ファイル読み込み

		/// <summary>
		/// テキストファイルからコマンドを読み出す。
		/// </summary>
		bool Source(string[] args, TextWriter sout)
		{
			if(args.Length < 1)
			{
				sout.Write(SOURCE_MESSAGE);
				return false;
			}

			StreamReader sin = null;
			try
			{
				sin = new StreamReader(args[0], System.Text.Encoding.Default);
				return this.Parse(sin, sout);
			}
			catch(Exception exc)
			{
				Console.Error.Write(exc);
				return true;
			}
			finally
			{
				if(sin != null) sin.Close();
			}
		}

		#endregion
		
		#endregion
	}
}
