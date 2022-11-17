using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SG
{
    public class PlayerManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerLocomotion playerLocomotion;
        CameraHandler cameraHandler;
        Animator anim;

        [Header("Player Flag")]
        public bool isInteracting;
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;

        private void Awake()
        {
            cameraHandler = CameraHandler.singleton;
        }

        // Start is called before the first frame update
        void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponentInChildren<PlayerLocomotion>();
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;
            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }
        }

        void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = anim.GetBool("isInteracting");
            isSprinting = inputHandler.b_Input;
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;
            inputHandler.TickInput(delta);
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
            // 先寫死： 若角色掉落，回到MENU(紀錄ＨＩＧＨＳＣＯＲＥ)
            if (playerLocomotion.myTransform.position.y < -13)
            {
                //TODO: 紀錄遊戲進行時間到HIGHSCORE
                Debug.Log("Player falls down");
                SceneManager.LoadScene(0);
            }
        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;
            isSprinting = inputHandler.b_Input;
            if (isInAir)
            {
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
            }
        }
    }
}
