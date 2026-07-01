using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPGTest.Architecture;
using RPGTest.Core.Managers;
using RPGTest.Core.Events;
using RPGTest.Data;

namespace RPGTest.UI.OverworldUI 
{
    public class PartyUIController : MonoBehaviour
    {
        [Header("Party Component Setting")]
        public Transform partyListParent;
        public GameObject partyMemberPrefab;
    
        [Header("Start Party")]
        public List<CharacterData> startingPartyMembers; 
    
        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
    
            foreach (Transform child in partyListParent) 
            { 
                Destroy(child.gameObject); 
            }
    
            if (ServiceLocator.Current.Contains<GameManager>())
            {
                GameManager gm = ServiceLocator.Current.Get<GameManager>();
    
                if (gm.activeParty.Count > 0)
                {
                    foreach (CharacterData member in gm.activeParty)
                    {
                        HandleCharacterJoined(new PartyJoinedEvent(member));
                    }
                }
                else
                {
                    foreach (CharacterData member in startingPartyMembers) 
                    {
                        HandleCharacterJoined(new PartyJoinedEvent(member));
                    }
                }
            }
        }
    
        private void OnEnable() 
        {
            EventBus<PartyJoinedEvent>.Subscribe(HandleCharacterJoined);
            EventBus<PartyUpdatedEvent>.Subscribe(RefreshUI);
        }
        
        private void OnDisable() 
        {
            EventBus<PartyJoinedEvent>.Unsubscribe(HandleCharacterJoined);
            EventBus<PartyUpdatedEvent>.Unsubscribe(RefreshUI);
        }
    
        private void HandleCharacterJoined(PartyJoinedEvent eventData)
        {
            if (partyMemberPrefab == null || partyListParent == null) return;
    
            GameObject newUISlot = Instantiate(partyMemberPrefab, partyListParent);
    
            PartyMemberUIElement uiElement = newUISlot.GetComponent<PartyMemberUIElement>();
    
            if (uiElement != null) 
            {
                uiElement.SetupUI(eventData.Character);
            }
        }
    
        private void RefreshUI(PartyUpdatedEvent eventData)
        {
            foreach (Transform child in partyListParent) 
            { 
                Destroy(child.gameObject); 
            }
            
            GameManager gm = ServiceLocator.Current.Get<GameManager>();
    
            if (gm != null)
            {
                foreach (CharacterData member in gm.activeParty)
                {
                    HandleCharacterJoined(new PartyJoinedEvent(member));
                }
            }
        }
    }
}