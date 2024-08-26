using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum.Puzzle
{
    internal class MazeKey : MonoBehaviour
    {
        [SerializeField] private MazeController _controller;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                _controller.End();
            }
        }

        private void Awake()
        {
            if (_controller == null) _controller = FindAnyObjectByType<MazeController>();
        }
    }
}
