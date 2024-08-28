using PlazmaGames.Core;
using PsychoSerum.MonoSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum
{
    [RequireComponent(typeof(BoxCollider))]
    internal sealed class Trigger : MonoBehaviour
    {
        [SerializeField] private int _id;

        [SerializeField] private bool _isActive = true;

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _audioClip;

        private bool _triggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!_isActive) return;

            if (!_triggered && other.tag == "Player")
            {
                _triggered = true;
                if (_audioSource != null && _audioClip != null) _audioSource.PlayOneShot(_audioClip);
                GameManager.GetMonoSystem<IEventMonoSystem>().RunEvent(_id);
            }
        }
    }
}
