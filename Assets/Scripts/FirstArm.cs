using UnityEngine;

public class FirstArm : MonoBehaviour {
    public float rotateSpeed = 30f;  // 每秒旋轉速度 (度/秒)
    public float minAngle = 0f;      // 最低仰角
    public float maxAngle = 60f;     // 最高仰角

    private float currentAngle = 0f; // 當前仰角
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        float input = 0f;

        if (Input.GetKey(KeyCode.W)) {
            input = 1f;
            Debug.Log("W key pressed, rotating up");
        } else if (Input.GetKey(KeyCode.S)) {
            input = -1f;
            Debug.Log("S key pressed, rotating down");
        }

        // 更新角度
        currentAngle += input * rotateSpeed * Time.deltaTime;
        currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);

        // 設定旋轉
        transform.localRotation = Quaternion.Euler(-currentAngle, 0f, 0f);
    }
}
