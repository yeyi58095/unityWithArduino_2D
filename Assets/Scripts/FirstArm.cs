using UnityEngine;

public class FirstArm : MonoBehaviour {
    public float rotateSpeed = 30f;  // 每秒旋轉速度 (度/秒)
    private float currentAngle = -180f; // 當前旋轉角度（0 表示水平）
    private float radius;

    void Start() {
        // 計算半徑（Cylinder 高度的一半）
        radius = GetComponent<CapsuleCollider>().height * 0.5f;
        transform.position = new Vector3(radius, 0f, 0f); // 初始位置
    }

    void Update() {
        float input = 0f;
        if (Input.GetKey(KeyCode.W)) {
            input = -1f;
        } else if (Input.GetKey(KeyCode.S)) {
            input = 1f;
        }

        // 更新角度
        currentAngle += input * rotateSpeed * Time.deltaTime ;

        // 計算新位置 (以原點為圓心繞Z軸旋轉)
        float rad = currentAngle * Mathf.Deg2Rad;
        transform.position = new Vector3(radius * Mathf.Cos(rad), radius * Mathf.Sin(rad), 0f);

        // 設定朝向 (讓物體朝著圓心外的方向)
        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle + 90f);
        Debug.Log(currentAngle);
    }
}
