const unsigned long maxLong = 4294967295;
const int redPin =  9;
const int yellowPin = 10;
const int bluePin =  12;
const int greenPin =  14;
const int orangePin = 15;
const int statusLedPin = 11;

unsigned long redLowMillis = maxLong;
unsigned long yellowLowMillis = maxLong;
unsigned long blueLowMillis = maxLong;
unsigned long greenLowMillis = maxLong;
unsigned long orangeLowMillis = maxLong;

void setup()   {                
  // initialize the digitals pin as an outputs
  pinMode(redPin, OUTPUT);
  pinMode(yellowPin,OUTPUT);
  pinMode(bluePin,OUTPUT);
  pinMode(greenPin, OUTPUT);
  pinMode(orangePin, OUTPUT);
  pinMode(statusLedPin,OUTPUT);
  Serial.begin(9600);
  //light up the led to show its ready
  digitalWrite(statusLedPin,HIGH);
}

void loop()                     
{
  unsigned long time = millis();
  boolean noteSetLow = false;
  if(time >= redLowMillis)
  {
    digitalWrite(redPin, LOW);
    redLowMillis = maxLong;
    noteSetLow = true;
  }
  if(time >= yellowLowMillis)
  {
    digitalWrite(yellowPin,LOW);
    yellowLowMillis = maxLong;
    noteSetLow = true;
  }
  if(time >= blueLowMillis)
  {
    digitalWrite(bluePin,LOW);
    blueLowMillis = maxLong;
    noteSetLow = true;
  }
  if(time >= greenLowMillis)
  {    
    digitalWrite(greenPin,LOW);
    greenLowMillis = maxLong;
    noteSetLow = true;
  }
  if(time >= orangeLowMillis)
  {
    digitalWrite(orangePin,LOW);
    orangeLowMillis = maxLong;
    noteSetLow = true;
  }

  if(Serial.available() > 0)
  {
    int incomingByte = Serial.read();
    switch(incomingByte)
    {
    case 'R':
      digitalWrite(redPin,HIGH);
      redLowMillis = millis() + 25;
      break;
    case 'Y':
      digitalWrite(yellowPin,HIGH);
      yellowLowMillis = millis() + 25;
      break;
    case 'B':
      digitalWrite(bluePin,HIGH);
      blueLowMillis = millis() + 25;
      break;
    case 'G':
      digitalWrite(greenPin,HIGH);
      greenLowMillis = millis() + 25;
      break;
    case 'O':
      digitalWrite(orangePin,HIGH);
      orangeLowMillis = millis() + 50; 
      break;
    }
  }
}




