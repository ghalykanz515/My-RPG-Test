using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using RPGTest.Architecture;
using RPGTest.Data;
using RPGTest.Interfaces;
using RPGTest.Core.Managers;
using RPGTest.Battle;

namespace RPGTest.Battle.States 
{
    public class BattleSetupState : IBattleState
    {
        private readonly BattleSystem system;

        public BattleSetupState(BattleSystem system)
        {
            this.system = system;
        }

        public IEnumerator Enter()
        {
            List<CharacterData> heroesToSpawn = system.heroesToSpawnMockup;
            List<CharacterData> enemiesToSpawn = system.enemiesToSpawnMockup;

            GameManager gm = ServiceLocator.Current.Get<GameManager>();
            if (gm != null)
            {
                if (gm.activeParty.Count > 0) 
                {
                    heroesToSpawn = gm.activeParty;
                }

                if (gm.enemiesForBattle.Count > 0) 
                { 
                    enemiesToSpawn = gm.enemiesForBattle;
                }
            }

            for (int i = 0; i < heroesToSpawn.Count; i++)
            {
                if (i >= system.heroStations.Length) break;

                GameObject go = Object.Instantiate(heroesToSpawn[i].battlePrefab, system.heroStations[i].position, Quaternion.identity);
                BattleUnit unit = go.GetComponent<BattleUnit>();

                if (unit == null) continue;

                unit.SetupUnit(heroesToSpawn[i], true, system);
                system.AllUnitsInBattle.Add(unit);
            }

            for (int i = 0; i < enemiesToSpawn.Count; i++)
            {
                if (i >= system.enemyStations.Length) break;

                GameObject go = Object.Instantiate(enemiesToSpawn[i].battlePrefab, system.enemyStations[i].position, Quaternion.identity);
                BattleUnit unit = go.GetComponent<BattleUnit>();

                if (unit == null) continue;

                unit.SetupUnit(enemiesToSpawn[i], false, system);
                system.AllUnitsInBattle.Add(unit);
            }

            system.AllUnitsInBattle = system.AllUnitsInBattle.OrderByDescending(u => u.speed).ToList();

            yield return new WaitForSeconds(1.5f);

            system.ProcessNextTurn();
        }

        public IEnumerator Exit()
        {
            yield return null;
        }
    }
}