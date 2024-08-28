using PsychoSerum.Inspectables;
using PsychoSerum.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum
{
    internal class TaskPickup : InspectableObject
    {
        public override bool IsPickupable()
        {
            return true;
        }

        public override void OnPickup(Interactor interactor)
        {
            interactor.pickupManager.PickupTaskList();
            Destroy(gameObject);
        }
    }
}
