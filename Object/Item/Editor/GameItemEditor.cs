using UnityEngine;
using UnityEditor;

namespace FairyO.Object
{
    abstract public class GameItemEditor : Editor
    {

        // ------------ fields ------------
        private string jsonText;

        /*
        This is all just prototype code below -- nothing really neatly firmed up here yet
        There are some obvious bits that can be converted into methods and placed in an abstract class
         */
        public override void OnInspectorGUI()
        {
            GameItem myTarget = (GameItem)target; // grab target

            // basics
            myTarget.title = EditorGUILayout.TextField("Name", myTarget.title);
            if (string.IsNullOrEmpty(myTarget.title))
            {
                EditorGUILayout.HelpBox("No Name Entered!", MessageType.Warning);
            }

            EditorGUILayout.LabelField("Item Description");
            myTarget.description = EditorGUILayout.TextArea(myTarget.description, GUILayout.Height(50));

            EditorGUILayout.Space();

            myTarget.iconGraphic = (Sprite)EditorGUILayout.ObjectField("Icon Graphic", myTarget.iconGraphic, typeof(Sprite), false);

            myTarget.prefab= (GameObject)EditorGUILayout.ObjectField("Prefab", myTarget.prefab, typeof(GameObject), false);
            if (myTarget.prefab== null)
            {
                EditorGUILayout.HelpBox("No prefabReferenced!", MessageType.Warning);
            }

            EditorGUILayout.LabelField("Item Type", myTarget.Type.ToString());
            myTarget.tier = EditorGUILayout.IntSlider("Item tier", myTarget.tier, 1, 5);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Gatherable Properties");

            myTarget.gatherTime = EditorGUILayout.FloatField("Gather Time", myTarget.gatherTime);

            EditorGUILayout.Space();

            myTarget.slug = EditorGUILayout.TextField("Slug", myTarget.slug);

            if (!string.IsNullOrEmpty(myTarget.title))
            {
                if (GUILayout.Button("Gamesparks JSON"))
                {
                    jsonText = myTarget.ToJSON();
                }
            }

            if (!string.IsNullOrEmpty(jsonText))
            {
                EditorGUILayout.TextArea(jsonText, GUILayout.Height(200));
                EditorGUILayout.HelpBox("Cut and paste this JSON and insert it into the Collectibles metacollection.", MessageType.None);
            }

            EditorUtility.SetDirty(myTarget);
        }

        protected virtual void GenerateJSON(GameItem target, ref string jsonText)
        {
            jsonText = target.ToJSON();

            /* 
            CollectibleJSON json = new CollectibleJSON();

            string asset = AssetDatabase.GetAssetPath(target.GetInstanceID());
            int index = asset.IndexOf("Resources/") + 10;

            json.name = target.title;
            json.asset = asset.Substring(index).Replace(".asset", "");
            json.type = target.Type.ToString();
            json.tier = target.tier;

            jsonText = json.ToJson();
            */
        }
    }


}