using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Player
{
    public class ColorChangerComponent : NetworkBehaviour
    {
        private readonly SyncList<Color32> _currentColors = new();
        private readonly List<Material> _cachedMaterials = new();
        [SyncVar, SerializeField] private Color32 _playerHitColor;
        private Color32[] _defaultColors;

        public override void OnStartServer()
        {
            base.OnStartServer();
            InitCachedMaterials();
            CollectDefaultPlayerColors();
            InitDefaultSyncColors();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            InitCachedMaterials();
            CollectDefaultPlayerColors();
            _currentColors.Callback += OnColorUpdated;
        }

        private void InitCachedMaterials()
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
                _cachedMaterials.Add(renderers[i].material);
        }

        private void CollectDefaultPlayerColors()
        {
            _defaultColors = new Color32[_cachedMaterials.Count];
            for (int i = 0; i < _cachedMaterials.Count; i++)
                _defaultColors[i] = _cachedMaterials[i].color;
        }

        private void InitDefaultSyncColors()
        {
            for (var i = 0; i < _cachedMaterials.Count; i++)
                _currentColors.Add(_cachedMaterials[i].color);
        }

        [Command]
        public void CmdSetPlayerHitColor()
        {
            for (int i = 0; i < _currentColors.Count; i++)
                _currentColors[i] = _playerHitColor;
        }

        [Command]
        public void CmdSetPlayerDefaultColor()
        {
            for (int i = 0; i < _currentColors.Count; i++)
                _currentColors[i] = _defaultColors[i];
        }

        private void OnColorUpdated(SyncList<Color32>.Operation op, int itemIndex, Color32 oldItem,
            Color32 newItem)
        {
            switch (op)
            {
                case SyncList<Color32>.Operation.OP_ADD:
                    _cachedMaterials[itemIndex].color = newItem;
                    break;
                case SyncList<Color32>.Operation.OP_SET:
                    _cachedMaterials[itemIndex].color = newItem;
                    break;
            }
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            _currentColors.Callback -= OnColorUpdated;
        }

        private void OnDestroy()
        {
            foreach (var material in _cachedMaterials)
                Destroy(material);
        }
    }
}