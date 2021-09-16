using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera cam;
    
    //wasd
    float speed;

    //zooming
    float minZoom, maxZoom;
    float zoomSpeed;
    
    void Awake()
    {
        cam = Camera.main;
        
        //set camera position and what to look at
        cam.transform.position = new Vector3(0, 20, -15);
        cam.transform.rotation = Quaternion.Euler(60, 0, 0);

        //set speed according to the height of the cam
        speed = (cam.transform.position.y + 10 / 1.5f);

        //zoom
        minZoom = 10;
        maxZoom = 30;
        zoomSpeed = 10;
    }
    
    void Update()
    {
        #region wasd
        //change camera position within limits
        if (Input.GetKey(KeyCode.W))
        {
            cam.transform.position += Vector3.forward * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            cam.transform.position += Vector3.back * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            cam.transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            cam.transform.position += Vector3.right * speed * Time.deltaTime;
        }
        #endregion

        #region zooming
        Vector3 pos = cam.transform.position;

        //move camera either forward/back and tilt it down/up
        pos += new Vector3(0, -(Input.GetAxis("Mouse ScrollWheel") * zoomSpeed), 0);

        if (pos.y > maxZoom)
        {
            pos.y = maxZoom;
        }
        else if (pos.y < minZoom)
        {
            pos.y = minZoom;
        }

        //update camera move speed depending on zoom
        speed = (pos.y + 10 / 1.5f);

        cam.transform.position = pos;
        #endregion
    }
}
