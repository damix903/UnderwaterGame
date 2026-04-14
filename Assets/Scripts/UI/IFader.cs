using System.Threading;
using Cysharp.Threading.Tasks;

namespace UI
{
    public interface IFader
    {
        // フェードインは画面が暗くなっている状態から明るくなることを想定
        UniTask FadeInAsync(CancellationToken ct);
        // フェードアウトは画面が明るい状態から暗くなることを想定
        UniTask FadeOutAsync(CancellationToken ct);
    }
}