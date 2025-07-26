using UnityEngine;
using System.IO.Ports;
using System;

public class SerialManager : MonoBehaviour {
    public static SerialManager Instance;
    private SerialPort serialPort;

    [Header("Serial Port Settings")]
    public string portName = "COM3";
    public int baudRate = 9600;

    // �ƥ�G���s���O��Ĳ�o
    public event Action<string> OnCommandReceived;

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
        } catch (Exception e) {
            Debug.LogError("Failed to open serial port: " + e.Message);
        }
    }

    void Update() {
        if (serialPort != null && serialPort.IsOpen && serialPort.BytesToRead > 0) {
            try {
                string cmd = serialPort.ReadLine().Trim();
                //Debug.Log("Serial Command from Manager: " + cmd);
                OnCommandReceived?.Invoke(cmd); // �s�����q�\��
            } catch { }
        }


        // debug mode for only keyboard input  
        if (Input.GetKey(KeyCode.W)) OnCommandReceived?.Invoke("W");
        if (Input.GetKey(KeyCode.S)) OnCommandReceived?.Invoke("S");
        if (Input.GetKey(KeyCode.A)) OnCommandReceived?.Invoke("A");
        if (Input.GetKey(KeyCode.D)) OnCommandReceived?.Invoke("D");
    }

    void OnApplicationQuit() {
        if (serialPort != null && serialPort.IsOpen)
            serialPort.Close();
    }
}
