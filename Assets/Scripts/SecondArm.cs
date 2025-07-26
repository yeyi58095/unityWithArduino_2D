using UnityEngine;

public class SecondArm : MonoBehaviour {
    public Transform firstArm;     // 參考第一節手臂
    public float moveSpeed = 2f;   // 每秒伸縮速度
    public float maxLength = 0.5f;   // 最大伸長距離
    
    
    /* debug mode */
    private float currentLength = 0f;
    private Vector3 initialLocalPos;

    void Start() {
        initialLocalPos = transform.localPosition;  // 記住初始位置
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
        // 沿著第一節手臂的方向 (假設第一節的長軸是 X 軸)
        //Vector3 dir = firstArm.TransformDirection(Vector3.right);
        transform.localPosition = new Vector3(0f, -currentLength, 0f);
    }
}
