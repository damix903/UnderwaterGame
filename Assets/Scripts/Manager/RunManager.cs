using System;
using Manager.Upgrade;
using MessagePipe;
using PlayerSystem;
using VContainer;
using VContainer.Unity;

namespace Manager
{
    public class RunManager : IDisposable, IPostStartable
    {
        private readonly UpgradeManager _upgradeManager;
        private readonly StageGenerator _stageGenerator;
        private readonly IPlayerProvider _playerProvider;
        
        [Inject] private ISubscriber<LevelClearedMessage> _levelClearedMessage;
        private IDisposable _subscription;
        
        public RunManager(UpgradeManager upgradeManager,
            StageGenerator stageGenerator, 
            IPlayerProvider playerProvider,
            ISubscriber<LevelClearedMessage> levelClearedSub)
        {
            _upgradeManager = upgradeManager;
            _stageGenerator = stageGenerator;
            _playerProvider = playerProvider;

            _subscription = levelClearedSub.Subscribe(_ => ProcessUpGrade());
            _upgradeManager.OnUpgradePhaseEnded += ProcessGenerate;
            _playerProvider.OnPlayerChanged += player => { ProcessGenerate(); };
            _stageGenerator.OnStageGenerated += vector3 => {_playerProvider?.SetPosition(vector3); };
        }

        private void ProcessGenerate()
        {
            if (!_playerProvider.TryGetPlayer(out var player)) return;
            
            _stageGenerator.GenerateFromEditor();
            //_playerProvider?.SetPosition(_stageGenerator.StageStartPoint.position);
        }
        
        private void ProcessUpGrade()
        {
            _upgradeManager.StartUpGradePhase();
        }

        public void Dispose()
        {
            _subscription?.Dispose();
            _upgradeManager.OnUpgradePhaseEnded -= ProcessUpGrade;
        }

        public void PostStart()
        {
            ProcessGenerate();
        }
    }
    
    public struct LevelClearedMessage
    { }
}