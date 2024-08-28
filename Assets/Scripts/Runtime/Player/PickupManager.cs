using PlazmaGames.Core;
using PsychoSerum.MonoSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum.Player
{
    internal class PickupManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _pickupSound;

        [Header("Flags")]
        public bool hasPickupPsychoSerum = false;
        public bool hasPickupTaskList = false;

        public void PickupPsychoSerum()
        {
            _audioSource.PlayOneShot(_pickupSound);
            hasPickupPsychoSerum = true;

            if (hasPickupTaskList) GameManager.GetMonoSystem<IEventMonoSystem>().RunEvent(1);
        }

        public void PickupTaskList()
        {
            _audioSource.PlayOneShot(_pickupSound);
            hasPickupTaskList = true;
            if (hasPickupPsychoSerum) GameManager.GetMonoSystem<IEventMonoSystem>().RunEvent(1);
        }
    }
}
