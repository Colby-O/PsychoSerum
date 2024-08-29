using PsychoSerum.Puzzle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum.MonoSystem
{
    [CreateAssetMenu(fileName = "Puzzle2Start", menuName = "Dialogue/Events/New Puzzle 2 Start Event")]
    internal class Puzzle2StartEvent : DialogueEvent
    {
        public override void OnEnter()
        {

        }

        public override void OnExit()
        {
            FindObjectOfType<InfiniteController>().StartPuzzle();
        }

        public override void OnUpdate()
        {

        }
    }
}
