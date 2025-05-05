using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using SkillSystem;

using UISystem;

using UnityEngine;

public abstract class BattleBehavior : MonoBehaviour
{
    public event Action<TurnAction> OnTurnActionConfirmed;
    public abstract void Execute(Context context);
    protected void ExecuteAction(TurnAction turnAction) => OnTurnActionConfirmed?.Invoke(turnAction);

    public class Context
    {
        public BattleCharacter character;
        public IList<BattleCharacter> allies;
        public IList<BattleCharacter> enemies;
    }
}

public class BattleBehaviorWrapper
{
    private BattleBehavior behavior;
    private TurnAction confirmedAction;
    public TurnAction ConfirmedAction => confirmedAction;

    public BattleBehaviorWrapper(BattleBehavior behavior) => this.behavior = behavior;

    public IEnumerator ExecuteCR(BattleBehavior.Context context)
    {
        behavior.OnTurnActionConfirmed += ExecuteTurnAction;
        behavior.Execute(context);
        yield return new WaitUntil(() => confirmedAction != null);

        void ExecuteTurnAction(TurnAction action)
        {
            behavior.OnTurnActionConfirmed -= ExecuteTurnAction;
            confirmedAction = action;
        }
    }
}

// temp: find a way to change IList to IEnumerable
public class TurnAction
{
    public readonly SkillDefinition skill;
    public readonly IList<BattleCharacter> targets;

    public TurnAction(SkillDefinition skill, IList<BattleCharacter> targets)
    {
        this.skill = skill;
        this.targets = targets;
    }
}

public class PlayerBattleBehavior : BattleBehavior
{
    // props
    BattleController Controller => BattleController.Instance;
    UIController UI => UIController.Instance;
    Menu TargetMenu => Controller.TargetMenu;
    Menu SkillMenu => Controller.SkillMenu;
    Menu ActionMenu => Controller.ActionMenu;

    // fields
    SkillDefinition selectedSkill;

    public override void Execute(Context context)
    {
        var character = context.character;
        var enemies = context.enemies;
        var allies = context.allies;

        // setup skill names
        var skills = character.InfoSubject.Info.Get<SkillsInfo>().Skills;
        var skillNames = skills.Select(skill => skill.name);
        Controller.SkillGenerator.Generate(skillNames);

        // setup action menu
        ActionMenu.OnSelected += OnActionMenuSelected;
        void OnActionMenuSelected()
        {
            var skill = Controller.Skills[ActionMenu.OptionIndex];
            if (skill) OpenTargetMenu(skill);
        }

        // setup skill menu
        SkillMenu.OnSelected += OnSkillMenuSelected;
        void OnSkillMenuSelected() => OpenTargetMenu(skills[SkillMenu.OptionIndex]);

        // setup target menu
        TargetMenu.OnSelected += OnTargetSelected;
        void OnTargetSelected()
        {
            // clear events
            ActionMenu.OnSelected -= OnActionMenuSelected;
            SkillMenu.OnSelected -= OnSkillMenuSelected;
            TargetMenu.OnSelected -= OnTargetSelected;

            // close all UI
            UI.CloseAll();

            // execute action
            var targets = TargetMenu.SelectedIndices.Select(i => enemies[i]);
            ExecuteAction(new(selectedSkill, targets.ToList()));
        }

        // open menu
        UI.Open(ActionMenu);

        void OpenTargetMenu(SkillDefinition skill)
        {
            // save selection
            selectedSkill = skill;

            // setup target names
            var side = skill.Infos.Get<TargetSideInfo>()?.Side;
            var targets = side switch
            {
                TargetSideInfo.TargetSide.Ally => allies,
                TargetSideInfo.TargetSide.Enemy => enemies,
                _ => enemies,
            };
            var names = targets.Select(target => target.gameObject.name);
            Controller.TargetGenerator.Generate(names);
            TargetMenu.Refresh();

            // target type
            var targetType = skill.Infos.Get<TargetTypeInfo>()?.Type ?? TargetTypeInfo.TargetType.Single;

            // setup menu
            if (targetType == TargetTypeInfo.TargetType.All)
            {
                TargetMenu.lockNavigation = true;
                TargetMenu.showCursor = false;
            }
            else
            {
                TargetMenu.lockNavigation = false;
                TargetMenu.showCursor = true;
            }

            // open menu
            UI.Open(TargetMenu);

            // toggle on all button
            if (targetType == TargetTypeInfo.TargetType.All) TargetMenu.ToggleAll(true);
        }
    }
}