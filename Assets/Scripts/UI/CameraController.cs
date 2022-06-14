using System.Net.NetworkInformation;
using UnityEngine;
public class CameraController : MonoBehaviour
{
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 2F;
    public float sensitivityY = 2F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -90F;
    public float maximumY = 90F;
    float rotationY = -60F;

    // For camera movement
    float CameraPanningSpeed = 10.0f;


    void Update()
    {
        MouseInput();
    }

    void MouseInput()
    {

        if (Input.GetMouseButton(0))
        {
        }
        else if (Input.GetMouseButton(1))
        {
            MouseRightClick();
        }
        else if (Input.GetMouseButton(2))
        {
            MouseMiddleButtonClicked();
        }
        else
        {
            MouseWheeling();
        }
    }

   
    void MouseMiddleButtonClicked()
    {
        Vector3 NewPosition = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));
        Vector3 pos = transform.position;
        if (NewPosition.x > 0.0f)
        {
            pos -= transform.right * 1.5f;
        }
        
        if (NewPosition.x < 0.0f)
        {
            pos += transform.right * 1.5f;
        }
        
        if (NewPosition.z > 0.0f)
        {
            pos -= transform.forward * 1.5f;
        }
        
        if (NewPosition.z < 0.0f)
        {
            pos += transform.forward * 1.5f;
        }
        pos.y = transform.position.y;
        transform.position = pos;
    }

    void MouseRightClick()
    {
        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }

    void MouseWheeling()
    {
        Vector3 pos = transform.position;
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            pos = pos - transform.forward * 10;
            transform.position = pos;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            pos = pos + transform.forward * 10;
            transform.position = pos;
        }
    }

}