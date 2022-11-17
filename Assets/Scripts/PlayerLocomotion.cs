using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerLocomotion : MonoBehaviour
    {
        Transform cameraObject;
        InputHandler inputHandler;
        public PlayerManager playerManager;
        public AnimatedHandler animationHandler;
        public Vector3 moveDirection;
        public Rigidbody rigidBody;

        [HideInInspector]
        public Transform myTransform;

        // [HideInInspector]
        public GameObject normalCamera;

        [Header("Ground & Air Detection Stats")]
        [SerializeField]
        float groundDetectionRayStartPoint = 0.5f;

        [SerializeField]
        float minimumDistanceNeededToBeginFall = 1f;

        [SerializeField]
        float groundDirectionRayDistance = 0.2f;
        LayerMask ignoreForGroundCheck;
        public float inAirTimer;

        [Header("Movement Stats")]
        [SerializeField]
        float movementSpeed = 5;

        [SerializeField]
        float springSpeed = 7;

        [SerializeField]
        float rotationSpeed = 5;

        [SerializeField]
        float fallingSpeed = 200;

        // Start is called before the first frame update
        void Start()
        {
            // rigidbody = rigidBody;
            playerManager = GetComponent<PlayerManager>();
            inputHandler = GetComponent<InputHandler>();
            animationHandler = GetComponentInChildren<AnimatedHandler>();
            cameraObject = Camera.main.transform;
            myTransform = transform;
            animationHandler.Initialize();
            playerManager.isGrounded = true;
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
        }

        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;

        private void HandleRotation(float delta)
        {
            Vector3 targetDirection = Vector3.zero;
            float moveOverride = inputHandler.moveAmount;

            targetDirection = cameraObject.forward * inputHandler.vertical;
            targetDirection += cameraObject.right * inputHandler.horizontal;
            targetDirection.Normalize();

            if (targetDirection == Vector3.zero)
                targetDirection = myTransform.forward;

            float rs = rotationSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDirection);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);
            
            Debug.Log("targetRotation:");
            Debug.Log(targetRotation);
            Debug.DrawRay(
                myTransform.position,
                targetDirection,
                Color.blue,
                0.1f,
                false
            );
            myTransform.rotation = targetRotation;
        }

        public void HandleMovement(float delta)
        {
            if (inputHandler.rollFlag)
                return;

            if(playerManager.isInteracting)
                return;

            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;

            if (inputHandler.sprintFlag)
            {
                speed = springSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;
            }
            else
            {
                moveDirection *= speed;
            }

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            // Debug.Log(projectedVelocity);
            rigidBody.velocity = projectedVelocity;

            animationHandler.UpdateAnimatorValues(
                inputHandler.moveAmount,
                0,
                playerManager.isSprinting
            );

            if (animationHandler.canRotate)
            {
                HandleRotation(delta);
            }
        }
        #endregion

        public void HandleRollingAndSprinting(float delta)
        {
            if (animationHandler.anim.GetBool("isInteracting"))
            {
                return;
            }

            if (inputHandler.rollFlag)
            {
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;

                // multiply 1.2 to the moveDirection to make the roll faster
                moveDirection = moveDirection.normalized * 2f;

                if (inputHandler.moveAmount > 0)
                {
                    animationHandler.PlayTargetAnimation("Rolling", true);
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = rollRotation;
                }
                else
                {
                    animationHandler.PlayTargetAnimation("Backstep", true);
                }
            }
        }

        public void HandleFalling(float delta, Vector3 moveDirection)
        {
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position;

            // 角色collider底部上浮0.5f
            origin.y += groundDetectionRayStartPoint;

            // 从角色collider底部发射一条射线，检测是否接触到地面
            if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
            {
                // 若碰到地面，則角色不動
                moveDirection = Vector3.zero;
            }

            if (playerManager.isInAir)
            {
                rigidBody.AddForce(-Vector3.up * fallingSpeed);
                // rigidBody.velocity(-Vector3.up * fallingSpeed);

                // 若碰到邊緣，推動角色向前進方向，使他不會卡住
                // rigidBody.AddForce(moveDirection * fallingSpeed / 5f);
            }

            Vector3 dir = moveDirection;
            dir.Normalize();
            origin = origin + dir * groundDirectionRayDistance;

            targetPosition = myTransform.position;

            Debug.DrawRay(
                origin,
                -Vector3.up * minimumDistanceNeededToBeginFall,
                Color.red,
                0.1f,
                false
            );

            var result = Physics.Raycast(
                    origin,
                    -Vector3.up,
                    out hit,
                    minimumDistanceNeededToBeginFall,
                    ignoreForGroundCheck
                );
            Debug.Log(result);

            // 若已觸地
            if ( result )
            {
                // 設定角色觸地
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPosition.y = tp.y;

                if (playerManager.isInAir)
                {
                    if (inAirTimer > 0.5f)
                    {
                        Debug.Log("You were in the air for" + inAirTimer);
                        animationHandler.PlayTargetAnimation("Land", true);
                    }
                    else
                    {
                        // animationHandler.PlayTargetAnimation("Locomotion", false);
                        animationHandler.PlayTargetAnimation("Empty", false);
                        inAirTimer = 0;
                    }
                    playerManager.isInAir = false;
                }
            }
            else
            {
                // 設定角色在空中
                if (playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }
                if (playerManager.isInAir == false)
                {
                    if (playerManager.isInteracting == false)
                    {
                        // animationHandler.PlayTargetAnimation("Falling", true);
                    }
                    Vector3 vel = rigidBody.velocity;
                    vel.Normalize();
                    rigidBody.velocity = vel * (movementSpeed);
                    playerManager.isInAir = true;
                }
            }

            if (playerManager.isGrounded)

                if (playerManager.isInteracting || inputHandler.moveAmount > 0)
                {
                    myTransform.position = Vector3.Lerp(
                        myTransform.position,
                        targetPosition,
                        Time.deltaTime
                    );
                }
                else
                {
                    myTransform.position = targetPosition;
                }
        }
    }
}
