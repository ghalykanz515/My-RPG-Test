using UnityEngine;
using Fungus;

using RPGTest.Interfaces;

namespace RPGTest.Overworld  
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Interaction Settings")]
        public float interactionRadius = 1.5f;
        public LayerMask interactableLayer;

        private PlayerControls playerControls;
        private IInteractable currentTarget;

        private void Awake()
        {
            playerControls = new PlayerControls();
            playerControls.Gameplay.Interact.performed += _ => TryInteract();
        }

        private void OnEnable() 
        { 
            playerControls.Gameplay.Enable();
        }

        private void OnDisable() 
        {
             playerControls.Gameplay.Disable();
        }

        private void Update()
        {
            // deteksi apakah dialog sedang aktif
            bool isDialogActive = (SayDialog.ActiveSayDialog != null && SayDialog.ActiveSayDialog.gameObject.activeInHierarchy) || (MenuDialog.ActiveMenuDialog != null && MenuDialog.ActiveMenuDialog.gameObject.activeInHierarchy);

            Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius, interactableLayer);
            IInteractable closest = null;
            float min = float.MaxValue;

            foreach (Collider col in colliders)
            {
                IInteractable inter = col.GetComponent<IInteractable>();
                if (inter != null)
                {
                    float d = Vector3.Distance(transform.position, col.transform.position);
                    if (d < min) { min = d; closest = inter; }
                }
            }

            if (closest != null && !isDialogActive)
            {
                if (currentTarget != closest)
                {
                    if (currentTarget != null) currentTarget.HideInteractIcon();
                    currentTarget = closest;
                    currentTarget.ShowInteractIcon();
                }
            }
            else
            {
                if (currentTarget != null) 
                {
                    currentTarget.HideInteractIcon();
                }
                
                if (isDialogActive) return;
                 
                currentTarget = null;
            }
            // if (SayDialog.ActiveSayDialog != null && SayDialog.ActiveSayDialog.gameObject.activeInHierarchy) 
            // {
            //     ClearTarget();
            //     return;
            // }
            // if (MenuDialog.ActiveMenuDialog != null && MenuDialog.ActiveMenuDialog.gameObject.activeInHierarchy) 
            // {
            //     ClearTarget();
            //     return;
            // }

            // Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius, interactableLayer);
            // IInteractable closestInteractable = null;
            // float minDistance = float.MaxValue;

            // foreach (Collider col in colliders)
            // {
            //     IInteractable interactable = col.GetComponent<IInteractable>();
            //     if (interactable != null)
            //     {
            //         float dist = Vector3.Distance(transform.position, col.transform.position);
            //         if (dist < minDistance)
            //         {
            //             minDistance = dist;
            //             closestInteractable = interactable;
            //         }
            //     }
            // }

            // if (currentTarget != closestInteractable)
            // {
            //     ClearTarget();

            //     currentTarget = closestInteractable;

            //     if (currentTarget != null) 
            //     {
            //         currentTarget.ShowInteractIcon();
            //     }
            // }
        }

        private void ClearTarget()
        {
            if (currentTarget != null)
            {
                currentTarget.HideInteractIcon();
                currentTarget = null;
            }
        }

        private void TryInteract()
        {
            if (currentTarget != null) currentTarget.Interact();
            // if (SayDialog.ActiveSayDialog != null && SayDialog.ActiveSayDialog.gameObject.activeInHierarchy) return;

            // if (MenuDialog.ActiveMenuDialog != null && MenuDialog.ActiveMenuDialog.gameObject.activeInHierarchy) return;

            // if (currentTarget != null)
            // {
            //     currentTarget.Interact();
            //     ClearTarget();
            // }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRadius);
        }
    }
}