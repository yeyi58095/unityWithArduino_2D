using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HapticObject : MonoBehaviour {
    [Header("Haptic Parameters")]
    public float elasticCoefficient = 1.0f;

    [Header("Debug Settings")]
    public bool showDebug = true;

    private bool isColliding = false;
    private char currentAxis = 'N';

    void OnTriggerEnter(Collider other) {
        if (!IsArmObject(other)) return;
        if (isColliding) return;

        char detectedAxis = GetDetectedAxis();

        if (detectedAxis == 'N') {
            Debug.LogWarning("Collision detected, but active axis is unknown.");
            return;
        }

        isColliding = true;
        currentAxis = detectedAxis;

        int positionValue = GetCurrentAxisValue(currentAxis);
        int espInt = Mathf.RoundToInt(elasticCoefficient * 10f);

        string msg = $"C,{currentAxis},{positionValue},{espInt}";

        if (showDebug) {
            Debug.Log(msg);
        }

        SendToArduino(msg);
    }

    void OnTriggerStay(Collider other) {
        // Keep the first collision data.
    }

    void OnTriggerExit(Collider other) {
        if (!IsArmObject(other)) return;
        if (!isColliding) return;

        isColliding = false;

        string msg = $"R,{currentAxis}";

        if (showDebug) {
            Debug.Log(msg);
        }

        SendToArduino(msg);

        currentAxis = 'N';
    }

    bool IsArmObject(Collider other) {
        return other.GetComponent<ArmCollisionTag>() != null;
    }

    char GetDetectedAxis() {
        if (SerialManager.Instance == null) {
            return 'N';
        }

        return SerialManager.Instance.GetCurrentActiveAxis();
    }

    int GetCurrentAxisValue(char axis) {
        if (SerialManager.Instance == null) {
            return 0;
        }

        if (axis == 'X') {
            return Mathf.RoundToInt(SerialManager.Instance.GetCurrentXValue());
        }

        if (axis == 'Y') {
            return Mathf.RoundToInt(SerialManager.Instance.GetCurrentYValue());
        }

        return 0;
    }

    void SendToArduino(string msg) {
        if (SerialManager.Instance != null) {
            SerialManager.Instance.SendData(msg);
        }
    }
}