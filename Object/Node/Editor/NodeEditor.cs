using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FairyO.Object
{
    abstract public class NodeEditor : Editor
    {

        // ------------ fields ------------
        private string jsonText;

        /*
        This is all just prototype code below -- nothing really neatly firmed up here yet
        There are some obvious bits that can be converted into methods and placed in an abstract class
         */
        public override void OnInspectorGUI()
        {
            Node myTarget = (Node)target; // grab target
            // basics

            myTarget.name = EditorGUILayout.TextField("Name", myTarget.name);
            if (string.IsNullOrEmpty(myTarget.name))
            {
                EditorGUILayout.HelpBox("No Name Entered!", MessageType.Warning);
            }

            myTarget.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", myTarget.prefab, typeof(GameObject), false);
            if (myTarget.prefab == null)
            {
                EditorGUILayout.HelpBox("No prefab Referenced!", MessageType.Warning);
            }

            EditorGUILayout.LabelField("Node Type", myTarget.Type.ToString());
            myTarget.tier = EditorGUILayout.IntSlider("Node tier", myTarget.tier, 1, 5);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            bool weightsLocked = WeightsLocked(myTarget.items);

            // list of items
            EditorGUILayout.LabelField("Items contained in this Node");

            for (int i = 0; i < myTarget.items.Count; i++)
            {
                GameItem item = myTarget.items[i];

                EditorGUI.BeginChangeCheck();

                // item row
                EditorGUILayout.BeginHorizontal();

                    item = EditorGUILayout.ObjectField(myTarget.items[i], typeof(GameItem), false) as GameItem;

                    if (!item.isWeightLocked && !weightsLocked)
                        item.weight = Mathf.Round(EditorGUILayout.Slider(item.weight, 0, 1) * 100) / 100;
                    else
                        EditorGUILayout.Slider(item.weight, 0, 1);

                    if (EditorGUI.EndChangeCheck())
                    {
                        myTarget.items[i] = item;
                        myTarget.BalanceWeights(i);
                    }

                    item.isWeightLocked = EditorGUILayout.Toggle(item.isWeightLocked, GUILayout.MaxWidth(15));

                    if (GUILayout.Button("-", GUILayout.MaxWidth(15)))
                    {
                        myTarget.items.RemoveAt(i);
                        myTarget.BalanceWeights(-1); // balance all weights
                    }

                EditorGUILayout.EndHorizontal();

                if (item == null)
                    EditorGUILayout.HelpBox("No thing set", MessageType.Warning);

            }

            if (GUILayout.Button("Add an Item!"))
            {
                myTarget.items.Add(new GameItem());
                myTarget.BalanceWeights(-1); // balance all weights
            }

            EditorGUILayout.Space();

            if (myTarget.items.Count > 0)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Node Yield", GUILayout.MinWidth(100));
                EditorGUILayout.LabelField("min", GUILayout.MaxWidth(30));
                myTarget.yield_min = EditorGUILayout.IntField(myTarget.yield_min, GUILayout.MaxWidth(100));
                EditorGUILayout.LabelField("max", GUILayout.MaxWidth(30));
                myTarget.yield_max = EditorGUILayout.IntField(myTarget.yield_max, GUILayout.MaxWidth(100));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
            }
            else
            {
                EditorGUILayout.HelpBox("Nothing in node - create some items etc.!", MessageType.Warning);
            }

            if (!string.IsNullOrEmpty(myTarget.nodeName))
            {
                if (GUILayout.Button("Generate JSON"))
                {
                    GenerateJSON(myTarget, ref jsonText);

                }
            }

            if (!string.IsNullOrEmpty(jsonText))
            {
                EditorGUILayout.TextArea(jsonText, GUILayout.Height(100));
                EditorGUILayout.HelpBox("Cut and paste this JSON and insert it into the Collectibles metacollection.", MessageType.None);
            }

            EditorUtility.SetDirty(myTarget);
        }

        protected bool WeightsLocked(List<GameItem> gatherables)
        {
            int l = 0;
            foreach (var gatherable in gatherables)
            {
                if (gatherable.isWeightLocked) l++;
            }

            if (gatherables.Count - l <= 1)
                return true;
            else
                return false;

        }

        protected virtual void GenerateJSON(Node target, ref string jsonText)
        {
            NodeJSON json = new NodeJSON();

            string asset = AssetDatabase.GetAssetPath(target.GetInstanceID());
            int index = asset.IndexOf("Resources/") + 10;

            json.name = target.nodeName;
            json.asset = asset.Substring(index).Replace(".asset", "");
            json.type = target.Type.ToString();
            json.tier = target.tier;

            foreach (var item in target.items)
            {
                ItemJSON obj = new ItemJSON();
                obj.name = item.title;
                obj.weight = Mathf.Round(item.weight * 100) / 100;

                json.items.Add(obj);
            }

            json.yield_min = target.yield_min;
            json.yield_max = target.yield_max;

            jsonText = json.ToJson();
        }

        private class NodeJSON
        {
            public string name;
            public string asset;
            public string type;
            public int tier;
            public List<ItemJSON> items = new List<ItemJSON>();
            public int yield_min;
            public int yield_max;

            public string ToJson()
            {
                return JsonUtility.ToJson(this, true);
            }
        }

        [System.Serializable]
        private class ItemJSON
        {
            public string name;
            public float weight;
        }
    }


}