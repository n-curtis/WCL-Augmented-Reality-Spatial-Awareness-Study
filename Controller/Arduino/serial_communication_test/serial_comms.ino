// serial comms moorse code translater
// Nicholas Curtis 2015

String readString;
String outputString;
const int led = 13;

void setup()
{
  pinMode(led, OUTPUT);
  Serial.begin(9600);  // initialize serial communications at 9600 bps
  digitalWrite(led, LOW);
}

void loop()
{
  while(!Serial.available()) {}
  
  // serial read section
  while (Serial.available())
  {
    if (Serial.available() > 0)
    {
      char c = Serial.read();  // gets one byte from serial buffer
      readString += c;         // adds to the input string
    }
  }
  if (readString.length() > 1)
  {
    Serial.flush();
    Serial.print("Arduino received: \t" + readString);  
    delay(2000);
    
    //translate
    char message[readString.length() + 1];
    readString.toCharArray(message, (readString.length() + 1));
    
    Serial.flush();
    Serial.print("standby");
    
    int i = 0;
    int c = strlen(message);  
    while (i < c){
      translate(message[i]);
      i++;
    }
    
    Serial.flush();
    Serial.print("*Arduino translated: \t" + outputString);
    readString = "";
    outputString = "";
  }
  
  delay(1000);
}

void dot() {
  outputString += '.';
  digitalWrite(led, HIGH);
  delay(100);
  digitalWrite(led, LOW);
  delay(200);
}

void dash() {
  outputString += '-';
  digitalWrite(led, HIGH);
  delay(300);
  digitalWrite(led, LOW);
  delay(200);
}

void translate(char i){
  if (outputString.length() > 0){
    outputString += ' ';
  }
  switch (i) {
  case 'a':
    dot();
    dash();
    break;
  case 'b':
    dash();
    dot();
    dot();
    break;
  case 'c':
    dash();
    dot();
    dash();
    dot();
    break;
  case 'd':
    dash();
    dot();
    dot();
    break;
  case 'e':
    dot();
    break;
  case 'f':
    dot();
    dot();
    dash();
    dot();
    break;
  case 'g':
    dash();
    dash();
    dot();
    break;
  case 'h':
    dot();
    dot();
    dot();
    dot();
    break;
  case 'i':
    dot();
    dot();
    break;
  case 'j':
    dot();
    dash();
    dash();
    dash();
    break;
  case 'k':
    dash();
    dot();
    dash();
    break;
  case 'l':
    dot();
    dash();
    dot();
    dot();
    break;
  case 'm':
    dash();
    dash();
    break;
  case 'n':
    dash();
    dot();
    break;
  case 'o':
    dash();
    dash();
    dash();
    break;
  case 'p':
    dot();
    dash();
    dash();
    dot();
    break;
  case 'q':
    dash();
    dash();
    dot();
    dash();
    break;
  case 'r':
    dot();
    dash();
    dot();
    break;
  case 's':
    dot();
    dot();
    dot();
    break;
  case 't':
    dash();
    break;
  case 'u':
    dot();
    dot();
    dash();
    break;
  case 'v':
    dot();
    dot();
    dot();
    dash();
    break;
  case 'w':
    dot();
    dash();
    dash();
    break;
  case 'x':
    dash();
    dot();
    dot();
    dash();
    break;
  case 'y':
    dash();
    dot();
    dash();
    dash();
    break;
  case 'z':
    dash();
    dash();
    dot();
    dot();
    break;
  case ' ':
    outputString += '|';
    break;
  }
}
