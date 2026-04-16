using UnityEngine;

namespace Movement.Data
{
    [CreateAssetMenu(fileName = "MM_", menuName = "Data/Movement/ChaseModifier", order = 0)]
    public class ChaseMovementModifier : ScriptableObject, IMovementModifier
    {
        [SerializeField] private float speedMultiplier;
        
        public void Apply(ref MovementRuntimeStats stats)
        {
            stats.movementMaxSpeed *= speedMultiplier;
        }
    }
}