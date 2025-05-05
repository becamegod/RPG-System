using UnityEngine;

namespace UISystem
{
    public class UIAudio : GeneralAudio
    {
        [SerializeField] AudioClip navigateSound, selectSound, exitSound;

        private void Start()
        {
            var ui = UIController.Instance;
            ui.OnNavigated += () =>
            {
                if (navigateSound) Play(navigateSound);
            };
            ui.OnSelected += () =>
            {
                if (selectSound) Play(selectSound);
            };
            ui.OnExitted += () =>
            {
                if (exitSound) Play(exitSound);
            };
        }
    }
}
