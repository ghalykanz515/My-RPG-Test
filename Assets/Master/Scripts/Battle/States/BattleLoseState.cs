using System.Collections;
using UnityEngine;

using RPGTest.Architecture;
using RPGTest.Data;
using RPGTest.Interfaces;
using RPGTest.Core.Managers;
using RPGTest.Battle;

namespace RPGTest.Battle.States 
{
    public class BattleLoseState : IBattleState
    {
        private readonly BattleSystem system;

        public BattleLoseState(BattleSystem system)
        {
            this.system = system;
        }

        public IEnumerator Enter()
        {
            if (system.loseUI != null)
            {
                system.loseUI.ShowLosePanel();
            }

            yield return null;
        }

        public IEnumerator Exit()
        {
            yield return null;
        }
    }
}