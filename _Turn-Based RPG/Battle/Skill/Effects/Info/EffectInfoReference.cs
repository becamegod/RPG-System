using System;

using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(menuName = Global.AssetMenu + "Effect Info")]
    public class EffectInfoReference : ScriptableObject
    {
        [SerializeReference] EffectInfo effectInfo;
        public EffectInfo Info => effectInfo;
    }

    [Serializable]
    public class EffectInfo : Info, IIcon
    {
        [SerializeField] string popupText;
        public string PopupText => popupText;

        [SerializeField] Sprite icon;
        public Sprite Icon => icon;

        [SerializeField] VisualEffectEvent visualEffect;
        public VisualEffectEvent VisualEffect => visualEffect;
    }
}
