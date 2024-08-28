using PlazmaGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PsychoSerum.MonoSystem
{
    internal sealed class PausedView : View
    {
        [SerializeField] private GameObject _pausetext;

        [SerializeField] private Button _resume;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _quit;

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _clickSound;

        private void Resume()
        {
            _audioSource.PlayOneShot(_clickSound);
            PsychoSerumGameManager.allowInput = true;
            GameManager.GetMonoSystem<IUIMonoSystem>().ShowLast();
        }

        private void Settings()
        {
            _audioSource.PlayOneShot(_clickSound);
            GameManager.GetMonoSystem<IUIMonoSystem>().Show<SettingView>();
        }

        private void Quit()
        {
            _audioSource.PlayOneShot(_clickSound);
            Application.Quit();
        }

        public override void Hide()
        {
            base.Hide();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _pausetext.SetActive(false);
        }

        public override void Show()
        {
            base.Show();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            _pausetext.SetActive(true);
        }

        public override void Init()
        {
            _resume.onClick.AddListener(Resume);
            _settings.onClick.AddListener(Settings);
            _quit.onClick.AddListener(Quit);
        }
    }
}
