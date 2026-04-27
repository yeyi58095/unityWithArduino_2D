const int encoderA = 2;  // A 相
const int encoderB = 4;  // B 相

volatile long encoderCount = 0;
volatile int direction = 0;  // 1: 順時針, -1: 逆時針
long lastCount = 0;          // 用來判斷是否有變化

void setup() {
  pinMode(encoderA, INPUT_PULLUP);
  pinMode(encoderB, INPUT_PULLUP);

  attachInterrupt(digitalPinToInterrupt(encoderA), readEncoder, CHANGE);

  Serial.begin(9600);
}

void loop() {
  if (encoderCount != lastCount) {
    // 只有 count 變化時才印出
    if (direction == 1) {
      Serial.println("S");
    } else if (direction == -1) {
      Serial.println("W");
    }
    lastCount = encoderCount;
  }
}

void readEncoder() {
  int A = digitalRead(encoderA);
  int B = digitalRead(encoderB);

  if (A == B) {
    encoderCount++;
    direction = 1;
  } else {
    encoderCount--;
    direction = -1;
  }
}
