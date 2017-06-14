using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoMap;
using GoShared;

namespace FairyO.World
{

    public class GOTileNodes : MonoBehaviour
    {
        // ---------- constants ----------
        public const float CELLS_PER_TILE = 64; // powers of 2

        // ---------- properties ----------

        // ---------- fields -------------
        private GOTileCell[,] cells = new GOTileCell[(int)Mathf.Sqrt(CELLS_PER_TILE), (int)Mathf.Sqrt(CELLS_PER_TILE)];
        private List<GOTileCell> taggedCells = new List<GOTileCell>();
        private float tile_edge_x;
        private float tile_edge_z;
        private float cell_edge_x;
        private float cell_edge_z;
        private Vector3 vert0;	// lower left most vertex of Tile

        public void CreateCells(GOTile tile)
        {
            vert0 = tile.vertices[0] + tile.tileCenter.convertCoordinateToVector(0); // verts[0] relative to world coords

            tile_edge_x = tile.vertices[2].x - tile.vertices[1].x;
            tile_edge_z = tile.vertices[1].z - tile.vertices[0].z;

            cell_edge_x = tile_edge_x / Mathf.Sqrt(CELLS_PER_TILE);
            cell_edge_z = tile_edge_z / Mathf.Sqrt(CELLS_PER_TILE);

            GameObject markerPrefab = (GameObject)Resources.Load("Prefabs/pref_NodeMarker", typeof(GameObject));

            for (int x = 0; x < Mathf.Sqrt(CELLS_PER_TILE); x++)    // column
            {
                for (int z = 0; z < Mathf.Sqrt(CELLS_PER_TILE); z++) // row
                {
                    float center_x = tile.vertices[1].x + ((x * cell_edge_x) + (cell_edge_x / 2)) + this.transform.position.x;
                    float center_z = tile.vertices[0].z + ((z * cell_edge_z) + (cell_edge_z / 2)) + this.transform.position.z;

                    // visualize -- place marker
                    GameObject marker = GameObject.Instantiate(markerPrefab, new Vector3(), new Quaternion());
                    marker.transform.parent = this.gameObject.transform;
                    marker.layer = LayerMask.NameToLayer("Debug");
                    ///marker.transform.localPosition = new Vector3(center_x, 1.5f, center_z);

                    // fill array element
                    cells[x, z] = new GOTileCell(new Vector3(center_x, 0, center_z), marker, new Vector2(x, z), cell_edge_x, cell_edge_z);

                    marker.GetComponent<NodeMarker>().Init(cells[x, z]);
                }
            }
        }

        public void ScanTileForRoads(IDictionary mapData)
        {
            IList features = (IList)mapData["features"];
            foreach (IDictionary geo in features)
            {
                IDictionary geometry = (IDictionary)geo["geometry"];
                IDictionary properties = (IDictionary)geo["properties"];
                string type = (string)geometry["type"];
                string kind = (string)properties["kind"];
                string name = (string)properties["name"];

                if (type == "MultiLineString" || type == "Polygon")
                {
                    IList lines = new List<object>();
                    lines = (IList)geometry["coordinates"];
                    foreach (IList coordinates in lines)
                    {
                        TagLine(coordinates, name);
                    }
                }

                else if (type == "LineString")
                {
                    IList coordinates = (IList)geometry["coordinates"];
                    TagLine(coordinates, name);
                }

                else
                {
                    Debug.LogWarning("unknown type: "+type+" found in road layer -- nodes might not be catching this");
                }
            }
        }

        private void TagLine(IList coordinates, string name = null)
        {
			List<NodeVert> verts  = new List<NodeVert>();

            /* 
			// build list of verts
			for (int i = 0; i < coordinates.Count; i++)
            {
				IList c = (IList)coordinates[i];
				Coordinates coords = new Coordinates((double)c[1], (double)c[0], 0);
				GameObject vertGO = PlotVert(coords, name+"_"+i);
				verts.Add(vertGO.GetComponent<NodeVert>());

				TagCellAtPoint(coords.convertCoordinateToVector(0), name);
			}

			// for every line
			for (int i = 0; i < verts.Count; i++)
			{
				if( i < verts.Count - 1 )
				{
					if(verts[i] == null || verts[i+1] == null)
					{
						Debug.LogWarning("No verts found!");
					}

					TagCellsOnLine(verts[i].transform.position, verts[i + 1].transform.position, name, verts[i], verts[i + 1]);
				}
			}
			*/
			


			
			// plot verts
            for (int i = 0; i < coordinates.Count; i++)
            {
                IList c = (IList)coordinates[i];
                Coordinates coords_a = new Coordinates((double)c[1], (double)c[0], 0);

                GameObject vert_a = PlotVert(coords_a, name+"_"+i);

				GOTileCell tagged_a = TagCellAtPoint(coords_a.convertCoordinateToVector(0), name); // start point
				
                if (i < coordinates.Count - 1) // if this is a line
                {
                    IList c_b = (IList)coordinates[i + 1];
                    Coordinates coords_b = new Coordinates((double)c_b[1], (double)c_b[0], 0);

					GameObject vert_b = PlotVert(coords_a, name+"_"+i);

					LineSeg line = new LineSeg(coords_a.convertCoordinateToVector(0), coords_b.convertCoordinateToVector(0));
					line.Draw(Color.black);
					GOTileCell tagged_b = TagCellAtPoint(coords_b.convertCoordinateToVector(0), name); // end point

					if(tagged_a != tagged_b)
					{
						tagged_a.marker.GetComponent<NodeMarker>().LaunchingLineFrom(vert_a);
						TagCellsOnLine(coords_a.convertCoordinateToVector(0), coords_b.convertCoordinateToVector(0), name); // line
					}
                }
            }
			
        }

		private GameObject PlotVert(Coordinates coordinates, string name)
		{
			GameObject vertPrefab = (GameObject)Resources.Load("Prefabs/pref_NodeVert", typeof(GameObject));

			Vector3 v = coordinates.convertCoordinateToVector(0);
			GameObject vert = GameObject.Instantiate(vertPrefab);
			vert.name = name;
            vert.GetComponent<MeshRenderer>().material.color = Color.black;
            vert.layer = LayerMask.NameToLayer("Debug");
            vert.transform.parent = this.transform;
            vert.transform.position = new Vector3(v.x, 1.5f, v.z);

			return vert;
		}

        // tag cell by line segment
        private void TagCellsOnLine(Vector3 a, Vector3 b, string name = null, NodeVert v1 = null, NodeVert v2 = null)
        {
            // start at a
            GOTileCell starting_cell = ClosestCellTo(a);
            LineSeg line = new LineSeg(a, b);

			if(v1 == null || v2 == null)
			{
				Debug.LogWarning("no verts in TagCellsOnLine");
			}

            foreach (var cell in CellsIntersectedAround(starting_cell, line, null, null, v1, v2))
            {
                cell.marker.GetComponent<NodeMarker>().IntersectsWithLine(name); // update marker
                if (!taggedCells.Contains(cell))
                {
                    taggedCells.Add(cell); // add tagged cell
                }
            }
        }

		// tag cell by coordinate 
        private GOTileCell TagCellAtPoint(Vector3 p, string name = null)
        {
            GOTileCell cell = ClosestCellTo(p);

            if (cell != null)
                cell.marker.GetComponent<NodeMarker>().ContainsPoint(name); // update marker
            if (!taggedCells.Contains(cell))
            {
                taggedCells.Add(cell); // add tagged cell
            }

			return cell;

        }

        private List<GOTileCell> CellsIntersectedAround(GOTileCell c, LineSeg l, List<GOTileCell> cells = null, List<GOTileCell> tagged = null, NodeVert v1 = null, NodeVert v2 = null)
        {
            // init
            if (cells == null) cells = new List<GOTileCell>();
			if (tagged == null) tagged = new List<GOTileCell>();

            foreach (var cell in SurroundingCellsOf(c))
            {
                if (cell != null && !cells.Contains(cell))
                {
					if(cell.marker.GetComponent<MeshRenderer>().material.color != Color.green)
                    	cell.marker.GetComponent<MeshRenderer>().material.color = Color.black;

					cells.Add(cell);

					//cell.marker.GetComponent<NodeMarker>().LineTestedFrom(v1, v2, cell.edges);

					if(CellIntersectedBy(cell, l)) //  && !tagged.Contains(cell)
					{
						tagged.Add(cell);
						CellsIntersectedAround(cell, l, cells, tagged, v1, v2); // recursion
					}
                }
            }

            return tagged;
        }

        private bool CellIntersectedBy(GOTileCell c, LineSeg l)
        {

			bool intersected = false;
            foreach(var edge in c.edges)
            {
                if (LinesIntersect.DoLinesIntersect(edge, l))
                {
					l.Draw(Color.green);	// draw road line green
					edge.Draw(Color.green); // draw cell edge green
                    intersected = true; 	// flag this cell to be intersected
                }
				else
				{
					if(l.color != Color.green)
					{
						l.Draw(Color.red);  // draw road line red 
					}
				}
            }

            return intersected;
        }

        public GOTileCell ClosestCellTo(Vector3 v)
        {
			// find where v is relative to the tile (0 - 1)
            float pX = (v.x - vert0.x) / tile_edge_x;
            float pZ = (v.z - vert0.z) / tile_edge_z;

			// how many tiles per axis
            int c = (int)Mathf.Sqrt(CELLS_PER_TILE);

			// find tile coordinates by mutiplying porpotions to the array of tiles (0-7)
            int cell_x = Mathf.FloorToInt(pX * c);
			if(cell_x == c) --cell_x;
            int cell_z = Mathf.FloorToInt(pZ * c);
			if(cell_z == c) --cell_z;

            return cells[cell_x, cell_z];
        }

        private List<GOTileCell> SurroundingCellsOf(GOTileCell cell)
        {
            List<GOTileCell> cells = new List<GOTileCell>();

            // cells.Add(cell); // add in mother cell for testing

            Vector2 aP = cell.arrayPosition;

            int max_x = (int)Mathf.Sqrt(CELLS_PER_TILE);
            int max_y = (int)Mathf.Sqrt(CELLS_PER_TILE);

            if ((int)aP.x + 1 < max_x)
            {
                cells.Add(this.cells[(int)aP.x + 1, (int)aP.y + 0]); // right
                if ((int)aP.y + 1 < max_y)
                {
                    cells.Add(this.cells[(int)aP.x + 1, (int)aP.y + 1]); // above right
                }
                if ((int)aP.y - 1 >= 0)
                {
                    cells.Add(this.cells[(int)aP.x + 1, (int)aP.y + -1]); // lower right
                }
            }

            if ((int)aP.x - 1 >= 0)
            {
                cells.Add(this.cells[(int)aP.x + -1, (int)aP.y + 0]); // left
                if ((int)aP.y + 1 < max_y)
                {
                    cells.Add(this.cells[(int)aP.x + -1, (int)aP.y + 1]); // upper left
                }
                if ((int)aP.y - 1 >= 0)
                {
                    cells.Add(this.cells[(int)aP.x + -1, (int)aP.y + -1]); // lower left
                }
            }

            if ((int)aP.y + 1 < max_y)
            {
                cells.Add(this.cells[(int)aP.x + 0, (int)aP.y + 1]); // above
            }

            if ((int)aP.y - 1 >= 0)
            {
                cells.Add(this.cells[(int)aP.x + 0, (int)aP.y + -1]); // lower
            }

            return cells;
        }
    }

    [System.Serializable]
    public class GOTileCell
    {
        public Vector3 center;
        public Vector2 arrayPosition;
        public GameObject marker;

		public List<LineSeg> edges;

        private float edge_x;
        private float edge_z;

        public GOTileCell(Vector3 center, GameObject marker, Vector2 arrayPosition, float edge_x, float edge_z)
        {
            this.center = center;
            this.marker = marker;
            this.arrayPosition = arrayPosition;
            this.edge_x = edge_x;
            this.edge_z = edge_z;

			LineSeg top_edge = new LineSeg(
                new Vector2(center.x - (edge_x / 2), center.z + (edge_z / 2)),
                new Vector2(center.x + (edge_x / 2), center.z + (edge_z / 2)));

            LineSeg right_edge = new LineSeg(
                new Vector2(center.x + (edge_x / 2), center.z + (edge_z / 2)),
                new Vector2(center.x + (edge_x / 2), center.z - (edge_z / 2)));

            LineSeg bottom_edge = new LineSeg(
                new Vector2(center.x + (edge_x / 2), center.z - (edge_z / 2)),
                new Vector2(center.x - (edge_x / 2), center.z - (edge_z / 2)));

            LineSeg left_edge = new LineSeg(
                new Vector2(center.x - (edge_x / 2), center.z - (edge_z / 2)),
                new Vector2(center.x - (edge_x / 2), center.z + (edge_x / 2)));

			edges = new List<LineSeg>{ top_edge, right_edge, bottom_edge, left_edge};

        }

        public override string ToString()
        {
            return arrayPosition.ToString();
        }

    }
}