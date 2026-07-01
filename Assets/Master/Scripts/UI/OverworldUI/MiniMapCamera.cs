using UnityEngine;

namespace RPGTest.UI.OverworldUI 
{
    public class MiniMapCamera : MonoBehaviour
    {
        public Transform target;
        public float followSpeed;
    
        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, target.position, followSpeed * Time.deltaTime);
        }
    }
}
