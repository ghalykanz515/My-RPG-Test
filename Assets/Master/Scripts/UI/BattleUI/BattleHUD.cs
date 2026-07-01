using UnityEngine;
using UnityEngine.UI;
using TMPro;

using RPGTest.Data;

namespace RPGTest.UI.BattleUI 
{
    public class BattleHUD : MonoBehaviour
    {
        [Header("UI Components")]
        public TMP_Text nameText;
        public TMP_Text levelText;
        public Slider hpSlider;
        public Slider mpSlider;

        public void SetHUD(CharacterData data)
        {
            nameText.text = data.characterName;
            levelText.text = $"Lvl.{data.level}";

            hpSlider.maxValue = data.maxHP;
            hpSlider.value = data.maxHP;

            mpSlider.maxValue = data.maxMP;
            mpSlider.value = data.maxMP;
        }

        public void UpdateHP(int currentHP)
        {
            hpSlider.value = currentHP;
        }

        public void UpdateMP(int currentMP)
        {
            mpSlider.value = currentMP;
        }
    }
}