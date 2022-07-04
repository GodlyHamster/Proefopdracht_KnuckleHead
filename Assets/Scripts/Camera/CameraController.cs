using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 80;
    [SerializeField] private InputActionAsset cameraControls;

    private void Awake()
    {
        cameraControls.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 input = cameraControls["Rotate"].ReadValue<Vector2>();

        float mouseX = input.x;
        float mouseY = input.y;

        Vector3 rotation = gameObject.transform.eulerAngles;
        rotation += new Vector3(-mouseY, mouseX, 0) * mouseSensitivity * Time.deltaTime;
        rotation.x = ClampAngle(rotation.x, -75f, 75f);
        gameObject.transform.rotation = Quaternion.Euler(rotation);
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (min < 0 && max > 0 && (angle > max || angle < min))
        {
            angle -= 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }
        else if (min > 0 && (angle > max || angle < min))
        {
            angle += 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }

        if (angle < min) return min;
        else if (angle > max) return max;
        else return angle;
    }
}
