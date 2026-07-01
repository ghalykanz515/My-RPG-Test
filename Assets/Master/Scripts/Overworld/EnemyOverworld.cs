using System.Collections.Generic;
using UnityEngine;

using RPGTest.Architecture;
using RPGTest.Core.Managers;
using RPGTest.Data;

namespace RPGTest.Overworld 
{
    public class EnemyOverworld : MonoBehaviour
    {
        [Header("Enemy ID")]
        public string uniqueEnemyID;

        [Header("Enemy Troops Data")]
        public List<CharacterData> enemyTroops = new List<CharacterData>();

        private void Awake()
        {
            if (ServiceLocator.Current.Contains<GameManager>())
            {
                GameManager gm = ServiceLocator.Current.Get<GameManager>();

                if (gm.defeatedEnemies.Contains(uniqueEnemyID))
                {
                    gameObject.SetActive(false); 
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (ServiceLocator.Current.Contains<GameManager>())
                {
                    GameManager gm = ServiceLocator.Current.Get<GameManager>();

                    if (gm.isBattleCooldown) return;

                    if (!string.IsNullOrEmpty(uniqueEnemyID))
                    {
                        gm.defeatedEnemies.Add(uniqueEnemyID);
                    }

                    gm.EnterBattle(enemyTroops, other.transform.position);
                }
            }
        }
    }
}