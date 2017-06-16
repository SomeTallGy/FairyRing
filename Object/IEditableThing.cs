using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FairyO.Object
{
    public interface IEditableThing
    {
		Identity Identity { get; set; }
        GameObject Prefab { get; set; }
        string ToJSON();
    }
}