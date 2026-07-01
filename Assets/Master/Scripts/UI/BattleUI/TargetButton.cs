using UnityEngine;
using UnityEngine.UI;
using TMPro;

using RPGTest.Battle;

namespace RPGTest.UI.BattleUI 
{
    public class TargetButton : MonoBehaviour
    {
        public TMP_Text nameText;
        public Button button;

        private BattleUnit targetUnit;
        private Camera mainCamera;

        public Vector3 offset = new Vector3(0, -1f, 0); 

        public void SetupTarget(BattleUnit unit, BattleSystem battleSystem)
        {
            targetUnit = unit;
            mainCamera = Camera.main;

            nameText.text = unit.unitName;

            button.onClick.AddListener(() => battleSystem.OnTargetSelected(targetUnit));
        }

        private void Update()
        {
            if (targetUnit != null)
            {
                Vector3 screenPos = mainCamera.WorldToScreenPoint(targetUnit.transform.position + offset);
                transform.position = screenPos;
            }
        }
    }
}