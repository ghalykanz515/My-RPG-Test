using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPGTest.Architecture;
using RPGTest.Data;
using RPGTest.Interfaces;
using RPGTest.Core.Managers;
using RPGTest.UI.BattleUI;
using RPGTest.Battle.States;

namespace RPGTest.Battle
{
    public class BattleSystem : MonoBehaviour
    {
        [Header("Spawn Stations")]
        public Transform[] heroStations;
        public Transform[] enemyStations;

        [Header("Party Data")]
        public List<CharacterData> heroesToSpawnMockup;
        public List<CharacterData> enemiesToSpawnMockup;

        [Header("UI Targeting (Screen Space)")]
        public GameObject targetButtonPrefab;
        public Transform targetCanvas;

        public List<BattleUnit> AllUnitsInBattle { get; set; } = new List<BattleUnit>();
        public int CurrentTurnIndex { get; set; } = 0;
        public bool IsSpecialSelected { get; set; } = false;
        public ItemData SelectedItem { get; set; } = null;

        [Header("UI for Victory & Losing")]
        public BattleVictoryUI victoryUI;
        public BattleLoseUI loseUI;

        private IBattleState currentState;
        private bool isTransitioning = false;

        private void Start()
        {
            ChangeState(new BattleSetupState(this));
        }

        public void ChangeState(IBattleState newState)
        {
            if (isTransitioning) return;

            StartCoroutine(TransitionToState(newState));
        }

        private IEnumerator TransitionToState(IBattleState newState)
        {
            isTransitioning = true;

            if (currentState != null)
            {
                yield return StartCoroutine(currentState.Exit());
            }

            currentState = newState;

            isTransitioning = false; 

            yield return StartCoroutine(currentState.Enter());
        }

        public void ProcessNextTurn()
        {
            int aliveEnemies = AllUnitsInBattle.FindAll(unit => !unit.isPlayerTeam && unit.currentHP > 0).Count;
            int aliveHeroes = AllUnitsInBattle.FindAll(unit => unit.isPlayerTeam && unit.currentHP > 0).Count;

            if (aliveEnemies <= 0)
            {
                ChangeState(new BattleVictoryState(this));
                return;
            }
            if (aliveHeroes <= 0)
            {
                ChangeState(new BattleLoseState(this));
                return;
            }

            BattleUnit nextUnit = AllUnitsInBattle[CurrentTurnIndex];

            while (nextUnit.currentHP <= 0)
            {
                IncrementTurnIndex();
                nextUnit = AllUnitsInBattle[CurrentTurnIndex];
            }

            if (nextUnit.isPlayerTeam)
            {
                ChangeState(new PlayerTurnState(this, nextUnit));
            }
            else
            {
                ChangeState(new EnemyTurnState(this, nextUnit));
            }
        }

        public void IncrementTurnIndex()
        {
            CurrentTurnIndex++;
            if (CurrentTurnIndex >= AllUnitsInBattle.Count)
            {
                CurrentTurnIndex = 0;
            }
        }

        public IEnumerator PerformMeleeAttack(BattleUnit attacker, BattleUnit target, int damageValue)
        {
            float offsetDir = attacker.isPlayerTeam ? -1.5f : 1.5f; 
            Vector3 attackPosition = target.transform.position + new Vector3(offsetDir, 0, 0);
            Vector3 originalPosition = attacker.startPosition;
            float moveSpeed = 15f;

            while (Vector3.Distance(attacker.transform.position, attackPosition) > 0.01f)
            {
                attacker.transform.position = Vector3.MoveTowards(attacker.transform.position, attackPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            if (attacker.animator != null) attacker.animator.SetBool("IsAttacking", true); 
            yield return new WaitForSeconds(0.4f); 

            // buat nambah uadio attask
            if (ServiceLocator.Current.Contains<AudioManager>() && attacker.myDataRef.attackSFX != null)
            {
                ServiceLocator.Current.Get<AudioManager>().PlaySFX(attacker.myDataRef.attackSFX);
            }

            target.TakeDamage(damageValue);

            if (attacker.animator != null) 
            {
                attacker.animator.SetBool("IsAttacking", false);
            }

            yield return new WaitForSeconds(0.2f);

            while (Vector3.Distance(attacker.transform.position, originalPosition) > 0.01f)
            {
                attacker.transform.position = Vector3.MoveTowards(attacker.transform.position, originalPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            attacker.transform.position = originalPosition; 

            yield return new WaitForSeconds(0.3f); 
        }

        public void OnClickAttackOption(bool isSpecial)
        {
            if (currentState is PlayerTurnState playerTurn)
            {
                playerTurn.CreateTargetButtons(isSpecial);
            }
        }

        public void OnTargetSelected(BattleUnit targetEnemy)
        {
            if (currentState is PlayerTurnState playerTurn)
            {
                playerTurn.SelectTarget(targetEnemy);
            }
        }

        public void OnClickItemOption(ItemData item)
        {
            SelectedItem = item;
            if (currentState is PlayerTurnState playerTurn)
            {
                playerTurn.CreateTargetButtonsForHeroes();
            }
        }

        public void OnHeroTargetSelected(BattleUnit targetHero)
        {
            if (currentState is PlayerTurnState playerTurn)
            {
                playerTurn.ExecuteItemToTarget(targetHero);
            }
        }
    }
}