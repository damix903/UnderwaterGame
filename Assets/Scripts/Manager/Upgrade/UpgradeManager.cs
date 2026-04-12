using System;
using System.Collections.Generic;
using MessagePipe;
using PlayerSystem;
using ProjectileSystem;
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
        public event Action<List<UpgradeData>> OnUpgradeSelection; 
        
        [ContextMenu("StartUpGrade")]
        public void StartUpGrade()
        {
            var upGrades = upgradeSetting.GetRandomUpGrade(_currentUpgradeList);
            
            OnUpgradeSelection?.Invoke(upGrades);
            
            // プレイヤーの選択を待機するべき？
            //_upGradeSelectEventPub.Publish(new UpGradeSelectEvent(upGrades));
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
    
    // UpgradeをUIに伝えるためのイベント
    public class UpGradeSelectEvent
    {        
        public List<UpgradeData> UpGradeList { get; }

        public UpGradeSelectEvent(List<UpgradeData> upGradeList)
        {
            UpGradeList = upGradeList;
        }
    }
}