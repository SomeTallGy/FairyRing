using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

namespace FairyO.Object
{
    [CreateAssetMenu(menuName = "FairyO_Node")]
    public class Node : ScriptableObject, IEditableThing
    {
        // ------ properties -----------------
        
        public Identity Identity {                                  // identity Property
            get {return this.identity;} 
            set {this.identity = value;} 
        } 

        public GameObject Prefab {                                  // prefab Property
            get {return this.prefab;} 
            set {this.prefab = value;}
        }      

        public string AssetName {get{                                   // asset name of node
            return Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(this.GetInstanceID()));
        }}
        
        // ------ public fields --------------
        public GameObject prefab;                                       // item prefab
        public Identity identity;                                       // item identity
        public List<ItemInNode> itemsYielded = new List<ItemInNode>();  // list of items yielded by node
        public Yield itemYield;                                         // item yield of node


        public string ToJSON()
        {
            return JsonUtility.ToJson(this); // temporary
        }
    }

    [System.Serializable]
    public struct Yield
    {
        public int min;
        public int max;
    }

    [System.Serializable]
    public class ItemInNode
    {
        public ItemInNode(Item item)
        {
            this.item = item;
            this.weight = 0;
            this.isWeightLocked = false;
        }
        public Item item;
        public float weight;
        public bool isWeightLocked;
    }
}