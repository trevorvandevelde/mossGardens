using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class JoystickLocomotion : MonoBehaviour
{

    public GameObject player;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var joystickAxis = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.LTouch);
        float fixedY = player.transform.position.y;

        player.transform.position += (transform.right * joystickAxis.x + transform.forward * joystickAxis.y) * Time.deltaTime * speed;
        player.transform.position = new Vector3(player.transform.position.x, fixedY, player.transform.position.z);
    }
}
