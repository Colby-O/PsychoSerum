using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlazmaGames.Core;
using PlazmaGames.MonoSystems.Animation;
using PsychoSerum.MonoSystem;

namespace PsychoSerum
{
	internal sealed class PsychoSerumGameManager : GameManager
	{
		[Header("Holders")]
		[SerializeField] private GameObject _monoSystemParnet;

		[Header("MonoSystems")]
		[SerializeField] private AnimationMonoSystem _animationMonoSystem;
        [SerializeField] private AudioMonoSystem _audioMonoSystem;
        [SerializeField] private PuzzleMonoSystem _puzzleMonoSystem;

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
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

        private void Start()
        {
            _audioMonoSystem.PlayAudio("ambient_1", MonoSystem.AudioType.Ambient, true);
        }
    }
}
