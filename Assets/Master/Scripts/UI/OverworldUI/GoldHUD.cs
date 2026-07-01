using UnityEngine;
using TMPro;

using RPGTest.Architecture;
using RPGTest.Core.Managers;
using RPGTest.Core.Events;

namespace RPGTest.UI.OverworldUI
{
    public class GoldHUD : MonoBehaviour
    {
        public TMP_Text goldText;

        private void Start()
        {
            if (ServiceLocator.Current.Contains<GameManager>())
            {
                UpdateGoldDisplay(new GoldChangedEvent { NewGold = ServiceLocator.Current.Get<GameManager>().currentGold });
            }
        }

        private void OnEnable() 
        {
            EventBus<GoldChangedEvent>.Subscribe(UpdateGoldDisplay);
        }

        private void OnDisable() 
        { 
            EventBus<GoldChangedEvent>.Unsubscribe(UpdateGoldDisplay);
        }

        private void UpdateGoldDisplay(GoldChangedEvent eventData)
        {
            if (goldText != null) 
            {
                goldText.text = eventData.NewGold.ToString();
            }
        }
    }
}