using UnityEngine;
using UnityEngine.UI;
using TMPro;

// following this Tutorial https://www.youtube.com/watch?v=EEtOt0Jf7PQ

namespace gdg_playground.Assets.scripts
{
    public class ShopManager : MonoBehaviour
    {
        public int points;
        public TMP_Text pointsUI;
        public ShopItemSO[] shopItemsSo;
        public GameObject[] shopPanelsGo;
        public ShopTemplate[] shopPanels;
        public Button[] purchaseButtons;

        private void Start()
        {
            for (var i = 0; i < shopItemsSo.Length; i++)
            {
                shopPanelsGo[i].SetActive(true);
            }

            pointsUI.text = "Points: " + points;
            LoadPanels();
            CheckPurchasable();
        }


        public void AddPoints()
        {
            points++;
            pointsUI.text = "Points: " + points;
            CheckPurchasable();
        }

        private void CheckPurchasable()
        {
            for (var i = 0; i < shopItemsSo.Length; i++)
            {
                purchaseButtons[i].interactable = false;
                if (shopItemsSo[i].purchased) continue;
                purchaseButtons[i].interactable = points >= shopItemsSo[i].basePrice;
            }
        }

        public void PurchaseItem(int buttonNum)
        {
            if (points < shopItemsSo[buttonNum].basePrice) return;
            points -= shopItemsSo[buttonNum].basePrice;
            pointsUI.text = "Points: " + points;
            shopItemsSo[buttonNum].purchased = true;
            CheckPurchasable();
        }

        private void LoadPanels()
        {
            for (var i = 0; i < shopItemsSo.Length; i++)
            {
                shopPanels[i].titleText.text = shopItemsSo[i].title;
                shopPanels[i].descriptionText.text = shopItemsSo[i].description;
                shopPanels[i].priceText.text = shopItemsSo[i].basePrice.ToString();
            }
        }
    }
}