using UnityEngine;

public class FirstArm : MonoBehaviour {
    public float rotateSpeed = 30f;  // �C�����t�� (��/��)
    public float minAngle = 0f;      // �̧C����
    public float maxAngle = 60f;     // �̰�����

    private float currentAngle = 0f; // ��e����
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

        // ��s����
        currentAngle += input * rotateSpeed * Time.deltaTime;
        currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);

        // �]�w����
        transform.localRotation = Quaternion.Euler(-currentAngle, 0f, 0f);
    }
}
