using UnityEngine;

public class FirstArm : MonoBehaviour {
    public char controlAxis = 'Y';

    [Header("Encoder Settings")]
    public float impulsesPerRound = 2000f;

    [Header("Unity Angle Calibration")]
    public float angleOffset = -180f;
    public bool invertDirection = false;

    private float currentAngle = -180f;

    void OnEnable() {
        if (SerialManager.Instance != null)
            SerialManager.Instance.OnMotorDataReceived += HandleMotorData;
    }

    void OnDisable() {
        if (SerialManager.Instance != null)
            SerialManager.Instance.OnMotorDataReceived -= HandleMotorData;
    }

    void HandleMotorData(char axis, float encoderImpulse) {
        if (axis != controlAxis) return;

        float motorAngle = encoderImpulse / impulsesPerRound * 360f;

        if (invertDirection) {
            motorAngle = -motorAngle;
        }

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