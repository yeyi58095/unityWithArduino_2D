using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HapticObject : MonoBehaviour {
    [Header("Haptic Parameters")]
    public float elasticCoefficient = 1.0f;

    [Header("Debug Settings")]
    public bool showDebug = true;

    private bool isColliding = false;

    void OnCollisionEnter(Collision collision) {
        HandleCollision(collision, "ENTER");
    }

    void OnCollisionStay(Collision collision) {
        HandleCollision(collision, "STAY");
    }

    void OnCollisionExit(Collision collision) {
        ArmCollisionTag armTag = collision.gameObject.GetComponent<ArmCollisionTag>();

        if (armTag == null) return;

        isColliding = false;

        if (showDebug) {
            Debug.Log(
                $"[HAPTIC RELEASE] " +
                $"Axis={armTag.axis}, " +
                $"Object={gameObject.name}, " +
                $"Collision Released"
            );
        }
    }

    void HandleCollision(Collision collision, string state) {
        ArmCollisionTag armTag = collision.gameObject.GetComponent<ArmCollisionTag>();

        if (armTag == null) return;

        isColliding = true;

        ContactPoint contact = collision.contacts[0];
        Vector3 contactPoint = contact.point;

        float collisionAngle = Mathf.Atan2(contactPoint.y, contactPoint.x) * Mathf.Rad2Deg;

        if (showDebug) {
            Debug.Log(
                $"[HAPTIC COLLISION {state}] " +
                $"Axis={armTag.axis}, " +
                $"Object={gameObject.name}, " +
                $"ElasticCoefficient={elasticCoefficient:F2}, " +
                $"CollisionAngle={collisionAngle:F2} deg"
            );
        }
    }
}