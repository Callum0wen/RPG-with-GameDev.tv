using System;
using UnityEngine;

namespace RPG.Core
{
	public class PersitantObjectSpawner : MonoBehaviour
	{
		[SerializeField] GameObject persistantObjectPrefab;

		static bool hasSpawned = false;

		private void Awake()
		{
			if (hasSpawned) return;

			SpawnPersistentObjects();

			hasSpawned = true;
		}

		private void SpawnPersistentObjects()
		{
			GameObject persistentObject = Instantiate(persistantObjectPrefab);
			DontDestroyOnLoad(persistentObject);
		}
	}
}