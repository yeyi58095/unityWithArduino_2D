using UnityEngine;
using UnityEngine.Windows;

public class FirstArm : MonoBehaviour {
    public float rotateSpeed = 30f;
    private float currentAngle = -180f;
    float input = 0f;
    void OnEnable() {
        if (SerialManager.Instance != null)
            SerialManager.Instance.OnCommandReceived += HandleCommand;
    }

    void OnDisable() {
        if (SerialManager.Instance != null)
            SerialManager.Instance.OnCommandReceived -= HandleCommand;
    }

    void HandleCommand(string commend) {

        float radius = (GetComponent<CapsuleCollider>().height * 0.5f) * transform.localScale.y;

        input = 0f;

        Debug.Log("Command received: " + commend);
        if (commend == "W") {
            input = -1f;
        } else if (commend == "S") {
            input = 1f;
        }

        // ��s����
        currentAngle += input * rotateSpeed * Time.deltaTime;

        // �p��s��m (�H���I�����¶Z�b����)
        float rad = currentAngle * Mathf.Deg2Rad;
        transform.position = new Vector3(radius * Mathf.Cos(rad), radius * Mathf.Sin(rad), 0f);

        // �]�w�¦V (������µ۶�ߥ~����V)
        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle + 90f);
        // Debug.Log(currentAngle);
    }
}
