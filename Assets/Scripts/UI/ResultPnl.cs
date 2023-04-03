using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class ResultPnl : MonoBehaviour
    {
        [SerializeField] private TMP_Text resultText;

        public void ShowResult(ResultType type)
        {
            string content = "";
            switch (type)
            {
                case ResultType.PlayerWin:
                    content = "Player Win";
                    break;
                case ResultType.AIWin:
                    content = "AI Win";
                    break;
                default:
                    content = "Draw";
                    break;
            }

            resultText.SetText(content);
        }

        public void LoadMenu()
        {
            this.gameObject.SetActive(false);
            SceneManager.LoadScene("MenuScene");
        }
    }
}