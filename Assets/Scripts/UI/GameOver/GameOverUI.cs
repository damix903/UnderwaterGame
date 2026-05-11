using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private Button finishButton;
        [SerializeField] private TextMeshProUGUI scoreText;
        
        [Inject] private PlayerStatsManager _playerStatsManager;
        
        private UniTaskCompletionSource<GameOverSelection> _selectionTcs;
        
        private void OnEnable()
        {
            restartButton.onClick.AddListener(OnRestartClicked);
            finishButton.onClick.AddListener(OnFinishClicked);
        }

        private void OnDisable()
        {
            restartButton.onClick.RemoveListener(OnRestartClicked);
            finishButton.onClick.RemoveListener(OnFinishClicked);
        }

        public void SetScore(int score) => scoreText.text = $"スコア : {score} m";

        public async UniTask<GameOverSelection> WaitSelection(CancellationToken ct)
        {
            _selectionTcs = new UniTaskCompletionSource<GameOverSelection>();
            
            SetScore((int)_playerStatsManager.TotalDepth);

            await using (ct.Register(() => _selectionTcs.TrySetCanceled()))
            {
                return await _selectionTcs.Task;
            }
        }

        private void OnRestartClicked() => _selectionTcs.TrySetResult(GameOverSelection.Restart);

        private void OnFinishClicked() => _selectionTcs.TrySetResult(GameOverSelection.Finish);
    }
    
    public enum GameOverSelection
    {
        Restart,
        Finish
    }
}