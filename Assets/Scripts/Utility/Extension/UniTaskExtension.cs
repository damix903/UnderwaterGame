using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Utility
{
    public static class UniTaskExtension
    {
        /// <summary>
        /// startからtargetに向かって、duration秒かけて線形補間するUniTask
        /// 補完する対象を指定できるようにするため、Action<float>を引数に取る
        /// </summary>
        public static async UniTask LerpAsync(float start, float target, float duration, Action<float> onUpdate, CancellationToken ct)
        {
            float elapsed = 0f;

            try
            {
                while (elapsed < duration)
                {
                    // キャンセルが要求された場合はループを抜ける
                    if (ct.IsCancellationRequested) break;

                    elapsed += Time.deltaTime;
                    float value = Mathf.Lerp(start, target, elapsed / duration);
                    onUpdate?.Invoke(value);

                    await UniTask.Yield(ct);
                }
            }
            finally
            {
                // 最終的にターゲット値を確実に設定する
                onUpdate?.Invoke(target);
            }
        }
    }
}