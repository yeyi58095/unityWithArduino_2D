int value = -1000;   // 起始值
int step = 10;       // 每次變化量（可以調整速度）

void setup() {
  Serial.begin(115200);
}

void loop() {

  // 傳送格式：1234X
  Serial.print(value);
  Serial.println("Y");

  // 更新數值
  value += step;

  // 到邊界就反轉方向
  if (value >= 1000 || value <= -1000) {
    step = -step;
  }

  delay(20); // 控制傳送速度（越小越快）
}