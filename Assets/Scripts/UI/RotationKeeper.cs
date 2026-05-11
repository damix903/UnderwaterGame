using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class RotationKeeper : MonoBehaviour
    {
        private RectTransform _transform;
        private Quaternion _initialRotation;
        
        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
            _initialRotation = _transform.rotation;
        }
        
        // Updateの後に呼ばれるLateUpdateで、回転が初期値から変わっていたら元に戻す
        private void LateUpdate()
        {
            if (_transform.rotation != _initialRotation)
                _transform.rotation = _initialRotation;
        }
    }
}