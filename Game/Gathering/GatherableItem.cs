using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyO.Object;

namespace FairyO.Game.Gathering
{
    public class GatherableItem : MonoBehaviour
    {
		// ----------- events ------------
		public static event CollectedHandler onCollected;
		public delegate void CollectedHandler(GameItem item);

		// ---------- enum ----------------
		public enum CollectState {OutBasket, InBasket, Collected}

		// ---------- properties ----------
		public CollectState State {get; private set;}
		public GameItem GameItem {get; internal set;}

		// ---------- private fields ------
		private SelfDestruct selfDestruct;

		void Start()
		{
			selfDestruct = this.GetComponent<SelfDestruct>();
			selfDestruct.onDestruct += OnDestructHandler;
		}

		void OnTriggerEnter(Collider col)
		{
			if(State != CollectState.Collected && col.gameObject.name == "Basket")
			{
				State = CollectState.InBasket;
				selfDestruct.Reset(true);
				selfDestruct.onDestruct += Collect;
				selfDestruct.StartDestruct(0.5f + GameItem.gatherTime);
				//selfDestruct.StartDestruct(0.5f + Basket.AbsorbDelay / 4);
			}
			else if(State != CollectState.Collected && col.gameObject.name == "Trash")
			{
				selfDestruct.Reset(true);
				selfDestruct.StartDestruct(0.1f);
			}
			
		}

		void OnTriggerExit(Collider col)
		{
			if(State != CollectState.Collected)
			{
				State = CollectState.OutBasket;
				selfDestruct.Reset(false);
				selfDestruct.onDestruct -= Collect;
			}
		}

		private void Collect(GameObject go)
		{
			State = CollectState.Collected;
			if(onCollected != null)
				onCollected(GameItem);
		}

		private void OnDestructHandler(GameObject go)
		{
			this.transform.parent.GetComponent<GatheringGame>().GatherableItemsSpent++;
		}
		
    }
}