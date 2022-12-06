using UnityEngine;
using UnityEngine.UI;

namespace MirrorServiceTest.Code.Runtime.Player.UI
{
    public class ScoreElement : MonoBehaviour
    {
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _scoreText;

        public bool Active { get; private set; }

        public void ActivateElement(string playerName, int score)
        {
            Active = true;
            _nameText.text = playerName;
            _scoreText.text = score.ToString();
            gameObject.SetActive(true);
        }

        public void DeactivateElement()
        {
            Active = false;
            gameObject.SetActive(false);
        }
    }
}