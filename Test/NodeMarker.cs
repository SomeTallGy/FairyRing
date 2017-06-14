using System.Collections.Generic;
using UnityEngine;

namespace FairyO.World
{
    public class NodeMarker : MonoBehaviour
    {

        public List<string> intersectsWith = new List<string>();
		public List<string> pointsWith = new List<string>();
		public List<string> launchingLineFrom = new List<string>();

		public List<LineVerts> lineTestedFrom = new List<LineVerts>();

        public void Init(GOTileCell cell)
        {
			transform.position = new Vector3(cell.center.x, 1.5f, cell.center.z);
			
			for(int i = 0; i < cell.edges.Count; i++)
			{
				cell.edges[i].Draw(Color.black);
			}

			this.gameObject.name = "node_marker_"+cell.ToString();
			 
        }

        public void IntersectsWithLine(string streetName)
        {
            intersectsWith.Add(streetName);
            GetComponent<MeshRenderer>().material.color = Color.green;
        }

        public void ContainsPoint(string streetName)
        {
            pointsWith.Add(streetName);
            GetComponent<MeshRenderer>().material.color = Color.green;
        }

		public void LaunchingLineFrom(GameObject vert)
		{
			launchingLineFrom.Add(vert.name);
			vert.GetComponent<MeshRenderer>().material.color = Color.blue;
		}

		public void LineTestedFrom(NodeVert a, NodeVert b, List<LineSeg> edges)
		{
			lineTestedFrom.Add(new LineVerts(a,b, edges));
		}
    }

	[System.Serializable]
	public struct LineVerts
	{
		public NodeVert a;
		public NodeVert b;

		public LineSeg lineSeg;
		public List<LineSeg> edgesTested;

		public LineVerts(NodeVert a, NodeVert b, List<LineSeg> edges)
		{
			this.a = a;
			this.b = b;

			lineSeg = new LineSeg(a.transform.position, b.transform.position);

			this.edgesTested = edges;
		}
	}
}