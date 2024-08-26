using PsychoSerum.Puzzle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum.MonoSystem
{
    internal sealed class PuzzleMonoSystem : MonoBehaviour, IPuzzleMonoSystem
    {
        [SerializeField] private PuzzleType _currentPuzzleType = PuzzleType.None;
        private PuzzleController _currentPuzzleController;

        public void ChangePuzzle(PuzzleController controller, PuzzleType type)
        {
            _currentPuzzleType = type;
            _currentPuzzleController = controller;
            controller.StartPuzzle();
        }

        public void FinishPuzzle(PuzzleController controller, PuzzleType type)
        {
            
        }
    }
}
