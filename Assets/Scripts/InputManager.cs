using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{

    public Transform leftAnchor;        // get the transform of the OculusGo Controller device
    public Transform rightAnchor;
    
    public GameObject indicatorObj; // get the object to use to indicate the proposed teleportation spot
    public GameObject player;

    public float MAX_DISTANCE;
    public float MAX_DISTANCE_HIT = 2f;

    public ControllerGrabber leftGrabber;

    private bool teleportEnabled = true;

    public GameObject debugSphere;
    private Renderer debugRend;
    public Material debugOn;
    public Material debugOff;

    // Use this for initialization
    void Start()
    {
        indicatorObj.SetActive(false);  // indicator is invisible unless the pointer intersects the ground
        debugRend = debugSphere.GetComponent<Renderer>();
        debugRend.material = debugOff;
    }

    // Update is called once per frame
    void Update()
    {
        if (teleportEnabled)
        {
            updateTargetPos();
        }

        checkUserInput();
    }

    private void checkUserInput()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger, OVRInput.Controller.Touch))
        {
            RTriggerDown();
        }

        if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger, OVRInput.Controller.Touch))
        {
            LTriggerDown();
        }

        if (OVRInput.GetDown(OVRInput.RawButton.LHandTrigger, OVRInput.Controller.Touch))
        {
            leftGrabber.userGrab = true;
        }

        if (OVRInput.GetUp(OVRInput.RawButton.LHandTrigger, OVRInput.Controller.Touch))
        {
            leftGrabber.userGrab = false;
        }
    }

    private void updateTargetPos()
    {
        Ray rightRay = new Ray(rightAnchor.position, rightAnchor.forward); // cast a ray from the controller out towards where it is pointing
        RaycastHit rightHit;

        if (Physics.Raycast(rightRay, out rightHit, MAX_DISTANCE))
        {
            if (rightHit.transform.gameObject.CompareTag("ground"))
            {
                // valid object was hit
                Vector3 newPosition = new Vector3(rightHit.point.x, rightHit.point.y + 0.1f, rightHit.point.z); // WARNING: assumes target is just above ground 
                indicatorObj.transform.position = newPosition;
                if (!indicatorObj.activeSelf) indicatorObj.SetActive(true); // make sure it is visible
            }
            else
            {
                // valid object was not hit
                if (indicatorObj.activeSelf) indicatorObj.SetActive(false); // if nothihng is hit make it invisible
            }
        }
        else
        {
            // valid object was not hit
            if (indicatorObj.activeSelf) indicatorObj.SetActive(false); // if nothihng is hit make it invisible
        }
    }

    public void disableTeleport()
    {
        teleportEnabled = false;
        indicatorObj.SetActive(false);
    }

    public void enableTeleport()
    {
        teleportEnabled = true;
    }

    // function called when user pulls trigger
    private void LTriggerDown()
    {
        Ray ray = new Ray(leftAnchor.position, leftAnchor.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, MAX_DISTANCE_HIT))
        {
            if (hit.transform.gameObject.CompareTag("flower"))
            {
                Destroy(hit.transform.gameObject);
            }
        }
    }

    private void RTriggerDown()
    {
        // refresh hit to get exact location for teleportation
        Ray ray = new Ray(rightAnchor.position, rightAnchor.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, MAX_DISTANCE))
        {
            if (hit.transform.gameObject.CompareTag("ground"))
            {
                //transform the player to the hit position (X and Z plane only)
                Vector3 newpos = new Vector3(hit.point.x, hit.point.y + 1f, hit.point.z); // WARNING: assumes only moving along the ground plane
                player.transform.position = newpos;
            }
        }
    }
}