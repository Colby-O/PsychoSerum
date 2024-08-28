using PlazmaGames.Core;
using PsychoSerum.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PsychoSerum.Inspectables
{
    internal class NoteObject : InspectableObject
    {
        [SerializeField] public TMP_Text text;

        public override bool Interact(Interactor interactor)
        {
            _auidoSource.PlayOneShot(_auidoclip);
            Inspector inspector = interactor.GetComponent<Inspector>();
            PlayerController pc = interactor.GetComponent<PlayerController>();
            pc.ZeroInput();
            inspector.StartExamine(transform, _type, offsetPoint, text.text);
            return true;
        }
    }
}
