using System;

using UnityEngine;

namespace UISystem
{
    public interface INavigatable
    {
        public bool Navigate(Vector2 direction);
        public bool Navigate(int index);
    }

    public interface IAltNavigatable
    {
        public void AltNavigate(int direction);
    }

    public interface ISelectable
    {
        public bool Select(bool isLMB);
    }

    public interface IToggleable
    {
        public event Action OnToggledOn, OnToggledOff;
    }

    public interface IInteractable
    {
        public bool Interact();
    }

    public interface IOptionable
    {
        public void Option();
        public string OptionDescription();
    }

    public interface IExitListener
    {
        public void OnExit();
    }

    public interface IExitBlocking
    {
        public bool CanExit();
    }

    public interface IVolatileInput
    {
        public event Action OnInputChanged;
    }

    public class ComplexMenu : Menu, IAltNavigatable
    {
        public void AltNavigate(int direction) => throw new NotImplementedException();
    }
}