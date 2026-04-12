using MessagePipe;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class UIHUD : MonoBehaviour
{
    [Inject] private ISubscriber<EventPublisher, HealthChangeEvent> _subscriber;
    private HealthBar _healthBar;
    
    void Start()
    {
        _subscriber?.Subscribe(EventPublisher.Player, HandleHealthChangeEvent);
        _healthBar = GetComponentInChildren<HealthBar>();
    }

    private void HandleHealthChangeEvent(HealthChangeEvent args)
    {
        _healthBar.ChangeBar(args.Current, args.Max, args.ChangedAmount);
    }
}
