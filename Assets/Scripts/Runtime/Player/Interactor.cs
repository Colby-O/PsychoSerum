using PsychoSerum.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PsychoSerum.Player
{
	[RequireComponent(typeof(PlayerInput))]
	internal sealed class Interactor : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] private PlayerInput _playerInput;

		[SerializeField] private Transform _interactionPoint;
		[SerializeField] private LayerMask _interactionLayer;
		[SerializeField] private float _interactionRadius = 0.1f;

		private InputAction _interactAction;

		private void StartInteraction(IInteractable interactable)
		{
			interactable.Interact(this);
		}

		private void CheckForInteraction()
		{
			Collider[] colliders = Physics.OverlapSphere(_interactionPoint.position, _interactionRadius, _interactionLayer);

			if (colliders.Length == 0) return;

			for (int i = 0; i < colliders.Length; i++)
			{
				IInteractable interactable = colliders[i].GetComponent<IInteractable>();
				if (interactable != null) StartInteraction(interactable);
			}
		}

		private void Interact(InputAction.CallbackContext e)
		{
			CheckForInteraction();
		}

		private void Awake()
		{
			if (_playerInput == null) _playerInput = GetComponent<PlayerInput>();

			_interactAction = _playerInput.actions["Interact"];

			_interactAction.performed += Interact;
		}
	}
}
