// Arduino Joystick Serial Communication Module
// Requires Arduino set up on COM7 for serial communication
// (c) 2016 Nicholas Curtis

// Arduino pin setup
const int SW_pin = 4; // digital pin connected to switch output
const int X_pin = 0; // analog pin connected to X axis
const int Y_pin = 1; // analog pin connected to Y axis
int input = 0;
int sw = 0;
int x = 0;
int y = 0;
double rest[] = {0, 0, 0};
double accel[] = {0, 0, 0};
boolean analog = false;
 
void setup() {
  pinMode(SW_pin, INPUT);
  digitalWrite(SW_pin, HIGH);
  Serial.begin(9600);
  
  pinMode(13, OUTPUT);
  digitalWrite(13, LOW);
  delay(2000);
  for (int i = 0; i < 3; i++){
    rest[i] = analogRead(i + 3);
    delay(200);
  }
  digitalWrite(13, HIGH);
}
 
void loop() {
  
  // switch input
  sw = digitalRead(SW_pin);
  if (sw == LOW){
   if (analog == false){
     analog = true;
   }
   else {
     analog = false;
   }
  }
  
  // x axis input
  input = analogRead(X_pin);
  if (analog == true){
    x = map(input, 0, 1019, -100, 100);
    if (x > -2 && x < 2){
      x = 0;
    }
  }
  else {
    if (input > 600){x = 1;}
    else if (input < 400){x = -1;}
    else {x = 0;}
  }
  
  // y axis input
  input = analogRead(Y_pin);
  if (analog == true){
    y = map(input, 0, 1019, -100, 100);
    if (y > -2 && y < 2){
      y = 0;
    }
  }
  else {
    if (input > 600){y = 1;}
    else if (input < 400){y = -1;}
    else {y = 0;}
  }
  
  for (int i = 0; i < 3; i++){
   accel[i] = analogRead(i - 3) - rest[i];
  }
  
  // output
  Serial.print(!sw);
  Serial.print("\t");
  Serial.print(x);
  Serial.print("\t");
  Serial.print(y * -1);
  for (int i = 0; i < 3; i++){
    Serial.print("\t");
    Serial.print(accel[i]); 
  }
  Serial.println();
  delay(150);
}
