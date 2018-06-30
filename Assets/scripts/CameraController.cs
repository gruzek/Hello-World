using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace CameraController
{

    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotateSpeed;

        private Camera camera;
        private Vector3 originalCameraPosition;
        private Vector2 input;


        // Use this for initialization
        private void Start()
        {
            camera = Camera.main;
            originalCameraPosition = camera.transform.localPosition;
        }


        /*
        private void Update()
        {
        }
        */

        private void FixedUpdate()
        {
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");
            float rotate = CrossPlatformInputManager.GetAxis("Rotation");
            input = new Vector2(horizontal, vertical);

            Vector3 desiredMove = transform.forward*input.y*moveSpeed + transform.right*input.x*moveSpeed;
            desiredMove.y=0;

            //Vector3 moveDir;
            //moveDir.x = input.x*moveSpeed;
            //moveDir.z = input.y*moveSpeed;
            //moveDir.y = 0;

            Vector3 newCameraPosition;
            newCameraPosition = camera.transform.localPosition + desiredMove;
            camera.transform.localPosition = newCameraPosition;

            Vector3 eulerAngles = new Vector3(0,rotate*rotateSpeed,0);
            transform.Rotate(eulerAngles, Space.World);

        }

    }
}
