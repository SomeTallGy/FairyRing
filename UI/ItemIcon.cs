using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FairyO.Object;

namespace FairyO.UI
{
    public class ItemIcon : MonoBehaviour
    {
        // ------- inspector fields --------
        public Image iconGraphic;
        public Image iconFill;
        public Transform numberContainer;
        public Text numberText;

        public List<Sprite> iconFills;
        
        // ------- private fields ----------
        private Item item;
        private int num;

        // -------- properties -------------
        public Item Item {get { return item;} set{
            this.item = value;
            SetItem();
        }}

        public int Num {get { return num;} set{
            this.num = value;
            SetNum();
        }}

        private void SetItem()
        {
            // set fill
            switch((ItemTier)this.item.identity.tier - 1)
            {
                case ItemTier.Uncommon:
                case ItemTier.Rare:
                case ItemTier.Legendary:
                case ItemTier.Epic:
                    iconFill.sprite = this.iconFills[this.item.identity.tier - 2];
                    break;
                case ItemTier.Common:
                default:
                    iconFill.gameObject.SetActive(false); // hide
                    break;
            }

            // set graphic
            this.iconGraphic.sprite = this.item.iconGraphic;
        }

        private void SetNum()
        {
            if(this.num > 0)
            {
                this.numberContainer.gameObject.SetActive(true); // show number
               this.numberText.text = this.num.ToString(); // set number text
            }
            else
            {
                this.numberContainer.gameObject.SetActive(false); // hide number
            }
        }


    }
}