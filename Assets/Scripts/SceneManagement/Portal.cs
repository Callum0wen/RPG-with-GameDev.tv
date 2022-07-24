using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
	public class Portal : MonoBehaviour
	{
		enum DestinationIdentifier
		{
			A, B, C, D, E
		}

		[SerializeField] int sceneToLoad = -1;
		[SerializeField] Transform spawnPoint;
		[SerializeField] DestinationIdentifier destination;

		[SerializeField] float fadeInTime = 1f;
		[SerializeField] float fadeOutTime = 1f;
		[SerializeField] float fadeWaitTime = 0.5f;


		private void OnTriggerEnter(Collider other)
		{
			if(other.gameObject.tag == "Player")
			{
				StartCoroutine(Transition());
			}
		}

		private IEnumerator Transition()
		{
			if (sceneToLoad < 0)
			{
				Debug.LogError("Scene to load not set.");
				yield break;
			}

			Fader fader = FindObjectOfType<Fader>();
			yield return fader.FadeOut(fadeOutTime);

			DontDestroyOnLoad(gameObject);
			yield return SceneManager.LoadSceneAsync(sceneToLoad);

			Portal otherPortal = GetOtherPortal();
			UpdatePlayer(otherPortal);

			yield return new WaitForSeconds(fadeWaitTime);
			yield return fader.FadeIn(fadeInTime);

			Destroy(gameObject);
		}

		private void UpdatePlayer(Portal otherPortal)
		{
			var player = GameObject.FindWithTag("Player");
			player.transform.position = otherPortal.spawnPoint.position;
			player.transform.rotation = otherPortal.spawnPoint.rotation;
		}

		private Portal GetOtherPortal()
		{
			foreach(Portal portal in FindObjectsOfType<Portal>())
			{
				if (portal == this) continue;
				if (portal.destination != destination) continue;
				return portal;
			}

			return null;
		}
	}
}