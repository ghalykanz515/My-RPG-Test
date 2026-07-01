using System;
using UnityEngine;

using RPGTest.Data;

namespace RPGTest.Core.Events 
{
    public static class GameEvents 
    {
        public static Action<CharacterData, int, int> OnCharacterStatsChanged;
        public static Action<CharacterData> OnCharacterJoinedParty;
    }

    public struct PartyUpdatedEvent { }
    public struct GoldChangedEvent { public int NewGold; }
}