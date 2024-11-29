using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneWords.Scripts.Game.GameRoot.UI
{
    public class UIRootView : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingScreen;
        [SerializeField] private Transform _uiLayerHUD;
        [SerializeField] private Transform _uiLayerFXUnderPopup;
        [SerializeField] private Transform _uiLayerPopup;
        [SerializeField] private Transform _uiLayerFXOverPopup;

        private List<Transform> _uiLayers;

        private void Awake()
        {
            HideLoadingScreen();
            _uiLayers = new List<Transform>
            {
                _uiLayerHUD,
                _uiLayerFXUnderPopup,
                _uiLayerPopup,
                _uiLayerFXOverPopup
            };
        }

        public void ShowLoadingScreen()
        {
            _loadingScreen.SetActive(true);
        }

        public void HideLoadingScreen()
        {
            _loadingScreen.SetActive(false);
        }

        public void AttachSceneUI(GameObject uiObject, UILayer layer)
        {
            var parent = GetLayerTransform(layer);
            if (parent == null)
                throw new Exception("UI attachment failed: the specified parent layer is null or invalid.");

            uiObject.transform.SetParent(parent, false);
        }

        public void ClearSceneUI()
        {
            foreach (var layer in _uiLayers)
            foreach (Transform child in layer)
                Destroy(child.gameObject);
        }

        private Transform GetLayerTransform(UILayer layer)
        {
            return layer switch
            {
                UILayer.HUD => _uiLayerHUD,
                UILayer.FXUnderPopup => _uiLayerFXUnderPopup,
                UILayer.Popup => _uiLayerPopup,
                UILayer.FXOverPopup => _uiLayerFXOverPopup,
                _ => null
            };
        }
    }

    public enum UILayer
    {
        HUD,
        FXUnderPopup,
        Popup,
        FXOverPopup
    }
}