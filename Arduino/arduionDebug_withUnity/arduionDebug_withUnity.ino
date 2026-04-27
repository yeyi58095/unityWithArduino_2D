volatile long encoder1Count = 0;
volatile long encoder2Count = 0;

// Encoder pins
const int ENC1_A = 2;
const int ENC1_B = 4;

const int ENC2_A = 3;
const int ENC2_B = 7;

// Motor 1 = Y axis
const int ENA = 5;
const int IN1 = 8;
const int IN2 = 9;

// Motor 2 = X axis
const int ENB = 6;
const int IN3 = 11;
const int IN4 = 10;

// Base P gain
float baseKpY = 15.0;
float baseKpX = 0.1;

// Collision state
bool collisionX = false;
bool collisionY = false;

long contactX = 0;
long contactY = 0;

float elasticX = 1.0;
float elasticY = 1.0;

// Serial send timing
unsigned long lastSendTime = 0;
const unsigned long sendInterval = 20;

// Serial receive buffer
char rxBuffer[64];
int rxIndex = 0;

void setup() {
  Serial.begin(115200);

  pinMode(ENC1_A, INPUT_PULLUP);
  pinMode(ENC1_B, INPUT_PULLUP);
  pinMode(ENC2_A, INPUT_PULLUP);
  pinMode(ENC2_B, INPUT_PULLUP);

  pinMode(ENA, OUTPUT);
  pinMode(IN1, OUTPUT);
  pinMode(IN2, OUTPUT);

  pinMode(ENB, OUTPUT);
  pinMode(IN3, OUTPUT);
  pinMode(IN4, OUTPUT);

  attachInterrupt(digitalPinToInterrupt(ENC1_A), readEncoder1, CHANGE);
  attachInterrupt(digitalPinToInterrupt(ENC2_A), readEncoder2, CHANGE);

  stopMotor1();
  stopMotor2();

  Serial.println("[Arduino log] Haptic controller start");
  Serial.println("[Arduino log] Protocol: C,X,pos,elastic or R,X");
}

void loop() {
  handleSerial();
  updateHapticControl();
  sendEncoderData();
}

void updateHapticControl() {
  long pos1, pos2;

  noInterrupts();
  pos1 = encoder1Count;
  pos2 = encoder2Count;
  interrupts();

  // pos1 = Y axis
  // pos2 = X axis

  if (collisionY) {
    long errorY = pos1 - contactY;
    float cmdY = -baseKpY * elasticY * errorY;
    driveMotor1(cmdY);
  } else {
    stopMotor1();
  }

  if (collisionX) {
    long errorX = pos2 - contactX;
    float cmdX = -baseKpX * elasticX * errorX;
    driveMotor2(cmdX);
  } else {
    stopMotor2();
  }
}

void sendEncoderData() {
  unsigned long now = millis();

  if (now - lastSendTime < sendInterval) {
    return;
  }

  lastSendTime = now;

  long pos1, pos2;

  noInterrupts();
  pos1 = encoder1Count;
  pos2 = encoder2Count;
  interrupts();

  Serial.print(pos2);
  Serial.print("X,");
  Serial.print(pos1);
  Serial.println("Y");
}

void handleSerial() {
  while (Serial.available() > 0) {
    char c = Serial.read();

    if (c == '\n') {
      rxBuffer[rxIndex] = '\0';
      parseCommand(rxBuffer);
      rxIndex = 0;
    } 
    else if (c != '\r') {
      if (rxIndex < 63) {
        rxBuffer[rxIndex++] = c;
      }
    }
  }
}

void parseCommand(char *msg) {
  if (msg[0] == '\0') return;

  if (msg[0] == 'C') {
    parseCollision(msg);
  } 
  else if (msg[0] == 'R') {
    parseRelease(msg);
  } 
  else if (msg[0] == 'z' || msg[0] == 'Z') {
    resetAllCollision();
  } 
  else {
    Serial.print("[Arduino log] Unknown command: ");
    Serial.println(msg);
  }
}

void parseCollision(char *msg) {
  // Format: C,X,1471,10

  char *type = strtok(msg, ",");
  char *axisText = strtok(NULL, ",");
  char *posText = strtok(NULL, ",");
  char *elasticText = strtok(NULL, ",");

  if (type == NULL || axisText == NULL || posText == NULL || elasticText == NULL) {
    Serial.println("[Arduino log] Parse error: collision format");
    return;
  }

  char axis = axisText[0];
  long contactPos = atol(posText);

  // Unity sends elastic coefficient multiplied by 10
  float elastic = atof(elasticText) / 10.0;

  if (elastic <= 0) {
    elastic = 1.0;
  }

  if (axis == 'X') {
    collisionX = true;
    contactX = contactPos;
    elasticX = elastic;

    Serial.print("[Arduino log] Collision X contact=");
    Serial.print(contactX);
    Serial.print(" elastic=");
    Serial.println(elasticX);
  } 
  else if (axis == 'Y') {
    collisionY = true;
    contactY = contactPos;
    elasticY = elastic;

    Serial.print("[Arduino log] Collision Y contact=");
    Serial.print(contactY);
    Serial.print(" elastic=");
    Serial.println(elasticY);
  } 
  else {
    Serial.println("[Arduino log] Parse error: unknown axis");
  }
}

void parseRelease(char *msg) {
  // Format: R,X

  char *type = strtok(msg, ",");
  char *axisText = strtok(NULL, ",");

  if (type == NULL || axisText == NULL) {
    Serial.println("[Arduino log] Parse error: release format");
    return;
  }

  char axis = axisText[0];

  if (axis == 'X') {
    collisionX = false;
    stopMotor2();
    Serial.println("[Arduino log] Release X");
  } 
  else if (axis == 'Y') {
    collisionY = false;
    stopMotor1();
    Serial.println("[Arduino log] Release Y");
  } 
  else {
    Serial.println("[Arduino log] Parse error: unknown release axis");
  }
}

void resetAllCollision() {
  collisionX = false;
  collisionY = false;

  stopMotor1();
  stopMotor2();

  Serial.println("[Arduino log] All collision states reset");
}

void readEncoder1() {
  bool a = digitalRead(ENC1_A);
  bool b = digitalRead(ENC1_B);

  if (a == b) encoder1Count++;
  else encoder1Count--;
}

void readEncoder2() {
  bool a = digitalRead(ENC2_A);
  bool b = digitalRead(ENC2_B);

  if (a == b) encoder2Count++;
  else encoder2Count--;
}

void driveMotor1(float cmd) {
  int pwm = abs((int)cmd);

  if (pwm < 8) {
    stopMotor1();
    return;
  }

  pwm = constrain(pwm, 60, 220);

  if (cmd > 0) {
    digitalWrite(IN1, HIGH);
    digitalWrite(IN2, LOW);
  } else {
    digitalWrite(IN1, LOW);
    digitalWrite(IN2, HIGH);
  }

  analogWrite(ENA, pwm);
}

void driveMotor2(float cmd) {
  int pwm = abs((int)cmd);

  if (pwm < 8) {
    stopMotor2();
    return;
  }

  pwm = constrain(pwm, 60, 220);

  if (cmd > 0) {
    digitalWrite(IN3, HIGH);
    digitalWrite(IN4, LOW);
  } else {
    digitalWrite(IN3, LOW);
    digitalWrite(IN4, HIGH);
  }

  analogWrite(ENB, pwm);
}

void stopMotor1() {
  digitalWrite(IN1, LOW);
  digitalWrite(IN2, LOW);
  analogWrite(ENA, 0);
}

void stopMotor2() {
  digitalWrite(IN3, LOW);
  digitalWrite(IN4, LOW);
  analogWrite(ENB, 0);
}