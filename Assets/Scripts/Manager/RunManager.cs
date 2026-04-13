using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Manager.Upgrade;
using MessagePipe;
using PlayerSystem;
using UI;
using VContainer;
using VContainer.Unity;

namespace Manager
{
    public class RunManager : IDisposable, IPostStartable
    {
        private readonly UpgradePresenter _upgradePresenter;
        private readonly StageGenerator _stageGenerator;
        private readonly IPlayerProvider _playerProvider;
        
        [Inject] private ISubscriber<LevelClearedMessage> _levelClearedMessage;
        private IDisposable _subscription;
        
        private CancellationTokenSource _cts = new CancellationTokenSource();
        
        public RunManager(UpgradePresenter upgradePresenter,
            StageGenerator stageGenerator, 
            IPlayerProvider playerProvider,
            ISubscriber<LevelClearedMessage> levelClearedSub)
        {
            _upgradePresenter = upgradePresenter;
            _stageGenerator = stageGenerator;
            _playerProvider = playerProvider;

            _subscription = levelClearedSub.Subscribe(_ => ProcessUpGrade().Forget());

            _playerProvider.OnPlayerChanged += player => { ProcessGenerate(); };
            _stageGenerator.OnStageGenerated += vector3 => {_playerProvider?.SetPosition(vector3); };
        }

        private void ProcessGenerate()
        {
            if (!_playerProvider.TryGetPlayer(out var player)) return;
            
            _stageGenerator.GenerateFromEditor();
            //_playerProvider?.SetPosition(_stageGenerator.StageStartPoint.position);
        }
        
        private async UniTask ProcessUpGrade()
        {
            await _upgradePresenter.StartUpgradeSelectionAsync(_cts.Token);
            ProcessGenerate();
        }

        public void Dispose()
        {
            _subscription?.Dispose();
            _cts?.Cancel();
            _cts?.Dispose();
        }

        public void PostStart()
        {
            ProcessGenerate();
        }
    }
    
    public struct LevelClearedMessage
    { }
}