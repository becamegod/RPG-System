using System.Collections.Generic;
using System.Linq;

using UISystem;

using Unity.Cinemachine;

using UnityEngine;

using static Unity.Cinemachine.CinemachineTargetGroup;

public class BattleCamera : Singleton<BattleCamera>
{
    [SerializeField] CinemachineCamera idleCamera;
    [SerializeField] float targetWeight = 2;
    [SerializeField] CinemachineCamera actionCamera;
    [SerializeField] float actionRadius = 1;

    // props
    BattleController Battle => BattleController.Instance;
    CameraManager Camera => CameraManager.Instance;
    Menu TargetMenu => Battle.TargetMenu;

    private void Start()
    {
        Battle.OnNewTurn += UseIdleCamera;
        Battle.OnAction += UseActionCamera;
        TargetMenu.OnFocus += FocusTarget;
        TargetMenu.OnOptionChanged += FocusTarget;
        TargetMenu.OnLoseFocus += ResetFocus;
    }

    private void ResetFocus()
    {
        var group = idleCamera.LookAt.GetComponent<CinemachineTargetGroup>();
        foreach (var target in group.Targets) target.Weight = 1;
    }

    private void FocusTarget()
    {
        var group = idleCamera.LookAt.GetComponent<CinemachineTargetGroup>();

        // reset weights
        foreach (var target in group.Targets) target.Weight = 1;

        // set weights
        foreach (var i in TargetMenu.SelectedIndices)
        {
            var target = group.Targets[i];
            target.Weight = targetWeight;
        }
    }

    private void OnDestroy()
    {
        Battle.OnNewTurn -= UseIdleCamera;
        Battle.OnAction -= UseActionCamera;
        TargetMenu.OnFocus -= FocusTarget;
        TargetMenu.OnOptionChanged -= FocusTarget;
        TargetMenu.OnLoseFocus -= ResetFocus;
    }

    private void UseIdleCamera(BattleCharacter character, IEnumerable<BattleCharacter> enemies)
    {
        Camera.SetCamera(idleCamera);

        // update group targets
        var group = idleCamera.LookAt.GetComponent<CinemachineTargetGroup>();
        group.Targets = enemies.Select(enemy => new Target() { Object = enemy.transform }).ToList();
    }

    private void UseActionCamera(BattleCharacter character, IEnumerable<BattleCharacter> enemies)
    {
        Camera.SetCamera(actionCamera);

        var group = actionCamera.Follow;
        var subjects = enemies.With(character);
        group.GetComponent<CinemachineTargetGroup>().Targets = subjects.Select(enemy => new Target() { Object = enemy.transform, Radius = actionRadius }).ToList();
    }
}
