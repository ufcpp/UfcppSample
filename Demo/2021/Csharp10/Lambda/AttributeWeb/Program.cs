using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// �e���v����1�s��ǉ��BDI �p�B
builder.Services.AddSingleton<Counter>();

var app = builder.Build();

// �e���v����1�s���������B������ DI �Ŏ󂯎������A�N�G�������񂩂�󂯎������B
// counter: �y�[�W�������[�h���邽�т� +1�B
// value: �N�G��������Ő��l���w��B
// ����2�̒l���牽�炩�̌v�Z���ĕԂ��B
app.MapGet("/", ([FromServices] Counter counter, [FromQuery] int? value) => counter.Count * (value ?? 1));

app.Run();

// �e���v����1�N���X�ǉ��B��L DI �œn���f���p�̌^�B
class Counter
{
    private int _count;
    public int Count { get => _count++; }
}
