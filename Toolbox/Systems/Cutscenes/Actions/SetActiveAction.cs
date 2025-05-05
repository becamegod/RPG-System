using System.Collections;

using UnityEngine;

namespace CutsceneSystem
{
    public class SetActiveAction : CutsceneAction
    {
        [SerializeField] string gameObjectId;
        [SerializeField] bool status;

        public override IEnumerator Play()
        {
            yield return base.Play();
            var subject = GameObjectIdManager.Instance.GetById(gameObjectId);
            subject.SetActive(status);
        }
    }
}
