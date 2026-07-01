using UnityEngine;
using Fungus;

using RPGTest.Architecture;
using RPGTest.Core.Managers;
using RPGTest.Core.Events;
using RPGTest.Data;

namespace RPGTest.Overworld 
{
    public class RecruitNPC : NPCInteractable 
    {
        public CharacterData characterData;
        public bool isRecruitable = false;
    
        protected override void Start() 
        {
            base.Start();
            
            if (isRecruitable && characterData != null && ServiceLocator.Current.Contains<GameManager>())
            {
                if (ServiceLocator.Current.Get<GameManager>().activeParty.Contains(characterData))
                {
                    if (!gameObject.name.Contains("_ActivePrefab")) Destroy(gameObject);
                    else { Destroy(GetComponent<Flowchart>()); Destroy(this); }
                }
            }
        }
    
        public void RecruitToParty()
        {
            EventBus<PartyJoinedEvent>.Publish(new PartyJoinedEvent(characterData));
            PartyFollower followerScript = GetComponent<PartyFollower>();
    
            if (followerScript != null)
            {
                followerScript.enabled = true;
                followerScript.targetToFollow = GameObject.FindGameObjectWithTag("Player").transform;
            }
    
            if (flowchart != null)
            {
                Destroy(flowchart.gameObject);
            }
    
            Destroy(this);
        }
    }
}