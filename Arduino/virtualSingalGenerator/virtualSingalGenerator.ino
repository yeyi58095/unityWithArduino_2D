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

/*int xValue = 0;
int xStep = 10;

int yValue = -1000;
int yStep = 10;

void setup() {
  Serial.begin(115200);
}

void loop() {
  Serial.print(xValue);
  Serial.print("X,");
  Serial.print(0);
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
}*/

int xValue = 0;
int xStep = 10;

int yValue = -1000;
int yStep = 10;

unsigned long lastSendTime = 0;
const unsigned long sendInterval = 20;

void setup() {
  Serial.begin(115200);
  Serial.println("Virtual Arduino ready");
}

void loop() {
  sendEncoderData();
  receiveUnityData();
}

void sendEncoderData() {
  unsigned long now = millis();

  if (now - lastSendTime < sendInterval) {
    return;
  }

  lastSendTime = now;

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
}

void receiveUnityData() {
  if (Serial.available() <= 0) {
    return;
  }

  String msg = Serial.readStringUntil('\n');
  msg.trim();

  if (msg.length() == 0) {
    return;
  }

  Serial.print("RAW_FROM_UNITY: ");
  Serial.println(msg);

  parseUnityMessage(msg);
}

void parseUnityMessage(String msg) {
  if (msg.startsWith("C,")) {
    parseCollision(msg);
  } else if (msg.startsWith("R,")) {
    parseRelease(msg);
  } else {
    Serial.print("UNKNOWN_MSG: ");
    Serial.println(msg);
  }
}

void parseCollision(String msg) {
  int p1 = msg.indexOf(',');
  int p2 = msg.indexOf(',', p1 + 1);
  int p3 = msg.indexOf(',', p2 + 1);

  if (p1 < 0 || p2 < 0 || p3 < 0) {
    Serial.println("PARSE_ERROR_COLLISION");
    return;
  }

  String axisText = msg.substring(p1 + 1, p2);
  String angleText = msg.substring(p2 + 1, p3);
  String elasticText = msg.substring(p3 + 1);

  char axis = axisText.charAt(0);
  int angle = angleText.toInt();
  int elastic = elasticText.toInt();

  Serial.println("PARSED_COLLISION");
  Serial.print("Axis: ");
  Serial.println(axis);
  Serial.print("Angle: ");
  Serial.println(angle);
  Serial.print("Elastic: ");
  Serial.println(elastic);
}

void parseRelease(String msg) {
  int p1 = msg.indexOf(',');

  if (p1 < 0 || p1 + 1 >= msg.length()) {
    Serial.println("PARSE_ERROR_RELEASE");
    return;
  }

  String axisText = msg.substring(p1 + 1);
  char axis = axisText.charAt(0);

  Serial.println("PARSED_RELEASE");
  Serial.print("Axis: ");
  Serial.println(axis);
}