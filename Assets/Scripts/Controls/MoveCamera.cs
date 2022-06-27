using UnityEngine;

public class MoveCamera : MonoBehaviour{

    private float horizontalSpeed = 1.0F;
    private float verticalSpeed = 1.0F;


    private float cameraMovementSpeed = 25F;
    private float speedMultiply = 1.0F;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private void Update(){
        yaw += horizontalSpeed * Input.GetAxis("Mouse X");// * Time.deltaTime;
        pitch -= verticalSpeed * Input.GetAxis("Mouse Y");// * Time.deltaTime;

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        
        if (Input.GetKey(KeyCode.LeftShift)){
            speedMultiply = 2.0F;
        } else {
            speedMultiply = 1.0F;
        }

        transform.position += GetMovementVector().normalized * cameraMovementSpeed * Time.deltaTime * speedMultiply;
    }

    private Vector3 GetMovementVector(){
        Vector3 movementVector = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            movementVector += transform.forward;
        if (Input.GetKey(KeyCode.S))
            movementVector -= transform.forward;
        if (Input.GetKey(KeyCode.D))
            movementVector += transform.right;
        if (Input.GetKey(KeyCode.A))
            movementVector -= transform.right;
        return movementVector;
    }
}