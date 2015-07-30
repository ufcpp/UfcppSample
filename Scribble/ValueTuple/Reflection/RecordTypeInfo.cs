using System;
using System.Collections.Generic;

namespace ValueTuples.Reflection
{
    /// <summary>
    /// リフレクションを使えない環境で、事前コード生成で作っておく型情報。
    /// </summary>
    /// <remarks>
    /// 継承階層を持ったクラスとかをシリアライズ、デシリアライズしたいという要件があって、
    /// 一般的なシリアライザーの類が使いにくかったりする。
    /// </remarks>
    public abstract class RecordTypeInfo
    {
        /// <summary>
        /// 元の型情報。
        /// </summary>
        public abstract Type Type { get; }

        /// <summary>
        /// 単純型(プリミティブ、文字列、時刻など)かどうか。
        /// </summary>
        public virtual bool IsSimple => false;

        /// <summary>
        /// 配列かどうか。
        /// </summary>
        public virtual bool IsArray => false;

        /// <summary>
        /// 継承階層の親側かどうか。
        /// </summary>
        public virtual bool IsBase => false;

        /// <summary>
        /// 継承階層の子側かどうか。
        /// この場合、<see cref="Discriminator"/> で具体的な型を弁別する。
        /// </summary>
        public bool IsChild => Discriminator != null;

        /// <summary>
        /// フィールド情報。
        /// </summary>
        public abstract IEnumerable<RecordFieldInfo> Fields { get; }

        /// <summary>
        /// 継承階層を持っているときに、具体的な型を判別するために使う数値。
        /// </summary>
        /// <remarks>
        /// 同じ階層内で一意ならなんでもいい。
        /// </remarks>
        public virtual int? Discriminator => null;

        /// <summary>
        /// 継承階層を持っているときに、<see cref="Discriminator"/> の値から具体的な型を得る。
        /// </summary>
        /// <param name="discriminator">型判別用の数値。</param>
        /// <returns></returns>
        public virtual RecordTypeInfo GetType(int discriminator) => this;

        /// <summary>
        /// <see cref="Activator.CreateInstance(Type)"/> 代わり。
        /// 引数なしのコンストラクターを呼んでインスタンスを作る。
        /// </summary>
        /// <returns></returns>
        public abstract object GetInstance();

        /// <summary>
        /// <see cref="Array.CreateInstance(Type, int)"/> 代わり。
        /// 個数を指定して配列を作る。
        /// </summary>
        /// <returns></returns>
        public abstract Array GetArray(int length);

        /// <summary>
        /// アクセサーを作る。
        /// </summary>
        /// <param name="instance">アクセサーをかませたいインスタンス。</param>
        /// <returns></returns>
        public abstract IRecordAccessor GetAccessor(object instance);
    }
}
