using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Manager.Upgrade;

namespace UI
{
    /// <summary>
    /// アップグレードの処理を担当するクラス
    /// UpgradeViewとUpgradeManagerを仲介する役割を持つ
    /// </summary>
    public class UpgradePresenter : IDisposable
    {
        private readonly IUpgradeView _view;
        private readonly IUpgradeModel _model;
        
        private CancellationTokenSource _cts = new CancellationTokenSource();
        
        public UpgradePresenter(IUpgradeView view, IUpgradeModel model)
        {
            _view = view;
            _model = model;
            _view.HideUpgrade();
        }
        
        // 上位のRunManagerから呼び出されることを想定
        public async UniTask StartUpgradeSelectionAsync(CancellationToken ct)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            var upgrades = _model.GetUpGradeList();
            _view.ShowUpgrades(upgrades);

            try
            {
                var data = await _view.OnUpgradeSelectedAsync(_cts.Token);
                _model.SelectUpGrade(data);
            }
            catch (OperationCanceledException)
            {
                
            }
            finally
            {
                _view?.HideUpgrade();
            }
        }

        public void Dispose()
        {
            _view?.HideUpgrade();
            
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}