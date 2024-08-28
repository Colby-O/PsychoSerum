using PlazmaGames.Core;
using PsychoSerum.Inspectables;
using PsychoSerum.MonoSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Device;

namespace PsychoSerum.Puzzle
{
    internal static class EnumerableExtensions
    {

        public static IList<T> Shuffle<T>(this IEnumerable<T> sequence)
        {
            return sequence.Shuffle(new System.Random());
        }

        public static IList<T> Shuffle<T>(this IEnumerable<T> sequence, System.Random randomNumberGenerator)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            if (randomNumberGenerator == null)
            {
                throw new ArgumentNullException("randomNumberGenerator");
            }

            T swapTemp;
            List<T> values = sequence.ToList();
            int currentlySelecting = values.Count;
            while (currentlySelecting > 1)
            {
                int selectedElement = randomNumberGenerator.Next(currentlySelecting);
                --currentlySelecting;
                if (currentlySelecting != selectedElement)
                {
                    swapTemp = values[currentlySelecting];
                    values[currentlySelecting] = values[selectedElement];
                    values[selectedElement] = swapTemp;
                }
            }

            return values;
        }
    }

    internal sealed class BlockPuzzleController : PuzzleController
    {
        [SerializeField] private List<BlockSlot> _slots;
        [SerializeField] private List<Block> _blocks;
        [SerializeField] private Camera _screen;
        [SerializeField] private GameObject _screenCover;

        [SerializeField] private float _studyTime = 60f;
        [SerializeField] private float _timeLimit = 300f;

        private bool _isStarted = false;

        private bool _isSolving = false;

        private void GeneratePuzzle()
        {
            List<BlockSlot> rand = _slots.Shuffle().ToList();

            for (int i = 0; i < _blocks.Count; i++)
            {
                rand[i].correctBlock = _blocks[i];
                rand[i].uiView.GetComponent<Renderer>().material.color = _blocks[i].color;
            }
        }

        public override void StartPuzzle()
        {
            Debug.Log("Puzzle 1 Starting!");
            _isStarted = true;
            GameManager.GetMonoSystem<IUIMonoSystem>().Show<TimerView>();
            GameManager.GetMonoSystem<IUIMonoSystem>().GetView<TimerView>().StartStopwatch(_studyTime);
        }

        private void FinsihPuzzle(bool sucessful)
        {
            Debug.Log("Puzzle 1 Finished!");
            _isStarted = false;
            _isSolving = false;
            GameManager.GetMonoSystem<IUIMonoSystem>().ShowLast();
            GameManager.GetMonoSystem<IEventMonoSystem>().RunEvent((sucessful) ? 5 : 6);
        }

        private bool IsSolved()
        {
            bool isSolved = true;

            foreach (BlockSlot slot in _slots) isSolved &= slot.isCorrect;

            return isSolved;
        }

        private void Awake()
        {
            _screenCover.SetActive(false);
            GeneratePuzzle();
        }

        private void Update()
        {
            if (!_isStarted) return;
            if (IsSolved()) FinsihPuzzle(true);

            if (!_isSolving && GameManager.GetMonoSystem<IUIMonoSystem>().GetView<TimerView>().GetTime() <= 0)
            {
                _isSolving = true;
                _screen.gameObject.SetActive(false);
                _screenCover.SetActive(true);
                GameManager.GetMonoSystem<IUIMonoSystem>().GetView<TimerView>().StartTimer();
                foreach (Block b in _blocks) b.GetComponent<MoveableObject>().allowInteract = true;
            }

            if (_isSolving && GameManager.GetMonoSystem<IUIMonoSystem>().GetView<TimerView>().GetTime() >= _timeLimit)
            {
                FinsihPuzzle(false);
            }
        }
    }
}
