using System;
using System.Collections.Generic;
using MessagePipe;
using PlayerSystem;
using ProjectileSystem;
using UnityEngine;
using VContainer;

namespace Manager.UpGrade
{
    public class UpGradeManager : MonoBehaviour
    {
        [SerializeField] private UpGradeSetting _upGradeSetting;
        [SerializeField] private List<UpGradeData> _currentUpGradeList = new List<UpGradeData>();

        private RunState _runState = new RunState();
        // [Inject] private IPublisher<RunState> _runTimeStatePub;
        // [Inject] private IPublisher<UpGradeSelectEvent> _upGradeSelectEventPub;
        [Inject] private IPlayerProvider _playerProvider;
        
        [ContextMenu("StartUpGrade")]
        public void StartUpGrade()
        {
            var upGrades = _upGradeSetting.GetRandomUpGrade(_currentUpGradeList);
            
            foreach (var upGrade in upGrades)
                Debug.Log(upGrade);

            _selection = upGrades;
            // プレイヤーの選択を待機するべき？
            //_upGradeSelectEventPub.Publish(new UpGradeSelectEvent(upGrades));
        }

        private List<UpGradeData> _selection;
        [SerializeField] private int index;

        [ContextMenu("SelectUpGrade")]
        public void SelectUpGrade()
        {
            // select from _selection by index
            if (_selection == null || _selection.Count == 0) throw new InvalidOperationException("No UpGrades available for selection.");
            if (index < 0 || index >= _selection.Count) throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range of the selection list.");
            
            var upGrade = _selection[index];
            SelectUpGrade(upGrade);
        }

        public void SelectUpGrade(UpGradeData upGrade)
        {
            upGrade.Apply(_runState);
            _currentUpGradeList.Add(upGrade);
            _runState.UpGradeList.Add(upGrade);
            if(_playerProvider.TryGetPlayerClass(out var player))
                player.ApplyRunState(_runState);
        }
    }
    
    public class RunState
    {
        public List<IProjectileModifier> ProjModifiers = new List<IProjectileModifier>();
        public List<UpGradeData> UpGradeList = new List<UpGradeData>();
    }
    
    // UpgradeをUIに伝えるためのイベント
    public class UpGradeSelectEvent
    {        
        public List<UpGradeData> UpGradeList { get; }

        public UpGradeSelectEvent(List<UpGradeData> upGradeList)
        {
            UpGradeList = upGradeList;
        }
    }
}