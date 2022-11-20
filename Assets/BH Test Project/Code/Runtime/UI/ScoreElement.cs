using UnityEngine;
using UnityEngine.UI;

namespace BH_Test_Project.Code.Runtime.UI
{
    public class ScoreElement : MonoBehaviour
    {
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _scoreText;

        private string _playerName;

        public void Init(string playerName, uint playerId)
        {
            _playerName = playerName;
            _nameText.text = $"{playerName}";
            _scoreText.text = "0";
            NetId = playerId;
        }

        public uint NetId { get; private set; }

        public void UpdateScore(int newScore)
        {
            _scoreText.text = $"{newScore}";
        }
    }
}