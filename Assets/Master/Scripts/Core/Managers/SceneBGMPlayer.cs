using UnityEngine;

using RPGTest.Architecture;

namespace RPGTest.Core.Managers 
{
    public class SceneBGMPlayer : MonoBehaviour
    {
        public AudioClip sceneBGM;

        private void Start()
        {
            if (ServiceLocator.Current.Contains<AudioManager>())
            {
                ServiceLocator.Current.Get<AudioManager>().PlayBGM(sceneBGM);
            }
        }
    }
}