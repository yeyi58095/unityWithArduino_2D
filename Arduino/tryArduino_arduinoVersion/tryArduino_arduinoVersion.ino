void setup() {
  Serial.begin(9600); // 初始化串口
}

void loop() {
  Serial.println("Hello Unity!123"); // 傳送字串
  delay(1000); // 每 1 秒傳一次
}
