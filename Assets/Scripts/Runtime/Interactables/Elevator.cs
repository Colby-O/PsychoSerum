using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PsychoSerum.Interfaces;
using PsychoSerum.Player;
using PlazmaGames.Core;
using PlazmaGames.MonoSystems.Animation;
using Unity.VisualScripting;

namespace PsychoSerum.Interactables
{
	internal class ElevatorButton : MonoBehaviour, IInteractable
	{
		[SerializeField] private const float _bottomPos = -4.6f;//4;
        [SerializeField] private const float _topPos = 18.605f;//-117;

        [SerializeField] private AudioSource _audioSrc;
        [SerializeField] private AudioClip _gateSound;
        [SerializeField] private Transform _elevator;
        [SerializeField] private Transform _gateTop;
        [SerializeField] private Transform _gateBottom;
		[SerializeField] private Vector2 _gateRange;
        [SerializeField] private float _elevatorSpeed = 1;
        private float _elevatorPos = 1;

		private float _startX, _startZ;

        private bool _isGateOpening = false;

        public void EndInteraction()
		{
		}

		public bool Interact(Interactor interactor)
		{
            if (_isGateOpening) return false;
			_elevatorSpeed *= -1;
			return true;
		}

        public void OpenGate(float t, Transform gate, float start, float end)
        {
            gate.localPosition = new Vector3(Mathf.Lerp(start, end, t), gate.localPosition.y, gate.localPosition.z);
        }

        private void Start()
        {
            Debug.Log("Lerp Test: " + Mathf.Lerp(_gateBottom.localPosition.x, _gateRange.y, 1f));
            _startX = _elevator.localPosition.x;
            _startZ = _elevator.localPosition.z;
        }

        private void FixedUpdate()
        {
            if (!PsychoSerumGameManager.hasStarted) return;
            if (_isGateOpening) return;

            _elevatorPos = Mathf.Clamp(_elevatorPos - _elevatorSpeed, 0, 1);
			float lastHeight = _elevator.localPosition.y;
            _elevator.localPosition = new Vector3(_startX, Mathf.SmoothStep(_bottomPos, _topPos, _elevatorPos), _startZ);

            if (_elevator.localPosition.y != lastHeight && !_audioSrc.isPlaying) _audioSrc.Play();
            else if (_elevator.localPosition.y == lastHeight && _audioSrc.isPlaying && !_isGateOpening) _audioSrc.Stop();

            if (_elevator.localPosition.y == _topPos && _elevator.localPosition.y != lastHeight)
            {
                _isGateOpening = true;
                _audioSrc.Stop();
                _audioSrc.PlayOneShot(_gateSound);
                float start = _gateBottom.localPosition.x;
                float end = _gateRange.y;
                GameManager.GetMonoSystem<IAnimationMonoSystem>().RequestAnimation(
                    this,
                    _gateSound.length,
                    (float t) => OpenGate(
                        t, 
                        _gateBottom, 
                        start,
                        end
                        ),
                    () => _isGateOpening = false
                );
            }
            else if (lastHeight == _topPos && _elevator.localPosition.y != lastHeight)
            {
                _isGateOpening = true;
                _audioSrc.Stop();
                _audioSrc.PlayOneShot(_gateSound);
                float start = _gateBottom.localPosition.x;
                float end = _gateRange.x;
                GameManager.GetMonoSystem<IAnimationMonoSystem>().RequestAnimation(
                    this,
                    _gateSound.length,
                    (float t) => OpenGate(
                        t, 
                        _gateBottom,
                        start,
                        end
                    ),
                    () => _isGateOpening = false
                );
            }
            else if (_elevator.localPosition.y == _bottomPos && _elevator.localPosition.y != lastHeight)
            {
                _isGateOpening = true;
                _audioSrc.Stop();
                _audioSrc.PlayOneShot(_gateSound);
                float start = _gateTop.localPosition.x;
                float end = _gateRange.y;
                GameManager.GetMonoSystem<IAnimationMonoSystem>().RequestAnimation(
                    this,
                    _gateSound.length,
                    (float t) => OpenGate(
                        t, 
                        _gateTop,
                        start,
                        end
                    ),
                    () => _isGateOpening = false
                );
            }
            else if (lastHeight == _bottomPos && _elevator.localPosition.y != lastHeight)
            {
                _isGateOpening = true;
                _audioSrc.Stop();
                _audioSrc.PlayOneShot(_gateSound);
                float start = _gateTop.localPosition.x;
                float end = _gateRange.x;
                GameManager.GetMonoSystem<IAnimationMonoSystem>().RequestAnimation(
                    this,
                    _gateSound.length,
                    (float t) => OpenGate(
                        t, 
                        _gateTop,
                        start,
                        end
                    ),
                    () => _isGateOpening  = false
                );
            }
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
