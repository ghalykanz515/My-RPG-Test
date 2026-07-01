using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using RPGTest.Architecture;
using RPGTest.Core.Managers;

namespace RPGTest.UI.BattleUI 
{
    public class BattleLoseUI : MonoBehaviour
    {
        public GameObject losePanel;
        public Button restartButton;
        public Button quitButton;

        private void Start()
        {
            if (restartButton != null) 
            {
                restartButton.onClick.AddListener(RestartGame);
            }

            if (quitButton != null) 
            {
                quitButton.onClick.AddListener(QuitToMainMenu);
            }
        }

        public void ShowLosePanel()
        {
            losePanel.SetActive(true);
        }

        private void RestartGame()
        {
            if (ServiceLocator.Current.Contains<GameManager>())
            {
                ServiceLocator.Current.Get<GameManager>().ResetGameState();
            }

            SceneManager.LoadScene("Overworld_Gameplay");
        }

        private void QuitToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}