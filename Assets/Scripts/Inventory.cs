using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject yellowSeed;
    public GameObject purpleSeed;
    public GameObject blueSeed;
    public GameObject pinkSeed;

    private GameObject[] seeds = new GameObject[4];
    private GameObject activeSeed;
    private bool choosingSeed;
    private int currSeedIdx = 0;

    public Transform leftAnchor;

    private void Start()
    {
        seeds[0] = yellowSeed;
        seeds[1] = purpleSeed;
        seeds[2] = blueSeed;
        seeds[3] = pinkSeed;

        choosingSeed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.Y, OVRInput.Controller.Touch)) {
            if (!choosingSeed)
            {
                choosingSeed = true;
                activeSeed = InstantiateSeed(currSeedIdx);
            }
            else
            {
                if (activeSeed)
                {
                    Destroy(activeSeed);
                }
                choosingSeed = false;
            }
        }

        else if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickRight))
        {
            if (choosingSeed)
            {
                currSeedIdx += 1;
                if (currSeedIdx >= seeds.Length)
                {
                    currSeedIdx = 0;
                }
                if (activeSeed)
                {
                    Destroy(activeSeed);
                }
                activeSeed = InstantiateSeed(currSeedIdx);
            }
        }

        else if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickLeft))
        {
            if (choosingSeed)
            {
                currSeedIdx -= 1;
                if (currSeedIdx < 0)
                {
                    currSeedIdx = seeds.Length - 1;
                }
                if (activeSeed)
                {
                    Destroy(activeSeed);
                }
                activeSeed = InstantiateSeed(currSeedIdx);
            }
        }

        else if (OVRInput.GetDown(OVRInput.RawButton.LHandTrigger))
        {
            if (choosingSeed)
            {
                if (activeSeed)
                {
                    activeSeed.transform.position -= 0.15f * leftAnchor.forward;
                }

                choosingSeed = false;
            }
        }
    }

    public void UnGrab()
    {
        if (activeSeed)
        {
            Destroy(activeSeed);
        }
        activeSeed = InstantiateSeed(currSeedIdx);
        choosingSeed = true;
    }

    private GameObject InstantiateSeed(int currSeedIdx)
    {
        activeSeed = Instantiate(seeds[currSeedIdx], leftAnchor.position + (0.2f * leftAnchor.forward), Quaternion.identity);
        activeSeed.GetComponent<Rigidbody>().isKinematic = true;
        activeSeed.transform.SetParent(leftAnchor);

        return activeSeed;
    }
}
