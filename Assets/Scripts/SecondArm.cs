using UnityEngine;

public class SecondArm : MonoBehaviour {
    public Transform firstArm;     // �ѦҲĤ@�`���u
    public float moveSpeed = 2f;   // �C����Y�t��
    public float maxLength = 0.5f;   // �̤j�����Z��
    
    
    /* debug mode */
    private float currentLength = 0f;
    private Vector3 initialLocalPos;

    void Start() {
        initialLocalPos = transform.localPosition;  // �O���l��m
       // float firstArmLength = firstArm.GetComponent<MeshRenderer>().bounds.size.y;
        //maxLength = firstArmLength / 4f;
    }

    void Update() {
        string command = SerialManager.Instance.ReadCommand();
        float input = 0f;
        if (command == "D") input = 1f;
        else if (command == "A") input = -1f;

        if (currentLength <= 1f) {
            currentLength += input * moveSpeed * Time.deltaTime;
            currentLength = Mathf.Clamp(currentLength, 0f, 1f);
        }
        // �u�۲Ĥ@�`���u����V (���]�Ĥ@�`�����b�O X �b)
        //Vector3 dir = firstArm.TransformDirection(Vector3.right);
        transform.localPosition = new Vector3(0f, -currentLength, 0f);
    }
}
