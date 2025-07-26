
using UnityEngine;

public class SecondArm : MonoBehaviour {
    public Transform firstArm;     // 參考第一節手臂
    public float moveSpeed = 2f;   // 每秒伸縮速度
    public float maxLength = 0.5f; // 最大伸長距離

    private float currentLength = 0f;
    private float input;

    void Start() {
        // 訂閱 SerialManager 事件
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
        // 沿著第一節手臂的方向 (假設第一節的長軸是 X 軸)
        //Vector3 dir = firstArm.TransformDirection(Vector3.right);
        transform.localPosition = new Vector3(0f, -currentLength, 0f);
    }
}
