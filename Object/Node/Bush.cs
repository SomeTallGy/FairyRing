using UnityEngine;

namespace FairyO.Object
{
    [CreateAssetMenu(menuName = "FairyO_Node/Bush")]
    public class Bush : Node
    {
        // ------ properties  --------------
        public override NodeType Type { get { return NodeType.Bush; } }

    }
}