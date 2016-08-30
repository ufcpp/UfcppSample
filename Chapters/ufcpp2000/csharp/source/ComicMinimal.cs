using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace LinqToSqlTest
{
	public class ComicDataContext : DataContext
	{
		public ComicDataContext(string connectionString) : base(connectionString) { }
		public Table<Author> Author;
		public Table<Publisher> Publisher;
		public Table<Series> Series;
		public Table<Book> Book;
	}

	[Table(Name = "Authors")]
	public class Author
	{
		[Column(AutoSync = AutoSync.OnInsert, IsPrimaryKey = true, IsDbGenerated = true)]
		public int Id;
		[Column]
		public string Name;
		[Column]
		public string Kana;
		[Column]
		public DateTime? Birthday;
		[Column]
		public string Url;

		[Association(OtherKey = "AuthorId")]
		public EntitySet<Series> Series;
	}

	[Table(Name = "Publishers")]
	public class Publisher
	{
		[Column(AutoSync = AutoSync.OnInsert, IsPrimaryKey = true, IsDbGenerated = true)]
		public int Id;
		[Column]
		public string Name;
	}

	[Table(Name = "Series")]
	public class Series
	{
		[Column(AutoSync = AutoSync.OnInsert, IsPrimaryKey = true, IsDbGenerated = true)]
		public int Id;
		[Column]
		public string Name;
		[Column]
		public int AuthorId;
		[Column]
		public int PublisherId;

		[Association(OtherKey = "SeriesId")]
		public EntitySet<Book> Book;

		[Association(Storage = "_Author", ThisKey = "AuthorId")]
		public Author Author
		{
			get { return this._Author.Entity; }
			set { this._Author.Entity = value; }
		}
		private EntityRef<Author> _Author;

		[Association(Storage = "_Publisher", ThisKey = "PublisherId")]
		public Publisher Publisher
		{
			get { return this._Publisher.Entity; }
			set { this._Publisher.Entity = value; }
		}
		private EntityRef<Publisher> _Publisher;
	}

	[Table(Name = "Books")]
	public class Book
	{
		[Column(AutoSync = AutoSync.OnInsert, IsPrimaryKey = true, IsDbGenerated = true)]
		public int Id;
		[Column]
		public int SeriesId;
		[Column]
		public int Volume;
		[Column]
		public DateTime? ReleaseDate;
		[Column]
		public int? Price;

		[Association(Storage = "_Series", ThisKey = "SeriesId")]
		public Series Series
		{
			get { return this._Series.Entity; }
			set { this._Series.Entity = value; }
		}
		private EntityRef<Series> _Series;
	}
}
