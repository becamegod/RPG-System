using System;

namespace UISystem
{
    public class AnyButton : BaseUI
    {
        public event Action OnTrigger;

        public void Trigger()
        {
            OnTrigger?.Invoke();
        }
    }
}
