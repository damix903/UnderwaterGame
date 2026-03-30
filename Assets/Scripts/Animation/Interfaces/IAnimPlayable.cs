using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IAnimPlayable
{
    public void PlayBaseClip(AnimationClip clip);
    public UniTask<bool> PlayOverlayAnimation(OverlayAnimData data, CancellationToken ct = default);
}