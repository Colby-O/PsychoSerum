using PsychoSerum.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PsychoSerum.Task
{
    [System.Serializable]
    internal struct Task
    {
        public int id;
        public string task;
        public bool isCompleted;
        public TaskElement element;

        public Task(int id, string task, bool isCompleted)
        {
            this.id = id;
            this.task = task;
            this.isCompleted = isCompleted;
            element = null;
        }
    }

    [RequireComponent(typeof(PlayerInput))]
    internal class Taskboard : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private TaskElement _taskPrefab;
        [SerializeField] private GameObject _taskContainer;
        [SerializeField] private GameObject _holder;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _takeOutSound;

        public bool inTaskMenu = false;

        private List<Task> _tasks;

        private InputAction _openTaskAction;

        public void AddTask(Task task)
        {
            TaskElement ele = Instantiate(_taskPrefab, _taskContainer.transform);
            ele.gameObject.SetActive(true);
            ele.task.text = task.task;
            ele.completed.gameObject.SetActive(false);
            task.element = ele;
            _tasks.Add(task);
        }

        public void MarkTaskComplete(int id)
        {
            Task task = _tasks.First((Task other) => other.id == id);
            task.isCompleted = true;
            task.element.completed.gameObject.SetActive(true);
            task.element.completed.text = String.Concat(Enumerable.Repeat("_", task.element.task.text.Length).ToArray());
        }

        private void OpenTask(InputAction.CallbackContext e)
        {
            if (PsychoSerumGameManager.player.GetComponent<Inspector>().IsExaming || !PsychoSerumGameManager.allowInput || !PsychoSerumGameManager.player.GetPickupManager().hasPickupTaskList) return;
            _audioSource.PlayOneShot(_takeOutSound);
            inTaskMenu = !inTaskMenu;
            _holder.SetActive(inTaskMenu);
        }

        private void Awake()
        {
            _tasks = new List<Task>();

            if (_playerInput == null) _playerInput = GetComponent<PlayerInput>();
            _openTaskAction = _playerInput.actions["Task"];
            _openTaskAction.performed += OpenTask;
            _holder.SetActive(false);
        }
    }
}
