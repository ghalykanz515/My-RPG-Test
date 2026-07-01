using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

namespace RPGTest.Overworld 
{
    [RequireComponent(typeof(Rigidbody))]
    public class PartyFollower : MonoBehaviour
    {
        public static List<PartyFollower> activeFollowers = new List<PartyFollower>();

        [Header("Target Setting")]
        public Transform targetToFollow;

        [Header("Movement Setting")]
        public float moveSpeed = 5f; 
        public float stopDistance = 1.5f;

        [Header("Character Spacing Adjustment")]
        public float separationRadius = 1.2f; 
        public float separationForce = 3f;  

        private Rigidbody rb;
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private Collider myCollider;

        private Vector3 currentVelocity; 
        private NavMeshPath path;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation; 
            rb.useGravity = true; 

            animator = GetComponentInChildren<Animator>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            myCollider = GetComponent<Collider>();

            path = new NavMeshPath();
        }

        private void OnEnable()
        {
            if (!activeFollowers.Contains(this)) 
            {
                activeFollowers.Add(this);
            }

            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null && myCollider != null)
            {
                Collider playerCollider = player.GetComponent<Collider>();
                if (playerCollider != null) Physics.IgnoreCollision(myCollider, playerCollider);
            }

            foreach (PartyFollower f in activeFollowers)
            {
                if (f != this && f.myCollider != null && myCollider != null)
                {
                    Physics.IgnoreCollision(myCollider, f.myCollider);
                }
            }
        }

        private void OnDisable()
        {
            if (activeFollowers.Contains(this)) 
            {
                activeFollowers.Remove(this);
            }
        }

        private void FixedUpdate()
        {
            if (targetToFollow == null) return;

            Vector3 currentPos = transform.position;
            Vector3 targetPos = targetToFollow.position;

            float distanceToTarget = Vector3.Distance(new Vector3(currentPos.x, 0, currentPos.z), new Vector3(targetPos.x, 0, targetPos.z));
            Vector3 desiredVelocity = Vector3.zero;

            NavMesh.CalculatePath(currentPos, targetPos, NavMesh.AllAreas, path);

            if (distanceToTarget > stopDistance && path.corners.Length > 1)
            {
                Vector3 nextWaypoint = path.corners[1];
                Vector3 followDirection = (nextWaypoint - currentPos);
                followDirection.y = 0; 
                desiredVelocity += followDirection.normalized * moveSpeed;
            }

            // buat jaga jarak
            Vector3 separationVelocity = Vector3.zero;
            foreach (PartyFollower other in activeFollowers)
            {
                if (other != this && other != null) 
                {
                    float distanceToOther = Vector3.Distance(new Vector3(currentPos.x, 0, currentPos.z), new Vector3(other.transform.position.x, 0, other.transform.position.z));

                    if (distanceToOther < separationRadius && distanceToOther > 0.01f)
                    {
                        Vector3 pushAway = (currentPos - other.transform.position);
                        pushAway.y = 0; 
                        float forcePercentage = (separationRadius - distanceToOther) / separationRadius;
                        separationVelocity += pushAway.normalized * forcePercentage * separationForce;
                    }
                }
            }

            if (distanceToTarget <= stopDistance)
            {
                separationVelocity *= 0.1f; 
            }

            desiredVelocity += separationVelocity;

            // rem paksa
            if (desiredVelocity.magnitude < 0.5f)
            {
                rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0); 
            }
            else
            {
                desiredVelocity = Vector3.ClampMagnitude(desiredVelocity, moveSpeed * 1.5f);
                Vector3 finalVelocity = new Vector3(desiredVelocity.x, rb.linearVelocity.y, desiredVelocity.z);
                rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, finalVelocity, ref currentVelocity, 0.1f);
            }

            UpdateAnimationAndVisual(rb.linearVelocity);
        }

        private void UpdateAnimationAndVisual(Vector3 velocity)
        {
            bool isWalking = new Vector2(velocity.x, velocity.z).magnitude > 0.2f; 
            if (animator != null) animator.SetBool("IsWalking", isWalking);

            if (isWalking)
            {
                if (isWalking)
                {
                    if (velocity.x > 0.1f) 
                    {
                        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    }
                    else if (velocity.x < -0.1f) 
                    {
                        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    }
                }

                // if (velocity.x > 0.1f) 
                // {
                //     spriteRenderer.flipX = false;
                // }
                // else if (velocity.x < -0.1f) 
                // {
                //     spriteRenderer.flipX = true;
                // }
            }
        }
    }
}