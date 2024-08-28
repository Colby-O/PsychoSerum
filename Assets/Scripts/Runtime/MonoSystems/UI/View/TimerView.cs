using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PsychoSerum.MonoSystem
{
    internal sealed class TimerView : View
    {
        [SerializeField] private GameObject _view;
        [SerializeField] private TMP_Text _timeText;

        private bool _isTimerOn = false;
        private bool _isStopWatchOn = false;
        private float _time;

        public float GetTime()
        {
            return _time;
        }

        public void StartTimer()
        {
            StopStopwatch();
            _isTimerOn = true;
            _time = 0f;
        }

        public void StopTimer()
        {
            _isTimerOn = false;
        }

        public void StartStopwatch(float timeLimit)
        {
            StopTimer();
            _isStopWatchOn = true;
            _time = timeLimit;
        }

        public void StopStopwatch()
        {
            _isStopWatchOn = true;
        }

        public override void Init()
        {
            _isTimerOn = false;
            _isStopWatchOn = false;
            _time = 0f;
        }

        public override void Show()
        {
            base.Show();
            _view.SetActive(true);
        }

        public override void Hide()
        {
            base.Hide();
            _view.SetActive(false);
        }

        private void Update()
        {
            if (_isTimerOn)
            {
                _time += Time.deltaTime;
            }
            else if (_isStopWatchOn)
            {
                _time -= Time.deltaTime;
                if (_time >= 0f) StopStopwatch();
            }

            float minutes = Mathf.FloorToInt(_time / 60);
            float seconds = Mathf.FloorToInt(_time % 60);
            _timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
