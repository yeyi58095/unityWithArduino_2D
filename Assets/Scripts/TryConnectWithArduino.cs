using System.IO.Ports;
using UnityEngine;

public class TryConnectWithArduino : MonoBehaviour {
    SerialPort sp = new SerialPort("COM4", 9600); // COM3 換成你的 Arduino 埠

    void Start() {
        Debug.Log("Connecting to Arduino...");
        if (!sp.IsOpen) {
            Debug.Log("Opening COM port...");
            sp.Open();  // 開啟 COM port
            sp.ReadTimeout = 100; // 設定超時
        }
    }

    void Update() {
        if (sp.IsOpen) {
            try {
                string data = sp.ReadLine(); // 讀取一行字串
                Debug.Log("Arduino: " + data);
            } catch (System.Exception) {
                // 忽略超時錯誤
                Debug.Log("Read timeout or error occurred.");
            }
        } else {
            Debug.Log("fuck, COM port is not open.");
        }
    }

    void OnApplicationQuit() {
        if (sp.IsOpen)
            sp.Close(); // 關閉埠
    }
}