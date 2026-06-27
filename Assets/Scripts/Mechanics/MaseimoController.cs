using Platformer.Gameplay;
using Platformer.Model;
using Platformer.Core;
using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// A controller for the Maseimo enemy. Walks towards the player when they are in range,
    /// and otherwise patrols or stands still.
    /// </summary>
    public class MaseimoController : EnemyController
    {
        [Header("Maseimo Settings")]
        [Tooltip("Distance within which Maseimo will detect and chase the player.")]
        public float detectionRange = 5f;

        [Tooltip("Movement speed when chasing the player.")]
        public float chaseSpeed = 3f;

        [Tooltip("Movement speed when patrolling (if path is assigned).")]
        public float patrolSpeed = 1.5f;

        private PlayerController playerController;

        protected override void Awake()
        {
            base.Awake();
            FindPlayer();
        }

        private void FindPlayer()
        {
            playerController = FindObjectOfType<PlayerController>();
        }

        protected override void Update()
        {
            // If control is disabled (e.g. enemy is dead), do nothing
            if (control == null || !control.enabled) return;

            // Ensure we have a reference to the player
            if (playerController == null)
            {
                FindPlayer();
            }

            // Check if player is active and alive
            if (playerController != null && !playerController.isDead)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, playerController.transform.position);
                if (distanceToPlayer <= detectionRange)
                {
                    // Chase the player (move horizontally towards the player)
                    control.maxSpeed = chaseSpeed;
                    float direction = Mathf.Sign(playerController.transform.position.x - transform.position.x);
                    control.move.x = direction;
                    return; // Bypass default patrol behavior
                }
            }

            // Fallback to patrol path or idle if player is not detected or is dead
            if (path != null)
            {
                control.maxSpeed = patrolSpeed;
                if (mover == null) mover = path.CreateMover(control.maxSpeed * 0.5f);
                control.move.x = Mathf.Clamp(mover.Position.x - transform.position.x, -1, 1);
            }
            else
            {
                // Stand still if no patrol path is set
                control.move.x = 0f;
            }
        }

        // Draw detection range in the Editor scene view for debugging convenience
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }
}
