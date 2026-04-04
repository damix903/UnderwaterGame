using System;
using MessagePipe;
using TMPro;
using UnityEngine;
using VContainer;

public class PlayerStatsManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float comboTime = 5f;
    
    [Inject] private IPublisher<EventPublisher, ComboEvent> _publisher;

    private IDisposable _subscription;
    private int _comboCounter;
    private float _comboTimer;

    [Inject]
    public void Construct(ISubscriber<DeathEvent> subscriber)
    {
        if (_subscription != null) return;
        _subscription = subscriber.Subscribe(HandleDeathEvent);
        _comboCounter = 0;
        _comboTimer = 0f;
    }

    private void Update()
    {
        _comboTimer = Mathf.Max(0f, _comboTimer - Time.deltaTime);
        if (_comboTimer <= 0f && _comboCounter != 0)
            ChangeComboCount(true);

        ChangeText();
    }

    private void HandleDeathEvent(DeathEvent e)
    {
        if (e.TeamID != TeamID.Enemy) return;

        ChangeComboCount();
    }

    private void ChangeComboCount(bool isReset = false)
    {
        if (isReset)
        {
            _comboCounter = 0;
            _comboTimer = 0f;
        }
        else
        {
            _comboCounter++;
            _comboTimer = comboTime;
        }
        
        _publisher?.Publish(EventPublisher.System, new ComboEvent(_comboCounter, _comboTimer));
    }
    
    private void ChangeText() => text.text = $"{_comboCounter} : {_comboTimer}";

    private void OnDestroy()
    {
        _subscription?.Dispose();
    }
}

public struct ComboEvent
{
    public readonly int Count;
    public readonly float Time;

    public ComboEvent(int count, float time)
    {
        Count = count;
        Time = time;
    }
}