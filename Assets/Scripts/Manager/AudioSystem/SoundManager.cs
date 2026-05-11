using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;
using SpawnSystem;
using UnityEngine;
using VContainer;

namespace Manager.AudioSystem
{
    public class SoundManager : MonoBehaviour, IAudioService
    {
        [SerializeField] private int maxFrequentSoundCount = 10;
        [SerializeField] private SoundData bgm;
        
        [Inject] private IEntityFactory<SoundEmitter> _factory;
        [Inject] private ISubscriber<EffectData> _effectSub;
        [Inject] private ISubscriber<DamageResult> _damageSub;
        
        private IDisposable _subscription;
        
        private readonly Queue<SoundEmitter> _frequentEmitters = new Queue<SoundEmitter>();
        private SoundEmitter _currentBGM;
        private bool _isBGMTransitioning;

        private void Start()
        {
            var bag = DisposableBag.CreateBuilder();
            _effectSub.Subscribe((data => PlaySound(data.SoundData))).AddTo(bag);
            _damageSub.Subscribe((result => PlaySound(result.DamageInfo.EffectData.SoundData))).AddTo(bag);
            if (bgm != null) StartBGM(bgm).Forget();
            
            _subscription = bag.Build();
        }

        private void OnDestroy()
        {
            _subscription.Dispose();
        }

        public SoundEmitter GetEmitter(SoundData soundData)
        {
            if (!CanPlay(soundData)) return null;
            
            var emitter = _factory.Create(soundData, new SpawnPoint(transform.position, Quaternion.identity));
            
            if (emitter == null) return null;

            if (soundData.FrequentSound) _frequentEmitters.Enqueue(emitter);

            return emitter;
        }

        public void PlaySound(SoundData soundData, Vector3 position = default)
        {
            var emitter = GetEmitter(soundData);
            if (emitter == null) return;
            
            emitter.transform.position = position;
            emitter.Play();
        }

        public async UniTask StartBGM(SoundData soundData, float fadeDuration = 1, CancellationToken ct = default)
        {
            if (_isBGMTransitioning) return;
            if (_currentBGM != null && _currentBGM.Data == soundData) return;

            _isBGMTransitioning = true;
            var linkedCt = CancellationTokenSource
                .CreateLinkedTokenSource(ct, this.GetCancellationTokenOnDestroy()).Token;

            try
            {
                if (_currentBGM != null) await StopBGM(fadeDuration, linkedCt);

                _currentBGM = GetEmitter(soundData);
                if (_currentBGM == null) return;

                _currentBGM.SetVolume(0f); // 音量0で再生開始
                _currentBGM.Play(false);
                await _currentBGM.FadeAsync(soundData.Volume, fadeDuration, linkedCt);
            }
            catch (OperationCanceledException)
            {

            }
            finally
            {
                _isBGMTransitioning = false;
            }
        }

        public async UniTask StopBGM(float fadeDuration = 1, CancellationToken ct = default)
        {
            if (_currentBGM == null) return;
            
            var linkedCt = CancellationTokenSource
                .CreateLinkedTokenSource(ct, this.GetCancellationTokenOnDestroy()).Token;
            
            await _currentBGM.FadeAsync(0f, fadeDuration, linkedCt);
            _currentBGM = null;
        }

        private bool CanPlay(SoundData soundData)
        {
            if (!soundData.FrequentSound) return true;

            if (_frequentEmitters.Count >= maxFrequentSoundCount
                && _frequentEmitters.TryDequeue(out var emitter))
            {
                try
                {
                    emitter?.Stop();
                    return true;
                }
                catch 
                {
                    Debug.Log("Emitter already stopped.");
                }
                return false;
            }

            return true;
        }
    }
}