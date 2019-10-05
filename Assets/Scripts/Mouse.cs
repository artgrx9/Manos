using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    public float distance = 1.0f;
    //public bool useInitialCameraDistance = false;
    //private float actualDistance;

    // Start is called before the first frame update
    void Start()
    {
        //Código para hacer que el mouse siga a la cámara

        //if (useInitialCameraDistance)
        {
            //Vector3 toObjectVector = transform.position - Camera.main.transform.position;
            //Vector3 linearDistanceVector = Vector3.Project(toObjectVector, Camera.main.transform.forward);
            //actualDistance = linearDistanceVector.magnitude;
        }
        //else
        {
            //actualDistance = distance;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //float actualDistance;
        //if (useInitialCameraDistance)
        {
            //actualDistance = (transform.position - Camera.main.transform.position).magnitude;
        }
        //else
        {
            //actualDistance = distance;
        }

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = distance;
        transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
