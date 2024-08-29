using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum.Enemy
{
    internal class EnemyMove : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;
        [SerializeField] private AudioSource _audioSource;

        private bool _started = false;

        public void StartMovement()
        {
            _started = true;
            _audioSource.Play();
        }

        private void FixedUpdate()
        {
            if (_started)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - _speed);
            }
        }
    }
}
