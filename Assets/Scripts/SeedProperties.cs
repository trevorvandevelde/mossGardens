using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedProperties : MonoBehaviour
{
    public GameObject prefabFlower;

    public void Plant(Vector3 plantPos)
    {
        Instantiate(GetComponent<SeedProperties>().prefabFlower, plantPos, Quaternion.identity);
        Destroy(gameObject);
    }
}
