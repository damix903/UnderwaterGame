using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MessagePipe;
using PlayerSystem;
using ProjectileSystem;
using UI;
using UnityEngine;
using VContainer;

namespace Manager.Upgrade
{
    public class UpgradeManager : MonoBehaviour, IUpgradeModel
    {
        [SerializeField] private UpgradeSetting upgradeSetting;
        [SerializeField] private List<UpgradeData> _currentUpgradeList = new List<UpgradeData>();

        [Inject] private RunState _runState;
        [Inject] private IPlayerProvider _playerProvider;

        [ContextMenu("StartUpGrade")]
        public void StartUpGradePhase()
        {
            // var upGrades = upgradeSetting.GetRandomUpGrade(_currentUpgradeList);
            //
            // OnUpgradePhaseStarted?.Invoke(upGrades);
            //_upgradePresenter.StartUpgradeSelectionAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        public List<UpgradeData> GetUpGradeList()
        {
            return upgradeSetting.GetRandomUpGrade(_currentUpgradeList);
        }

        public void SelectUpGrade(UpgradeData upgrade)
        {
            upgrade.Apply(_runState);
            _currentUpgradeList.Add(upgrade);
            _runState.UpGradeList.Add(upgrade);
            if(_playerProvider.TryGetPlayerClass(out var player))
                player.ApplyRunState(_runState);
        }
    }
    
    public class RunState
    {
        public List<IProjectileModifier> ProjModifiers = new List<IProjectileModifier>();
        public List<UpgradeData> UpGradeList = new List<UpgradeData>();
    }
}