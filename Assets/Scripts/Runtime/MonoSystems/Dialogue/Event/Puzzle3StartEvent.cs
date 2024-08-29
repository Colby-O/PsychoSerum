using PsychoSerum.Puzzle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum.MonoSystem
{
    [CreateAssetMenu(fileName = "Puzzle3Start", menuName = "Dialogue/Events/New Puzzle 3 Start Event")]
    internal class Puzzle3StartEvent : DialogueEvent
    {
        public override void OnEnter()
        {

        }

        public override void OnExit()
        {
            FindObjectOfType<MazeController>().StartPuzzle();
        }

        public override void OnUpdate()
        {

        }
    }
}
