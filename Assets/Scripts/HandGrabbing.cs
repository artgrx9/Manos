using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.XR; //needs to be UnityEngine.VR in version before 2017.2

public class HandGrabbing : MonoBehaviour
{
    //public string InputName;
    //public XRNode NodeType;
    public Vector3 ObjectGrabOffset; //(0,-0.15,0)
    public float GrabDistance = 0.2f;
    public string GrabTag = "Grab";
    public float ThrowMultiplier = 1.5f;
    //public Transform Grabbing_Hand;

    private Transform _currentObject;
    private Vector3 _lastFramePosition;

    private Transform Hand;

    //public Animator animator;

    // Use this for initialization
    void Start()
    {
        _currentObject = null;
        _lastFramePosition = transform.position;
        Hand = GameObject.Find("Hand").transform;
        
        MeshRenderer rend_open = Hand.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
        MeshRenderer rend_closed = Hand.GetChild(1).GetChild(0).GetComponent<MeshRenderer>();
        rend_open.enabled = true;
        rend_closed.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        Hand = GameObject.Find("Hand").transform;
        MeshRenderer rend_open = Hand.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
        MeshRenderer rend_closed = Hand.GetChild(1).GetChild(0).GetComponent<MeshRenderer>();


        //update hand position and rotation
        //transform.localPosition = InputTracking.GetLocalPosition(NodeType);
        //transform.localRotation = InputTracking.GetLocalRotation(NodeType);


        //if we don't have an active object in hand, look if there is one in proximity
        if (_currentObject == null)
        {
            //check for colliders in proximity
            Collider[] colliders = Physics.OverlapSphere(transform.position, GrabDistance); //(punto,radio)
            if (colliders.Length > 0)
            {
                //if there are colliders, take the first one if we press the grab button and it has the tag for grabbing
                //Input.GetMouseButtonDown(0) vs Input.GetAxis(InputName) >= 0.01f
                if (Input.GetMouseButton(0) == true && colliders[0].transform.CompareTag(GrabTag))
                {
                    //Change the sprite of the hand
                    //Instantiate(Mano_cerrada, gameObject.transform.position, Quaternion.identity);
                    rend_open.enabled = false;
                    rend_closed.enabled = true;

                    //set current object to the object we have picked up
                    _currentObject = colliders[0].transform;

                    //if there is no rigidbody to the grabbed object attached, add one
                    if (_currentObject.GetComponent<Rigidbody>() == null)
                    {
                        _currentObject.gameObject.AddComponent<Rigidbody>();
                    }

                    //set grab object to kinematic (disable physics)
                    _currentObject.GetComponent<Rigidbody>().isKinematic = true;


                }
            }
        }
        else
        //we have object in hand, update its position with the current hand position (+defined offset from it)
        {
            _currentObject.position = transform.position + ObjectGrabOffset;

            //if we we release grab button, release current object
            //Input.GetMouseButtonDown(0) vs Input.GetAxis(InputName) <= 0.01f
            if (Input.GetMouseButton(0) == false)
            {
                //Change the sprite of the hand
                rend_open.enabled = true;
                rend_closed.enabled = false;

                //set grab object to non-kinematic (enable physics)
                Rigidbody _objectRGB = _currentObject.GetComponent<Rigidbody>();
                _objectRGB.isKinematic = false;

                //calculate the hand's current velocity
                Vector3 CurrentVelocity = (transform.position - _lastFramePosition) / Time.deltaTime;

                //set the grabbed object's velocity to the current velocity of the hand
                _objectRGB.velocity = CurrentVelocity * ThrowMultiplier;

                //release the reference
                _currentObject = null;
            }

        }

        //save the current position for calculation of velocity in next frame
        _lastFramePosition = transform.position;


    }
}
