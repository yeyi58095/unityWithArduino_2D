using UnityEngine;

public class FirstArm : MonoBehaviour {
    public float rotateSpeed = 30f;  // �C�����t�� (��/��)
    private float currentAngle = -180f; // ��e���ਤ�ס]0 ��ܤ����^
    private float radius;

    void Start() {
        // �p��b�|�]Cylinder ���ת��@�b�^
        radius = GetComponent<CapsuleCollider>().height * 0.5f;
        transform.position = new Vector3(radius, 0f, 0f); // ��l��m
    }

    void Update() {
        float input = 0f;
        if (Input.GetKey(KeyCode.W)) {
            input = -1f;
        } else if (Input.GetKey(KeyCode.S)) {
            input = 1f;
        }

        // ��s����
        currentAngle += input * rotateSpeed * Time.deltaTime ;

        // �p��s��m (�H���I�����¶Z�b����)
        float rad = currentAngle * Mathf.Deg2Rad;
        transform.position = new Vector3(radius * Mathf.Cos(rad), radius * Mathf.Sin(rad), 0f);

        // �]�w�¦V (������µ۶�ߥ~����V)
        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle + 90f);
        Debug.Log(currentAngle);
    }
}
