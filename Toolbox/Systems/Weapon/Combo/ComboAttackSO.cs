using UnityEngine;
using UnityEngine.InputSystem;

namespace WeaponSystem
{
    [CreateAssetMenu(menuName = Global.AttackMenu + "Combo Attack")]
    public class ComboAttackSO : ScriptableObject
    {
        [SerializeField] InputActionReference input;
        public InputActionReference Input => input;

        [SerializeField] ComboTree comboTree;
        public ComboTree ComboTree => comboTree;
    }
}