using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float rotateSpeed = 1.0f;

    public float speedInThisFrame;

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

    private void Update()
    {
        speedInThisFrame = 0.0f;
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

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), rotateSpeed * Time.deltaTime);

        speedInThisFrame = direction.magnitude * moveSpeed;
        cc.Move((direction * moveSpeed + Physics.gravity) * Time.deltaTime);
    }

    public SkinnedMeshRenderer Mesh { get { return transform.GetChild(0).GetComponent<SkinnedMeshRenderer>(); } }
}
