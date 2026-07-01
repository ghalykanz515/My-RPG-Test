using RPGTest.Data;

namespace RPGTest.Core.Events 
{
    public struct PartyJoinedEvent 
    {
        public CharacterData Character;
        public PartyJoinedEvent(CharacterData character) => Character = character;
    }

    public struct UnitHPChangedEvent
    {
        public string UnitName;
        public int CurrentHP;
        public int MaxHP;
        public bool IsPlayerTeam;

        public UnitHPChangedEvent(string name, int currentHP, int maxHP, bool isPlayerTeam)
        {
            UnitName = name;
            CurrentHP = currentHP;
            MaxHP = maxHP;
            IsPlayerTeam = isPlayerTeam;
        }
    }

    public struct UnitMPChangedEvent
    {
        public string UnitName;
        public int CurrentMP;
        public int MaxMP;

        public UnitMPChangedEvent(string name, int currentMP, int maxMP)
        {
            UnitName = name;
            CurrentMP = currentMP;
            MaxMP = maxMP;
        }
    }
}