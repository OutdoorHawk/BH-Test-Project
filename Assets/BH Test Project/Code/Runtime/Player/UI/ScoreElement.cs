using BH_Test_Project.Code.Infrastructure.Data;
using UnityEngine;
using UnityEngine.UI;

namespace BH_Test_Project.Code.Runtime.Player.UI
{
    public class ScoreElement : MonoBehaviour
    {
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _scoreText;
        public int NetId { get; private set; }

        public bool Active { get; private set; }

        public void ActivateElement()
        {
            Active = true;
            _nameText.name = PlayerPrefs.GetString(Constants.PLAYER_NAME);
            _scoreText.text = "0";
            gameObject.SetActive(true);
        }

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
            NetId = msgNetId;
        }

    }
}