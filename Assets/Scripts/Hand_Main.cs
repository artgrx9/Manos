using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand_Main : MonoBehaviour
{
    public float distance = 1.0f;   //Offset de la mano respecto a la cámara
    public float temp = 1.0f;              //Temp para la rueda de Scroll

    public Vector3 ObjectGrabOffset;        //(0,-0.15,0)
    public float GrabDistance = 0.2f;       //Radio desde la posición de la mano de la cuál puedes agarrar objetos
    public string GrabTag = "Grab";         //Etiqueta que deben de tener los objetos para poder ser agarrados
    public float ThrowMultiplier = 1.5f;    //(Para la física del objeto agarrado) "Fuerza" con la que es lanzado al ser soltado
    private Transform _currentObject;       //Variable para el objeto agarrado
    private Vector3 _lastFramePosition;     //Variable para la posición de la mano en el frame anterior

    private Transform Hand;     //Variable para recuperar los atributos de los modelos de la mano

    void Start()
    {
        _currentObject = null;      //Significa que no tenenmos ningún objeto agarrado
        _lastFramePosition = transform.position;        //recupera la posición

        ObjectGrabOffset.Set(0.0f, -0.22f, 0.0f);       //offset para que objeto no se encime con la mano

        Hand = GameObject.Find("Hand").transform;       //Recupera el Empty Object "Hand "que contiene ambos modelos de mano
        MeshRenderer rend_open = Hand.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();     //GetChild(0) = Hand_Open
        MeshRenderer rend_closed = Hand.GetChild(1).GetChild(0).GetComponent<MeshRenderer>();   //GetChild(1) = Grabbing_Hand
        rend_open.enabled = true;       //Al inicio, empezar con la mano abierta
        rend_closed.enabled = false;
    }


    void Update()
    {
        
        //Recuperar los inputs de posición del mouse
        Vector3 mousePosition = Input.mousePosition;
        
        //if (Input.mouseScrollDelta.y != 0)
        //{
        //    temp = transform.position.z;
        //    //mousePosition.z = Input.mouseScrollDelta.y + distance;
        //    mousePosition.z++;
        //}
        //else
        //{
        //    mousePosition.z = temp;
        //}
        //mousePosition.z = distance + Input.mouseScrollDelta.y; //mousePosition.z += Input.mouseScrollDelta.y;
        //float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        mousePosition.z = v;
        //gameObject.transform.Translate(new Vector3(h * .3f, v * .3f, 0));
        
        /*
         * //Modificaciones del Profe
        var pos = transform.position;
        pos.z += v;
        var mPos = Camera.main.ScreenToWorldPoint(mousePosition);
        pos.x = mPos.x;
        pos.y = mPos.y;
        Debug.Log(mPos);
        transform.position = pos;
        */

        //Camera.main.ScreentToWorldPoint() -> hace que la mano esté referenciada respecto a la cámara
        //transform.position = ... -> asigna la posición de la cámara (la del mouse en X,Y + offset (distance) en Z) a la mano
        transform.position = Camera.main.ScreenToWorldPoint(mousePosition);



        //Mismos códigos que en Start()
        Hand = GameObject.Find("Hand").transform;
        MeshRenderer rend_open = Hand.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
        MeshRenderer rend_closed = Hand.GetChild(1).GetChild(0).GetComponent<MeshRenderer>();


        //Si no hay un objeto agarrado, permitir buscar uno para agarrar
        if (_currentObject == null)
        {
            //Checar si hay colliders en la proximidad
            Collider[] colliders = Physics.OverlapSphere(transform.position, GrabDistance); //(punto,radio)
            if (colliders.Length > 0)
            {
                //Revisar collider, tag y que haya click del mouse 
                if (Input.GetMouseButton(0) == true && colliders[0].transform.CompareTag(GrabTag))
                {
                    //Cambiar de modelo de mano -> abierta
                    rend_open.enabled = false;
                    rend_closed.enabled = true;

                    //Definir "_currentobject" como el que hemos agarrado
                    _currentObject = colliders[0].transform;

                    //Si el objeto agarrado no tiene "RigidBody", agregárselo
                    if (_currentObject.GetComponent<Rigidbody>() == null)
                    {
                        _currentObject.gameObject.AddComponent<Rigidbody>();
                    }

                    //Definir el objeto agarrado cómo cinemático (para poder agarrarlo) (desactiva la física)
                    _currentObject.GetComponent<Rigidbody>().isKinematic = true;


                }
            }
        }
        else
        //Tenemos un objeto agarrado; actualizar su posición con la posición de la mano + offset predefinido
        {
            _currentObject.position = transform.position + ObjectGrabOffset;

            //Si se deja de hacer click, soltar el objeto
            if (Input.GetMouseButton(0) == false)
            {
                //Cambiar de modelo de mano -> cerrada
                rend_open.enabled = true;
                rend_closed.enabled = false;

                //Definir el objeto soltado cómo no-cinemático (para caer/ser lanzado) (activa la física)
                Rigidbody _objectRGB = _currentObject.GetComponent<Rigidbody>();
                _objectRGB.isKinematic = false;

                //Calcular la velocidad actual de la mano
                Vector3 CurrentVelocity = (transform.position - _lastFramePosition) / Time.deltaTime;

                //Definir la velocidad del objeto a soltar a la velocidad de la mano multiplicado por la "Fuerza" para lanzarlo
                _objectRGB.velocity = CurrentVelocity * ThrowMultiplier;

                //Indicar que no se tiene ningún objeto agarrado
                _currentObject = null;
            }

        }

        //Guarda la posición actual para el cálculo de velocidad en el siguiente Frame
        _lastFramePosition = transform.position;


    }


}

