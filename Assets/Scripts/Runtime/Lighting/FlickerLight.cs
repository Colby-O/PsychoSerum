using PlazmaGames.Core;
using PlazmaGames.MonoSystems.Animation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum.Lighting
{
    internal class FlickerLight : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private List<Light> _lights;

        [Header("Settings")]
        [SerializeField] private Vector2 _flickerDelayLimits;
        [SerializeField] private Vector2 _recoverDelayLimits;

        private bool _isFlickering;
        private float _delay;

        private void LightEnable()
        {
            foreach (Light light in _lights)
            {
                light.enabled = true;
            }
            _audioSource?.Play();
        }

        private void LightDisable()
        {
            foreach (Light light in _lights)
            {
                light.enabled = false;
            }
            _audioSource?.Stop();
        }

        private IEnumerator Flicker()
        {
            _isFlickering = true;
            LightDisable();
            _delay = Random.Range(_flickerDelayLimits.x, _flickerDelayLimits.y);
            yield return new WaitForSeconds(_delay);
            LightEnable();
            _delay = Random.Range(_recoverDelayLimits.x, _recoverDelayLimits.y);
            yield return new WaitForSeconds(_delay);
            _isFlickering = false;
        }

        private void Update()
        {
            if (!_isFlickering)
            {
                StartCoroutine(Flicker());
            }
        }
    }
}
