using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fungus;

using RPGTest.Architecture;
using RPGTest.Core.Managers;
using RPGTest.Core.Events;
using RPGTest.Data;

namespace RPGTest.UI.OverworldUI 
{
    public class MerchantUI : MonoBehaviour
    {
        public static bool IsShopOpen = false;
    
        [Header("Main Panel")]
        public GameObject shopPanel;
        public TMP_Text playerGoldText;
    
        [Header("Button Item")]
        public Button buyHealthPotionBtn;
        public Button buyManaPotionBtn;
        public Button healPartyBtn;
        public Button exitBtn;
    
        [Header("Item Data")]
        public ItemData hpPotionData;
        public ItemData mpPotionData;
        private int healPartyCost = 300;
    
        [Header("Item Counts")]
        public TMP_Text hpPotionOwnedText;
        public TMP_Text mpPotionOwnedText;
    
        [Header("Upgrade Parties")]
        public GameObject upgradeButtonPrefab;
        public Transform upgradeContainer;
    
        private void Start()
        {
            buyHealthPotionBtn.onClick.AddListener(BuyHealthPotion);
            buyManaPotionBtn.onClick.AddListener(BuyManaPotion);
            healPartyBtn.onClick.AddListener(HealParty);
            exitBtn.onClick.AddListener(CloseShop);
        }
    
        public void OpenShop()
        {
            shopPanel.SetActive(true);
            IsShopOpen = true;
            UpdateGoldDisplay();
            UpdateItemOwnedDisplay();
            RefreshUpgradeList();
    
            if (SayDialog.ActiveSayDialog != null)
            {
                SayDialog.ActiveSayDialog.gameObject.SetActive(false);
            }
    
            if (MenuDialog.ActiveMenuDialog != null)
            {
                MenuDialog.ActiveMenuDialog.gameObject.SetActive(false);
            }
    
            shopPanel.SetActive(true);
            IsShopOpen = true;
            
            UpdateGoldDisplay();
            UpdateItemOwnedDisplay(); 
            RefreshUpgradeList();
        }
    
        public void CloseShop()
        {
            shopPanel.SetActive(false);
            IsShopOpen = false;
    
            Flowchart.BroadcastFungusMessage("TokoTutup");
        }
    
        public void UpdateGoldDisplay()
        {
            if (playerGoldText != null && ServiceLocator.Current.Contains<GameManager>())
            {
                playerGoldText.text = $"G{ServiceLocator.Current.Get<GameManager>().currentGold}";
            }
        }
    
        private void UpdateItemOwnedDisplay()
        {
            if (!ServiceLocator.Current.Contains<GameManager>()) return;
    
            GameManager gm = ServiceLocator.Current.Get<GameManager>();
    
            if (hpPotionOwnedText != null)
            {
                int hpCount = GetItemAmount(gm, hpPotionData);
                hpPotionOwnedText.text = $"Owned: {hpCount}";
            }
    
            if (mpPotionOwnedText != null)
            {
                int mpCount = GetItemAmount(gm, mpPotionData);
                mpPotionOwnedText.text = $"Owned: {mpCount}";
            }
        }
    
        private int GetItemAmount(GameManager gm, ItemData targetItem)
        {
            InventorySlot slot = gm.playerInventory.Find(s => s.item == targetItem);
    
            return slot != null ? slot.amount : 0;
        }
    
        private void RefreshUpgradeList()
        {
            foreach (Transform child in upgradeContainer) 
            { 
                Destroy(child.gameObject);
            }
    
            GameManager gm = ServiceLocator.Current.Get<GameManager>();
    
            if (gm == null) return;
    
            foreach (CharacterData hero in gm.activeParty)
            {
                GameObject btnObj = Instantiate(upgradeButtonPrefab, upgradeContainer);
                UpgradeHeroButton upgradeScript = btnObj.GetComponent<UpgradeHeroButton>();
                
                if (upgradeScript != null)
                {
                    upgradeScript.Setup(hero, this);
                }
            }
        }
    
        private void BuyHealthPotion()
        {
            GameManager gm = ServiceLocator.Current.Get<GameManager>();
    
            if (gm.currentGold >= hpPotionData.price)
            {
                gm.SpendGold(hpPotionData.price);
                gm.AddItem(hpPotionData, 1);
                Debug.Log("[Shop] Membeli Health Potion");
                UpdateGoldDisplay();
                UpdateItemOwnedDisplay();
            }
        }
    
        private void BuyManaPotion()
        {
            GameManager gm = ServiceLocator.Current.Get<GameManager>();
    
            if (gm.currentGold >= mpPotionData.price)
            {
                gm.SpendGold(mpPotionData.price);
                gm.AddItem(mpPotionData, 1);
                Debug.Log("[Shop] Membeli Mana Potion");
                UpdateGoldDisplay();
                UpdateItemOwnedDisplay();
            }
        }
    
        private void HealParty()
        {
            GameManager gm = ServiceLocator.Current.Get<GameManager>();
    
            if (gm.currentGold >= healPartyCost)
            {
                gm.SpendGold(healPartyCost);
                
                foreach (CharacterData hero in gm.activeParty)
                {
                    hero.currentHP = hero.maxHP;
                    hero.currentMP = hero.maxMP;
                }
                
                EventBus<PartyUpdatedEvent>.Publish(new PartyUpdatedEvent());
                
                Debug.Log("Harusnya HP dan MP langsung full!!!");
    
                UpdateGoldDisplay();
            }
        }
    }
}