using UnityEngine;

public class SecondArm : MonoBehaviour {
    public char controlAxis = 'X';

    public float minMotorAngle = 0f;
    public float maxMotorAngle = 180f;

    public float minLength = 0f;
    public float maxLength = 1f;

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

        float t = Mathf.InverseLerp(minMotorAngle, maxMotorAngle, motorAngle);
        float currentLength = Mathf.Lerp(minLength, maxLength, t);

        transform.localPosition = new Vector3(0f, -currentLength, 0f);
    }
}