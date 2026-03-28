using MessagePipe;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class UI : MonoBehaviour
{
    private Slider _slider;
    [SerializeField] private PlayerHealth _health;
    [Inject] private ISubscriber<EventPublisher, HealthChangeEventArgs> _subscriber;
    
    void Start()
    {
        _slider = GetComponent<Slider>();

        _subscriber?.Subscribe(EventPublisher.Player, HandleHealthChangeEvent);
    }

    private void HandleHealthChangeEvent(HealthChangeEventArgs args)
    {
        _slider.value = args.Current / args.Max;
    }
}
