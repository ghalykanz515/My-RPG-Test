using UnityEngine;
using UnityEngine.UI;
using TMPro;

using RPGTest.Data;

namespace RPGTest.UI.OverworldUI 
{
    public class PartyMemberUIElement : MonoBehaviour
    {
        [Header("UI Ref")]
        public TMP_Text nameText;
        public TMP_Text levelText;
    
        [Space]
        [Header("Character Image")]
        public Image avatarImage;
    
        [Space]
        [Header("Sliders")]
        public Slider hpSlider;
        public Slider mpSlider;
    
        public void SetupUI(CharacterData data)
        {
            nameText.text = data.characterName;
            levelText.text = $"Lvl.{data.level}";
            avatarImage.sprite = data.characterPortrait;
    
            if (avatarImage != null)
            {
                avatarImage.rectTransform.anchoredPosition = new Vector2(data.rectPosX, data.rectPosY);
                avatarImage.rectTransform.sizeDelta = new Vector2(data.rectWidth, data.rectHeight);
            }
            
            hpSlider.maxValue = data.maxHP;
            hpSlider.value = data.currentHP;
    
            mpSlider.maxValue = data.maxMP;
            mpSlider.value = data.currentMP;
        }
    }
}