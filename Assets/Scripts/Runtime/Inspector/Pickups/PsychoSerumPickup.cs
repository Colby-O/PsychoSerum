using PsychoSerum.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum.Inspectables
{
    internal class PsychoSerumPickup : InspectableObject
    {
        public override bool IsPickupable()
        {
            return true;
        }

        public override void OnPickup(Interactor interactor)
        {
            interactor.pickupManager.PickupPsychoSerum();
            Destroy(gameObject);
        }
    }
}
