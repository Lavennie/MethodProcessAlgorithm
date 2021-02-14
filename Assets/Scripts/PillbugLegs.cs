using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PillbugLegs : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    [SerializeField] private float loopPoint = 0.0f;
    [SerializeField] private float legSpeed = 2.0f;
    [SerializeField] private float moveDistance = 0.02f;
    [SerializeField] private float liftHeight = 0.02f;
    [SerializeField] private float moveInsideDistance = 0.02f;
    [SerializeField] private AnimationCurve liftCurve;
    [SerializeField] private AnimationCurve moveInsideCurve;

    [SerializeField] private Leg[] legs;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
        foreach (var leg in legs)
        {
            leg.originalPosition = leg.ik.localPosition;
        }
    }

    private void Update()
    {
        foreach (var leg in legs)
        {
            if (player.speedInThisFrame == 0)
            {
                leg.ik.localPosition = leg.originalPosition;
            }
            else
            {
                leg.ik.localPosition = leg.originalPosition + new Vector3(
                    -Mathf.Sign(leg.originalPosition.x) * moveInsideCurve.Evaluate(Mathf.Repeat(loopPoint + leg.pointInLoop + leg.moveNearOffset, 1.0f)) * moveInsideDistance,
                    liftCurve.Evaluate(Mathf.Repeat(loopPoint + leg.pointInLoop, 1.0f)) * liftHeight,
                    (Mathf.PingPong(loopPoint + leg.pointInLoop, 0.5f) * 4.0f - 1.0f) * moveDistance);
            }

            Vector3 end = leg.ik.position - leg.chain[0].position;
            Vector3 axis = Vector3.Cross(end.normalized, transform.up);
            float dEnd = end.magnitude;
            float d1 = Vector3.Distance(leg.chain[0].position, leg.chain[1].position);
            float d2 = Vector3.Distance(leg.chain[1].position, leg.chain[2].position);
            if (dEnd - 0.01f > d1 + d2)
            {
                leg.chain[0].rotation = Quaternion.LookRotation(Vector3.Cross(axis, end.normalized), end.normalized);
                leg.chain[1].rotation = leg.chain[0].rotation;
            }
            else
            {
                float a1 = -Mathf.Acos((d1 * d1 + dEnd * dEnd - d2 * d2) / (2 * d1 * dEnd)) * Mathf.Rad2Deg + 90.0f;
                float a2 = a1 - Mathf.Acos((d1 * d1 + d2 * d2 - dEnd * dEnd) / (2 * d1 * d2)) * Mathf.Rad2Deg - 180.0f;
                Vector3 up1 = Vector3.Cross(Quaternion.AngleAxis(a1, axis) * end.normalized, axis);
                Vector3 up2 = Vector3.Cross(Quaternion.AngleAxis(a2, axis) * end.normalized, axis);

                leg.chain[0].rotation = Quaternion.LookRotation(Quaternion.AngleAxis(a1, axis) * end.normalized, up1);
                leg.chain[1].rotation = Quaternion.LookRotation(Quaternion.AngleAxis(a2, axis) * end.normalized, up2);
            }
            leg.chain[2].rotation = transform.rotation * Quaternion.Euler(0, 90, 180);
        }

        loopPoint = Mathf.Repeat(loopPoint + Time.deltaTime * legSpeed * player.speedInThisFrame, 1.0f);
    }

    [System.Serializable]
    public class Leg
    {
        [Range(0.0f, 1.0f)]
        public float pointInLoop = 0.0f;
        [Range(0.0f, 1.0f)]
        public float moveNearOffset = 0.0f;
        [HideInInspector]
        public Vector3 originalPosition;
        public Transform ik;
        public Transform[] chain;
    }
}
