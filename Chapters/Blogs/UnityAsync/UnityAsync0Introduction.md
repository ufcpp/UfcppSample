# Unity上でasync/await: はじめに (UnityAsync0Introduction)

たまにはAdvent Calendar参加。

このブログは[Unity Advent Calendar 2015](http://qiita.com/advent-calendar/2015/unity)の12月1日の記事です。

7月に書いた「[Unity(ゲームエンジン)上で async/await](http://ufcpp.net/blog/2015/07/unityasyncbridge/)」の続報というか進捗。
あと、補足説明いろいろ。

あれから4か月くらいたったわけでさすがに安定したというか。
むしろ、大して問題出なかったというか。

以下のコミット履歴を見てのとおり、4か月でコミット79個しかないものの、これでもう安定してたりします。

[https://github.com/OrangeCube/MinimumAsyncBridge/commits/master](https://github.com/OrangeCube/MinimumAsyncBridge/commits/master)

これが、「最初から安定してるライブラリは不活性に見えて不安がられる」というやつか…

むしろ、IL2CPPの安定を待ってるというか…

長くなりそうなので3部構成になっています:

- [背景](UnityAsync1Background)
- [現状](UnityAsync2CurrentStatus)
- [課題と感想](UnityAsync3Retrospective)

