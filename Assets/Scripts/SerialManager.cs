using UnityEngine;
using System.IO.Ports;
using System;

public class SerialManager : MonoBehaviour {
    public static SerialManager Instance;
    private SerialPort serialPort;

    [Header("Serial Port Settings")]
    public string portName = "COM3";
    public int baudRate = 115200;

    public event Action<char, float> OnMotorDataReceived;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }

        serialPort = new SerialPort(portName, baudRate);
        serialPort.ReadTimeout = 50;

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
                string data = serialPort.ReadLine().Trim();
                ParseMotorData(data);
            } catch { }
        }

        // Keyboard debug
        if (Input.GetKey(KeyCode.W)) OnMotorDataReceived?.Invoke('Y', 90f);
        if (Input.GetKey(KeyCode.S)) OnMotorDataReceived?.Invoke('Y', -90f);
        if (Input.GetKey(KeyCode.D)) OnMotorDataReceived?.Invoke('X', 180f);
        if (Input.GetKey(KeyCode.A)) OnMotorDataReceived?.Invoke('X', 0f);
    }

    void ParseMotorData(string data) {
        if (string.IsNullOrEmpty(data)) return;

        // ¤À³Î¡G1234X,456Y
        string[] parts = data.Split(',');

        foreach (string part in parts) {
            string p = part.Trim();

            if (p.Length < 2) continue;

            char axis = p[p.Length - 1];
            string valueText = p.Substring(0, p.Length - 1);

            if (float.TryParse(valueText, out float value)) {
                Debug.Log($"[Serial] axis={axis}, value={value}");
                OnMotorDataReceived?.Invoke(axis, value);
            } else {
                Debug.LogWarning("Invalid part: " + p);
            }
        }
    }

    void OnApplicationQuit() {
        if (serialPort != null && serialPort.IsOpen)
            serialPort.Close();
    }
}