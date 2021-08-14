using ParameterlessConstructors;

NewOrDefault.M();
Zeroed.M();
//Defaultable.M();
GenericNew.M();

// 他に書きたいデモ:
// * where T : sturct の扱い
// * record struct が入ったことで需要が高まって、C# 10.0 で再取組みされたという話
// * where T : new() => new T() は Activator.CreateInstance(typeof(T)) と一緒と言う話
// * (まだサポート期間が残ってる)古い .NET Framework で Activator.CreateInstance がバグったままらしい
