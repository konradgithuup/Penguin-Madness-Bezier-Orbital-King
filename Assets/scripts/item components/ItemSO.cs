using Unity.VisualScripting;
using UnityEngine;

/* New Item Scriptable Object that can be easily add in UnityEditor. Due to it's scalable nature any item with any effect
 * should be creatable and usable as long as it's added to the itemManager
 * 
 */

namespace gdg_playground.Assets.scripts.item_components
{
    [CreateAssetMenu(fileName = "newItem", menuName = "Scriptable Object / New game item", order = 2)]
    public class ItemSO : ScriptableObject
    {
        public string title;
        public string description;
        public int basePrice;
        public bool purchased;
        public Sprite icon;
        public Variables effect; 
    }
}