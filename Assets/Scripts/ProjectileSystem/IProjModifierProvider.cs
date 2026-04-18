using System.Collections.Generic;
using Manager.Upgrade;

namespace ProjectileSystem
{
    public interface IProjModifierProvider
    {
        public IReadOnlyList<IProjectileModifier> Modifiers { get; }
        public IReadOnlyList<IShooterModifier> ShooterModifiers { get; }
    }
    
    public class EmptyProjModifierProvider : IProjModifierProvider
    {
        public IReadOnlyList<IProjectileModifier> Modifiers { get; } = new List<IProjectileModifier>();
        public IReadOnlyList<IShooterModifier> ShooterModifiers { get; } = new List<IShooterModifier>();
    }
    
    public class ProjModifierProvider : IProjModifierProvider
    {
        private readonly RunState _runState;
        public IReadOnlyList<IProjectileModifier> Modifiers => _runState.ProjModifiers;
        public IReadOnlyList<IShooterModifier> ShooterModifiers => _runState.ShooterModifiers;

        public ProjModifierProvider(RunState runState)
        {
            _runState = runState;
        }
    }
}