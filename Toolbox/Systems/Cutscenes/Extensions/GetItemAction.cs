using InventorySystem;
using System.Collections;
using UnityEngine;

namespace CutsceneSystem
{
    public class GetItemAction : CutsceneAction
    {
        [SerializeField] Item item;
        [SerializeField] int amount;

        public override IEnumerator Play()
        {
            yield return base.Play();
            var closed = false;
            //Inventory.Instance.AddWithPopup(item, amount, () => closed = true);
            yield return new WaitUntil(() => closed);
        }
    }
}
