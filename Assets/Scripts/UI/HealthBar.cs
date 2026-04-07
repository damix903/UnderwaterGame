using System;
using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform bottomBar;
    [SerializeField] private RectTransform topBar;
    [Space]
    [SerializeField] private float animSpeed = 3f;

    private float _current;
    private float _max;

    private float _fullWidth;
    private float TargetWidth => _current * _fullWidth / _max;
    
    private Coroutine _coroutine;

    private void Start()
    {
        _fullWidth = background.rect.width;
        SetWidth(topBar, _fullWidth);
        SetWidth(bottomBar, _fullWidth);
    }

    public void ChangeBar(float current, float max, float amount)
    {
        _current = current;
        _max = max;
        
        if (_coroutine != null) StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(AdjustBarWidth(amount));
    }

    private IEnumerator AdjustBarWidth(float amount)
    {
        var sudden = amount >= 0f ? bottomBar : topBar;
        var slow = amount >= 0f ? topBar : bottomBar;
        SetWidth(sudden, TargetWidth);

        while (Mathf.Abs(sudden.rect.width - slow.rect.width) > 0.01f)
        {
            var width = Mathf.Lerp(slow.rect.width, TargetWidth, Time.deltaTime * animSpeed);
            SetWidth(slow, width);
            yield return null;
        }
        
        SetWidth(slow, TargetWidth);
    }

    private void SetWidth(RectTransform t, float width)
    {
        t.sizeDelta = new Vector2(width, t.rect.height);
    }
}
