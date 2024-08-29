using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum
{
	public class FacePlayer : MonoBehaviour
	{
		[SerializeField] private Transform _eyes;
		[SerializeField] private AudioSource _audioSource;
		[SerializeField] private AudioClip _spookSound;

		IEnumerator removeAfter(float secs)
		{
			yield return new WaitForSeconds(secs);
			Destroy(gameObject);
		}

		// Update is called once per frame
		void FixedUpdate()
		{
			Vector3 ppos = PsychoSerumGameManager.player.transform.position;
			transform.LookAt(ppos);
			RaycastHit hit;
			if (Physics.Raycast(
				_eyes.position,
				(ppos - _eyes.position).normalized, 
				out hit,
				20
			))
			{
				if (hit.transform.tag == "Player")
				{
					_audioSource.PlayOneShot(_spookSound);
					StartCoroutine(removeAfter(2.5f));
				}
			}
		}
	}
}
