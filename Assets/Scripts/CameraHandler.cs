using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class CameraHandler : MonoBehaviour
    {
        public Transform targetTransform;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        public Transform myTransform;
        private Vector3 cameraTransformPosition;
        private LayerMask ignoreLayers;
        private Vector3 cameraFollowVelocity = Vector3.zero;

        public static CameraHandler singleton;

        public float lookSpeed = 200f; 
        public float followSpeed = 150f;
        public float pivotSpeed = 0.004f;

        public float targetPosition;
        private float defaultPosition;
        private float lookAngle;
        private float pivotAngle;
        public float minimumPivot = -35f;
        public float maximumPivot = 35f;

        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float miniumCollisionOffset = 0.2f;

        private void Awake()
        {
            singleton = this;
            myTransform = transform; // myTransform = 此gameObject的transform 
            defaultPosition = cameraTransform.localPosition.z; 
            ignoreLayers = ~(1 << 8 | 1 << 9);
        }

        public void FollowTarget(float delta)
        {
            Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
            // Debug.Log("targetPosition: " + targetPosition);
            myTransform.position = targetPosition;
            HandleCameraCollisions(delta);
        }

        public void HandleCameraRotation(float delta, float mouseInputX, float mouseInputY)
        {
            lookAngle += (mouseInputX * lookSpeed) * delta;
            pivotAngle -= (mouseInputY * pivotSpeed) * delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            myTransform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = pivotAngle;

            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
        }

        private void HandleCameraCollisions(float delta)
        {
            targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransformPosition - cameraPivotTransform.position;
            direction.Normalize();
            cameraTransformPosition = cameraTransform.localPosition;
            float distance = direction.magnitude;
            if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit, distance, ignoreLayers)) 
            {
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(dis - cameraCollisionOffset);
            }

            if (Mathf.Abs(targetPosition) < miniumCollisionOffset)
            {
                targetPosition = -miniumCollisionOffset;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta * 9f);
            cameraTransform.localPosition = cameraTransformPosition;
        }
    }
}
