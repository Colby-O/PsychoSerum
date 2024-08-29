using PlazmaGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum.MonoSystem
{
    [CreateAssetMenu(fileName = "FinalEvent", menuName = "Dialogue/Events/New Final Event")]
    internal class FinalEvent : DialogueEvent
    {
        private IEnumerator End()
        {
            PsychoSerumGameManager.player.ToggleCam(true);
            GameManager.GetMonoSystem<IEventMonoSystem>().RunEvent(100);
            yield return new WaitForSeconds(2);
            PsychoSerumGameManager.player.ToggleCam(false);
        }

        public override void OnEnter()
        {

        }

        public override void OnExit()
        {
            PsychoSerumGameManager.player.StartCoroutine(End());
        }

        public override void OnUpdate()
        {

        }
    }
}
