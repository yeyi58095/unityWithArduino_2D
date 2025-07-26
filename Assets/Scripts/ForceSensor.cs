using UnityEngine;

/// <summary>
/// 用來檢測物體受到的碰撞力 (N)，不會影響旋轉或位置
/// </summary>
[RequireComponent(typeof(Collider))]
public class ForceSensor : MonoBehaviour {
    private Rigidbody rb;
    private Vector3 accumulatedImpulse; // 當前物理幀的總脈衝
    private float lastForce;            // 上一幀計算出的力

    void Awake() {
        // 如果沒有 Rigidbody，就自動添加
        rb = GetComponent<Rigidbody>();
        if (rb == null) {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // 設為 kinematic，避免干擾手臂的 Transform
        rb.isKinematic = true;
        rb.useGravity = false;

        // 凍結所有軸
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    void FixedUpdate() {
        // 根據 impulse 計算力 (N)
        lastForce = accumulatedImpulse.magnitude / Time.fixedDeltaTime;

        // 重置累積脈衝
        accumulatedImpulse = Vector3.zero;
    }

    void OnCollisionStay(Collision collision) {
        // 累加該物理幀內的所有碰撞 impulse
        accumulatedImpulse += collision.impulse;
    }

    void OnCollisionEnter(Collision collision) {
        accumulatedImpulse += collision.impulse;
    }

    /// <summary>
    /// 獲取最新力的大小 (牛頓)
    /// </summary>
    public float GetCurrentForce() {
        Debug.Log($"Current Force: {lastForce:F2} N");
        return lastForce;
    }

    void OnGUI() {
        // Debug 顯示力
        GUI.Label(new Rect(10, 10, 300, 30), $"Force: {lastForce:F2} N");
    }
}
