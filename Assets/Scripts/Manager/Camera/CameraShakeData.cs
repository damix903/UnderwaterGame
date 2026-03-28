using Unity.Cinemachine;
using UnityEngine;

[CreateAssetMenu(menuName =  "Data/CameraShake")]
public class CameraShakeData : ScriptableObject
{
    [Header("Impulse Source Settings")]
    [SerializeField] private float duration = .2f;
    [SerializeField] private Vector3 velocity = new Vector3(0f, 1f, 0f);
    [SerializeField] private CinemachineImpulseDefinition.ImpulseShapes shape = CinemachineImpulseDefinition.ImpulseShapes.Rumble;

    [Header("Reaction Settings")] 
    [SerializeField] private float gain = 1f;
    [SerializeField] private float amplitudeGain = 1f;
    [SerializeField] private float frequencyGain = 1f;
    [SerializeField] private float durationGain = 1f;
    
    public Vector3 Velocity => velocity;
    public float Gain => gain;
    public float AmplitudeGain => amplitudeGain;
    public float FrequencyGain => frequencyGain;
    public float DurationGain => durationGain;

    public CinemachineImpulseDefinition CreateDefinition()
    {
        var def = new CinemachineImpulseDefinition
        {
            ImpulseType = CinemachineImpulseDefinition.ImpulseTypes.Uniform,
            ImpulseDuration = duration,
            ImpulseShape = shape
        };
        return def;
    }
}
