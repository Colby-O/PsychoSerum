using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum.MonoSystem
{
    internal sealed class GameView : View
    {
        public override void Show()
        {
            base.Show();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public override void Init()
        {

        }
    }
}
