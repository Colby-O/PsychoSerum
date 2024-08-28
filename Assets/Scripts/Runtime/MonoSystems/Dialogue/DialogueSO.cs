using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PsychoSerum.MonoSystem
{
    [System.Serializable]
    public class Dialogue
    {
        public string msg;
        public DialogueEvent dialogueEvent;
    }

    [CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/New Dialogue")]
    public class DialogueSO : ScriptableObject
    {

        [SerializeField] private List<Dialogue> _dialogues = new List<Dialogue>();
        public Queue<Dialogue> dialogues;

        // To be called before a dialogue event is started
        public void StartDialogueEvent() => dialogues = new Queue<Dialogue>(_dialogues);
    }
}
