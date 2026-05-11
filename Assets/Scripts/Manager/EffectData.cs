using Manager.AudioSystem;
using UnityEngine;

namespace Manager
{
    [CreateAssetMenu(menuName = "Data/Effect/EffectData", order = 0)]
    public class EffectData : ScriptableObject
    {
        [SerializeField] private HitStopData hitStopData;
        [SerializeField] private CameraShakeData cameraShakeData;
        [SerializeField] private SoundData soundData;

        public HitStopData HitStopData => hitStopData;
        public CameraShakeData CameraShakeData => cameraShakeData;
        public SoundData SoundData => soundData;
    }
}