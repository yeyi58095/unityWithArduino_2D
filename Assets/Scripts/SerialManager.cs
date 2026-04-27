using UnityEngine;
using System.IO.Ports;
using System;

public class SerialManager : MonoBehaviour {
    public static SerialManager Instance;
    private SerialPort serialPort;

    [Header("Serial Port Settings")]
    public string portName = "COM3";
    public int baudRate = 115200;

    [Header("Axis Detection")]
    public float xChangeThreshold = 1f;
    public float yChangeThreshold = 1f;

    public event Action<char, float> OnMotorDataReceived;

    private float lastXValue = 0f;
    private float lastYValue = 0f;

    private bool hasXValue = false;
    private bool hasYValue = false;

    private char currentActiveAxis = 'N';

    public float GetCurrentXValue() {
        return lastXValue;
    }

    public float GetCurrentYValue() {
        return lastYValue;
    }

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

                //Debug.Log(data);
                // Check if this is Arduino log
                if (data.StartsWith("[Arduino")) {
                    Debug.Log(data);
                    return;
                }

                // Otherwise parse as motor data
                
                ParseMotorData(data);

            } catch { }
        }
    }

    void ParseMotorData(string data) {
        if (string.IsNullOrEmpty(data)) return;

        string[] parts = data.Split(',');

        float xDelta = 0f;
        float yDelta = 0f;

        bool receivedX = false;
        bool receivedY = false;

        foreach (string part in parts) {
            string p = part.Trim();

            if (p.Length < 2) continue;

            char axis = p[p.Length - 1];
            string valueText = p.Substring(0, p.Length - 1);

            if (!float.TryParse(valueText, out float value)) {
                Debug.LogWarning("Invalid serial data part: " + p);
                continue;
            }

            if (axis == 'X') {
                if (hasXValue) {
                    xDelta = Mathf.Abs(value - lastXValue);
                }

                lastXValue = value;
                hasXValue = true;
                receivedX = true;

                OnMotorDataReceived?.Invoke('X', value);
            } else if (axis == 'Y') {
                if (hasYValue) {
                    yDelta = Mathf.Abs(value - lastYValue);
                }

                lastYValue = value;
                hasYValue = true;
                receivedY = true;

                OnMotorDataReceived?.Invoke('Y', value);
            }
        }

        UpdateActiveAxis(receivedX, receivedY, xDelta, yDelta);
    }

    void UpdateActiveAxis(bool receivedX, bool receivedY, float xDelta, float yDelta) {
        bool xMoved = receivedX && xDelta >= xChangeThreshold;
        bool yMoved = receivedY && yDelta >= yChangeThreshold;

        if (xMoved && yMoved) {
            currentActiveAxis = xDelta >= yDelta ? 'X' : 'Y';
        } else if (xMoved) {
            currentActiveAxis = 'X';
        } else if (yMoved) {
            currentActiveAxis = 'Y';
        }
    }

    public char GetCurrentActiveAxis() {
        return currentActiveAxis;
    }

    public void SendData(string msg) {
        Debug.Log("[Unity Send] " + msg);

        if (serialPort != null && serialPort.IsOpen) {
            serialPort.WriteLine(msg);
        } else {
            Debug.LogWarning("Serial port is not open.");
        }
    }

    void OnApplicationQuit() {
        if (serialPort != null && serialPort.IsOpen) {
            serialPort.Close();
        }
    }
}