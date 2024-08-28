using PlazmaGames.Core;
using PlazmaGames.MonoSystems.Animation;
using PsychoSerum.Interfaces;
using PsychoSerum.Player;
using PsychoSerum.Puzzle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum.Interactables
{
    internal class PortalDoor : MonoBehaviour, IInteractable
    {
        [SerializeField] private float _rotationAmount = 90.0f;
        [SerializeField][Range(-1, 1)] private float _forwardDirection = 0;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Portal _portal;

        private bool _isOpen;
        private Vector3 _startRotation;
        private Vector3 _forward;

        private void Rotate(float progress, Quaternion start, Quaternion end)
        {
            transform.localRotation = Quaternion.Slerp(start, end, progress);
        }

        public void Open(Vector3 playerPos, float forwardDirection = 0)
        {
            _isOpen = true;
            float dot = Vector3.Dot(_forward, (playerPos - transform.position).normalized);
            Quaternion start = transform.localRotation;
            Quaternion end;

            if (dot >= forwardDirection) end = Quaternion.Euler(0, _startRotation.y - _rotationAmount, 0);
            else end = Quaternion.Euler(0, _startRotation.y + _rotationAmount, 0);

            GameManager.GetMonoSystem<IAnimationMonoSystem>().RequestAnimation(
                this,
                _audioSource.clip.length,
                (float progress) => Rotate(progress, start, end)
            );
        }

        public void Close()
        {
            _isOpen = false;
            Quaternion start = transform.localRotation;
            Quaternion end = Quaternion.Euler(_startRotation);
            GameManager.GetMonoSystem<IAnimationMonoSystem>().RequestAnimation(
                 this,
                 _audioSource.clip.length,
                 (float progress) => Rotate(progress, start, end),
                 () => {
                    _portal.ClosePortal();
                    _portal.other.ClosePortal();
                }
             );
        }

        public bool Interact(Interactor interactor)
        {
            if (_portal.other == null) return false;

            if (!_isOpen)
            {
                _audioSource.Play();
                _portal.other.GetComponent<PortalDoor>().Open(interactor.transform.position, 1);
                _portal.OpenPortal();
                _portal.other.OpenPortal();
                Open(interactor.transform.position, _forwardDirection);
            }
            else
            {
                _audioSource.Play();
                _portal.other.GetComponent<PortalDoor>().Close();
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

        public bool IsPickupable()
        {
            return false;
        }

        public void OnPickup(Interactor interactor)
        {

        }
    }
}
