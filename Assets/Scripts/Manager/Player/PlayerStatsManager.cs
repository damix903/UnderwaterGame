using System;
using MessagePipe;
using PlayerSystem;
using TMPro;
using UnityEngine;
using VContainer;

public class PlayerStatsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    [Inject] private IPublisher<ComboEvent> _publisher;
    [Inject] private IPlayerProvider _playerProvider;

    private ICollisionDetectable _playerCollision;
    private IDisposable _subscription;
    private int _comboCounter;

    [Inject]
    public void Construct(ISubscriber<DeathEvent> deathSub,
        ISubscriber<EventPublisher, DamageResult> damageSub)
    {
        if (_subscription != null) return;
        var bag = DisposableBag.CreateBuilder();
        deathSub.Subscribe(HandleDeathEvent).AddTo(bag);
        damageSub.Subscribe(EventPublisher.Player, result => ChangeComboCount(true)).AddTo(bag);
        _subscription = bag.Build();
        
        _comboCounter = 0;
        ChangeText();
    }

    private void Start()
    {
        if (!_playerProvider.TryGetPlayerClass(out var player)) return;

        _playerCollision = player.gameObject.GetComponent<ICollisionDetectable>();
        if (_playerCollision != null) _playerCollision.OnLanded += HandleLanded;
    }

    private void HandleLanded() => ChangeComboCount(true);

    private void HandleDeathEvent(DeathEvent e)
    {
        if (e.TeamID != TeamID.Enemy) return;

        ChangeComboCount();
    }

    private void ChangeComboCount(bool isReset = false)
    {
        _comboCounter = isReset ? 0 : _comboCounter + 1;
        
        if (_playerCollision.IsGrounded) _comboCounter = 0; 
        
        _publisher?.Publish(new ComboEvent(_comboCounter, 0f));
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
        if (_playerCollision != null) _playerCollision.OnLanded -= HandleLanded;
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