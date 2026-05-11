using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI
{
    public class Fader : MonoBehaviour, IFader
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeDuration = 0.5f;
        
        private void Awake()
        {
            canvasGroup.alpha = 0f; // 初期状態は透明
        }

        public async UniTask FadeInAsync(CancellationToken ct) => await FadeAsync(1f, 0f, ct);

        public async UniTask FadeOutAsync(CancellationToken ct) => await FadeAsync(0f, 1f, ct);
        
        private async UniTask FadeAsync(float from, float to, CancellationToken ct)
        {
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                if (ct.IsCancellationRequested) break;

                elapsed += Time.unscaledDeltaTime;
                canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
                
                await UniTask.Yield(ct);
            }
            canvasGroup.alpha = to;
        }
    }
}