using UnityEngine;

namespace FairyO.Object
{
    [CreateAssetMenu (menuName = "FairyO_Item/Berry") ]
    public class Berry : GameItem
    {
         public override ItemType Type {get{ return ItemType.Berry; } }
         public override GatherableType GatherableType {get{ return GatherableType.Normal; } }
    }
}