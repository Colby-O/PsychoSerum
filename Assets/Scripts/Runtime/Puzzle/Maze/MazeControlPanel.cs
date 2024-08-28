using PlazmaGames.Core;
using PlazmaGames.MonoSystems.Animation;
using PsychoSerum.Interfaces;
using PsychoSerum.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace PsychoSerum.Puzzle
{
    internal class MazeControlPanel : MonoBehaviour, IInteractable
    {
        [SerializeField] private MazeController _mazeController;

        public void OpenGate(float t, Transform gate, float start, float end)
        {
            gate.localPosition = new Vector3(Mathf.Lerp(start, end, t), gate.localPosition.y, gate.localPosition.z);
        }

        public bool Interact(Interactor interactor)
        {
            _mazeController.Begin();
            return true;
        }

        public void EndInteraction()
        {

        }

        private void Awake()
        {
            if (_mazeController == null) _mazeController = FindAnyObjectByType<MazeController>();
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
