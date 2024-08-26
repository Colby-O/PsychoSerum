using PsychoSerum.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum.Puzzle
{
    internal class PortalTeleporter : MonoBehaviour
    {
        [SerializeField] private Portal _portal;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                PlayerController pc = other.GetComponent<PlayerController>();
                pc.Disable();
                other.transform.position = new Vector3(_portal.other.target.position.x, other.transform.position.y, _portal.other.target.position.z);
                if (_portal.other.transform.forward != _portal.transform.forward) pc.Rotate(180f);
                pc.Enable();
            }
        }
    }
}
