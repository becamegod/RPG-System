using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "New Category", menuName = "Inventory/Category")]
    public class Category : ScriptableObject
    {
        public new string name;
    }
}