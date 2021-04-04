using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    private Transform player;
    private Renderer rend;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        rend.sharedMaterial.SetVector("_PlayerPos", player.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 bounds = GetComponent<Renderer>().bounds.size;
        //Gizmos.DrawWireCube(transform.position, bounds);
        Gizmos.DrawWireMesh(GetComponent<MeshFilter>().sharedMesh, transform.position, transform.rotation, transform.localScale);
        Gizmos.DrawLine(transform.position + transform.forward, transform.position - transform.forward);
        Gizmos.DrawLine(transform.position + transform.right, transform.position - transform.right);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }
}
