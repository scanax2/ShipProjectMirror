using UnityEngine;

namespace ProjectShips
{
    public class FreeCamera : MonoBehaviour
    {
        [SerializeField] float movementSpeed = 10f;
        [SerializeField] float fastMovementSpeed = 100f;
        [SerializeField] float mouseSensitivity = 3f;

        private bool isRotating = false;

        void Update()
        {
            var fastMode = Input.GetKey(KeyCode.LeftShift);
            var speed = fastMode ? fastMovementSpeed : movementSpeed;

            if (Input.GetKey(KeyCode.W))
                transform.position += transform.forward * speed * Time.deltaTime;
            if (Input.GetKey(KeyCode.A))
                transform.position += -transform.right * speed * Time.deltaTime;
            if (Input.GetKey(KeyCode.S))
                transform.position += -transform.forward * speed * Time.deltaTime;
            if (Input.GetKey(KeyCode.D))
                transform.position += transform.right * speed * Time.deltaTime;

            if (Input.GetKey(KeyCode.E))
                transform.position += transform.up * speed * Time.deltaTime;
            if (Input.GetKey(KeyCode.Q))
                transform.position += -transform.up * speed * Time.deltaTime;

            if (isRotating)
            {
                float newRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
                float newRotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * mouseSensitivity;
                transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0);
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
                SetRotatingState(true);
            else if (Input.GetKeyUp(KeyCode.Mouse1))
                SetRotatingState(false);
        }

        void OnDisable()
        {
            SetRotatingState(false);
        }

        public void SetRotatingState(bool enabled)
        {
            isRotating = enabled;
            Cursor.visible = !enabled;
            Cursor.lockState = enabled ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}