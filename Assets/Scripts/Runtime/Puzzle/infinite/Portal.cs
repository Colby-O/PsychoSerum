using PsychoSerum.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum.Puzzle
{
    internal class Portal : MonoBehaviour
    {
        public Portal other;

        public GameObject display;
        public Camera view;
        public Transform target;

        public RenderTexture renderTexture;

        public void OpenPortal()
        {
            display.SetActive(true);
            view.gameObject.SetActive(true);
        }

        public void ClosePortal()
        {
            display.SetActive(false);
            view.gameObject.SetActive(false);
        }

        private RenderTexture CreateRenderTexture()
        {
            RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
            rt.Create();
            return rt;
        }

        private void Awake()
        {
            renderTexture = CreateRenderTexture();
            view.targetTexture = renderTexture;
            ClosePortal();
        }

        private void Start()
        {
            if (other != null)  display.GetComponent<Renderer>().material.mainTexture = other.renderTexture;
        }
    }
}
