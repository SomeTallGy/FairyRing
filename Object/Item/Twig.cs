using UnityEngine;

namespace FairyO.Object
{
    [CreateAssetMenu (menuName = "FairyO_Item/Twig") ]
    public class Twig : GameItem
    {
        override public ItemType Type {get{ return ItemType.Twig; } }
        public override GatherableType GatherableType {get{ return GatherableType.Normal; } }
    }
}