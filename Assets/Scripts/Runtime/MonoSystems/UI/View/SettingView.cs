using PlazmaGames.Core;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace PsychoSerum.MonoSystem
{
    internal sealed class SettingView : View
    {
        [Header("References")]
        [SerializeField] private GameObject _settingstext;
        [SerializeField] private Scrollbar _sfxSlider;
        [SerializeField] private Scrollbar _musicSlider;
        [SerializeField] private Scrollbar _overallSlider;
        [SerializeField] private Scrollbar _sfxSliderUI;
        [SerializeField] private Scrollbar _musicSliderUI;
        [SerializeField] private Scrollbar _overallSliderUI;
        [SerializeField] private Button _backButton;

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _clickSound;

        private void UpdateMusicVolume(float val)
        {
            _musicSliderUI.value = val;
            GameManager.GetMonoSystem<IAudioMonoSystem>().SetMusicVolume(val);
        }

        private void UpdateSfXVolume(float val)
        {
            _sfxSliderUI.value = val;
            GameManager.GetMonoSystem<IAudioMonoSystem>().SetSfXVolume(val);
        }

        private void UpdateOverallVolume(float val)
        {
            _overallSliderUI.value = val;
            GameManager.GetMonoSystem<IAudioMonoSystem>().SetOverallVolume(val);
        }

        public override void Show()
        {
            base.Show();

            _settingstext.SetActive(true);
            _sfxSlider.gameObject.SetActive(true);
            _musicSlider.gameObject.SetActive(true);
            _overallSlider.gameObject.SetActive(true);
            _sfxSliderUI.gameObject.SetActive(true);
            _musicSliderUI.gameObject.SetActive(true);
            _overallSliderUI.gameObject.SetActive(true);
            _backButton.gameObject.SetActive(true);
        }

        public override void Hide()
        {
            base.Hide();

            _settingstext.SetActive(false);
            _sfxSlider.gameObject.SetActive(false);
            _musicSlider.gameObject.SetActive(false);
            _overallSlider.gameObject.SetActive(false);
            _sfxSliderUI.gameObject.SetActive(false);
            _musicSliderUI.gameObject.SetActive(false);
            _overallSliderUI.gameObject.SetActive(false);
            _backButton.gameObject.SetActive(false);
        }

        private void Back()
        {
            _audioSource.PlayOneShot(_clickSound);
            GameManager.GetMonoSystem<IUIMonoSystem>().ShowLast();
        }

        public override void Init()
        {
            _backButton.onClick.AddListener(Back);

            _musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
            _sfxSlider.onValueChanged.AddListener(UpdateSfXVolume);
            _overallSlider.onValueChanged.AddListener(UpdateOverallVolume);

            _overallSliderUI.value = _overallSlider.value = GameManager.GetMonoSystem<IAudioMonoSystem>().GetOverallVolume();
            _musicSliderUI.value = _musicSlider.value = GameManager.GetMonoSystem<IAudioMonoSystem>().GetMusicVolume();
            _sfxSliderUI.value = _sfxSlider.value = GameManager.GetMonoSystem<IAudioMonoSystem>().GetSfXVolume();
        }
    }
}
