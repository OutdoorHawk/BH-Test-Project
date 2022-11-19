using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Player
{
    public class ColorChanger
    {
        private readonly Color32 _playerHitColor;
        private readonly Renderer[] _renderers;
        private readonly Color32[] _defaultColors;

        public ColorChanger(MonoBehaviour mono, Color32 playerHitColor)
        {
            _renderers = mono.GetComponentsInChildren<Renderer>();
            _playerHitColor = playerHitColor;
            _defaultColors = new Color32[_renderers.Length];
            CollectDefaultPlayerColors();
        }

        private void CollectDefaultPlayerColors()
        {
            for (int i = 0; i < _renderers.Length; i++) 
                _defaultColors[i] = _renderers[i].material.color;
        }

        public void SetPlayerHitColor()
        {
            for (int i = 0; i < _renderers.Length; i++) 
                _renderers[i].material.color = _playerHitColor;
        }

        public void SetPlayerDefaultColor()
        {
            for (int i = 0; i < _renderers.Length; i++) 
                _renderers[i].material.color = _defaultColors[i];
        }
    }
}