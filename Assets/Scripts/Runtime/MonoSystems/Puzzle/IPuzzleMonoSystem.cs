using PlazmaGames.Core.MonoSystem;
using PsychoSerum.Puzzle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum.MonoSystem
{
    internal interface IPuzzleMonoSystem : IMonoSystem
    {
        public void ChangePuzzle(PuzzleController controller, PuzzleType type);
        public void FinishPuzzle(PuzzleController controller, PuzzleType type);
    }
}
