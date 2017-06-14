using UnityEngine;
using LitJson;
using System.Text;
using System.IO;
using UnityEditor;

namespace FairyO.Object
{
    public enum ItemType { Berry, Twig }
    public enum ItemTier { Common, Uncommon, Rare, Legendary, Epic }

    [System.Serializable]
    public class Item : ScriptableObject
    {
        // ---------- constants -------------
        public const string RESOURCES_PATH = "/Items";
        // ---------- properties ------------
        public virtual ItemType Type { get; }           // type of item
        public string AssetName {get{                   // asset name of item
            return Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(this.GetInstanceID()));
        }}

        // ----- public fields -------------
        public Sprite iconGraphic;                      // icon graphic of item
        public string title;                            // name of item
        public string slug;                             // slug identifier
        public int tier;                                // tier of item 1-5

        // ------ other properties ---------
        public string description;                      // item description

        
        
        public static string GetInventoryJSON(Item item, int quantity)
        {
            return JsonUtility.ToJson(new InventoryItem(item, quantity), true);
        }

        public string ToJSON()
        {
            return GetInventoryJSON(this, 1);

            /* 
            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            writer.PrettyPrint = true;

            // write type
            writer.WriteObjectStart();
            writer.WritePropertyName("type");
            writer.Write(this.Type.ToString());
            writer.WriteObjectEnd();

            
            // write asset
            string asset = AssetDatabase.GetAssetPath(this.GetInstanceID());
            int index = asset.IndexOf("Resources/") + 10;
            writer.WriteObjectStart();
            writer.WritePropertyName("asset");
            writer.Write(asset.Substring(index).Replace(".asset", ""));
            writer.WriteObjectEnd();
            

            return sb.ToString();
            */
        }
    }

    internal class InventoryItem
    {
        public string scriptableObject;
        public int quantity;

        public InventoryItem(Item item, int quantity)
        {
            this.scriptableObject = item.AssetName;
            this.quantity = quantity;

        }
    }
}