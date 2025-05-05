using EasyButtons;
using System.Collections.Generic;
using UISystem;
using UnityEngine;
using UnityEngine.UI;

namespace InputHinting
{
    public class InputHintScheme : MonoBehaviour
    {
        [SerializeField] ContentFitterRefresh refresher;
        [SerializeField] GameObject navigate;
        [SerializeField] GameObject sprint;
        [SerializeField] GameObject interact;
        [SerializeField] GameObject proceed;
        [SerializeField] GameObject exit;
        [SerializeField] GameObject menu;
        [SerializeField] HintUI option;
        [SerializeField] GameObject map;

        // props
        private UIController UI => UIController.Instance;

        // fields
        private bool interactLastStatus;
        private IVolatileInput prevVolatileInputUI;

        private void Awake() => TurnOffAll();

        public void TurnOffAll()
        {
            navigate.SetActive(false);
            sprint.SetActive(false);
            interact.SetActive(false);
            proceed.SetActive(false);
            exit.SetActive(false);
            menu.SetActive(false);
            option.gameObject.SetActive(false);
            map.SetActive(false);
        }

        private void Start()
        {
            UI.OnNewUIFocused += Refresh;
            UI.OnDelayedFocus += TurnOffAll;
            Refresh();
        }

        private void OnDestroy()
        {
            UI.OnNewUIFocused -= Refresh;
            UI.OnDelayedFocus -= TurnOffAll;
        }

        public void Refresh()
        {
            TurnOffAll();
            if (prevVolatileInputUI != null)
            {
                prevVolatileInputUI.OnInputChanged -= Refresh;
                prevVolatileInputUI = null;
            }
            var ui = UI.CurrentUI;

            // when in UI
            if (ui is IVolatileInput volatileInput)
            {
                volatileInput.OnInputChanged += Refresh;
                prevVolatileInputUI = volatileInput;
            }
            if (ui is INavigatable) navigate.SetActive(true);
            if (ui is ISelectable) proceed.SetActive(true);
            if (ui is IExitBlocking exitBlocking && exitBlocking.CanExit()
                || ui is not IExitBlocking && ui is not AnyButton) exit.SetActive(true);
            if (ui is IOptionable optionUI)
            {
                option.gameObject.SetActive(true);
                option.Description = optionUI.OptionDescription();
            }

            refresher.RefreshContentFitters();
        }

        [Button]
        private void UpdateLayout()
        {
            var contentSizeFitters = GetComponentsInChildren<ContentSizeFitter>();
            var layoutGroups = GetComponentsInChildren<LayoutGroup>();
            var statusMap = new Dictionary<Behaviour, bool>();
            foreach (var contentSizeFitter in contentSizeFitters)
            {
                statusMap[contentSizeFitter] = contentSizeFitter.enabled;
                contentSizeFitter.enabled = true;
            }
            foreach (var layoutGroup in layoutGroups)
            {
                statusMap[layoutGroup] = layoutGroup.enabled;
                layoutGroup.enabled = true;
            }
            Canvas.ForceUpdateCanvases();
            foreach (var contentSizeFitter in contentSizeFitters) contentSizeFitter.enabled = statusMap[contentSizeFitter];
            foreach (var layoutGroup in layoutGroups) layoutGroup.enabled = statusMap[layoutGroup];
            statusMap.Clear();
        }
    }
}
