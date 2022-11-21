using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace BH_Test_Project.Code.Runtime.UI
{
    public class ScoreElement : MonoBehaviour
    {
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _scoreText;
        
        public void SetName(string playerName)
        {
            _nameText.text = playerName;
        }

        public void SetScore(int score)
        {
            _scoreText.text = score.ToString();
        }
    }
}