using UnityEngine;

namespace FairyO.Game.Gathering
{
    public class Basket : MonoBehaviour
    {
		// ---------- constants -----------
		public const string TAG_NAME = "Basket";

        // ----- properties ----------
		public float Size { 
			get{ return size; } 
			set{ 
				if(value <= 10 && value >= 0)
				{
					size = value;
					this.transform.localScale = new Vector3(1 + (size / 10), 1 + (size / 10), 1 + (size / 10));
				}
				else
				{
					Debug.LogError("Cannot set value higher than 10 or below 0");
				}
			}
		}
		public static float AbsorbDelay { 
			get{ return absorbDelay; }
			set{ 
				if(value <= 10 && size >= 0)
				{
					absorbDelay = value; 
				}
				else
				{
					Debug.LogError("Cannot set value higher than 10 or below 0");
				}
			}
		}

        // ----- inspector -----------
        public bool showDebugControl = false;


		// ----- private fields ------
        private static float size = 0;
        private static float absorbDelay = 10;

        void OnGUI()
        {
            if (showDebugControl)
            {
                // size slider
                GUI.Label(new Rect(25, 55, 100, 30), "Size");
                Size = GUI.HorizontalSlider(new Rect(25, 40, 100, 30), Size, 0.0F, 10.0F);

                // absorb slider
                GUI.Label(new Rect(150, 55, 100, 30), "Absorb Delay");
                AbsorbDelay = GUI.HorizontalSlider(new Rect(150, 40, 100, 30), AbsorbDelay, 0.0F, 10.0F);
            }
        }
    }
}
