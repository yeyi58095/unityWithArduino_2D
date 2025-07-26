using UnityEngine;
using System.IO.Ports;

public class SerialManager : MonoBehaviour {
    public static SerialManager Instance; // 單例模式
    private SerialPort serialPort;

    [Header("Serial Port Settings")]
    public string portName = "COM5"; // 你的 Arduino COM port
    public int baudRate = 9600;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }

        serialPort = new SerialPort(portName, baudRate);
        try {
            serialPort.Open();
            Debug.Log("Serial port opened: " + portName);
        } catch (System.Exception e) {
            Debug.LogError("Failed to open serial port: " + e.Message);
        }
    }

    void OnApplicationQuit() {
        if (serialPort != null && serialPort.IsOpen)
            serialPort.Close();
    }

    public string ReadCommand() {
        if (serialPort != null && serialPort.IsOpen && serialPort.BytesToRead > 0) {
            try {
                return serialPort.ReadLine().Trim();
            } catch { }
        }
        return null;
    }

    public void SendCommand(string cmd) {
        if (serialPort != null && serialPort.IsOpen) {
            serialPort.WriteLine(cmd);
        }
    }
}
