using Newtonsoft.Json.Linq;

namespace SerpentWorks.SavingSystem
{
    public interface IJsonSaveable
    {
        /// <summary>
        /// Override to return a JToken representing the state
        /// of the IJsonSaveble entity
        /// </summary>
        /// <returns>A JToken</returns>
        JToken CaptureAsJToken();

        /// <summary>
        /// Restores the state of a component using the data
        /// in a JToken
        /// </summary>
        /// <param name="state">A JToken object representing the state of the component</param>
        void RestoreFromJToken(JToken state);
    }
}