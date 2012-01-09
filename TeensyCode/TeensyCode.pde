const int redPin =  9;
const int yellowPin = 10;
const int bluePin =  12;
const int greenPin =  14;
const int orangePin = 15;
const int statusLedPin = 11;
const unsigned long maxLong = 4294967295;

unsigned long redLowMillis = 0;
unsigned long yellowLowMillis = 0;
unsigned long blueLowMillis = 0;
unsigned long greenLowMillis = 0;
unsigned long orangeLowMillis = 0;

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
    noteSetLow = true;
  }
  if(time >= yellowLowMillis)
  {
    digitalWrite(yellowPin,LOW);
    noteSetLow = true;
  }
  if(time >= blueLowMillis)
  {
    digitalWrite(bluePin,LOW);
    noteSetLow = true;
  }
  if(time >= greenLowMillis)
  {    digitalWrite(greenPin,LOW);
      noteSetLow = true;
  }
  if(time >= orangeLowMillis)
  {
    digitalWrite(orangePin,LOW);
    noteSetLow = true;
  }
  if(noteSetLow)
  {
    delay(5);
  }

  if(Serial.available() > 0)
  {
    int incomingByte = Serial.read();
    if(incomingByte == 'R')
      digitalWrite(redPin, HIGH);
    else if (incomingByte == 'Y')
      digitalWrite(yellowPin,HIGH);
    else if (incomingByte == 'B')
      digitalWrite(bluePin,HIGH);
    else if (incomingByte == 'G')
      digitalWrite(greenPin,HIGH);
    else if (incomingByte == 'O')
    {
      //not as responsive as the other notes
      //trigger it 1 frame earlier ?
      digitalWrite(orangePin,HIGH);
      orangeNote = 1;
    }
  }
  delay(5);
  if(orangeNote)
    delay(10);
  digitalWrite(redPin, LOW);
  digitalWrite(yellowPin,LOW);
  digitalWrite(bluePin,LOW);
  digitalWrite(greenPin, LOW);
  digitalWrite(orangePin,LOW);
  delay(5);
}


