using System;
using UnityEngine;

namespace ScalePact.Core
{
    public class PersistantObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistantObjects = null;

        static bool hasBeenSpawned = false;

        private void Awake() {
            if(hasBeenSpawned) return;

            SpawnPersistantObject();

            hasBeenSpawned = true;
        }

        private void SpawnPersistantObject()
        {
            GameObject persistObj = Instantiate(persistantObjects);
            DontDestroyOnLoad(persistObj);
        }
    }
}