using PlazmaGames.Core.MonoSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum.MonoSystem
{
    internal interface IDialogueMonoSystem : IMonoSystem
    {
        public void CloseDialogue();
        public void Load(DialogueSO dialogueEvent);
    }
}
