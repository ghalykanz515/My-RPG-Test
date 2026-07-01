using UnityEngine;

namespace RPGTest.Architecture
{
    public class AutoReturnToPool : MonoBehaviour
    {
        [Header("Display Duration")]
        public float lifeTime = 2f;

        private void OnEnable()
        {
            Invoke(nameof(ReturnObject), lifeTime);
        }

        private void OnDisable()
        {
            CancelInvoke();
        }

        private void ReturnObject()
        {
            if (ServiceLocator.Current.Contains<ObjectPoolManager>())
            {
                ServiceLocator.Current.Get<ObjectPoolManager>().ReturnToPool(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}