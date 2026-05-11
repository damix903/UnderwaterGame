using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Input;
using MessagePipe;
using PlayerSystem;
using Stage;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Manager
{
    public class RunManager : IDisposable, IPostStartable
    {
        private readonly UpgradePresenter _upgradePresenter;
        private readonly StageGenerator _stageGenerator;
        private readonly IPlayerProvider _playerProvider;
        [Inject] private IFader _fader;
        [Inject] private CameraManager _cameraManager;
        [Inject] private InputReader _inputReader;
        [Inject] private TimeManager _timeManager;
        [Inject] private GameOverUI _gameOverUI;
        
        private IDisposable _subscription;
        
        private CancellationTokenSource _cts = new CancellationTokenSource();
        
        public RunManager(UpgradePresenter upgradePresenter,
            StageGenerator stageGenerator, 
            IPlayerProvider playerProvider,
            ISubscriber<LevelClearedMessage> levelClearedSub,
            ISubscriber<EventPublisher, DeathEvent> deathSub)
        {
            _upgradePresenter = upgradePresenter;
            _stageGenerator = stageGenerator;
            _playerProvider = playerProvider;

            var bag = DisposableBag.CreateBuilder();
            levelClearedSub.Subscribe(_ => ProcessUpgrade().Forget()).AddTo(bag);
            deathSub?.Subscribe(EventPublisher.Player, e => ProcessDeath(e).Forget()).AddTo(bag);
            
            _subscription = bag.Build();
        }

        private async UniTask ProcessDeath(DeathEvent e)
        {
            _timeManager.PauseGame();
            _gameOverUI.gameObject.SetActive(true);
            _cts = new CancellationTokenSource();
            var selection = await _gameOverUI.WaitSelection(_cts.Token);
            
            _gameOverUI.gameObject.SetActive(false);
            
            switch (selection)
            {
                case GameOverSelection.Restart:
                    await _fader.FadeOutAsync(_cts.Token);
                    _cameraManager.SetLookaheadEnabled(false);
                    
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    
                    break;
                case GameOverSelection.Finish:
                    Application.Quit();
                    break;
            }
            _timeManager.ResumeGame();
        }

        public void PostStart()
        {
            _gameOverUI.gameObject.SetActive(false);
            ProcessGenerate().Forget();
        }

        private async UniTask ProcessGenerate()
        {
            if (!_playerProvider.TryGetPlayer(out var player)) return;
            
            _cameraManager.SetLookaheadEnabled(false);
            await _fader.FadeOutAsync(_cts.Token);
            
            var start= _stageGenerator.Generate();
            _playerProvider?.SetPosition(start);

            await _fader.FadeInAsync(_cts.Token);
            _cameraManager.SetLookaheadEnabled(true);
            _inputReader.TogglePlayerActionMap(true);
            _timeManager.ResumeGame();
        }
        
        private async UniTask ProcessUpgrade()
        {
            _inputReader.TogglePlayerActionMap(false);
            _timeManager.PauseGame();
            
            _cts = new CancellationTokenSource();
            await _upgradePresenter.StartUpgradeSelectionAsync(_cts.Token);
            
            ProcessGenerate().Forget();
        }

        public void Dispose()
        {
            _subscription?.Dispose();
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
    
    public struct LevelClearedMessage
    { }
}