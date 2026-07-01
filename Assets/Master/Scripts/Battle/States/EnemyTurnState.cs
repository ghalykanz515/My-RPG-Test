using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPGTest.Architecture;
using RPGTest.Data;
using RPGTest.Interfaces;
using RPGTest.Core.Managers;
using RPGTest.Battle;

namespace RPGTest.Battle.States 
{
    public class EnemyTurnState : IBattleState
    {
        private readonly BattleSystem system;
        private readonly BattleUnit enemyUnit;

        public EnemyTurnState(BattleSystem system, BattleUnit enemyUnit)
        {
            this.system = system;
            this.enemyUnit = enemyUnit;
        }

        public IEnumerator Enter()
        {
            yield return new WaitForSeconds(1.5f);

            List<BattleUnit> aliveHeroes = system.AllUnitsInBattle.FindAll(u => u.isPlayerTeam && u.currentHP > 0);

            if (aliveHeroes.Count > 0)
            {
                int randomIndex = Random.Range(0, aliveHeroes.Count);
                BattleUnit targetHero = aliveHeroes[randomIndex];

                int finalDamage = enemyUnit.attackPower;
                bool enemyUseSpecial = (enemyUnit.currentMP >= enemyUnit.specialCost);

                if (enemyUseSpecial)
                {
                    finalDamage *= 3;
                    enemyUnit.UseMP(enemyUnit.specialCost);
                }
                else
                {
                    Debug.Log($"{enemyUnit.unitName} is attaking {targetHero.unitName}");
                }

                yield return system.StartCoroutine(system.PerformMeleeAttack(enemyUnit, targetHero, finalDamage));

                if (!enemyUseSpecial)
                {
                    enemyUnit.GainMP(20);
                }
            }

            system.IncrementTurnIndex();
            system.ProcessNextTurn();
        }

        public IEnumerator Exit()
        {
            yield return null;
        }
    }
}