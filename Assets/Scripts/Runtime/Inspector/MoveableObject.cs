using PsychoSerum.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum.Inspectables
{ 
    internal class MoveableObject : InspectableObject
    {
        public bool allowInteract = false;

        public override bool Interact(Interactor interactor)
        {
            if (!allowInteract) return false;

            _auidoSource.PlayOneShot(_auidoclip);
            Inspector inspector = interactor.GetComponent<Inspector>();
            inspector.StartExamine(transform, _type, offsetPoint, string.Empty, true);
            return true;
        }
    }
}
