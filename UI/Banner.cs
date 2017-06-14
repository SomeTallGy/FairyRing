using UnityEngine;
using UnityEngine.UI;

namespace FairyO.UI
{
    public class Banner : MonoBehaviour
    {
        public Text bannerText;

        void Start()
        {
            // hide by default
            Hide();
        }
        public void Show(string msg, float time = 0)
        {
            bannerText.text = msg;
            gameObject.SetActive(true);

            if (time > 0)
            {
                CancelInvoke("Hide");
                Invoke("Hide", time);
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

