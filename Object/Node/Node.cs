using System.Collections.Generic;
using UnityEngine;

namespace FairyO.Object
{
    abstract public class Node : ScriptableObject
    {
        // ------ enums and constants --------
        public enum NodeType { Bush, Geode }

        // ------ properties -----------------
        public string nodeName;
        public GameObject prefab;
        public int tier;
        abstract public NodeType Type {get;}

        // ------ public fields --------------
        public List<GameItem> items = new List<GameItem>();
        public int yield_min = 20;
        public int yield_max = 30;
        
        public bool BalanceWeights(int indexChanged)
        {
            if (items.Count <= 1 && items.Count >= 0)
            {
                if (items.Count == 1)
                    items[0].weight = 1;

                return ValidWeights();
            }

            float t = 0; // total sum of weights
            int l = (indexChanged >= 0) ? 1 : 0; // number of locked weights
            float max = 1; // maximum Weight a Weight can go (adjusted by locked weights)

            foreach (var g in items)
            {
                t += g.weight;
                if (g.isWeightLocked)
                {
                    max -= g.weight;
                    l++; // add locked Weight
                }
            }

            // check if sum totals to 1 
            if (t == 1)
                return ValidWeights();

            else if (indexChanged >= 0) // adjust other weights other than indexChanged
            {
                // check to see we're not changing a locked Weight
                if (items[indexChanged].isWeightLocked)
                    return ValidWeights();

                int c = items.Count - l;   // number of other elements
                float d = (1 - t) / c;                      // difference to adjust per element
                foreach (var g in items)
                {
                    if (items.IndexOf(g) != indexChanged && !g.isWeightLocked)
                    {
                        g.weight += d;
                        if (g.weight < 0.001f) g.weight = 0; // normalize
                    }
                    else if (items.IndexOf(g) == indexChanged)
                    {
                        // check max
                        if (g.weight > max)
                            g.weight = max;
                    }
                }
            }
            else   // adjust all weights
            {
                float p = 1 / t;
                foreach (var g in items)
                {
                    g.isWeightLocked = false; // remove locks
                    g.weight *= p;
                    if (g.weight < 0.001f) g.weight = 0; // normalize
                }
            }

            return ValidWeights();
        }

        private bool ValidWeights()
        {
            foreach (var g in items)
            {
                if (g.weight <= 0 || g.weight > 1)
                {
                    return false;   // invalid Weight
                }

            }
            return true; // valid weights
        }

    }
}