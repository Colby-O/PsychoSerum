using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PsychoSerum.Ememy
{
    internal class Jumpscare : MonoBehaviour
    {
        [SerializeField] private GameObject _enemy;
        [SerializeField] private AudioSource _audioSource;

        private bool _used = false;

        public UnityAction onComplete = null; 

        private IEnumerator JumpScare(float start, float length, bool isFakeOut)
        {
            float t = 0;
            _audioSource.time = start;
            _audioSource.Play();
            while (t < length)
            {
                if (t > Random.Range(0f, 1f)) _enemy.SetActive(true && !isFakeOut);
                t += Time.deltaTime;
                if (start + t > _audioSource.clip.length) _audioSource.Stop();
                yield return new WaitForEndOfFrame();
            }
            _audioSource.Stop();
            _enemy.SetActive(false);
            onComplete?.Invoke();
        }

        public void StartJumpScare(float start, float time, bool isFakeOut)
        {
            _used = true;
            StartCoroutine(JumpScare(start, time, isFakeOut));
        }

        private void Start()
        {
            _enemy.SetActive(false);
            if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
        }
    }
}
