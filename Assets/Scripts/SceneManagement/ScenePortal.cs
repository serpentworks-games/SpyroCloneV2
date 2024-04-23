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
        [SceneName][SerializeField] string sceneName;
        
        private void OnTriggerEnter(Collider other) {

            if(other.CompareTag("Player"))
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}