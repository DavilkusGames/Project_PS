using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 4.0f;
    public float zoomSpeed = 2.0f;
    private Transform trans;

    private void Start()
    {
        trans = transform;
    }

    private void Update()
    {
        Vector3 moveVector = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, 
            -Input.GetAxis("Mouse ScrollWheel")  * zoomSpeed, 
            Input.GetAxis("Vertical") * moveSpeed);
        trans.position += moveVector * Time.deltaTime;
    }
}
