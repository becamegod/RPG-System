using System;
using System.Collections.Generic;
using System.Linq;

using NaughtyAttributes;

using UnityEngine;
using UnityEngine.InputSystem;

using WeaponSystem;

[Serializable]
public class ComboTree
{
    [SerializeReference] List<ComboAttack> comboAttacks;
    [SerializeField, Expandable] List<ComboAttackSO> nextAttacks;

    [Button]
    public void AddComboAttack() => comboAttacks.Add(new());

    internal ComboAttackSO GetNextAttack(InputActionReference input)
    {
        try { return nextAttacks.First(comboAttack => comboAttack.Input == input); }
        catch (InvalidOperationException) { return null; }
    }
}

[Serializable]
public class ComboAttack
{
    [SerializeField] InputActionReference input;
    [SerializeReference, NewButton] List<SubAction> actions;
    [SerializeReference] List<ComboAttack> connectedAttacks;

    [Button]
    public void AddAction() => EditorHelper.ShowTypesMenuToAdd(actions, false);
    public void AddConnectedAttack() => connectedAttacks.Add(new());
}
