using PlazmaGames.Core;
using PsychoSerum.Player;
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
        [SerializeField] private Scrollbar _sensitivity;
        [SerializeField] private Toggle _invertX;
        [SerializeField] private Toggle _invertY;
        [SerializeField] private Button _backButton;

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _clickSound;

        private void UpdateMusicVolume(float val)
        {
            GameManager.GetMonoSystem<IAudioMonoSystem>().SetMusicVolume(val);
        }

        private void UpdateSfXVolume(float val)
        {
            GameManager.GetMonoSystem<IAudioMonoSystem>().SetSfXVolume(val);
        }

        private void UpdateOverallVolume(float val)
        {
            GameManager.GetMonoSystem<IAudioMonoSystem>().SetOverallVolume(val);
        }

        private void UpdateSensitivity(float val)
        {
            PlayerSettings settings= PsychoSerumGameManager.player.GetPlayerSetings();
            settings.sensitivityX = Mathf.Lerp(settings.sensitivityMin, settings.sensitivityMax, val); 
        }

        private void UpdateInvertX(bool toggle)
        {
            PlayerSettings settings = PsychoSerumGameManager.player.GetPlayerSetings();
            settings.invertedViewX = toggle;
        }

        private void UpdateInvertY(bool toggle)
        {
            PlayerSettings settings = PsychoSerumGameManager.player.GetPlayerSetings();
            settings.invertedViewY = !toggle;
        }

        public override void Show()
        {
            base.Show();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            _settingstext.SetActive(true);
            _sfxSlider.gameObject.SetActive(true);
            _musicSlider.gameObject.SetActive(true);
            _overallSlider.gameObject.SetActive(true);
            _backButton.gameObject.SetActive(true);
            _sensitivity.gameObject.SetActive(true);
            _invertX.gameObject.SetActive(true);
            _invertY.gameObject.SetActive(true);
        }

        public override void Hide()
        {
            base.Hide();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _settingstext.SetActive(false);
            _sfxSlider.gameObject.SetActive(false);
            _musicSlider.gameObject.SetActive(false);
            _overallSlider.gameObject.SetActive(false);
            _backButton.gameObject.SetActive(false);
            _sensitivity.gameObject.SetActive(false);
            _invertX.gameObject.SetActive(false);
            _invertY.gameObject.SetActive(false);
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
            _sensitivity.onValueChanged.AddListener(UpdateSensitivity);
            _invertX.onValueChanged.AddListener(UpdateInvertX);
            _invertY.onValueChanged.AddListener(UpdateInvertY);
            

            _overallSlider.value = GameManager.GetMonoSystem<IAudioMonoSystem>().GetOverallVolume();
            _musicSlider.value = GameManager.GetMonoSystem<IAudioMonoSystem>().GetMusicVolume();
            _sfxSlider.value= GameManager.GetMonoSystem<IAudioMonoSystem>().GetSfXVolume();
            _sensitivity.value = 0.5f;
            _invertX.isOn = false;
            _invertY.isOn = false;
        }
    }
}
