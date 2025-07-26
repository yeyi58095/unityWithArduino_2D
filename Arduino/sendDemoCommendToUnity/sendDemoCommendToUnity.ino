void setup() {
  Serial.begin(9600);
}

void loop() {
  // 例如測試: 每秒輪流發送 W 和 S
  Serial.println("W");
  delay(1000);
  Serial.println("D");
  delay(1000);
  Serial.println("S");
  delay(1000);
  Serial.println("A");
  delay(1000);
}
