using PsychoSerum.Puzzle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PsychoSerum.Helpers;
using System.Linq;

namespace PsychoSerum.Puzzle
{
    internal class InfiniteController : PuzzleController
    {
        [SerializeField] private List<Portal> _portals;

        private void AssignPortals()
        {
            _portals.Shuffle();

            for (int i = 0; i < _portals.Count / 2; i++)
            {
                Portal portal1 = _portals[i];
                Portal portal2 = _portals[i + _portals.Count / 2];

                portal1.other = portal2;
                portal1.display.GetComponent<Renderer>().material.mainTexture = portal2.renderTexture;
                portal2.other = portal1;
                portal2.display.GetComponent<Renderer>().material.mainTexture = portal1.renderTexture;
            }
        }

        public override void StartPuzzle()
        {
            AssignPortals();
        }

        private void Start()
        {
            _portals = FindObjectsOfType<Portal>().ToList();
            StartPuzzle();
        }
    }
}
