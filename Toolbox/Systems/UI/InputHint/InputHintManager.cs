using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputHinting
{
    public class InputHintManager : MonoBehaviour
    {
        [Serializable]
        struct SchemeMap
        {
            [SerializeField] string name;
            [SerializeField] InputHintScheme scheme;

            public string Name => name;
            public InputHintScheme Scheme => scheme;
        }

        [SerializeField] SchemeMap[] schemes;

        // fields
        private InputControlScheme lastScheme;

        private void Awake() => HideAllSchemeHint();

        private void HideAllSchemeHint()
        {
            foreach (var entry in schemes) entry.Scheme.transform.localScale = Vector3.zero;
        }

        private void UpdateHintToScheme(string name)
        {
            var scheme = schemes.First(scheme => scheme.Name == name);
            HideAllSchemeHint();
            scheme.Scheme.transform.localScale = Vector3.one;
        }
    }
}
