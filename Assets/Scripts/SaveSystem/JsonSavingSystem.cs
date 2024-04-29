using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SerpentWorks.SavingSystem
{
    public class JsonSavingSystem : MonoBehaviour
    {
        /// <summary>
        /// Save file extension
        /// </summary>
        const string kExtention = ".json";

        const string kLastSceneBuildIndex = "lastSceneBuildIndex";

        #region Public Methods
        /// <summary>
        /// Loads the last scene that was saved and restores the state.
        /// </summary>
        /// <param name="saveFile">The save file to load.</param>>
        public IEnumerator LoadLastScene(string saveFile)
        {
            JObject state = LoadJsonFromFile(saveFile);
            IDictionary<string, JToken> stateDict = state;
            int buildIndex = SceneManager.GetActiveScene().buildIndex;

            if (stateDict.ContainsKey(kLastSceneBuildIndex))
            {
                buildIndex = (int)stateDict[kLastSceneBuildIndex];
            }
            yield return SceneManager.LoadSceneAsync(buildIndex);
            RestoreFromJToken(state);
        }

        /// <summary>
        /// Saves the current scene to the save file
        /// </summary>
        public void Save(string saveFile)
        {
            JObject state = LoadJsonFromFile(saveFile);
            CaptureAsJToken(state);
            SaveFileAsJson(saveFile, state);
        }

        /// <summary>
        /// Loads the current scene from the save file
        /// </summary>
        public void Load(string saveFile)
        {
            RestoreFromJToken(LoadJsonFromFile(saveFile));
        }

        /// <summary>
        /// Deletes the state in the given save file
        /// </summary>
        public void Delete(string saveFile)
        {
            File.Delete(GetPathFromSaveFile(saveFile));
        }

        public IEnumerable<string> ListSaves()
        {
            foreach (string path in Directory.EnumerateFiles(Application.persistentDataPath))
            {
                if (Path.GetExtension(path) == kExtention)
                {
                    yield return Path.GetFileNameWithoutExtension(path);
                }
            }
        }
        #endregion

        #region Private Methods
        private void SaveFileAsJson(string saveFile, JObject state)
        {
            string path = GetPathFromSaveFile(saveFile);
            Debug.Log($"Saving game to {path}");

            using var textWriter = File.CreateText(path);
            using var writer = new JsonTextWriter(textWriter);
            writer.Formatting = Formatting.Indented;
            state.WriteTo(writer);
        }

        private JObject LoadJsonFromFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);

            if (!File.Exists(path)) return new JObject();

            using var textReader = File.OpenText(path);
            using var reader = new JsonTextReader(textReader);
            reader.FloatParseHandling = FloatParseHandling.Double;
            return JObject.Load(reader);
        }

        private void CaptureAsJToken(JObject state)
        {
            IDictionary<string, JToken> stateDict = state;
            foreach (JsonSaveableEntity saveable in FindObjectsOfType<JsonSaveableEntity>())
            {
                stateDict[saveable.GetUUID()] = saveable.CaptureAsJToken();
            }

            stateDict[kLastSceneBuildIndex] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreFromJToken(JObject state)
        {
            IDictionary<string, JToken> stateDict = state;
            foreach (JsonSaveableEntity saveable in FindObjectsOfType<JsonSaveableEntity>())
            {
                string id = saveable.GetUUID();

                if(stateDict.ContainsKey(id)) 
                    saveable.RestoreFromJToken(stateDict[id]);
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + kExtention);
        }
        #endregion
    }
}