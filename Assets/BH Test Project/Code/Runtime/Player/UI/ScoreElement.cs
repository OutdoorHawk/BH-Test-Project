using UnityEngine;
using UnityEngine.UI;

namespace BH_Test_Project.Code.Runtime.Player.UI
{
    public class ScoreElement : MonoBehaviour
    {
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _scoreText;
        private int _msgNetId;

        public void SetName(string playerName)
        {
            _nameText.text = playerName;
        }

        public void SetScore(int score)
        {
            _scoreText.text = score.ToString();
        }

        public void SetNetId(int msgNetId)
        {
            _msgNetId = msgNetId;
        }

        public int GetNetId()
        {
            return _msgNetId;
        }
        
    }
}