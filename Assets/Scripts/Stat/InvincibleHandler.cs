using System;
using Animation;
using Underwater.Utility.Timer;
using UnityEngine;
using Utility;

namespace Stat
{
    public class InvincibleHandler : IDisposable
    {
        private GameObject gameObject;
        private IDamageable _damageable;
        private ISpriteBlinker[] _blinker;
        
        private float invincibleDuration = 1f;
        private int _defaultLayer;
        private int _invincibleLayer;

        // Builderパターンを使用して、必要な依存関係を注入するため、コンストラクタはprivateにする
        private InvincibleHandler(){}
        
        public void SetDuration(float duration) => invincibleDuration = duration;

        public void Dispose()
        {
            if (_damageable != null) _damageable.OnDamaged -= HandleDamage;
            
            FinishInvincible();
        }

        private void HandleDamage(DamageResult result)
        {
            _damageable.DefenseState = DefenseState.Invincible;

            gameObject.layer = _invincibleLayer != 0 ? _invincibleLayer : _defaultLayer;
            foreach (var b in _blinker) b?.StartBlinking(invincibleDuration);

            Timer.Set(invincibleDuration, FinishInvincible).BindTo(gameObject);
        }

        private void FinishInvincible()
        {
            _damageable.DefenseState = DefenseState.None;
            gameObject.layer = _defaultLayer;
            foreach (var b in _blinker) b?.StopBlinking();
        }
        
        public class Builder
        {
            private readonly InvincibleHandler _invincibleHandler = new InvincibleHandler();
            
            public Builder(GameObject gameObject, IDamageable damageable)
            {
                _invincibleHandler.gameObject = gameObject;
                _invincibleHandler._damageable = damageable;
            }

            public Builder WithDuration(float invincibleDuration)
            {
                _invincibleHandler.invincibleDuration = invincibleDuration;
                return this;
            }
            
            public Builder WithBlinker(ISpriteBlinker[] blinkers)
            {
                _invincibleHandler._blinker = blinkers;
                return this;
            }

            public Builder WithInvincibleLayer(LayerMask invincibleLayer)
            {
                _invincibleHandler._invincibleLayer = invincibleLayer.MaskToInt();
                _invincibleHandler._defaultLayer = _invincibleHandler.gameObject.layer;
                return this;
            }

            public InvincibleHandler Build()
            {
                _invincibleHandler._damageable.OnDamaged += _invincibleHandler.HandleDamage;
                return _invincibleHandler;
            }
        }
    }
}