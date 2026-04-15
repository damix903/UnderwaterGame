using System.Collections;
using UnityEngine;

namespace Animation
{
    public interface ISpriteBlinker
    {
        void StartBlinking(float duration = 0f);
        void StopBlinking();
    }

    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteBlinker : MonoBehaviour, ISpriteBlinker
    {
        [SerializeField] private float interval = .1f;
        
        private SpriteRenderer _sr;
        
        private Coroutine _blinkCoroutine;
        
        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
        }
        
        public void StartBlinking(float duration)
        {
            StopBlinking();
            _blinkCoroutine = StartCoroutine(Blink(duration));
        }

        public void StopBlinking()
        {
            if (_blinkCoroutine != null) StopCoroutine(_blinkCoroutine);
            _sr.enabled = true;
        }

        private IEnumerator Blink(float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                _sr.enabled = !_sr.enabled;
                yield return new WaitForSeconds(interval);
                elapsed += interval;
            }
            _sr.enabled = true;
        }
    }
}