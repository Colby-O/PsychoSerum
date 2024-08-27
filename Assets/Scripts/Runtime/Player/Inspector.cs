using PsychoSerum.Inspectables;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PsychoSerum.Player
{
	[RequireComponent(typeof(PlayerInput))]
	internal class Inspector : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] private PlayerInput _playerInput;
		[SerializeField] private GameObject _offset;
		[SerializeField] private PlayerController _controller;
		[SerializeField] private GameObject _head;

		private Vector3 _lastMousePosition;
		private Transform _examinedObject;
		private ExamineType _type;
		private GameObject _objectOffset;

		private Vector3 _origHeadPosition;
		private Quaternion _origHeadRotation;

		private Dictionary<Transform, Vector3> _origPositions = new Dictionary<Transform, Vector3>();
		private Dictionary<Transform, Quaternion> _origRotations = new Dictionary<Transform, Quaternion>();

		private bool _isExaming;
		private bool _movingBack = false;

		public void StartExamine(Transform obj, ExamineType type, GameObject offset)
		{
			if (obj == null) return;

			_isExaming = true;

			_examinedObject = obj;
			_type = type;
			_objectOffset = offset;

			_origHeadPosition = _head.transform.position;
			_origHeadRotation = _head.transform.rotation;

			_origPositions[_examinedObject] = _examinedObject.position;
			_origRotations[_examinedObject] = _examinedObject.rotation;

			_lastMousePosition = Input.mousePosition;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			_playerInput.enabled = false;
			_controller.Disable();

			Rigidbody rb = obj.GetComponent<Rigidbody>();
			if (rb)
			{
				rb.isKinematic = true;
			}
		}

		public void EndExamine()
		{
			if (_type == ExamineType.ComeTo)
			{
				_head.transform.position = _origHeadPosition;
				_head.transform.rotation = _origHeadRotation;
			}

			_isExaming = false;

			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			_playerInput.enabled = true;
			_controller.Enable();

			_movingBack = true;
		}

		void Examine()
		{
			if (_examinedObject != null)
			{

				if (_type == ExamineType.Goto)
				{
					_examinedObject.position = Vector3.Lerp(_examinedObject.position, _offset.transform.position, 0.2f);

					Vector3 deltaMouse = Input.mousePosition - _lastMousePosition;
					float rotationSpeed = 1.0f;
					_examinedObject.Rotate(deltaMouse.x * rotationSpeed * Vector3.up, Space.World);
					_examinedObject.Rotate(deltaMouse.y * rotationSpeed * Vector3.left, Space.World);
					_lastMousePosition = Input.mousePosition;
				}
				else if (_type == ExamineType.ComeTo)
				{
					_head.transform.rotation = Quaternion.LookRotation(-(_examinedObject.transform.position - _objectOffset.transform.position).normalized);
					_head.transform.position = Vector3.Lerp(_head.transform.position, _objectOffset.transform.position, 0.2f);
				}
			}
		}

		private void CancelExamine()
		{
			if (_examinedObject == null) return;

			if (_origPositions.ContainsKey(_examinedObject))
			{
				_examinedObject.position = Vector3.Lerp(_examinedObject.position, _origPositions[_examinedObject], 0.2f);
			}

			if (_origRotations.ContainsKey(_examinedObject))
			{
				_examinedObject.rotation = Quaternion.Slerp(_examinedObject.rotation, _origRotations[_examinedObject], 0.2f);
			}

			if (
				(_examinedObject.position - _origPositions[_examinedObject]).magnitude < 0.01
			)
			{
				Rigidbody rb = _examinedObject.GetComponent<Rigidbody>();
				if (rb)
				{
					rb.isKinematic = false;
				}
				_movingBack = false;
			}
		}

		private void Awake()
		{
			if (_playerInput == null) _playerInput = GetComponent<PlayerInput>();
			if (_controller == null) _controller = GetComponent<PlayerController>();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape)) EndExamine();
			if (_isExaming) Examine();
			else if (_movingBack) CancelExamine();
		}
	}

}
