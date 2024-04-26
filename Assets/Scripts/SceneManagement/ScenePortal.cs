using System;
using System.Collections;
using System.Collections.Generic;
using ScalePact.SceneManagement.Editors;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScalePact.SceneManagement
{
    public class ScenePortal : MonoBehaviour
    {
        [Header("Connecting Portal")]
        [SceneName][SerializeField] string sceneName;
        [SerializeField] DestinationID destinationID;

        [Header("This Portal Refs")]
        [SerializeField] Transform spawnPoint;

        [Serializable]
        public enum DestinationID
        {
            A, B, C, D, E, F, G
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("Player"))
            {
                StartCoroutine(SceneTransition());
            }
        }

        private IEnumerator SceneTransition()
        {
            if(sceneName.)
            {

            }
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(sceneName);

            ScenePortal destinationPortal = GetOtherScenePortal();
            UpdatePlayerLocation(destinationPortal);

            Destroy(gameObject);
        }

        private ScenePortal GetOtherScenePortal()
        {
            foreach (ScenePortal portal in FindObjectsOfType<ScenePortal>())
            {
                if(portal == this) continue;
                if(portal.destinationID != destinationID) continue;

                return portal;
            }

            return null;
        }

        private void UpdatePlayerLocation(ScenePortal destinationPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");

            player.transform.SetPositionAndRotation(
                destinationPortal.spawnPoint.position, 
                destinationPortal.spawnPoint.rotation
                );
        }
    }
}