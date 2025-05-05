using System;
using System.Collections.Generic;

using UISystem;

using UnityEngine;

namespace SaveSystem
{
    public class SaveUI : Menu, IOptionable
    {
        [Header("References")]
        [SerializeField] Transform slotsParent;
        [SerializeField] SaveSlotUI slotPrefab;
        [SerializeField] Menu savePointMenu;
        [SerializeField] GenericText header, footer;
        [SerializeField, ReadOnly] Menu slotsMenu;

        // const
        public const int SAVE = 1;
        public const int LOAD = 2;

        // props
        private SaveManager Manager => SaveManager.Instance;
        private ChoicePopup ChoicePopup => ChoicePopup.Instance;
        private MessagePopup MessagePopup => MessagePopup.Instance;

        // fields
        private List<SaveSlotUI> slotUIs;

        // events
        public event Action<object> OnLoaded;

        private new void Awake()
        {
            base.Awake();
            slotsMenu = this;
        }

        private new void Start()
        {
            base.Start();
            GenerateSlotUIs();
            savePointMenu.OnPreSelect += () => UpdateSaveLoadScreen();
            slotsMenu.OnPreSelect += ConfirmLoad; // default loading
        }

        public void UpdateSaveLoadScreen(int option = -1)
        {
            if (option == -1) option = savePointMenu.OptionIndex;
            header.Text = option switch
            {
                SAVE => "Save",
                LOAD => "Load",
                _ => "Something's wrong"
            };
            footer.Text = option switch
            {
                SAVE => "Select a slot to save",
                LOAD => "Select a saved game to load",
                _ => "Something's wrong"
            };

            slotsMenu.OnPreSelect -= Save;
            slotsMenu.OnPreSelect -= ConfirmLoad;
            slotsMenu.OnPreSelect += option switch
            {
                SAVE => Save,
                LOAD => ConfirmLoad,
                _ => null
            };
        }

        private void Save() => Save(false);
        private void Save(bool forced = false)
        {
            var result = Manager.Save(Manager.SaveDatas[slotsMenu.OptionIndex], slotsMenu.OptionIndex, forced);
            if (result == SaveResult.FileExisted)
                ChoicePopup.Show("The save data is existed in this slot.\nDo you want to overwrite?", OverwriteSave);
            else if (result == SaveResult.Succeed) MessagePopup.Show("Save successfully");
            else MessagePopup.Show("Something's wrong");

            void OverwriteSave() => Save(true);
        }

        private void ConfirmLoad()
        {
            if (Manager.SaveDatas[slotsMenu.OptionIndex] == null) return;
            ChoicePopup.Show("Do you want to load this save data?", Load);
        }

        private void Load()
        {
            var slotIndex = slotsMenu.OptionIndex;
            var result = Manager.Load(slotIndex);
            MessagePopup.Show(result switch
            {
                not null => "Load successfully",
                null => "Load failed",
            }, () => OnLoaded?.Invoke(result));
        }

        private void GenerateSlotUIs()
        {
            slotsParent.DestroyChildren();
            slotUIs = new();
            for (int i = 0; i < Manager.SlotNumber; i++)
            {
                var j = i;
                var slotUI = Instantiate(slotPrefab, slotsParent);
                slotUI.Index = i;
                slotUI.OnDelete.AddListener(() =>
                {
                    slotsMenu.OptionIndex = j;
                    ConfirmDelete();
                });
                slotUIs.Add(slotUI);
            }
            slotsMenu.Refresh();
        }

        public void Option() => ConfirmDelete();

        private void ConfirmDelete()
        {
            if (Manager.SaveDatas[slotsMenu.OptionIndex] == null) return;
            ChoicePopup.Show("Do you really want to delete this save data? This can't be undone.", Delete);
        }

        public string OptionDescription() => "Delete";

        private void Delete()
        {
            var slotIndex = slotsMenu.OptionIndex;
            var result = Manager.Delete(slotIndex);
            MessagePopup.Show(result switch
            {
                DeleteResult.Succeed => "Delete successfully",
                DeleteResult.Failed => "Delete failed",
                _ => "Something's wrong"
            });
        }
    }
}
