using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using RPGTest.Architecture;
using RPGTest.Data;
using RPGTest.Core.Events;

namespace RPGTest.Core.Managers 
{
    [Serializable]
    public class InventorySlot
    {
        public ItemData item;
        public int amount;
    }

    public class GameManager : MonoBehaviour
    {
        [Header("Active Party Data")]
        public List<CharacterData> activeParty = new List<CharacterData>();

        [Header("Enemies Data")]
        public List<CharacterData> enemiesForBattle = new List<CharacterData>();

        [Header("Last Data")]
        public List<string> defeatedEnemies = new List<string>();
        public List<string> playedCutscenes = new List<string>();

        [Header("Exploration Position Storage")]
        public string overworldSceneName = "Overworld_Gameplay";
        public string battleSceneName = "Battle_Gameplay";
        public Vector3 lastOverworldPosition;          
        public bool isBattleCooldown = false;

        [Header("Economy & Inventory")]
        public int currentGold = 500;
        public List<InventorySlot> playerInventory = new List<InventorySlot>();

        private void Awake()
        {
            if (!ServiceLocator.Current.Contains<GameManager>())
            {
                ServiceLocator.Current.Register<GameManager>(this);
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void OnEnable() 
        { 
            EventBus<PartyJoinedEvent>.Subscribe(OnHeroRecruited);
        }

        private void OnDisable() 
        {
            EventBus<PartyJoinedEvent>.Unsubscribe(OnHeroRecruited);
        }

        private void OnHeroRecruited(PartyJoinedEvent eventData)
        {
            if (!activeParty.Contains(eventData.Character))
            {
                activeParty.Add(eventData.Character);
                Debug.Log($"{eventData.Character.characterName} recruited!");
            }
        }

        public void EnterBattle(List<CharacterData> encounterEnemies, Vector3 playerPos)
        {
            lastOverworldPosition = playerPos;
            enemiesForBattle = encounterEnemies;
            SceneManager.LoadScene(battleSceneName);
        }

        public void ReturnToOverworld()
        {
            SceneManager.LoadScene(overworldSceneName);
            StartCoroutine(BattleCooldownRoutine());
        }

        private IEnumerator BattleCooldownRoutine()
        {
            isBattleCooldown = true;
            yield return new WaitForSeconds(1.0f);
            isBattleCooldown = false;
        }

        public void AddGold(int amount)
        {
            currentGold += amount;
            EventBus<GoldChangedEvent>.Publish(new GoldChangedEvent { NewGold = currentGold });
        }

        public void SpendGold(int amount)
        {
            if (currentGold >= amount) 
            {
                currentGold -= amount;
                EventBus<GoldChangedEvent>.Publish(new GoldChangedEvent { NewGold = currentGold });
            }
        }

        public void AddItem(ItemData newItem, int amount = 1)
        {
            InventorySlot existingSlot = playerInventory.Find(slot => slot.item == newItem);

            if (existingSlot != null)
            {
                existingSlot.amount += amount;
            }
            else
            {
                playerInventory.Add(new InventorySlot { item = newItem, amount = amount });
            }
        }

        public void UseItem(ItemData itemToUse)
        {
            InventorySlot slot = playerInventory.Find(s => s.item == itemToUse);

            if (slot != null && slot.amount > 0)
            {
                slot.amount--;
                if (slot.amount <= 0) playerInventory.Remove(slot);
            }
        }

        public void ResetGameState()
        {
            currentGold = 500;

            playerInventory.Clear();
            defeatedEnemies.Clear();
            playedCutscenes.Clear();

            foreach (CharacterData hero in activeParty)
            {
                hero.currentHP = hero.maxHP;
                hero.currentMP = hero.maxMP;
            }
        }
    }
}