using System.Collections;
using UnityEngine;

using RPGTest.Architecture;
using RPGTest.Data;
using RPGTest.Interfaces;
using RPGTest.Core.Managers;
using RPGTest.UI.BattleUI;

namespace RPGTest.Battle.States 
{
    public class PlayerTurnState : IBattleState
    {
        private readonly BattleSystem system;
        private readonly BattleUnit playerUnit;
        private bool actionExecuted = false;

        public PlayerTurnState(BattleSystem system, BattleUnit playerUnit)
        {
            this.system = system;
            this.playerUnit = playerUnit;
        }

        public IEnumerator Enter()
        {
            actionExecuted = false;

            if (playerUnit.actionMenu != null)
            {
                playerUnit.actionMenu.ShowMenu(playerUnit.currentMP, playerUnit.specialCost);
            }
            yield return null;
        }

        public void CreateTargetButtons(bool isSpecial)
        {
            if (actionExecuted) return;

            system.IsSpecialSelected = isSpecial;

            foreach (BattleUnit unit in system.AllUnitsInBattle)
            {
                if (!unit.isPlayerTeam && unit.currentHP > 0)
                {
                    GameObject btnObj = Object.Instantiate(system.targetButtonPrefab, system.targetCanvas);
                    TargetButton targetBtn = btnObj.GetComponent<TargetButton>();
                    targetBtn.SetupTarget(unit, system);
                }
            }
        }

        public void SelectTarget(BattleUnit targetEnemy)
        {
            if (actionExecuted) return;

            actionExecuted = true;

            foreach (Transform child in system.targetCanvas)
            {
                Object.Destroy(child.gameObject);
            }

            system.StartCoroutine(ExecuteAttackSequence(targetEnemy));
        }

        private IEnumerator ExecuteAttackSequence(BattleUnit targetEnemy)
        {
            int finalDamage = playerUnit.attackPower;

            if (system.IsSpecialSelected)
            {
                finalDamage *= 3;
                playerUnit.UseMP(playerUnit.specialCost);

                // audio dan vfx
                if (ServiceLocator.Current.Contains<AudioManager>())
                {
                    ServiceLocator.Current.Get<AudioManager>().PlaySFX(playerUnit.myDataRef.specialChargeSFX);
                }

                if (playerUnit.myDataRef.specialChargeVFX != null)
                {
                    Vector3 vfxPos = playerUnit.transform.position + new Vector3(0, -0.8f, 0); 
                    Object.Instantiate(playerUnit.myDataRef.specialChargeVFX, vfxPos, Quaternion.identity);
                }

                yield return new WaitForSeconds(1.0f);
            }

            yield return system.StartCoroutine(system.PerformMeleeAttack(playerUnit, targetEnemy, finalDamage));

            if (!system.IsSpecialSelected)
            {
                playerUnit.GainMP(20);
            }

            system.IncrementTurnIndex();
            system.ProcessNextTurn();
        }

        public IEnumerator Exit()
        {
            yield return null;
        }

        // Munculin tombol target di atas kepala heronya
        public void CreateTargetButtonsForHeroes()
        {
            if (actionExecuted) return;

            foreach (BattleUnit unit in system.AllUnitsInBattle)
            {
                if (unit.isPlayerTeam && unit.currentHP > 0)
                {
                    GameObject btnObj = Object.Instantiate(system.targetButtonPrefab, system.targetCanvas);
                    TargetButton targetBtn = btnObj.GetComponent<TargetButton>();

                    targetBtn.SetupTarget(unit, system); 

                    targetBtn.button.onClick.RemoveAllListeners();
                    targetBtn.button.onClick.AddListener(() => system.OnHeroTargetSelected(unit));
                }
            }
        }

        public void ExecuteItemToTarget(BattleUnit targetHero)
        {
            if (actionExecuted) return;

            actionExecuted = true;

            foreach (Transform child in system.targetCanvas) 
            { 
                Object.Destroy(child.gameObject); 
            }

            system.StartCoroutine(ItemSequence(targetHero));
        }

        private IEnumerator ItemSequence(BattleUnit targetHero)
        {
            ItemData usedItem = system.SelectedItem;

            GameManager gm = ServiceLocator.Current.Get<GameManager>();

            if (gm != null) 
            {
                gm.UseItem(usedItem);
            }

            if (ServiceLocator.Current.Contains<AudioManager>())
            {
                ServiceLocator.Current.Get<AudioManager>().PlaySFX(usedItem.useSFX);
            }
            if (usedItem.useVFX != null)
            {
                Vector3 vfxPos = targetHero.transform.position + new Vector3(0, -0.8f, 0);
                Object.Instantiate(usedItem.useVFX, vfxPos, Quaternion.identity);
            }

            if (playerUnit.animator != null) 
            {
                playerUnit.animator.SetTrigger("Hit"); 
            }

            yield return new WaitForSeconds(0.5f);

            if (usedItem.effectType == ConsumableType.HealHP)
            {
                targetHero.currentHP += usedItem.effectValue;

                if (targetHero.currentHP > targetHero.maxHP) 
                {
                    targetHero.currentHP = targetHero.maxHP;
                }

                targetHero.battleHUD.UpdateHP(targetHero.currentHP);
                targetHero.myDataRef.currentHP = targetHero.currentHP;
            }
            else if (usedItem.effectType == ConsumableType.HealMP)
            {
                targetHero.GainMP(usedItem.effectValue);
            }

            yield return new WaitForSeconds(1.0f);

            system.SelectedItem = null;
            system.IncrementTurnIndex();
            system.ProcessNextTurn();
        }
    }
}