using UnityEngine;

namespace Movement
{
    public interface IForceApplicable
    {
        void AddImpulseForce(Vector2 force, bool overwriteX = false, bool overwriteY = false);
        void AddConstantForce(Vector2 force);
        void RemoveConstantForce(Vector2 force);
    }
}