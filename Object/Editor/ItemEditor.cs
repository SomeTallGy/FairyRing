using UnityEngine;
using UnityEditor;

namespace FairyO.Object
{
    [CustomEditor(typeof(Item))]
    public class ItemEditor : ThingEditor
    {
        public override void OnInspectorGUI()
        {
            // 1. retrieve target as Item
            Item item = (Item)target; 

            // 2. update identity
            this.UpdateIdentity(item);
            EditorGUILayout.Space();

            // 3. update prefab
            this.UpdatePrefab(item);

            // 4. update icon graphic
            this.UpdateIconGraphic(item);
            
            // 5. update gather time
            this.UpdateGatherTime(item);

            // 6. update JSON
            this.UpdateJSON(item);
            
            // 7. save and finish!
            EditorUtility.SetDirty(item);
        }

        private void UpdateIconGraphic(Item item)
        {
            item.iconGraphic = (Sprite)EditorGUILayout.ObjectField("Icon Graphic", item.iconGraphic, typeof(Sprite), false);
        }

        private void UpdateGatherTime(Item item)
        {
            EditorGUILayout.LabelField("Gatherable Properties");
            item.gatherTime = EditorGUILayout.FloatField("Gather Time", item.gatherTime);
        }
    }
}