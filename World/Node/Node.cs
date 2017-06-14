using GameSparks.Core;
using System.Collections.Generic;
using UnityEngine;

namespace FairyO.World
{
    public class Node
    {
        // -------- properties ---------
        public int id;
        public List<INodeObject> objects;

        // ---------- fields -----------
        internal GSData data {get; private set;}
        
        public Node(GSData nodeData)
        {
            this.data = nodeData;
            this.id = (int) nodeData.GetInt("id");
            objects = GetNodeObjects<NodeGatherable>(nodeData);   // get gatherables
        }

        public static List<INodeObject> GetNodeObjects<T>(GSData nodeData) where T : INodeObject, new()
        {
            // parse type class name
            string[] a = typeof(T).ToString().ToLower().Split('.'); 

            // define the GSDataList's name by class name
            string gs_list_name = a[a.Length - 1] + "s";
            gs_list_name = gs_list_name.Substring(4);    // trim off "node" e.g. NodeGatherable(s)
            List<GSData> data = nodeData.GetGSDataList(gs_list_name);

            List<INodeObject> objects = new List<INodeObject>();  // make list of type T
            foreach(var d in data)
            {
                T obj = new T();    // create T:NodeObject
                obj.Init(d);        // Init T:NodeObject with Data
                objects.Add(obj);   // Add T:NodeObject to list
            }

            return objects;         // return list
        }

        public override string ToString()
        {
            return data.JSON;
        }
    }
}