using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Manager.Upgrade;
using UnityEngine;

namespace UI
{
    public class UpgradeView : MonoBehaviour, IUpgradeView
    {
        [SerializeField] private List<UpgradeElement> upGradeElements = new List<UpgradeElement>();
        
        private UniTaskCompletionSource<UpgradeData> _tcs;

        public void ShowUpgrades(List<UpgradeData> upgrades)
        {
            DisposeTcs();
            // アップグレードの選択を待機するためのUniTaskCompletionSourceを作成
            // 1つだけ選択できるようにする
            _tcs = new UniTaskCompletionSource<UpgradeData>();
            
            for (int i = 0; i < upgrades.Count; i++)
            {
                if (i >= upGradeElements.Count)
                {
                    Debug.LogWarning("Not enough UpGradeElements to display all upgrades.");
                    break;
                }
                
                var element = upGradeElements[i];
                var data = upgrades[i];
                
                element.RemoveListener();
                
                // elementのSetUpでアップグレードが選択されたときに、_tcsに結果がセットされるようにする
                element.SetUp(data, (x) => { _tcs?.TrySetResult(x); });
                element.gameObject.SetActive(true);
            }
        }

        public void HideUpgrade()
        {
            foreach (var element in upGradeElements)
            {
                element.RemoveListener();
                element.gameObject.SetActive(false);
            }
            DisposeTcs();
        }

        public async UniTask<UpgradeData> OnUpgradeSelectedAsync(CancellationToken ct)
        {
            if (_tcs == null)
            {
                throw new InvalidOperationException("Upgrade selection has not been started. Call ShowUpgrade first.");
            }
            
            // elementのSetUpでアップグレードが選択されたときに_tcsに結果がセットされるので、それを待機する
            // タスクが完了するか、キャンセルされるまで待機する
            return await _tcs.Task.AttachExternalCancellation(ct);
        }

        private void DisposeTcs()
        {
            _tcs?.TrySetCanceled();
            _tcs = null;
        }

        private void OnDestroy()
        {
            DisposeTcs();
        }
    }
}