using System.Collections.Generic;
using UnityEngine;

namespace FairyO.World
{
    public class NodeObjectBuilder : MonoBehaviour
    {
        // 0000000 Singleton 00000000
        private static NodeObjectBuilder singleton;
        void Awake()
        {
            // setup singleton
            if (singleton == null)
                singleton = this;
            else if (singleton != this)
                Destroy(gameObject);
        }

        // -------- constants --------
        public const string PREF_FOLDER_GATHERABLES = "Prefabs/Gatherables";
        public const string PREF_FOLDER_CREATURES = "Prefabs/Creatures";

       
        // -------- properties --------
        internal static Dictionary<string, GameObject> gatherablePrefabs = new Dictionary<string, GameObject>();         // object pool
        internal static Dictionary<string, GameObject> creaturePrefabs = new Dictionary<string, GameObject>();           // object pool

        public static GameObject NewObject(string assetName, NodeObject.Type type, Vector3 position, Transform childOf = null)
        {
            Dictionary<string, GameObject> dict = TypeOfDictionary(type);
            GameObject prefab;

            // attempt to get one from the dictionary
            if (dict.ContainsKey(assetName))
            {
                prefab = dict[assetName];
            }
            else
            {
                // get it from resources and add it to the dictionary
                prefab = PrefabFromResources(assetName, type);
                dict.Add(assetName, prefab);
            }

            return Instantiate(prefab, position, new Quaternion());
        }

        public static void Preload(string assetName, NodeObject.Type type)
        {
            Dictionary<string, GameObject> dict = TypeOfDictionary(type);
            if (!dict.ContainsKey(assetName))
            {
                dict.Add(assetName, PrefabFromResources(assetName, type));
            }
                
        }

        public static void ClearTypeOf(NodeObject.Type type)
        {

        }

        public static void ClearAll()
        {

        }

        private static GameObject PrefabFromResources(string assetName, NodeObject.Type type)
        {
            return (GameObject) Resources.Load(TypeOfResourceFolder(type) + "/pref_" + assetName, typeof(GameObject));
        }

        private static Dictionary<string, GameObject> TypeOfDictionary(NodeObject.Type type)
        {
            switch (type)
            {
                case NodeObject.Type.Gatherable:
                    return gatherablePrefabs;
                case NodeObject.Type.Creature:
                    return creaturePrefabs;
                default:
                    return null;
            }
        }

        private static string TypeOfResourceFolder(NodeObject.Type type)
        {
            switch (type)
            {
                case NodeObject.Type.Gatherable:
                    return PREF_FOLDER_GATHERABLES;
                case NodeObject.Type.Creature:
                    return PREF_FOLDER_CREATURES;
                default:
                    return null;
            }
        }
    }
}