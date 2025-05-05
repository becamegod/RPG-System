using EasyButtons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CutsceneSystem
{
    [CreateAssetMenu(fileName = "NewCutscene", menuName = "Cutscene/Cutscene")]
    public class Cutscene : ScriptableObject
    {
        [SerializeField] bool showCinematicBars = true;
        [SerializeReference] List<CutsceneAction> actions;

        public List<CutsceneAction> Actions => actions;
        public bool ShowCinematicBars => showCinematicBars;

        [Button]
        private void AddAction() => EditorHelper.ShowTypesMenuToAdd(Actions);
    }

    [Serializable]
    public class CutsceneAction
    {
        [SerializeField] protected float delay;

        public virtual IEnumerator Play()
        {
            yield return new WaitForSeconds(delay);
        }

        public virtual void Revert() { }
    }

    public class DelayAction : CutsceneAction { }

    public class AnimatorAction : CutsceneAction
    {
        [SerializeField] string animatorId;
        [SerializeField] string stateToPlay;

        private int StateId => stateId != 0 ? stateId : (stateId = Animator.StringToHash(stateToPlay));
        private Animator Animator => AnimatorIdManager.Instance.GetById(animatorId);

        private int stateId;
        private int originalState;

        public override IEnumerator Play()
        {
            yield return base.Play();
            originalState = Animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
            Animator.Play(StateId);
            yield return null;
        }

        public override void Revert()
        {
            base.Revert();
            Animator.Play(originalState);
        }
    }
}
