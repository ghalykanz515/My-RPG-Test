using UnityEngine;

namespace RPGTest.Data 
{
    public enum ConsumableType { HealHP, HealMP, Revive }

    [CreateAssetMenu(fileName = "New Item", menuName = "RPG/Item Data")]
    public class ItemData : ScriptableObject
    {
        [Header("Item Data")]
        public string itemName;
        [TextArea] public string description;
        public Sprite itemIcon;
        public int price;

        [Header("Item Effect")]
        public ConsumableType effectType;
        public int effectValue;

        [Header("SFX & VFX")]
        public AudioClip useSFX;
        public GameObject useVFX;
    }
}