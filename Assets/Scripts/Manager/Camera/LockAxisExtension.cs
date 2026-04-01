using Unity.Cinemachine;
using UnityEngine;

[ExecuteInEditMode]
[SaveDuringPlay]
public class LockAxisExtension : CinemachineExtension
{
    [SerializeField] private bool lockX, lockY, lockZ;
    [SerializeField] private Vector3 lockAxis;

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        base.PostPipelineStageCallback(vcam, stage, ref state, deltaTime);

        if (stage != CinemachineCore.Stage.Body) return;
        
        var pos = state.RawPosition;
        pos.x = lockX ? lockAxis.x : pos.x;
        pos.y = lockY ? lockAxis.y : pos.y;
        pos.z = lockZ ? lockAxis.z : pos.z;
        state.RawPosition = pos;
    }
}