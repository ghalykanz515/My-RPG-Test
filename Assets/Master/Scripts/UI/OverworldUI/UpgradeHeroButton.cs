using UnityEngine;
using UnityEngine.UI;
using TMPro;

using RPGTest.Architecture;
using RPGTest.Core.Managers;
using RPGTest.Data;

namespace RPGTest.UI.OverworldUI 
{
    public class UpgradeHeroButton : MonoBehaviour
    {
        [Header("UI Ref")]
        public Image heroIcon;
        public TMP_Text heroNameLevelText;
        public TMP_Text costText;
        public Button upgradeButton;
    
        private CharacterData myHero;
        private MerchantUI merchantUI;
        private int upgradeCost;
    
        public void Setup(CharacterData hero, MerchantUI shopUI)
        {
            myHero = hero;
            merchantUI = shopUI;
    
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(OnUpgradeClicked);
    
            UpdateUI();
        }
    
        private void OnUpgradeClicked()
        {
            GameManager gm = ServiceLocator.Current.Get<GameManager>();
            
            if (gm.currentGold >= upgradeCost)
            {
                gm.SpendGold(upgradeCost);
                
                myHero.level++;
                myHero.maxHP += 20;
                myHero.maxMP += 10;
                myHero.attackPower += 5;
                
                myHero.currentHP = myHero.maxHP;
                myHero.currentMP = myHero.maxMP;
                
                UpdateUI();
                
                merchantUI.UpdateGoldDisplay();
            }
            else
            {
                Debug.LogWarning("uangnya gk cukup!");
            }
        }
    
        private void UpdateUI()
        {
            heroIcon.sprite = myHero.characterPortrait;
            heroNameLevelText.text = $"{myHero.characterName} Lvl.{myHero.level}";
            
            upgradeCost = 1000 * myHero.level; 
            costText.text = $"G{upgradeCost}";
        }
    }
}