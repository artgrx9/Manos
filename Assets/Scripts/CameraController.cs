using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour

    //Código que hace el movimiento cíclico de la cámara
{
    public Transform Target;
    public Transform Target2;
    public Vector3 TargetPosition;

    public float Speed = 2;

    // Start is called before the first frame update
    void Start()
    {
        TargetPosition = Target.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(gameObject.transform.position, TargetPosition);
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position,
            TargetPosition,
            (Time.deltaTime * Speed) / distance); //paso

        if (distance < 0.1f)
        {
            if (TargetPosition == Target.position)
            {
                TargetPosition = Target2.position;
            }
            else
            {
                TargetPosition = Target.position;
            }
        }
    }
}
