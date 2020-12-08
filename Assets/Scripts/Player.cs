using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1.0f;

    private CharacterController cc;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        Mesh.materials[0].color = ColorPalette.BgNormal;
        Mesh.materials[1].color = ColorPalette.LightColor;
        Mesh.materials[2].color = ColorPalette.SlateNormal;
        Mesh.materials[3].color = ColorPalette.BgDark;
        Mesh.materials[4].color = ColorPalette.BgDark;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            Objective.PickPickup(other.gameObject);
        }
    }

    public void Rotate(float angle)
    {
        transform.Rotate(0, angle * Time.deltaTime, 0);
    }
    public void Move(Vector3 direction)
    {
        direction.y = 0;
        direction.Normalize();
        cc.Move((direction * speed + Physics.gravity) * Time.deltaTime);
    }

    public MeshRenderer Mesh { get { return transform.GetChild(0).GetComponent<MeshRenderer>(); } }
}
