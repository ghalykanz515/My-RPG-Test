using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using RPGTest.Architecture;
using RPGTest.Core.Managers;

namespace Vielpath.UI.Menu
{
    public class PauseMenuController : MonoBehaviour
    {
        public GameObject pausePanel;
        public Button resumeButton;
        public Button restartButton;
        public Button quitButton;

        private PlayerControls playerControls;
        private bool isPaused = false;

        private void Awake()
        {
            playerControls = new PlayerControls();
            playerControls.Gameplay.Pause.performed += _ => TogglePause();
        }

        private void Start()
        {
            if (resumeButton != null) 
            {
                resumeButton.onClick.AddListener(ResumeGame);
            }

            if (restartButton != null) 
            {
                restartButton.onClick.AddListener(RestartGame);
            }

            if (quitButton != null) 
            {
                quitButton.onClick.AddListener(QuitToMainMenu);
            }
        }

        private void OnEnable() 
        {
            playerControls.Gameplay.Enable();
        }

        private void OnDisable() 
        { 
            playerControls.Gameplay.Disable();
        }

        private void TogglePause()
        {
            Debug.Log("Harusnya Pause");

            if (isPaused) 
            {
                ResumeGame();
            }
            else 
            {
                PauseGame();
            }
        }

        private void PauseGame()
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }

        private void ResumeGame()
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }

        private void RestartGame()
        {
            Time.timeScale = 1f;

            if (ServiceLocator.Current.Contains<GameManager>())
            {
                ServiceLocator.Current.Get<GameManager>().ResetGameState();
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void QuitToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
    }
}