using PlazmaGames.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PsychoSerum.MonoSystem
{
    internal class TextView : View
    {
        [SerializeField] private TMP_Text _textArea;
        [SerializeField] private Scrollbar _scroll;

        public void SetText(string text)
        {
            _textArea.text = text;
        }

        public override void Show()
        {
            base.Show();
            _scroll.value = 1;
        }

        public override void Init()
        {

        }
    }
}
