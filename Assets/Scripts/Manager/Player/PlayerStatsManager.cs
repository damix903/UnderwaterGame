using System;
using MessagePipe;
using TMPro;
using UnityEngine;
using VContainer;

public class PlayerStatsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float comboTime = 5f;
    
    [Inject] private IPublisher<ComboEvent> _publisher;

    private IDisposable _subscription;
    private int _comboCounter;
    private float _comboTimer;

    [Inject]
    public void Construct(ISubscriber<DeathEvent> deathSub,
        ISubscriber<EventPublisher, DamageResult> damageSub,
        ISubscriber<EventPublisher, LandedEvent> landedSub)
    {
        if (_subscription != null) return;
        var bag = DisposableBag.CreateBuilder();
        deathSub.Subscribe(HandleDeathEvent).AddTo(bag);
        damageSub.Subscribe(EventPublisher.Player, result => ChangeComboCount(true)).AddTo(bag);
        landedSub.Subscribe(EventPublisher.Player, @event => ChangeComboCount(true)).AddTo(bag);
        _subscription = bag.Build();
        
        _comboCounter = 0;
        _comboTimer = 0f;
        ChangeText();
    }

    private void Update()
    {
        // _comboTimer = Mathf.Max(0f, _comboTimer - Time.deltaTime);
        // if (_comboTimer <= 0f && _comboCounter != 0)
        //     ChangeComboCount(true);
        //
        // ChangeText();
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
        
        _publisher?.Publish(new ComboEvent(_comboCounter, _comboTimer));
        ChangeText();
    }

    private void ChangeText()
    {
        text.text = $"{_comboCounter}";
        bool shouldActive = _comboCounter > 0;
        text.gameObject.SetActive(shouldActive);
    } 

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