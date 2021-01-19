using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Pointer : MonoBehaviour
{
    public float m_DefaultLength = 5.0f;
    public GameObject m_Dot;
    public VRInputModule m_InputModule;
    public Transform anchor;

    private LineRenderer m_LineRenderer = null;

    private void Awake()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        // use default or distance
        //float targetLength = m_DefaultLength;
        PointerEventData data = m_InputModule.GetData();
        float targetLength = data.pointerCurrentRaycast.distance == 0 ? m_DefaultLength : data.pointerCurrentRaycast.distance;

        // Raycast
        RaycastHit hit = CreateRaycast(targetLength);

        // Default
        Vector3 endPosition = anchor.position + (anchor.forward * targetLength);

        // or based on hit
        if (hit.collider != null)
            endPosition = hit.point;

        // set position of the dot
        m_Dot.transform.position = endPosition;

        // set linerenderer
        m_LineRenderer.SetPosition(0, anchor.position + anchor.forward * 0.03f);
        if (endPosition.z - anchor.position.z < 5f)
            m_LineRenderer.SetPosition(1, endPosition);
        else
            m_LineRenderer.SetPosition(1, anchor.position + anchor.forward * 5f);
    }

    private RaycastHit CreateRaycast(float length)
    {
        RaycastHit hit;
        Ray ray = new Ray(anchor.position, anchor.forward);
        Physics.Raycast(ray, out hit, m_DefaultLength);

        return hit;
    }
}