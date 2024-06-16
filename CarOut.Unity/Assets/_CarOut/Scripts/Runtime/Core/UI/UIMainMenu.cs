using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace CarOut.UI
{
    public class UIMainMenu : MonoBehaviour
    {
        private UIDocument _uiDocument;
        private MainMenuUIPresenter _mainMenuPresenter;
        private VisualElement _mainMenuView;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
        }

        private void Start()
        {
            InitViews();
            InitPresenters();
        }

        private void InitViews()
        {
            _mainMenuView = _uiDocument.rootVisualElement.Q("MainMenu");
        }
        
        private void InitPresenters()
        {
            _mainMenuPresenter = new MainMenuUIPresenter(_mainMenuView);
        }
    }
}
