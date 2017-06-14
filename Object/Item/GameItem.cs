using UnityEngine;

namespace FairyO.Object
{
    public enum GatherableType{ None, Normal, Bomb, Bonus }

    public class GameItem : Item
    {
        public virtual GatherableType GatherableType { get; }
        public GameObject prefab;
        
        // ------ gatherable properties -----
        public float gatherTime = 0.5f;        // how long it has to be in your basket before it's collected

        // ------ node properties -----------
        public float weight;                            
        public bool isWeightLocked;
    }
}
