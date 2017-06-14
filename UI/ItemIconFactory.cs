using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyO.Object;

namespace FairyO.UI
{
    public class ItemIconFactory
    {
		private GameObject iconPrefab;

		public ItemIconFactory(GameObject iconPrefab)
		{
			this.iconPrefab = iconPrefab;
		}

        public GameObject NewItemIcon(Item item, int num, Transform parent)
        {
			GameObject go = GameObject.Instantiate(iconPrefab);
			go.transform.SetParent(parent);

			ItemIcon itemIcon = go.GetComponent<ItemIcon>();
			itemIcon.Item = item;
			itemIcon.Num = num;

			return go;
        }

    }
}