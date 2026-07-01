using UnityEngine;
using UnityEngine.UI;
using TMPro;

using RPGTest.Data;
using RPGTest.Battle;

namespace RPGTest.UI.BattleUI 
{
    public class ItemActionButton : MonoBehaviour
    {
        public Image itemIcon;
        public TMP_Text amountText;
        public Button button;

        private ItemData myItem;
        private BattleSystem battleSystem;

        public void Setup(ItemData item, int amount, BattleSystem system)
        {
            myItem = item;
            battleSystem = system;

            itemIcon.sprite = item.itemIcon;
            amountText.text = amount.ToString();

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnItemClicked);
        }

        private void OnItemClicked()
        {
            battleSystem.OnClickItemOption(myItem);
            GetComponentInParent<PlayerActionMenu>().HideMenu();
        }
    }
}