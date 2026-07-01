using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using RPGTest.Architecture;
using RPGTest.Core.Managers;

namespace Vielpath.UI.Menu
{
    public class MainMenuController : MonoBehaviour
    {
        public Button startButton;
        public Button quitButton;

        private void Start()
        {
            if (startButton != null) 
            {
                startButton.onClick.AddListener(StartGame);
            }

            if (quitButton != null) 
            {
                quitButton.onClick.AddListener(QuitGame);
            }
        }

        private void StartGame()
        {
            if (ServiceLocator.Current.Contains<GameManager>())
            {
                ServiceLocator.Current.Get<GameManager>().ResetGameState();
            }

            SceneManager.LoadScene("Overworld_Gameplay");
        }

        private void QuitGame()
        {
            Application.Quit();
        }
    }
}