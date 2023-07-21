using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

// following this Tutorial https://www.youtube.com/watch?v=EEtOt0Jf7PQ

/*
 * Manages the shops and it's components. Items can be purchased and become unavailable in Shop once they have been
 * purchased once. 
 */

namespace gdg_playground.Assets.scripts
{
    public class ShopManager : MonoBehaviour
    {
        public static int points = 0;
        public static int numIceFloes = 3;
        public static bool[] isPurchased = { false, false, false, false, false, false };

        public TMP_Text pointsUI;
        public ShopItemSO[] shopItemsSo;
        public GameObject[] shopPanels;
        public Button[] purchaseButtons;
        

        private void Start()
        {
            // Set UI:
            pointsUI.text = "Points\n" + points;
            for (int i = 0; i < shopPanels.Length; i++)
            {
                UpdatePanel(shopItemsSo[i],i);
            }
        }

        private void Update()
        {
            
        }

        private void UpdatePanel(ShopItemSO item, int panelID)
        {
            // Get panel elements:
            TMP_Text titleText = shopPanels[panelID].transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
            TMP_Text descriptionText = shopPanels[panelID].transform.GetChild(1).gameObject.GetComponent<TMP_Text>();
            TMP_Text priceText = shopPanels[panelID].transform.GetChild(2).gameObject.GetComponent<TMP_Text>();
            Button purchaseButton = purchaseButtons[panelID];
            TMP_Text buttonText = purchaseButton.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();

            // Set panel elements:
            titleText.text = item.title;
            descriptionText.text = item.description;
            priceText.text = item.basePrice.ToString();
            buttonText.text = isPurchased[panelID] ? "sold" : "buy";
        }

        public void PurchaseItem(int buttonNum)
        {
            // Check and update points:
            if (points < shopItemsSo[buttonNum].basePrice || isPurchased[buttonNum]) return;
            points -= shopItemsSo[buttonNum].basePrice;

            // Update shop state:
            isPurchased[buttonNum] = true;
            if (buttonNum == 3 || buttonNum == 4) { numIceFloes++; }

            // Update shop ui:
            pointsUI.text = "Points\n" + points;
            UpdatePanel(shopItemsSo[buttonNum], buttonNum);
        }
    }
}