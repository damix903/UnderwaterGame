using UnityEngine;

public interface ICharacterController
{
    public GameObject GameObject { get; }
    public Transform Target { get; }
}