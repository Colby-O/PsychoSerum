using PlazmaGames.Core;
using PlazmaGames.MonoSystems.Animation;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace PsychoSerum.MonoSystem
{
    internal sealed class MainMenuView : View
    {
        [Header("References")]
        [SerializeField] private GameObject  _mainMenutext;

        [SerializeField] private GameObject _gate;
        [SerializeField] private Vector2 _gateRange;
        [SerializeField] private float _moveDuration = 2;
        [SerializeField] private Camera _backgroundCamera;
        [SerializeField] private Camera _textCamera;
        [SerializeField] private RawImage _background;
        [SerializeField] private RawImage _backgroundUI;
        [SerializeField] private Button _start;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _quit;

        [SerializeField] private Transform _mainTransform;
        [SerializeField] private Transform _startTransform;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _gateSound;
        [SerializeField] private AudioClip _clickSound;

        private RenderTexture _renderTexture;

        private bool _firstCall = true;

        private RenderTexture CreateRenderTexture()
        {
            RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
            rt.Create();
            return rt;
        }

        private void HideMenu()
        {
            _mainMenutext.gameObject.SetActive(false);

            _start.gameObject.SetActive(false);
            _settings.gameObject.SetActive(false);
            _quit.gameObject.SetActive(false);
        }

        private void ShowMenu()
        {
            _mainMenutext.gameObject.SetActive(true);

            _start.gameObject.SetActive(true);
            _settings.gameObject.SetActive(true);
            _quit.gameObject.SetActive(true);
        }

        private void Exit()
        {
            HideMenu();
            _audioSource.PlayOneShot(_clickSound);
            GameManager.GetMonoSystem<IAnimationMonoSystem>().RequestAnimation(
                this,
                    _moveDuration,
                    (float t) =>
                    {
                        _backgroundCamera.transform.position = new Vector3(
                            _backgroundCamera.transform.position.x,
                            _backgroundCamera.transform.position.y,
                            Mathf.Lerp(_mainTransform.localPosition.z, _startTransform.localPosition.z, t)
                        );
                    },
                () => {
                    _audioSource.PlayOneShot(_gateSound);
                    GameManager.GetMonoSystem<IAnimationMonoSystem>().RequestAnimation(
                        this,
                        _gateSound.length,
                        (float t) =>
                        {
                            _gate.transform.localPosition = new Vector3(
                                Mathf.Lerp(_gateRange.y, _gateRange.x, t),
                                _gate.transform.localPosition.y,
                                _gate.transform.localPosition.z
                            );
                        },
                        () => Application.Quit()
                    );
                }
            );
        }

        private void Settings()
        {
            _audioSource.PlayOneShot(_clickSound);
            GameManager.GetMonoSystem<IUIMonoSystem>().Show<SettingView>();
        }

        private void StartGame()
        {
            GameManager.GetMonoSystem<IAudioMonoSystem>().StopAudio(AudioType.Music);
            _background.gameObject.SetActive(false);
            _backgroundCamera.gameObject.SetActive(false);
            _audioSource.PlayOneShot(_clickSound);
            PsychoSerumGameManager.StartGame();
            GameManager.GetMonoSystem<IUIMonoSystem>().Show<GameView>();
        }

        public override void Show()
        {
            base.Show();

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            if (!_firstCall)
            {
                ShowMenu();
                return;
            }

            _firstCall = false;

            _audioSource.PlayOneShot(_gateSound);

            _backgroundCamera.transform.position = new Vector3(
                            _backgroundCamera.transform.position.x,
                            _backgroundCamera.transform.position.y,
                            _startTransform.localPosition.z
            );

            GameManager.GetMonoSystem<IAnimationMonoSystem>().RequestAnimation(
                this,
                _gateSound.length,
                (float t) =>
                {
                    _gate.transform.localPosition = new Vector3(
                        Mathf.Lerp(_gateRange.x, _gateRange.y, t),
                        _gate.transform.localPosition.y,
                        _gate.transform.localPosition.z
                    );
                },
                () => GameManager.GetMonoSystem<IAnimationMonoSystem>().RequestAnimation(
                    this,
                    _moveDuration,
                    (float t) =>
                    {
                        _backgroundCamera.transform.position = new Vector3(
                            _backgroundCamera.transform.position.x,
                            _backgroundCamera.transform.position.y,
                            Mathf.Lerp(_startTransform.localPosition.z, _mainTransform.localPosition.z, t)
                        );
                    },
                    () => ShowMenu()
                )
            );
        }

        public override void Hide()
        {
            base.Hide();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            HideMenu();
        }


        public override void Init()
        {
            GameManager.GetMonoSystem<IAudioMonoSystem>().PlayAudio("MainMenu", AudioType.Music, true);

            Camera.main.GetComponent<AudioListener>().enabled = false;
            _renderTexture = CreateRenderTexture();
            _backgroundCamera.targetTexture = _renderTexture;
            _background.texture = _renderTexture;
            _backgroundUI.texture = _textCamera.targetTexture;

            HideMenu();

            _start.onClick.AddListener(StartGame);
            _settings.onClick.AddListener(Settings);
            _quit.onClick.AddListener(Exit);
        }
    }
}
