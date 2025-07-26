
using UnityEngine;

public class SecondArm : MonoBehaviour {
    public Transform firstArm;     // �ѦҲĤ@�`���u
    public float moveSpeed = 2f;   // �C����Y�t��
    public float maxLength = 0.5f; // �̤j�����Z��

    private float currentLength = 0f;
    private float input;

    void Start() {
        // �q�\ SerialManager �ƥ�
        if (SerialManager.Instance != null)
            SerialManager.Instance.OnCommandReceived += HandleCommand;
    }

    void OnDestroy() {
        if (SerialManager.Instance != null)
            SerialManager.Instance.OnCommandReceived -= HandleCommand;
    }

    void HandleCommand(string command) {
        if (command == "D") {
            input = 1f;
        } else if (command == "A") {
            input = -1f;
        } else {
            input = 0f;
        }
    }

    void Update() {
        

        if (currentLength <= 1f) {
            currentLength += input * moveSpeed * Time.deltaTime;
            currentLength = Mathf.Clamp(currentLength, 0f, 1f);
        }
        // �u�۲Ĥ@�`���u����V (���]�Ĥ@�`�����b�O X �b)
        //Vector3 dir = firstArm.TransformDirection(Vector3.right);
        transform.localPosition = new Vector3(0f, -currentLength, 0f);
    }
}
