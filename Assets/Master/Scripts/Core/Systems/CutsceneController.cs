using UnityEngine;
using UnityEngine.Playables; // Wajib untuk Timeline
using Fungus;

using RPGTest.Architecture;
using RPGTest.Core.Managers;

namespace RPGTest.Core.Systems
{
    public class CutsceneController : MonoBehaviour
    {
        public static bool IsCutscenePlaying = false;

        [Header("Cutscene ID")]
        public string cutsceneID; 

        [Header("Intro Setting")]
        public bool playOnStart = false;

        [Header("Sequence Trigger Cutscene")]
        public GameObject nextCutsceneTrigger;

        [Header("Cutscene Ref")]
        public PlayableDirector director;
        public Flowchart flowchart;

        private void Start()
        {
            if (ServiceLocator.Current.Contains<GameManager>())
            {
                GameManager gm = ServiceLocator.Current.Get<GameManager>();
                if (gm.playedCutscenes.Contains(cutsceneID))
                {
                    // jika cutscene ini sudah pernah ditonton sebelumnya
                    // nyalain cutscene selanjutnya, kalo ada matikan sebelumnya
                    if (nextCutsceneTrigger != null) nextCutsceneTrigger.SetActive(true);
                    gameObject.SetActive(false);
                    return; 
                }
            }

            // buat intro
            if (playOnStart)
            {
                TryPlayCutscene();
            }
        }

        private void OnEnable()
        {
            if (director != null) 
            {
                director.played += OnTimelinePlayed;
                director.stopped += OnTimelineStopped;
            }
        }

        private void OnDisable()
        {
            if (director != null) 
            {
                director.played -= OnTimelinePlayed;
                director.stopped -= OnTimelineStopped;
            }
        }

        private void OnTimelinePlayed(PlayableDirector d) => IsCutscenePlaying = true;

        private void OnTimelineStopped(PlayableDirector d) 
        {
            IsCutscenePlaying = false;

            // Nyalain trigger selanjutnya
            if (nextCutsceneTrigger != null) 
            {
                nextCutsceneTrigger.SetActive(true);
            }

            gameObject.SetActive(false);
        }

        // private void Update()
        // {
        //     if (director != null)
        //     {
        //         IsCutscenePlaying = director.state == PlayState.Playing;
        //     }
        // }

        public void TryPlayCutscene()
        {
            if (ServiceLocator.Current.Contains<GameManager>())
            {
                GameManager gm = ServiceLocator.Current.Get<GameManager>();

                if (!gm.playedCutscenes.Contains(cutsceneID))
                {
                    gm.playedCutscenes.Add(cutsceneID); 

                    if (director != null)  
                    {
                        IsCutscenePlaying = true; 
                        director.Play();
                    }
                }
            }
        }

        // dipanggil di Timeline lewat signal receiver
        public void PauseTimelineAndPlayDialog(string blockName)
        {
            if (director != null) 
            {
                director.Pause();
            }

            if (flowchart != null) 
            {
                flowchart.ExecuteBlock(blockName);
            }
        }

        // dipanggil Fungus
        public void ResumeTimeline()
        {
            if (director != null) 
            {
                director.Play();
            }
        }
    }
}