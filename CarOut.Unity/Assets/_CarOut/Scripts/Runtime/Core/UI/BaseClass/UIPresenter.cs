using System;
using UnityEngine.UIElements;

namespace CarOut.UI
{
    public abstract class UIPresenter
    {
        protected VisualElement RootVisualElement;
        protected Action OnToggle;

        protected UIPresenter(VisualElement rootVisualElement)
        {
            RootVisualElement = rootVisualElement;
            SetupPage();
        }

        protected abstract void SetupPage();

        public void ChangePageTo(UIPresenter targetUI)
        {
            RootVisualElement.RemoveFromClassList("bottom-sheet-up");
            targetUI.RootVisualElement.AddToClassList("bottom-sheet-up");
            targetUI.OnToggle?.Invoke();
        }
    }
}
