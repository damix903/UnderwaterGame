using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Manager.AudioSystem
{
    public interface IAudioService
    {
        public SoundEmitter GetEmitter(SoundData soundData);
        public void PlaySound(SoundData soundData, Vector3 position = default);
        public UniTask StartBGM(SoundData soundData, float fadeDuration = 1f, CancellationToken ct = default);
        public UniTask StopBGM(float fadeDuration = 1f, CancellationToken ct = default);
    }
}