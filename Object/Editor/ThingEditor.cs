using UnityEngine;
using UnityEditor;

namespace FairyO.Object
{
    public abstract class ThingEditor : Editor
    {
        protected void UpdateIdentity(IEditableThing thing)
        {
            // 1. create a new identity to modify
            Identity identity = new Identity();

            // 2. update title
            identity.title = EditorGUILayout.TextField("Name", thing.Identity.title);
            if (string.IsNullOrEmpty(thing.Identity.title))
            {
                EditorGUILayout.HelpBox("No Name Entered!", MessageType.Warning);
            }

            // 3. update slug
            identity.slug = EditorGUILayout.TextField("Slug", thing.Identity.slug);

            // 4. update tier
            identity.tier = EditorGUILayout.IntSlider("Tier", thing.Identity.tier, 1, 5);

            // 5. update description
            EditorGUILayout.LabelField("Description");
            identity.description = EditorGUILayout.TextArea(thing.Identity.description, GUILayout.Height(50));

            // 6. update identity and finish!
            thing.Identity = identity;
        }

        protected void UpdatePrefab(IEditableThing thing)
        {
            // 1. create a new identity to modify
            GameObject prefab = null;

            // 2. update prefab
            prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", thing.Prefab, typeof(GameObject), false);
            if (prefab== null)
            {
                EditorGUILayout.HelpBox("No prefabReferenced!", MessageType.Warning);
            }

            // 3. update prefab and finish!
            thing.Prefab = prefab;
        }

        protected void UpdateJSON(IEditableThing thing)
        {
            string jsonText = null;

            if (!string.IsNullOrEmpty(thing.Identity.title))
            {
                if (GUILayout.Button("Gamesparks JSON"))
                {
                    jsonText = thing.ToJSON();
                }
            }

            if (!string.IsNullOrEmpty(jsonText))
            {
                EditorGUILayout.TextArea(jsonText, GUILayout.Height(200));
                EditorGUILayout.HelpBox("Cut and paste this JSON and insert it into the Collectibles metacollection.", MessageType.None);
            }
        }

    }
}