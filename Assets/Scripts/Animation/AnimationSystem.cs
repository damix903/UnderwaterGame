using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Animations;
using UnityEngine.Playables;
using VContainer;

[RequireComponent(typeof(Animator))]
public class AnimationSystem : MonoBehaviour, IAnimPlayable
{
    private Animator _anim;
    private IAnimEventListenable _listener;
    
    private AnimationClip _baseClip;
    
    [Header("Animation Graph")]
    private PlayableGraph _graph;
    private AnimationLayerMixerPlayable _mixer;

    private AnimationClipPlayable _basePlayable;
    private AnimationClipPlayable _overlayPlayable;
    private AnimationPlayableOutput _output;

    private const int BaseLayer = 0;
    private const int OverlayLayer = 1;

    private CancellationTokenSource _overlayCts = new();

    public void Initialize(IAnimEventListenable listener)
    {
        _listener = listener;
    }
    
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _anim.speed = 1f;
        
        SetupGraph();
    }
    
    private void SetupGraph()
    {
        // グラフと出力の作成
        _graph = PlayableGraph.Create(gameObject.name + "AnimationGraph");
        _graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        _output = AnimationPlayableOutput.Create(_graph, "Animation", GetComponent<Animator>());

        // レイヤーとミクサーの設定
        _mixer = AnimationLayerMixerPlayable.Create(_graph, 2);
        _output.SetSourcePlayable(_mixer);
        
        // レイヤーの重みの初期化、1で有効、0で無効
        _mixer.SetInputWeight(BaseLayer, 1f);
        _mixer.SetInputWeight(OverlayLayer, 0f);
        
        _graph.Play();
    }

    private void OnDestroy()
    {
        if (_graph.IsValid()) _graph.Destroy();
    }
    
    public void PlayBaseClip(AnimationClip clip)
    {
        if (clip == null || clip == _baseClip && _basePlayable.IsValid()) return;
        DestroyBasePlayable();

        // 新しいPlayableを作成して接続する
        _basePlayable = AnimationClipPlayable.Create(_graph, clip);
        _graph.Connect(_basePlayable, 0, _mixer, BaseLayer);
        _basePlayable.Play();
        _baseClip = clip;
    }

    private void DestroyBasePlayable()
    {
        if (_basePlayable.IsValid())
        {
            // 既存のPlayableを切断して破棄
            // 破棄しないとメモリリークする
            _graph.Disconnect(_mixer, BaseLayer);
            _basePlayable.Destroy();
        }
    }

    public async UniTask<bool> PlayOverlayAnimation(OverlayAnimData data, CancellationToken ct = default)
    {
        // すでに再生中なら再生を止める
        _overlayCts?.Cancel();
        _overlayCts?.Dispose();
        var thisCst = CancellationTokenSource.CreateLinkedTokenSource(ct);
        _overlayCts = thisCst;
        var linkedToken = _overlayCts.Token;

        SetupOverlayPlayable(data);
        
        try
        {
            if (data.OverlayType == OverlayType.Once)
            {
                await UniTask.WaitUntil(() => _overlayPlayable.IsDone(),
                    cancellationToken: linkedToken);
            }
            else
            {
                // キャンセルされるまで待機
                await UniTask.WaitUntil(() => false, cancellationToken: linkedToken);
            }

            return true;
        }
        catch (OperationCanceledException)
        {
            return false;
        }
        finally
        {
            // Overlayの上書きによってキャンセルされたときはfalseになる
            if (_overlayCts == thisCst)
            {
                if(_mixer.IsValid()) _mixer.SetInputWeight(OverlayLayer, 0f);
            }
        }
    }

    private void SetupOverlayPlayable(OverlayAnimData data)
    {
        if (_overlayPlayable.IsValid())
        {
            _graph.Disconnect(_mixer, OverlayLayer);
            _overlayPlayable.Destroy();
        }

        // クリップの作成
        _overlayPlayable = AnimationClipPlayable.Create(_graph, data.Clip);

        double duration = data.OverlayType switch
        {
            OverlayType.Once or OverlayType.Hold => data.Clip.length - .01f,
            OverlayType.Loop => double.MaxValue,
            _ => 0f
        };
        _overlayPlayable.SetDuration(duration);
        
        // Behaviorの作成
        var eventPlayable = ScriptPlayable<AnimEventBehavior>.Create(_graph, OverlayLayer);
        eventPlayable.SetDuration(data.Clip.length);
        var behaviour = eventPlayable.GetBehaviour();
        behaviour.Initialize(data, _listener);

        // behaviorをクリップに接続
        _graph.Connect(_overlayPlayable, 0, eventPlayable, 0);
        eventPlayable.SetInputWeight(0, 1f);

        _graph.Connect(eventPlayable, 0, _mixer, OverlayLayer);

        _mixer.SetInputWeight(OverlayLayer, 1f);
    }
}
