using UnityEngine;

public class FirstArm : MonoBehaviour {
    public char controlAxis = 'Y';
    public float angleOffset = -180f;

    private float currentAngle = -180f;

    void OnEnable() {
        if (SerialManager.Instance != null)
            SerialManager.Instance.OnMotorDataReceived += HandleMotorData;
    }

    void OnDisable() {
        if (SerialManager.Instance != null)
            SerialManager.Instance.OnMotorDataReceived -= HandleMotorData;
    }

    void HandleMotorData(char axis, float motorAngle) {
        if (axis != controlAxis) return;

        currentAngle = motorAngle + angleOffset;

        float radius = (GetComponent<CapsuleCollider>().height * 0.5f) * transform.localScale.y;

        float rad = currentAngle * Mathf.Deg2Rad;
        transform.position = new Vector3(
            radius * Mathf.Cos(rad),
            radius * Mathf.Sin(rad),
            0f
        );

        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle + 90f);
    }
}