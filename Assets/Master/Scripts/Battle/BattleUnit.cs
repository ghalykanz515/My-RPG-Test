using UnityEngine;

using RPGTest.Architecture;
using RPGTest.Data;
using RPGTest.Interfaces;
using RPGTest.Core.Managers;
using RPGTest.UI.BattleUI;
using RPGTest.Battle.States;

namespace RPGTest.Battle 
{
    public class BattleUnit : MonoBehaviour
    {
        [Header("Player Unit")]
        public string unitName;
        public bool isPlayerTeam;

        [Header("Current Status")]
        public int currentHP;
        public int currentMP;
        public int maxHP;
        public int maxMP;
        public int attackPower;
        public int speed; 
        public int specialCost;

        [Header("UI Ref")]
        public BattleHUD battleHUD; 
        public PlayerActionMenu actionMenu;

        [Header("Animation & Movement")]
        public Animator animator;      
        public Vector3 startPosition;  

        public CharacterData myDataRef; 

        public void SetupUnit(CharacterData data, bool isPlayer, BattleSystem manager)
        {
            myDataRef = data; 
            unitName = data.characterName;
            maxHP = data.maxHP;
            maxMP = data.maxMP;
            attackPower = data.attackPower;
            speed = data.speed; 
            specialCost = data.specialCost; 
            isPlayerTeam = isPlayer;

            if (data.currentHP <= 0 && data.maxHP > 0 && isPlayerTeam)
            {
                data.currentHP = data.maxHP;
                data.currentMP = 0;
            }

            currentHP = isPlayerTeam ? data.currentHP : data.maxHP; 
            currentMP = isPlayerTeam ? data.currentMP : 0;          

            startPosition = transform.position;

            if (animator == null) 
            {
                animator = GetComponent<Animator>();
            }

            if (battleHUD != null)
            {
                battleHUD.SetHUD(data);
                battleHUD.UpdateHP(currentHP); 
                battleHUD.UpdateMP(currentMP); 
            }

            if (isPlayerTeam && actionMenu != null) 
            {
                actionMenu.SetupMenu(manager);
            }
        }

        public bool TakeDamage(int dmg)
        {
            currentHP -= dmg;

            if (currentHP < 0) 
            {
                currentHP = 0;
            }

            if (isPlayerTeam && myDataRef != null) 
            {
                myDataRef.currentHP = currentHP; 
            }

            if (battleHUD != null) 
            {
                battleHUD.UpdateHP(currentHP);
            }

            if (currentHP <= 0)
            {
                if (animator != null) animator.SetBool("IsDead", true); 
            }
            else 
            {
                if (animator != null) animator.SetTrigger("Hit"); 
            }

            return currentHP <= 0; 
        }

        public void GainMP(int amount)
        {
            currentMP += amount;

            if (currentMP > maxMP) 
            {
                currentMP = maxMP;
            }

            if (isPlayerTeam && myDataRef != null) 
            {
                myDataRef.currentMP = currentMP;
            } 

            if (battleHUD != null) 
            {
                battleHUD.UpdateMP(currentMP);
            }
        }

        public void UseMP(int amount)
        {
            currentMP -= amount;

            if (currentMP < 0) 
            {
                currentMP = 0;
            }

            if (isPlayerTeam && myDataRef != null) 
            {
                myDataRef.currentMP = currentMP; 
            }

            if (battleHUD != null) 
            {
                battleHUD.UpdateMP(currentMP);
            }
        }
    }
}