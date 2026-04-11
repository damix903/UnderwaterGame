using System;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace EnemyAI
{
    /// <summary>
    /// 敵のMessagePipeを通じた通信を管理するクラス
    /// </summary>
    public class EnemyMessageBroker : IDisposable
    {
        private readonly IPublisher<DamageResult> _damagePub;
        private readonly IPublisher<DeathEvent> _deathPub;
        
        private readonly IDisposable _subscription;
        
        public event Action<ReleaseType> OnRelease;

        public EnemyMessageBroker(ISubscriber<ReleaseType> releaseSub,
            IPublisher<DamageResult> damagePub,
            IPublisher<DeathEvent> deathPub)
        {
            _damagePub = damagePub;
            _deathPub = deathPub;
            var bag = DisposableBag.CreateBuilder();

            releaseSub?.Subscribe((x) => OnRelease?.Invoke(x)).AddTo(bag);

            _subscription = bag.Build();
        }
        
        public void Publish(DamageResult result) => _damagePub?.Publish(result);
        public void Publish(DeathEvent death) => _deathPub?.Publish(death);
        
        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}