using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PsychoSerum.Interfaces;
using UnityEngine.Events;
using PlazmaGames.Core;
using PlazmaGames.MonoSystems.Animation;
using PsychoSerum.Player;

namespace PsychoSerum.Interactables
{
    internal class Door : MonoBehaviour, IInteractable
    {
        //[SerializeField] private float _rotationDuration = 1.0f;
        [SerializeField] private float _rotationAmount = 90.0f;
        [SerializeField][Range(-1, 1)] private float _forwardDirection = 0;
        [SerializeField] private AudioSource _audioSource;

        private bool _isOpen;
        private bool _isLocked;
        private Vector3 _startRotation;
        private Vector3 _forward;

        private void Rotate(float progress, Quaternion start, Quaternion end)
        {
            transform.localRotation = Quaternion.Slerp(start, end, progress);
        }

        private void Open(Vector3 playerPos)
        {
            _isOpen = true;
            float dot = Vector3.Dot(_forward, (playerPos - transform.position).normalized);
            Quaternion start = transform.localRotation;
            Quaternion end;

            if (dot >= _forwardDirection) end = Quaternion.Euler(0, _startRotation.y - _rotationAmount, 0);
            else end = Quaternion.Euler(0, _startRotation.y + _rotationAmount, 0);

            GameManager.GetMonoSystem<IAnimationMonoSystem>().RequestAnimation(
                this,
                _audioSource.clip.length,
                (float progress) => Rotate(progress, start, end)
            );
        }

        private void Close()
        {
            _isOpen = false;
            Quaternion start = transform.localRotation;
            Quaternion end = Quaternion.Euler(_startRotation);
            GameManager.GetMonoSystem<IAnimationMonoSystem>().RequestAnimation(
                 this,
                 _audioSource.clip.length,
                 (float progress) => Rotate(progress, start, end)
             );
        }

        public bool Interact(Interactor interactor)
        {
            if (_isLocked) return true;

            if (!_isOpen)
            {
                _audioSource.Play();
                Open(interactor.transform.position);
            }
            else
            {
                _audioSource.Play();
                Close();
            }

            return true;
        }

        public void EndInteraction()
        {

        }

        private void Awake()
        {
            if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
            _forward = -transform.right;
        }
    }
}
