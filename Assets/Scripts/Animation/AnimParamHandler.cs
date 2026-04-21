using System;
using Input;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(Animator))]
public class AnimParamHandler : MonoBehaviour
{
    private Animator _anim;
    [Inject] private IInputProvider _input;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float angle = .5f;
        if (_input.AimDir.sqrMagnitude > .01f)
        {
            angle = Mathf.Atan2(_input.AimDir.x, _input.AimDir.y) * Mathf.Rad2Deg;
            angle = angle == 0 ? 0f : Mathf.Abs(angle) / 180f;
            angle = Mathf.Clamp(angle, 0f, 1f);
        }
        
        _anim.SetFloat("Angle", angle);
    }
}