using System.IO.Ports;
using UnityEngine;

public class TryConnectWithArduino : MonoBehaviour {
    SerialPort sp = new SerialPort("COM4", 9600); // COM3 �����A�� Arduino ��

    void Start() {
        Debug.Log("Connecting to Arduino...");
        if (!sp.IsOpen) {
            Debug.Log("Opening COM port...");
            sp.Open();  // �}�� COM port
            sp.ReadTimeout = 100; // �]�w�W��
        }
    }

    void Update() {
        if (sp.IsOpen) {
            try {
                string data = sp.ReadLine(); // Ū���@��r��
                Debug.Log("Arduino: " + data);
            } catch (System.Exception) {
                // �����W�ɿ��~
                Debug.Log("Read timeout or error occurred.");
            }
        } else {
            Debug.Log("fuck, COM port is not open.");
        }
    }

    void OnApplicationQuit() {
        if (sp.IsOpen)
            sp.Close(); // ������
    }
}