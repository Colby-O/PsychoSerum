using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PsychoSerum.Player;

namespace PsychoSerum.Interfaces
{
    internal interface IInteractable 
    {
        public bool IsPickupable();

        public void OnPickup(Interactor interactor);

        /// <summary>
        /// Interaction with a Interactor
        /// </summary>
        public bool Interact(Interactor interactor);

        /// <summary>
        /// Method to be called once Interaction is complete
        /// </summary>
        public void EndInteraction();
    }
}
