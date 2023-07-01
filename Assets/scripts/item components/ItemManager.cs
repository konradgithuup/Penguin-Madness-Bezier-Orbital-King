using System.Collections.Generic;
using UnityEngine;

/* Controls all possible items, their actions, when they are being added (for example when purchased or spawn-event for
 * power-up type items). Simply needs to be added to Unity as a invisible background object to work. 
 */

namespace gdg_playground.Assets.scripts.item_components
{
    public class ItemManager
    {
        public List<ItemSO> itemsSo;
        public List<ItemSO> activeItemsSo;
        public GameObject[] itemsGo;


        private void Start()
        {
            for (var i = 0; i < activeItemsSo.Count; i++)
            {
                itemsGo[i].SetActive(true);
            }
            LoadItems();
        }

        public void AddItem(ItemSO item)
        {
            activeItemsSo.Add(itemsSo.Find(x => x == item));
            LoadItems();
        }

        private void LoadItems()
        {
            for (var i = 0; i < itemsSo.Count; i++)
            {
                continue; 
                /* Similar to ShopManager it takes one of the given existing GameObjects and simply converts it to
                    the matching Item your want to create. This way all items exist on first launch and then just get 
                    activated, this should safe overhead when enabling then, since they don't have to be created on the 
                    fly. 
                    
                    This can also easily be adopted for power-ups since they are just going to be another form of item
                    but wont be the purchasable type and rather just exist all the time.
                */
            }
        }
    }
}