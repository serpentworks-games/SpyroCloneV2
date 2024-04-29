using UnityEngine;

namespace SerpentWorks.SavingSystem
{
    /// <summary>
    /// A serializable wrapper for Vector3, allowing them to be saved
    /// </summary>
    [System.Serializable]
    public class SerializableVector3
    {
        float x, y, z;

        /// <summary>
        /// Copy the state from an existing vector3
        /// </summary>
        public SerializableVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        /// <summary>
        /// Creates a Vector3 from the cached state
        /// </summary>
        /// <returns></returns>
        public Vector3 ToVector()
        {
            return new Vector3(x, y, z);
        }
    }
}