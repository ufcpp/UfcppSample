using RgiSequenceFinder.TableGenerator;
using System.IO;
using System.Text;

// 2段コード生成になってる。
// RgiEmojiSequenceList はそんな複雑なデータでもないんで、こっちも emoji-data.json から直接読み込んでもいいんだけど。
// 2段くらいなら大した負担でもないし、とうめん2段コード生成する。

//RgiSequenceFinder.TableGenerator.Experimental.HashCode.CollisionCount(); return;
//RgiSequenceFinder.TableGenerator.Experimental.SingularEmoji.CheckCount(); return;
//RgiSequenceFinder.TableGenerator.Experimental.SingularEmoji.CollisionCount(); return;

var emojis = GroupedEmojis.Create();

using var writer = new StreamWriter("../../../../RgiSequenceFinder/RgiTable.Generated.cs", false, Encoding.UTF8);

SourceGenerator.Write(writer, emojis);
