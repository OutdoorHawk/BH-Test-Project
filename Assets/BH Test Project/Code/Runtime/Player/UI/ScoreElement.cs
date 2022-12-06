using UnityEngine;
using UnityEngine.UI;

namespace BH_Test_Project.Code.Runtime.Player.UI
{
    public class ScoreElement : MonoBehaviour
    {
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _scoreText;

        public bool Active { get; private set; }

        public void ActivateElement(string playerName)
        {
            Active = true;
            _nameText.text = playerName;
            _scoreText.text = "0";
            gameObject.SetActive(true);
        }

        public void DeactivateElement()
        {
            Active = false;
            gameObject.SetActive(false);
        }

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