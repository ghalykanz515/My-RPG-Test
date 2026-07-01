using UnityEngine;

namespace RPGTest.Data 
{
    [CreateAssetMenu(fileName = "New Character", menuName = "RPG/Character Data")]
    public class CharacterData : ScriptableObject
    {
        [Header("Character Profile")]
        public string characterName;
        public int level = 1;
    
        [Space]
        [Header("Character UI Image")]
        public Sprite characterPortrait; 
        public float rectPosX;
        public float rectPosY;
        public float rectWidth;
        public float rectHeight;
    
        [Header("Battle")]
        public GameObject battlePrefab;  
        public GameObject overworldPrefab; 
        public int maxHP;
        public int maxMP;
        public int attackPower;
        public int speed;
        public int specialCost;
    
        [Header("Current Status")]
        public int currentHP;
        public int currentMP;
    
        [Header("Reward (Enemy Only)")]
        public int rewardGold = 50;
    
        [Header("SFX & VFX for Battle")]
        public AudioClip attackSFX;
        public AudioClip specialChargeSFX;
        public GameObject specialChargeVFX;
    }
}