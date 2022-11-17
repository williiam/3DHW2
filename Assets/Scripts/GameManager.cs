using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class GameManager : MonoBehaviour
    {
        InputHandler inputHandler;
        Animator anim;

        public bool isInteracting;
        CameraHandler cameraHandler;

        private void Awake()
        {
            cameraHandler = CameraHandler.singleton;
        }

        // Start is called before the first frame update
        void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
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

        // Update is called once per frame
        void Update()
        {
            isInteracting = anim.GetBool("isInteracting");
            inputHandler.rollFlag = false;
        }
    }
}
