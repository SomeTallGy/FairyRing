using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyO.Object;
using FairyO.UI;
using DG.Tweening;

namespace FairyO.Game.Gathering
{
    public class GatheringGame : MonoBehaviour
    {
        // -------- inspector fields --------
        public Transform gatheringGameUI;
        public Transform itemsCollectedUI;
        public Transform basket;
        public Transform trash;

        // ------- booleans -----------------
        static public bool IsReady {get; set;}

        // -------- private fields ----------
        public List<Item> gatheredItems = new List<Item>();
        private ItemIconFactory itemIconFactory;

        // -------- bookkeeping -------------
        public int GatherableItemsTotal { get; set; }
        public int GatherableItemsSpent
        {
            get { return gatherableItemsSpent; }
            set
            {
                this.gatherableItemsSpent = value;
                if (gatherableItemsSpent >= GatherableItemsTotal)
                {
                    StartCoroutine(InventoryCollectedItems());
                }
            }
        }
        public int gatherableItemsSpent = 0;
        private int iconsVisible = 0;


        // Use this for initialization
        void Start()
        {
            GatherableItem.onCollected += OnCollected;
            InitNode();
            InitItemIconFactory();
            InitBasket();
        }

        private void InitNode()
        {
            
        }

        private void InitItemIconFactory()
        {
            GameObject prefab = Resources.Load("pref_ItemIcon") as GameObject;
            itemIconFactory = new ItemIconFactory(prefab);
        }

        private void InitBasket()
        {
            this.basket.transform.DOMoveZ(0, 1.0f).OnComplete(()=>{
                basket.GetComponent<ScreenDragObject>().enabled = true;
                GatheringGame.IsReady = true;
            });
        }

        private IEnumerator InventoryCollectedItems()
        {
            // terminating condition
            if (gatheredItems.Count == 0) yield return this;

            // 1. sort the gatheredItems list by slug
            gatheredItems.Sort(delegate (Item a, Item b)
            {
                return (a.slug).CompareTo(b.slug);
            });

            yield return new WaitForSeconds(0.5f);

            // 2. account each item
            ItemIcon icon = null;
            foreach (var item in gatheredItems)
            {
                if (icon != null)
                {
                    if (icon.Item == item)
                    {
                        icon.Num++;
                    }
                    else
                    {
                        icon = itemIconFactory.NewItemIcon(item, 1, itemsCollectedUI.transform).GetComponent<ItemIcon>();
                    }
                }
                else
                {
                    icon = itemIconFactory.NewItemIcon(item, 1, itemsCollectedUI.transform).GetComponent<ItemIcon>();
                }

                yield return new WaitForSeconds(0.1f);
            }

            // Finish!
            yield return this;
        }

        private void OnCollected(GameItem item)
        {
            // 1. add item to gatherItems
            gatheredItems.Add(item);

            // 2. create icon gameobject
            GameObject iconGO = itemIconFactory.NewItemIcon(item, 0, gatheringGameUI);

            // 3. position icon over basket and scale down
            iconGO.transform.position = BasketPositionToUI();
            iconGO.transform.localScale = new Vector2(0.1f, 0.1f);

            iconsVisible++;

            // 4. pop icon out of basket
            iconGO.transform.DOScale(new Vector2(0.5f, 0.5f), 0.4f).SetEase(Ease.OutSine);
            iconGO.transform.DOMoveY(iconGO.transform.position.y + (45 + (iconsVisible * 30)), 1.0f + (0.1f * iconsVisible)).OnComplete(() =>
            {
                iconGO.transform.DOScale(new Vector2(0.01f, 0.01f), 0.4f).SetEase(Ease.InSine).OnComplete(() =>
                {
                    iconsVisible--;
                    GameObject.Destroy(iconGO);
                });
            });
        }

        private Vector2 BasketPositionToUI()
        {
            // 1. calculate position of basket relative to the UI (screen)
            Vector2 basketPosition = Camera.main.WorldToScreenPoint(this.basket.position);
            basketPosition.y += 40;

            return basketPosition;
        }
    }
}