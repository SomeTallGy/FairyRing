using UnityEngine;
using System.IO;
using UnityEditor;

namespace FairyO.Object
{
    public enum ItemTier { Common, Uncommon, Rare, Legendary, Epic }
    public enum InventoryType { Standard, Hidden, Locked }

    [CreateAssetMenu (menuName = "FairyO_Item") ]
    public class Item : ScriptableObject, IEditableThing
    {
        // ---------- constants -------------
        public const string RESOURCES_PATH = "/Items";

        // ---------- properties ------------
        public Identity Identity {                                  // identity Property
            get {return this.identity;} 
            set {this.identity = value;} 
        } 

        public GameObject Prefab {                                  // prefab Property
            get {return this.prefab;} 
            set {this.prefab = value;}
        }      

        public string AssetName {                                   // asset name of item
            get{return Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(this.GetInstanceID()));}
        }

        // ----- public fields -------------
        public GameObject prefab;                                   // item prefab
        public Identity identity;                                   // item identity
        public Sprite iconGraphic;                                  // icon graphic of item
        public InventoryType inventoryType;                         // inventory type

        // ------ gatherable properties -----
        public float gatherTime = 0.5f;                             // how long it has to be in your basket before it's collected
        
        public static string GetInventoryJSON(Item item, int quantity)
        {
            return JsonUtility.ToJson(new InventoryItem(item, quantity), true);
        }

        public string ToJSON()
        {
            return GetInventoryJSON(this, 1);
        }
    }

    [System.Serializable]
    public struct Identity
	{
		public string title;
		public string slug;
		public int tier;
		public string description;
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