using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class AnimatedHandler : MonoBehaviour
    {
        public Animator anim;
        int vertical;
        int horizontal;
        public bool canRotate = true;

        PlayerLocomotion playerLocomotion;
        PlayerManager playerManager;

        public void Initialize()
        {
            anim = GetComponent<Animator>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            playerManager = GetComponentInParent<PlayerManager>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(
            float verticalMovement,
            float horizontalMovement,
            bool isSpringting
        )
        {
            float v = 0;
            float h = 0;
            if (isSpringting)
            {
                v = 2;
                h = horizontalMovement;
            }
            #region Vertical
            if (verticalMovement > 0 && verticalMovement < 0.55f)
                v = 0.5f;
            else if (verticalMovement > 0.55f)
                v = 1;
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
                v = -0.5f;
            else if (verticalMovement < -0.55f)
                v = -1;
            else
                v = 0;
            #endregion

            #region Horizontal
            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
                h = 0.5f;
            else if (horizontalMovement > 0.55f)
                h = 1;
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
                h = -0.5f;
            else if (horizontalMovement < -0.55f)
                h = -1;
            else
                h = 0;
            #endregion

            anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void CanRotate()
        {
            canRotate = true;
        }

        public void StopRotate()
        {
            canRotate = false;
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }

        private void OnAnimatorMove()
        {
            if (playerManager.isInteracting == false)
            {
                return;
            }

            float delta = Time.deltaTime;
            playerLocomotion.rigidBody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerLocomotion.rigidBody.velocity = velocity;
        }
    }
}
