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
        [SerializeField] private Light _light;

        [Header("Settings")]
        [SerializeField] private Vector2 _delayLimits;

        private bool _isFlickering;
        private float _delay;

        private IEnumerator Flicker()
        {
            _isFlickering = true;
            _light.enabled = false;
            _delay = Random.Range(_delayLimits.x, _delayLimits.y);
            yield return new WaitForSeconds(_delay);
            _light.enabled = true;
            _delay = Random.Range(_delayLimits.x, _delayLimits.y);
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
