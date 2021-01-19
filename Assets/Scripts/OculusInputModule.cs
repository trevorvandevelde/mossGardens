using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusInputModule : VRInputModule
{
    public OVRInput.Controller m_Source = OVRInput.Controller.RTouch;
    public OVRInput.RawButton m_Click = OVRInput.RawButton.RIndexTrigger;


    public override void Process()
    {
        base.Process();

        // pressed on canvas item
        if (OVRInput.GetDown(m_Click, m_Source))
            ProcessPress(m_Data);

        // Release
        if (OVRInput.GetUp(m_Click, m_Source))
            ProcessRelease(m_Data);

    }
}
