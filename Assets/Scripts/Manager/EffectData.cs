using UnityEngine;

namespace Manager
{
    [CreateAssetMenu(menuName = "Data/Effect/EffectData", order = 0)]
    public class EffectData : ScriptableObject
    {
        [SerializeField] private HitStopData hitStopData;
        [SerializeField] private CameraShakeData cameraShakeData;

        public HitStopData HitStopData => hitStopData;
        public CameraShakeData CameraShakeData => cameraShakeData;
    }
}