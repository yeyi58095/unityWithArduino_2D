/*int value = 0;   // 起始值
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
}*/

/*int value = 0;
int step = 10;

void setup() {
  Serial.begin(115200);
}

void loop() {
  Serial.print(value);
  Serial.println("X");

  value += step;

  if (value >= 3072) {
    value = 3072;
    step = -step;
  }

  if (value <= 0) {
    value = 0;
    step = -step;
  }

  delay(20);
}*/

int xValue = 0;
int xStep = 10;

int yValue = -1000;
int yStep = 10;

void setup() {
  Serial.begin(115200);
}

void loop() {
  Serial.print(xValue);
  Serial.print("X,");
  Serial.print(yValue);
  Serial.println("Y");

  xValue += xStep;
  yValue += yStep;

  if (xValue >= 3072) {
    xValue = 3072;
    xStep = -xStep;
  }

  if (xValue <= 0) {
    xValue = 0;
    xStep = -xStep;
  }

  if (yValue >= 1000) {
    yValue = 1000;
    yStep = -yStep;
  }

  if (yValue <= -1000) {
    yValue = -1000;
    yStep = -yStep;
  }

  delay(20);
}