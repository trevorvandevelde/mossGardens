using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabber : MonoBehaviour
{
    private bool _grabbingObject;
    private bool _intersectingObject;

    public bool userGrab;
    private GameObject grabbedObject;

    public GameObject seedIndicatorObj; //navigation target
    public Transform leftAnchor; //left oculus controller
    public Inventory inventory;

    float trajectoryHeight = 2;

    // Start is called before the first frame update
    void Start()
    {
        _grabbingObject = false;
        grabbedObject = null;
        seedIndicatorObj.SetActive(false); //navigation target is invinsible unless picked up seed
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!_intersectingObject)
        {
            _intersectingObject = true;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        Ray leftRay = new Ray(leftAnchor.position, leftAnchor.forward); // cast a ray from the controller out towards where it is pointing
        RaycastHit leftHit;

        Vector3 newPosition; 

        if (Physics.Raycast(leftRay, out leftHit, 8))
        {
            if (leftHit.transform.gameObject.CompareTag("ground"))
            {
                // valid object was hit
                newPosition = new Vector3(leftHit.point.x, leftHit.point.y + 0.1f, leftHit.point.z); // WARNING: assumes target is just above ground 
                seedIndicatorObj.transform.position = newPosition;
                if (!seedIndicatorObj.activeSelf) seedIndicatorObj.SetActive(true); // make sure it is visible
            }
            else
            {
                // valid object was not hit
                if (seedIndicatorObj.activeSelf) seedIndicatorObj.SetActive(false); // if nothihng is hit make it invisible
            }
        }
        else
        {
            // valid object was not hit
            if (seedIndicatorObj.activeSelf) seedIndicatorObj.SetActive(false); // if nothihng is hit make it invisible
        }

        
        if (userGrab && (!_grabbingObject))
        {
            _grabbingObject = true;

            if (other.gameObject.CompareTag("grabbable"))
            {
                grabbedObject = other.gameObject;
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true; //While holding, make it not affected by gravity
                grabbedObject.transform.SetParent(transform);
            }
        }
        

        else if ((!userGrab) && (grabbedObject != null))
        {
            seedIndicatorObj.SetActive(false); //turn off pointer
            _grabbingObject = false;
            grabbedObject.transform.SetParent(null);

            // final raycast
            leftRay = new Ray(leftAnchor.position, leftAnchor.forward);
            var hit = Physics.Raycast(leftRay, out leftHit, 8);

            if (!hit || !leftHit.transform.gameObject.CompareTag("ground"))
            {
                inventory.UnGrab();
            }
            else
            {
                grabbedObject.GetComponent<AudioSource>().Play();
                StartCoroutine(TossSeed(grabbedObject.transform.position, seedIndicatorObj.transform.position, 1, grabbedObject));
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (_intersectingObject)
        {
            _intersectingObject = false;
        }
    }

    IEnumerator TossSeed(Vector3 source, Vector3 target, float overTime, GameObject seed)
    {
        float startTime = Time.time;
        while(Time.time < startTime + overTime)
        {
            Vector3 seedTrajectory = Vector3.Lerp(source, target, (Time.time - startTime)/overTime);
            seedTrajectory.y += trajectoryHeight * Mathf.Sin(Mathf.Clamp01((Time.time - startTime)/overTime) * Mathf.PI);
            seed.transform.position = seedTrajectory; 
            
            yield return null;
        }
        seed.transform.position = target;
        PlantSeed(seed, target);
    }

    private void PlantSeed(GameObject seed, Vector3 target)
    {
        seed.GetComponent<SeedProperties>().Plant(target);
    }
}