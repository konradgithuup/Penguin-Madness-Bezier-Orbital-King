using UnityEngine;

namespace gdg_playground.Assets.scripts
{
    [CreateAssetMenu(fileName = "shopMenu", menuName = "Scriptable Object / New shop item", order = 1)]
    public class ShopItemSO : ScriptableObject
    {
        public string title;
        public string description;
        public int basePrice;
    }
}