using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{

    public Transform Player;
    public Vector3 offset = new Vector3(0,5,1);
    public bool IsMoving { set; get; }

    public Vector3 Rotation = new Vector3(35,0, 0);


    private void LateUpdate()
    {

        if (!IsMoving)
            return;


        Vector3 desiredPosition = Player.position + offset;
        desiredPosition.x = 0;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.05f);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Rotation), 0.15f);
    }

}
