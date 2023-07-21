using gdg_playground.Assets.scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseButton : MonoBehaviour
{
    public int buttonID;
    public GameObject shopManager;

    public void purchaseItem()
    {
        shopManager.GetComponent<ShopManager>().PurchaseItem(buttonID);
    }
}
