using System.Collections.Generic;
using Manager.Upgrade;

namespace ProjectileSystem
{
    public interface IModifierProvider
    {
        public IReadOnlyList<IProjectileModifier> Modifiers { get; }
    }
    
    public class EmptyModifierProvider : IModifierProvider
    {
        public IReadOnlyList<IProjectileModifier> Modifiers { get; } = new List<IProjectileModifier>();
    }
    
    public class ModifierProvider : IModifierProvider
    {
        private readonly RunState _runState;
        public IReadOnlyList<IProjectileModifier> Modifiers => _runState.ProjModifiers;

        public ModifierProvider(RunState runState)
        {
            _runState = runState;
        }
    }
}