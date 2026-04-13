using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IAnimPlayable
{
    public void Initialize(IAnimEventListenable listener, AnimData animData);
    public void PlayBaseClip(AnimationClip clip);
    public UniTask<bool> PlayOverlayAnimation(OverlayAnimData data, CancellationToken ct = default);
    public void Play(AnimType animType);
}

public enum AnimType{ Idle, Move, Fall, Attack, Hit, Die }