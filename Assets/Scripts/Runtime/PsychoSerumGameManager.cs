using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlazmaGames.Core;
using PlazmaGames.MonoSystems.Animation;
using PsychoSerum.MonoSystem;
using PsychoSerum.Player;

namespace PsychoSerum
{
	internal sealed class PsychoSerumGameManager : GameManager
	{
		[Header("Holders")]
		[SerializeField] private GameObject _monoSystemParnet;

		[Header("MonoSystems")]
        [SerializeField] private UIMonoSystem _uiMonoSystem;
        [SerializeField] private AnimationMonoSystem _animationMonoSystem;
        [SerializeField] private AudioMonoSystem _audioMonoSystem;
        [SerializeField] private PuzzleMonoSystem _puzzleMonoSystem;

		public static bool allowInput = false;
        public static bool hasStarted = false;

		public static PlayerController player;

		public static void StartGame()
		{
			hasStarted = true;
			allowInput = true;
			Camera.main.GetComponent<AudioListener>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            player.ToggleView(true);
        }

        /// <summary>
        /// Adds all events listeners
        /// </summary>
        private void AddListeners()
		{

		}

		/// <summary>
		/// Removes all events listeners
		/// </summary>
		private void RemoveListeners()
		{

		}

		/// <summary>
		/// Attaches all MonoSystems to the GameManager
		/// </summary>
		private void AttachMonoSystems()
		{
            AddMonoSystem<UIMonoSystem, IUIMonoSystem>(_uiMonoSystem);
            AddMonoSystem<AnimationMonoSystem, IAnimationMonoSystem>(_animationMonoSystem);
            AddMonoSystem<AudioMonoSystem, IAudioMonoSystem>(_audioMonoSystem);
            AddMonoSystem<PuzzleMonoSystem, IPuzzleMonoSystem>(_puzzleMonoSystem);
        }

		protected override string GetApplicationName()
		{
			return nameof(PsychoSerumGameManager);
		}

		protected override void OnInitalized()
		{
			// Ataches all MonoSystems to the GameManager
			AttachMonoSystems();

			// Adds Event Listeners
			AddListeners();

			// Ensures all MonoSystems call Awake at the same time.
			_monoSystemParnet.SetActive(true);
		}

		private void Awake()
		{
			//Cursor.lockState = CursorLockMode.Locked;
			//Cursor.visible = false;
		}

        private void Start()
        {
            player = FindAnyObjectByType<PlayerController>();
            _audioMonoSystem.PlayAudio("ambient_1", MonoSystem.AudioType.Ambient, true);
        }
    }
}
