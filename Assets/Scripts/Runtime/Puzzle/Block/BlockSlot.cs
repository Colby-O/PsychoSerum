using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum.Puzzle
{
    internal class BlockSlot : MonoBehaviour
    {
        public GameObject uiView;
        public Block correctBlock;

        public bool isCorrect = false;

        public void OnTriggerEnter(Collider other)
        {
            Block otherBlock = other.GetComponent<Block>();
            if (otherBlock == null) return;
            if (otherBlock.id == correctBlock.id) isCorrect = true;
        }

        public void OnTriggerExit(Collider other)
        {
            Block otherBlock = other.GetComponent<Block>();
            if (otherBlock == null) return;
            if (otherBlock.id == correctBlock.id)  isCorrect = false;
        }
    }
}
