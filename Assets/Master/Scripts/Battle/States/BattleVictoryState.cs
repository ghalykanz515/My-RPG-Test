using System.Collections;
using UnityEngine;

using RPGTest.Architecture;
using RPGTest.Data;
using RPGTest.Interfaces;
using RPGTest.Core.Managers;
using RPGTest.Battle;

namespace RPGTest.Battle.States 
{
    public class BattleVictoryState : IBattleState
    {
        private readonly BattleSystem system;

        public BattleVictoryState(BattleSystem system)
        {
            this.system = system;
        }

        public IEnumerator Enter()
        {
            yield return new WaitForSeconds(1.5f); 

            int totalGoldReward = 0;

            foreach (BattleUnit unit in system.AllUnitsInBattle)
            {
                if (!unit.isPlayerTeam && unit.myDataRef != null)
                {
                    totalGoldReward += unit.myDataRef.rewardGold;
                }
            }

            GameManager gm = ServiceLocator.Current.Get<GameManager>();
            if (gm != null)
            {
                gm.AddGold(totalGoldReward);
            }

            if (system.victoryUI != null)
            {
                system.victoryUI.ShowVictory(totalGoldReward);
            }
            else
            {
                if (gm != null) gm.ReturnToOverworld();
            }
        }

        public IEnumerator Exit()
        {
            yield return null;
        }
    }
}