using PsychoSerum.Puzzle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum.MonoSystem
{
    [CreateAssetMenu(fileName = "Puzzle1Start", menuName = "Dialogue/Events/New Puzzle 1 Start Event")]
    internal class Puzzle1StartEvent : DialogueEvent
    {
        public override void OnEnter()
        {

        }

        public override void OnExit()
        {
            FindAnyObjectByType<BlockPuzzleController>().StartPuzzle();
        }

        public override void OnUpdate()
        {

        }
    }
}
