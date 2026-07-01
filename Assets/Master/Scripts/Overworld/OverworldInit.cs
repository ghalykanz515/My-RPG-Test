using UnityEngine;

using RPGTest.Architecture;
using RPGTest.Core.Managers;
using RPGTest.Data;

namespace RPGTest.Overworld 
{
    public class OverworldInit : MonoBehaviour
    {
        private void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player == null) return;

            if (ServiceLocator.Current.Contains<GameManager>())
            {
                GameManager gm = ServiceLocator.Current.Get<GameManager>();

                if (gm.lastOverworldPosition != Vector3.zero)
                {
                    player.transform.position = gm.lastOverworldPosition;
                }

                for (int i = 1; i < gm.activeParty.Count; i++)
                {
                    CharacterData data = gm.activeParty[i];
                    if (data.overworldPrefab != null)
                    {
                        Vector3 spawnPos = player.transform.position;

                        float offsetX = (i % 2 == 0 ? 1f : -1f) * (i * 0.5f); 
                        float offsetZ = -(i * 0.5f); 

                        spawnPos.x += offsetX;
                        spawnPos.z += offsetZ;
                        spawnPos.y += 1.0f;

                        GameObject follower = Instantiate(data.overworldPrefab, spawnPos, Quaternion.identity);
                        follower.name = data.characterName + "_ActivePrefab"; 

                        PartyFollower script = follower.GetComponent<PartyFollower>();

                        if (script != null) 
                        {
                            script.targetToFollow = player.transform; 
                        }
                    }
                }
            }
        }
    }
}