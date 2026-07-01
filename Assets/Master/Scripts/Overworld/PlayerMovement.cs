using UnityEngine;
using UnityEngine.InputSystem;
using Fungus; 

using RPGTest.Core.Systems;
using RPGTest.UI.OverworldUI;

namespace RPGTest.Overworld 
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float moveSpeed = 5f;

        [Header("Visual Ref")]
        public Animator animator;
        public SpriteRenderer spriteRenderer;

        private PlayerControls playerControls; 
        private Rigidbody rb;
        private Vector2 movementInput;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            playerControls = new PlayerControls();
            rb.constraints = RigidbodyConstraints.FreezeRotation; 

            // animator = GetComponentInChildren<Animator>();
            // spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true;
            }
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
            // ngunci input kalo sedang ngobrol dgn Fungus
            rb.isKinematic = CutsceneController.IsCutscenePlaying;

            if (IsDialogActive())
            {
                movementInput = Vector2.zero;
                UpdateAnimationAndVisual();
                return; 
            }

            movementInput = playerControls.Gameplay.Move.ReadValue<Vector2>();
            UpdateAnimationAndVisual();
        }

        private void FixedUpdate()
        {
            if (rb.isKinematic) return;

            // berhentiin jika sedang ngobrol
            if (IsDialogActive())
            {
                rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
                return;
            }

            Vector3 moveDirection = new Vector3(movementInput.x, 0f, movementInput.y);
            moveDirection.Normalize();

            rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);
        }

        private void UpdateAnimationAndVisual()
        {
            bool isWalking = movementInput.magnitude > 0.1f;
            animator.SetBool("IsWalking", isWalking);

            // if (movementInput.x > 0.1f)
            // {
            //     spriteRenderer.flipX = false;
            // }
            // else if (movementInput.x < -0.1f)
            // {
            //     spriteRenderer.flipX = true;
            // }

            if (movementInput.x > 0.1f) 
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (movementInput.x < -0.1f) 
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }

        private bool IsDialogActive()
        {
            bool isSaying = SayDialog.ActiveSayDialog != null && SayDialog.ActiveSayDialog.gameObject.activeInHierarchy;
            bool isMenuing = MenuDialog.ActiveMenuDialog != null && MenuDialog.ActiveMenuDialog.gameObject.activeInHierarchy;
            bool isShopping = MerchantUI.IsShopOpen; 
            bool isCutscene = CutsceneController.IsCutscenePlaying;

            return isSaying || isMenuing || isShopping || isCutscene;
        }
    }
}