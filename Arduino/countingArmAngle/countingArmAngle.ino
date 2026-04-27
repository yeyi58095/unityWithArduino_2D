// 第一個馬達
const int encoderA1 = 2;  
const int encoderB1 = 4;  
volatile long encoderCount1 = 0;
volatile int direction1 = 0;  
long lastCount1 = 0;

// 第二個馬達
const int encoderA2 = 3;  
const int encoderB2 = 5;  
volatile long encoderCount2 = 0;
volatile int direction2 = 0;
long lastCount2 = 0;

void setup() {
  // 馬達1
  pinMode(encoderA1, INPUT_PULLUP);
  pinMode(encoderB1, INPUT_PULLUP);
  attachInterrupt(digitalPinToInterrupt(encoderA1), readEncoder1, CHANGE);

  // 馬達2
  pinMode(encoderA2, INPUT_PULLUP);
  pinMode(encoderB2, INPUT_PULLUP);
  attachInterrupt(digitalPinToInterrupt(encoderA2), readEncoder2, CHANGE);

  Serial.begin(115200);
}

void loop() {
  // 馬達1
  if (encoderCount1 != lastCount1) {
    if (direction1 == 1) Serial.println("S");
    else if (direction1 == -1) Serial.println("W");
    lastCount1 = encoderCount1;
  }

  // 馬達2
  if (encoderCount2 != lastCount2) {
    if (direction2 == 1) Serial.println("A");
    else if (direction2 == -1) Serial.println("D");
    lastCount2 = encoderCount2;
  }
}

void readEncoder1() {
  int A = digitalRead(encoderA1);
  int B = digitalRead(encoderB1);

  if (A == B) {
    encoderCount1++;
    direction1 = 1;
  } else {
    encoderCount1--;
    direction1 = -1;
  }
}

void readEncoder2() {
  int A = digitalRead(encoderA2);
  int B = digitalRead(encoderB2);

  if (A == B) {
    encoderCount2++;
    direction2 = 1;
  } else {
    encoderCount2--;
    direction2 = -1;
  }
}
