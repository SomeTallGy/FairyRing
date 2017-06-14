using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Core;

namespace FairyO.World
{
    public interface INodeObject
    {

        string name { get; }
        string asset { get; }
        GSData data { get; }

        void Init(GSData data);
    }
}