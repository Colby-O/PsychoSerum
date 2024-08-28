using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum.Puzzle
{
    internal class Block : MonoBehaviour
    {
        public int id;
        public Color color;

        private void Awake()
        {
            GetComponent<MeshRenderer>().material.color = color;
        }
    }
}
