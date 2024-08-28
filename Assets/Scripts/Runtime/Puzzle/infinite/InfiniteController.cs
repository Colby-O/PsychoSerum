using PsychoSerum.Puzzle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PsychoSerum.Helpers;
using System.Linq;
using TMPro;
using Unity.Burst.CompilerServices;

namespace PsychoSerum.Puzzle
{
    internal class InfiniteController : PuzzleController
    {
        [SerializeField] private List<TMP_Text> _wrongHints;
        [SerializeField] private List<TMP_Text> _correctHints;

        [SerializeField] private List<Portal> _portals;
        [SerializeField] private int _maxPuzzleLength = 4;

        [SerializeField] private List<int> _correctPath;

        [SerializeField] private List<int> _progress;

        private void AssignPortals()
        {
            _portals.Shuffle();

            int idCount = 0;

            for (int i = 0; i < _portals.Count / 2; i++)
            {
                Portal portal1 = _portals[i];
                Portal portal2 = _portals[i + _portals.Count / 2];

                portal1.id = i;
                portal2.id = i + _portals.Count / 2;

                portal1.label.text = i.ToString();
                portal2.label.text = (i + _portals.Count / 2).ToString();

                if (Random.value > 0.5f && _maxPuzzleLength > _correctPath.Count) _correctPath.Add((Random.value > 0.5f) ? i : i + _portals.Count / 2);

                portal1.other = portal2;
                portal1.display.GetComponent<Renderer>().material.mainTexture = portal2.renderTexture;
                portal2.other = portal1;
                portal2.display.GetComponent<Renderer>().material.mainTexture = portal1.renderTexture;
            }
        }

        private void FinsihPuzzle(bool sucessful)
        {
            Debug.Log("Door Room Solved!");
        }

        private void SetActiveHint(int id)
        {
            for(int i = 0; i< _correctHints.Count; i++)
            {
                if (int.Parse(_correctHints[i].text) == _correctPath[id])
                {
                    _correctHints[i].gameObject.SetActive(true);
                    _wrongHints[i].gameObject.SetActive(false);
                }
                else
                {
                    _correctHints[i].gameObject.SetActive(false);
                    _wrongHints[i].gameObject.SetActive(true);
                }
            }
        }

        public void OnEnterPortal(int id)
        {
            _progress.Add(id);

            for (int i = 0; i < _progress.Count; i++)
            {
                if (_progress[i] != _correctPath[i])
                {
                    _progress = new List<int>();
                    SetActiveHint(0);
                    return;
                }
            }

            if (_progress.Count == _correctPath.Count) FinsihPuzzle(true);
            else SetActiveHint(_progress.Count);
        }

        public override void StartPuzzle()
        {
            _correctPath = new List<int>();
            _progress = new List<int>();

            AssignPortals();

            for (int i = 0; i < _correctHints.Count; i++)
            {
                if (Random.value > 2.0f * _correctPath.Count / _portals.Count)
                {
                    _correctHints[i].gameObject.SetActive(true);
                    _wrongHints[i].gameObject.SetActive(false);
                    string val = _correctPath[Random.Range(0, _correctPath.Count)].ToString();
                    _wrongHints[i].text = val;
                    _correctHints[i].text = val;
                } 
                else
                {
                    _correctHints[i].gameObject.SetActive(false);
                    _wrongHints[i].gameObject.SetActive(true);
                    string val = Random.Range(0, _portals.Count).ToString();
                    _wrongHints[i].text = val;
                    _correctHints[i].text = val;
                }
            }

            SetActiveHint(0);
        }

        private void Start()
        {
            _portals = FindObjectsOfType<Portal>().ToList();
            StartPuzzle();
        }
    }
}
