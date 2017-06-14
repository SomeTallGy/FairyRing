using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Core;

namespace FairyO.World
{
    public class NodeGatherable : NodeObject, INodeObject
    {
        public override void Init(GSData data)
        {
            base.Init(data);

            NodeObjectBuilder.Preload(this.asset, Type.Gatherable); // preload asset
        }
    }
}