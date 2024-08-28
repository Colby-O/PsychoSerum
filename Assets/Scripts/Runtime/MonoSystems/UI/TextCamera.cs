using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PsychoSerum.MonoSystem
{
    internal class TextCamera : MonoBehaviour
    {
        [SerializeField] private Camera _cam;
        [SerializeField] private RawImage _view;
        private float _previousWidth;
        private float _previousHeight;

        private RenderTexture CreateRenderTexture()
        {
            RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
            rt.Create();
            return rt;
        }

        private void Awake()
        {
            _previousWidth = Screen.width;
            _previousHeight = Screen.height;
            _cam.targetTexture = CreateRenderTexture();
            _view.texture = _cam.targetTexture;
        }

        private void Update()
        {
            if (_previousWidth != Screen.width || _previousHeight != Screen.height)
            {
                _previousWidth = Screen.width;
                _previousHeight = Screen.height;
                _cam.targetTexture = CreateRenderTexture();
                _view.texture = _cam.targetTexture;
            }
        }
    }
}
