using UnityEngine;

public class SecondArm : MonoBehaviour {
    public char controlAxis = 'X';

    [Header("Encoder Settings")]
    public float encoderCPR = 1024f;
    public float maxTurns = 3f;

    [Header("Unity Length Settings")]
    public float minLength = 0f;
    public float maxLength = 3f;

    [Header("Direction Calibration")]
    public bool invertDirection = false;

    [Header("Move Direction")]
    public Vector3 localMoveDirection = Vector3.right;

    private Vector3 initialLocalPosition;

    void Start() {
        initialLocalPosition = transform.localPosition;
    }

    void OnEnable() {
        if (SerialManager.Instance != null)
            SerialManager.Instance.OnMotorDataReceived += HandleMotorData;
    }

    void OnDisable() {
        if (SerialManager.Instance != null)
            SerialManager.Instance.OnMotorDataReceived -= HandleMotorData;
    }

    void HandleMotorData(char axis, float encoderCount) {
        if (axis != controlAxis) return;

        float maxEncoderCount = encoderCPR * maxTurns;

        float t = Mathf.InverseLerp(0f, maxEncoderCount, encoderCount);

        if (invertDirection) {
            t = 1f - t;
        }

        float currentLength = Mathf.Lerp(minLength, maxLength, t);

        transform.localPosition =
            initialLocalPosition + localMoveDirection.normalized * currentLength;

        //Debug.Log(
        //    $"[X Axis] encoder={encoderCount}, t={t:F3}, localPos={transform.localPosition}"
        //);
    }
}