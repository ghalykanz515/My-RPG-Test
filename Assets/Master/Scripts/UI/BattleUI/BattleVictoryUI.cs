using UnityEngine;
using TMPro;
using UnityEngine.UI;

using RPGTest.Architecture;
using RPGTest.Core.Managers;

namespace RPGTest.UI.BattleUI
{
    public class BattleVictoryUI : MonoBehaviour
    {
        [Header("UI Components")]
        public GameObject victoryPanel;
        public TMP_Text titleText;
        public TMP_Text rewardText;
        public Button continueButton;

        public void ShowVictory(int totalGold)
        {
            victoryPanel.SetActive(true);
            titleText.text = "BATTLE COMPLETE!";
            rewardText.text = $"<color=#FFD700>+ {totalGold} Gold</color>";

            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(ReturnToOverworld);
        }

        private void ReturnToOverworld()
        {
            if (ServiceLocator.Current.Contains<GameManager>())
            {
                ServiceLocator.Current.Get<GameManager>().ReturnToOverworld();
            }
        }
    }
}