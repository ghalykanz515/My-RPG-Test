using UnityEngine;

using RPGTest.Core.Systems;

namespace RPGTest.Overworld 
{
    [RequireComponent(typeof(BoxCollider))]
    public class CutsceneTrigger : MonoBehaviour
    {
        [Header("Controller Ref")]
        public CutsceneController controller; 

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (controller != null)
                {
                    controller.TryPlayCutscene();
                }
            }
        }
    }
}