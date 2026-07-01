using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

using RPGTest.Architecture;
using RPGTest.Core.Managers;
using RPGTest.Battle;

namespace RPGTest.UI.BattleUI 
{
    public class PlayerActionMenu : MonoBehaviour
    {
        [Header("UI Ref")]
        [SerializeField] private RectTransform menuRect; 

        [Header("Action Buttons")]
        [SerializeField] private Button attack1Button;
        [SerializeField] private Button attack2Button;
        [SerializeField] private Button specialButton;

        [Header("Animation Settings")]
        [SerializeField] private float shownPositionY = -672.6f;
        [SerializeField] private float hiddenPositionY = -1831.8f;
        [SerializeField] private float animationDuration = 0.25f;

        [Header("Item Spawner")]
        [SerializeField] private GameObject itemButtonPrefab;
        [SerializeField] private Transform itemContainer;

        private BattleSystem battleSystem; 
        private bool isExpanded = false;
        private Coroutine currentAnimation;

        private void Awake()
        {
            menuRect.anchoredPosition = new Vector2(menuRect.anchoredPosition.x, hiddenPositionY);
        }

        public void SetupMenu(BattleSystem manager)
        {
            battleSystem = manager;
        }

        private void Start()
        {
            if (attack1Button != null)
            {
                attack1Button.onClick.RemoveAllListeners(); 
                attack1Button.onClick.AddListener(OnAttack1Clicked);
            }

            if (specialButton != null)
            {
                specialButton.onClick.RemoveAllListeners();
                specialButton.onClick.AddListener(OnSpecialClicked);
            }
        }

        private void OnAttack1Clicked()
        {
            if (!isExpanded) return; 
            HideMenu();

            if (battleSystem != null) battleSystem.OnClickAttackOption(false);
        }

        private void OnSpecialClicked()
        {
            if (!isExpanded) return;
            HideMenu();

            if (battleSystem != null) battleSystem.OnClickAttackOption(true);
        }

        public void ShowMenu(int currentMP, int specialCost)
        {
            if (isExpanded) return;

            if (specialButton != null)
            {
                specialButton.interactable = (currentMP >= specialCost);
            }

            if (itemContainer != null && itemButtonPrefab != null)
            {
                foreach (Transform child in itemContainer) 
                {
                    Destroy(child.gameObject);
                }

                GameManager gm = ServiceLocator.Current.Get<GameManager>();

                if (gm != null)
                {
                    foreach (InventorySlot slot in gm.playerInventory)
                    {
                        if (slot.amount > 0)
                        {
                            GameObject btnObj = Instantiate(itemButtonPrefab, itemContainer);
                            ItemActionButton itemBtn = btnObj.GetComponent<ItemActionButton>();
                            if (itemBtn != null) itemBtn.Setup(slot.item, slot.amount, battleSystem);
                        }
                    }
                }
            }

            isExpanded = true;

            AnimateTo(shownPositionY);
        }

        public void HideMenu()
        {
            if (!isExpanded) return;

            isExpanded = false;
            AnimateTo(hiddenPositionY);
        }

        private void AnimateTo(float targetY)
        {
            if (currentAnimation != null) 
            {
                StopCoroutine(currentAnimation);
            }

            currentAnimation = StartCoroutine(AnimateMenuCoroutine(targetY));
        }

        private IEnumerator AnimateMenuCoroutine(float targetY)
        {
            float time = 0;
            Vector2 startPos = menuRect.anchoredPosition;
            Vector2 targetPos = new Vector2(startPos.x, targetY);

            while (time < animationDuration)
            {
                time += Time.deltaTime;
                float t = Mathf.SmoothStep(0, 1, time / animationDuration); 
                menuRect.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
                yield return null;
            }

            menuRect.anchoredPosition = targetPos;
        }
    }
}