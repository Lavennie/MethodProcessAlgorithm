using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float rotateSpeed = 1.0f;

    public float speedInThisFrame;

    [SerializeField] private Transform directionArrow;

    private CharacterController cc;
    private Transform dummy;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        dummy = new GameObject("Player Move Dummy").transform;
        ResetDummy();
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
        // it is a barrier
        else if (other.GetComponent<TriggerPlate>() == null)
        {
            LevelManager.ReloadLevel();
        }
    }

    public void Rotate(float angle)
    {
        dummy.Rotate(0, angle, 0);
        // snap to 45 degrees angles
        angle = Vector3.SignedAngle(Vector3.forward, dummy.forward, Vector3.up);
        angle = Mathf.RoundToInt(angle / 45.0f) * 45;
        dummy.rotation = Quaternion.Euler(0, angle, 0);
    }
    public void Rotate(Vector3 direction)
    {
        dummy.forward = direction.normalized;
    }
    public void Move(Vector3 direction)
    {
        if (direction.magnitude > 1.0f)
        {
            direction.Normalize();
        }

        direction.y = 0;
        if (direction == Vector3.zero) { return; }
        direction = dummy.TransformDirection(direction);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), rotateSpeed * Time.deltaTime);

        speedInThisFrame = direction.magnitude * moveSpeed;
        cc.Move((direction * moveSpeed + Physics.gravity) * Time.deltaTime);
        dummy.position = transform.position;

        directionArrow.forward = direction;
    }

    public void ResetDummy()
    {
        dummy.position = transform.position;
        dummy.rotation = transform.rotation;
    }

    public SkinnedMeshRenderer Mesh { get { return transform.GetChild(0).GetComponent<SkinnedMeshRenderer>(); } }
}
