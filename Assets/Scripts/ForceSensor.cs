using UnityEngine;

/// <summary>
/// �Ψ��˴�������쪺�I���O (N)�A���|�v�T����Φ�m
/// </summary>
[RequireComponent(typeof(Collider))]
public class ForceSensor : MonoBehaviour {
    private Rigidbody rb;
    private Vector3 accumulatedImpulse; // ��e���z�V���`�߽�
    private float lastForce;            // �W�@�V�p��X���O

    void Awake() {
        // �p�G�S�� Rigidbody�A�N�۰ʲK�[
        rb = GetComponent<Rigidbody>();
        if (rb == null) {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // �]�� kinematic�A�קK�z�Z���u�� Transform
        rb.isKinematic = true;
        rb.useGravity = false;

        // �ᵲ�Ҧ��b
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    void FixedUpdate() {
        // �ھ� impulse �p��O (N)
        lastForce = accumulatedImpulse.magnitude / Time.fixedDeltaTime;

        // ���m�ֿn�߽�
        accumulatedImpulse = Vector3.zero;
    }

    void OnCollisionStay(Collision collision) {
        // �֥[�Ӫ��z�V�����Ҧ��I�� impulse
        accumulatedImpulse += collision.impulse;
    }

    void OnCollisionEnter(Collision collision) {
        accumulatedImpulse += collision.impulse;
    }

    /// <summary>
    /// ����̷s�O���j�p (���y)
    /// </summary>
    public float GetCurrentForce() {
        Debug.Log($"Current Force: {lastForce:F2} N");
        return lastForce;
    }

    void OnGUI() {
        // Debug ��ܤO
        GUI.Label(new Rect(10, 10, 300, 30), $"Force: {lastForce:F2} N");
    }
}
