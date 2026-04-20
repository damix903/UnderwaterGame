using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Stage;
using UnityEngine;
using Utility;

namespace Manager.AudioSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundEmitter : PoolableEntity
    {
        private AudioSource _audioSource;

        protected override ReleaseType ReleaseType => ReleaseType.Audio;
        public SoundData Data { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            _audioSource = gameObject.GetOrAddComponent<AudioSource>();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            if (data is not SoundData soundData) return;
            
            Data = soundData;
            _audioSource.clip = soundData.Clip;
            _audioSource.outputAudioMixerGroup = soundData.Mixer;
            _audioSource.loop = soundData.Loop;
            _audioSource.playOnAwake = soundData.PlayOnAwake;
            _audioSource.pitch = soundData.Pitch;
            _audioSource.volume = soundData.Volume;
        }

        public void Play(bool waitForEnd = true)
        {
            _audioSource.Play();
            if (waitForEnd) WaitForSoundToEndAsync(Cts.Token).Forget();
        }

        public void Stop(bool fadeOut = false)
        {
            _audioSource.Stop();
            Release();
        }
        
        public void SetVolume(float volume) => _audioSource.volume = volume;
        
        private async UniTaskVoid WaitForSoundToEndAsync(CancellationToken ct)
        {
            await UniTask.WaitWhile(() => _audioSource.isPlaying, cancellationToken: ct);
            Release();
        }
        
        public async UniTask FadeAsync(float target, float duration, CancellationToken ct)
        {
            if (!gameObject.activeInHierarchy) return;
            
            var cts = CancellationTokenSource.CreateLinkedTokenSource(ct, Cts.Token);
            float start = _audioSource.volume;

            try
            {
                await UniTaskExtension.LerpAsync(start, target, duration, value => _audioSource.volume = value, cts.Token);
            }
            finally
            {
                if (target <= 0f) Stop();
            }
        }
    }
}