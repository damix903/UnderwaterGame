using System;
using MessagePipe;
using TMPro;
using UnityEngine;
using VContainer;

public class PlayerStatsManager : MonoBehaviour
{
    [Inject] private ISubscriber<DeathEvent> _subscriber;
    
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private TextMeshProUGUI text;

    private int _comboCounter;
    private float _comboTimer;
    
    private void Start()
    {
        _subscriber?.Subscribe(HandleDeathEvent);
        _comboCounter = 0;
        _comboTimer = 0f;
    }

    private void Update()
    {
        _comboTimer = Mathf.Max(0f, _comboTimer - Time.deltaTime);
        if (_comboTimer <= 0f && _comboCounter != 0)
        {
            _comboCounter = 0;
        }
        ChangeText();
    }

    private void HandleDeathEvent(DeathEvent e)
    {
        if (e.TeamID != TeamID.Enemy) return;

        _comboTimer = 5f;
        _comboCounter++;
        
        if (_comboCounter < 5) return;
        var amount = _comboCounter / 3f;
        amount = Mathf.Min(amount, 10f);
        playerHealth.AddHealth(amount);
    }
    
    private void ChangeText() => text.text = $"{_comboCounter} : {_comboTimer}";
}