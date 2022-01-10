const int knockSensor = A5;
const int potentiometer = A0;
const int touchSensor = 2;
const int touchSensorHigh = 3;
const int touchSensorLow = 4;


void setup() {
  pinMode(touchSensor, INPUT);
  pinMode(touchSensorHigh, OUTPUT);
  pinMode(touchSensorLow, OUTPUT);
  digitalWrite(touchSensorHigh, HIGH);
  digitalWrite(touchSensorLow, LOW);
  Serial.begin(9600);
}

void loop() {
  int rotation = map(analogRead(potentiometer),0,1023,0,360);
  Serial.print(analogRead(knockSensor));
  Serial.print(",");
  Serial.print(rotation);
  Serial.print(",");
  Serial.print(digitalRead(touchSensor));
  Serial.print(",");
  Serial.println(20);

//  Serial.println(analogRead(knockSensor));
  delay(100);
}
