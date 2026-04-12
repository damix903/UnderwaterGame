using System;
using Manager.Upgrade;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [Serializable]
    public class UpgradeElement : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image image;

        public void SetUp(UpgradeData data, Action<UpgradeData> callback)
        {
            text.text = data.name;
            image = data.Image;
            button.onClick.AddListener(() => { callback?.Invoke(data); });
        }

        public void RemoveListener() => button.onClick.RemoveAllListeners();

        private void OnDisable()
        {
            RemoveListener();
        }
    }
}