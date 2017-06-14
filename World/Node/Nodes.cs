using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;
using GameSparks.Core;

namespace FairyO.World
{
    public class Nodes : MonoBehaviour
    {
        // -------- inspector --------
        public NodeSpace nodeSpace;

        // -------- properties -----------
        public Dictionary<int, Node> manifest {get; private set;}

        IEnumerator Start()
        {
            manifest = new Dictionary<int, Node>();
            yield return StartCoroutine(GetNodeManifest());
        }

        private IEnumerator GetNodeManifest()
        {
            // wait for gamesparks to become available if not available
            if (!GS.Available || !GS.Authenticated || !SparksLogin.loggedIn)
            {
                // wait for gamesparks to finish login
                while (!GS.Available || !GS.Authenticated || !SparksLogin.loggedIn)
				{
					yield return null;
				}
            }

            new LogEventRequest().SetEventKey("GET_NODES")
            	.SetEventAttribute("NUM", 10)
            	.Send((response) =>
                   {
                       if (!response.HasErrors)
                       {
						   ManifestNodes(response.ScriptData.GetGSDataList("nodes"));
                       }
                       else
                           Debug.LogError(response.Errors.ToString());
                   });
        }

		private void ManifestNodes(List<GSData> data)
		{
            int c = 0;
			foreach(GSData d in data)
			{
				Node node = new Node(d);
				manifest[node.id] = node;
                c++;
			}

            Debug.Log("[FairyO.Nodes] - Manifest created with "+c+" entries.");
		}
    }
}