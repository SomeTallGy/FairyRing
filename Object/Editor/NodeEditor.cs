using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FairyO.Object
{
    [CustomEditor(typeof(Node))]
    public class NodeEditor : ThingEditor
    {
        public override void OnInspectorGUI()
        {
            // 1. retrieve target as Node
            Node node = (Node)target;

            // 2. update identity
            this.UpdateIdentity(node);
            EditorGUILayout.Space();

            // 3. update prefabs
            this.UpdatePrefab(node);

            // 4. update list of items
            this.UpdateListOfItems(node);

            // 5. update yield
            this.UpdateYield(node);

            // 6. update JSON
            this.UpdateJSON(node);

            // 7. save and finish!
            EditorUtility.SetDirty(node);
        }

        private void UpdateListOfItems(Node node)
        {
            bool weightsLocked = WeightsLocked(node.itemsYielded);
            EditorGUILayout.LabelField("Items contained in this Node");

            for (int i = 0; i < node.itemsYielded.Count; i++)
            {
                EditorGUI.BeginChangeCheck();

                // item row
                EditorGUILayout.BeginHorizontal();
                Item item = EditorGUILayout.ObjectField(node.itemsYielded[i].item, typeof(Item), false) as Item;

                if (!node.itemsYielded[i].isWeightLocked && !weightsLocked)
                    node.itemsYielded[i].weight = Mathf.Round(EditorGUILayout.Slider(node.itemsYielded[i].weight, 0, 1) * 100) / 100;
                else
                    EditorGUILayout.Slider(node.itemsYielded[i].weight, 0, 1);

                if (EditorGUI.EndChangeCheck())
                {
                    node.itemsYielded[i].item = item;
                    BalanceWeights(i);
                }

                node.itemsYielded[i].isWeightLocked = EditorGUILayout.Toggle(node.itemsYielded[i].isWeightLocked, GUILayout.MaxWidth(15));

                if (GUILayout.Button("-", GUILayout.MaxWidth(15)))
                {
                    node.itemsYielded.RemoveAt(i);
                    BalanceWeights(-1); // balance all weights
                }
                EditorGUILayout.EndHorizontal();

                if (item == null)
                    EditorGUILayout.HelpBox("No thing set", MessageType.Warning);

            }

            if (GUILayout.Button("Add an Item!"))
            {
                node.itemsYielded.Add(new ItemInNode(new Item()));
                BalanceWeights(-1); // balance all weights
            }

            EditorGUILayout.Space();
        }

        protected void UpdateYield(Node node)
        {
            if (node.itemsYielded.Count > 0)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Node Yield", GUILayout.MinWidth(100));
                EditorGUILayout.LabelField("min", GUILayout.MaxWidth(30));
                node.itemYield.min = EditorGUILayout.IntField(node.itemYield.min, GUILayout.MaxWidth(100));
                EditorGUILayout.LabelField("max", GUILayout.MaxWidth(30));
                node.itemYield.max = EditorGUILayout.IntField(node.itemYield.max, GUILayout.MaxWidth(100));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
            }
            else
            {
                EditorGUILayout.HelpBox("Nothing in node - create some items etc.!", MessageType.Warning);
            }
        }

        #region BalanceWeights
        private bool BalanceWeights(int indexChanged)
        {
            Node node = (Node)target; // grab target

            if (node.itemsYielded.Count <= 1 && node.itemsYielded.Count >= 0)
            {
                if (node.itemsYielded.Count == 1)
                    node.itemsYielded[0].weight = 1;

                return ValidWeights();
            }

            float t = 0; // total sum of weights
            int l = (indexChanged >= 0) ? 1 : 0; // number of locked weights
            float max = 1; // maximum Weight a Weight can go (adjusted by locked weights)

            foreach (var item in node.itemsYielded)
            {
                t += item.weight;
                if (item.isWeightLocked)
                {
                    max -= item.weight;
                    l++; // add locked Weight
                }
            }

            // check if sum totals to 1 
            if (t == 1)
                return ValidWeights();

            else if (indexChanged >= 0) // adjust other weights other than indexChanged
            {
                // check to see we're not changing a locked Weight
                if (node.itemsYielded[indexChanged].isWeightLocked)
                    return ValidWeights();

                int c = node.itemsYielded.Count - l;   // number of other elements
                float d = (1 - t) / c;                      // difference to adjust per element
                foreach (var item in node.itemsYielded)
                {
                    if (node.itemsYielded.IndexOf(item) != indexChanged && !item.isWeightLocked)
                    {
                        item.weight += d;
                        if (item.weight < 0.001f) item.weight = 0; // normalize
                    }
                    else if (node.itemsYielded.IndexOf(item) == indexChanged)
                    {
                        // check max
                        if (item.weight > max)
                            item.weight = max;
                    }
                }
            }
            else   // adjust all weights
            {
                float p = 1 / t;
                foreach (var itemYielded in node.itemsYielded)
                {
                    itemYielded.isWeightLocked = false; // remove locks
                    itemYielded.weight *= p;
                    if (itemYielded.weight < 0.001f) itemYielded.weight = 0; // normalize
                }
            }

            return ValidWeights();
        }

        private bool ValidWeights()
        {
            Node node = (Node)target; // grab target

            foreach (var g in node.itemsYielded)
            {
                if (g.weight <= 0 || g.weight > 1)
                {
                    return false;   // invalid Weight
                }

            }
            return true; // valid weights
        }

        private bool WeightsLocked(List<ItemInNode> itemsYielded)
        {
            int l = 0;
            foreach (var itemYielded in itemsYielded)
            {
                if (itemYielded.isWeightLocked) l++;
            }

            if (itemsYielded.Count - l <= 1)
                return true;
            else
                return false;

        }
        #endregion

    }
}