using PsychoSerum.Interfaces;
using PsychoSerum.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum.Inspectables
{
    internal enum ExamineType
    {
        Goto,
        ComeTo
    }

    internal sealed class InspectableObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private ExamineType _type = ExamineType.Goto;
        [SerializeField] private GameObject offsetPoint = null;

        public bool Interact(Interactor interactor)
        {
            Inspector inspector = interactor.GetComponent<Inspector>();
            PlayerController pc = interactor.GetComponent<PlayerController>();
            pc.ZeroInput();
            inspector.StartExamine(transform, _type, offsetPoint);
            return true;
        }

        public void EndInteraction()
        {

        }

    }
}
