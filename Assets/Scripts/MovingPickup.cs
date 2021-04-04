using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPickup : MonoBehaviour
{
    public Vector3 translation;

    void Update()
    {
        transform.Translate(translation * Time.deltaTime, Space.World);
    }
}
