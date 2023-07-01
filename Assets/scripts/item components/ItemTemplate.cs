using Unity.VisualScripting;
using UnityEngine;

namespace gdg_playground.Assets.scripts.item_components
{
    public class ItemTemplate : MonoBehaviour
    {
        public string title;
        public string description;
        public int basePrice;
        public bool purchased;
        public Sprite icon;
        public Variables effect; 
    }
}