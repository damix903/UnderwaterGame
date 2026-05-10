using UnityEngine;

public interface ICharacterController
{
    public GameObject GameObject { get; }
    public Transform Transform { get; }
    public Transform Target { get; }
}