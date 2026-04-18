using SpawnSystem;
using UnityEngine;
using UnityEngine.Audio;

namespace Manager.AudioSystem
{
    [CreateAssetMenu(fileName = "SD_", menuName = "Data/SoundData", order = 0)]
    public class SoundData : EntityData
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private AudioMixerGroup mixer;
        [SerializeField] private bool loop;
        [SerializeField] private bool playOnAwake;
        [SerializeField] private bool frequentSound;
        
        [Header("Randomization")]
        [SerializeField] private bool randomPitch;
        [SerializeField] private float randomPitchRange = 0.05f;
        [Space]
        [SerializeField] private bool randomVolume;
        [SerializeField] private float randomVolumeRange = 0.1f;
        
        public AudioClip Clip => clip;
        public AudioMixerGroup Mixer => mixer;
        public bool Loop => loop;
        public bool PlayOnAwake => playOnAwake;
        public bool FrequentSound => frequentSound;
        public float Pitch => randomPitch ? 1f + Random.Range(-randomPitchRange, randomPitchRange) : 1f;
        public float Volume => randomVolume ? 1f + Random.Range(-randomVolumeRange, randomVolumeRange) : 1f;
    }
}