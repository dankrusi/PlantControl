// Constants
const boolean DEBUG_ENABLED = false;
const int BAUD_RATE = 9600; // 9600, 57600, 115200
const int LINE_BUFFER_SIZE = 64;

// Variables
char _buffer [LINE_BUFFER_SIZE];
int _index = 0;
boolean _ready = false;

// Get a string value in a series of space seperated values
String _getStringValue(String data, int index)
{
    char separator = ' ';
    int found = 0;
    int strIndex[] = {0, -1};
    int maxIndex = data.length()-1;
    for(int i=0; i<=maxIndex && found<=index; i++) {
        if(data.charAt(i)==separator || i==maxIndex) {
            found++;
            strIndex[0] = strIndex[1]+1;
            strIndex[1] = (i == maxIndex) ? i+1 : i;
        }
    }
    return found>index ? data.substring(strIndex[0], strIndex[1]) : "";
}

void _parseLine()
{
    // Init
    char* commandPtr = strtok(_buffer, " ");    // Everything up to the '=' is the color name
    char* valuePtr = strtok(NULL, "\n");  // Everything else is the color value
    if (commandPtr != NULL)
    {
        String command = String(commandPtr);
        String value = String(valuePtr);

        if(DEBUG_ENABLED) Serial.println(String("debug got cmd ") + command); 
        
        if(command == "analogread") {
          
            // CMD "analogread {pin}"
            int pin = value.toInt();
            int sensorValue = analogRead(pin);
            Serial.println(String(sensorValue)); 
        
        } else if(command == "digitalread") {
          
            // CMD "digitalread {pin}"
            int pin = value.toInt();
            int sensorValue = digitalRead(pin);
            Serial.println(String(sensorValue)); 
        
        } else if(command == "analogwrite") {
          
            // CMD "analogwrite {pin} {value}"
            int pin = _getStringValue(value,0).toInt();
            int pinValue = _getStringValue(value,1).toInt();
            if(DEBUG_ENABLED) Serial.println(String("debug analogwrite pin ") + String(pin) + String(" val ")  + String(pinValue)); 
            analogWrite(pin,pinValue);
            Serial.println(String("success")); 
        
        } else if(command == "digitalwrite") {
          
            // CMD "digitalwrite {pin} {high|low}"
            int pin = _getStringValue(value,0).toInt();
            String pinValue = _getStringValue(value,1);
            if(DEBUG_ENABLED) Serial.println(String("debug digitalwrite pin ") + String(pin) + String(" val ")  + String(pinValue)); 
            if(pinValue == "high") digitalWrite(pin,HIGH);
            else digitalWrite(pin,LOW);
            Serial.println(String("success")); 
        
        } else if(command == "pinmode") {
          
            // CMD "pinmode {pin} {in/out/pullup}"
            int pin = _getStringValue(value,0).toInt();
            String pinValue = _getStringValue(value,1);
            if(DEBUG_ENABLED) Serial.println(String("debug pinmode pin ") + String(pin) + String(" val ")  + String(pinValue)); 
            if(pinValue == "out") pinMode(pin, OUTPUT);
            else if(pinValue == "pullup") pinMode(pin, INPUT_PULLUP);
            else pinMode(pin, INPUT);
            Serial.println(String("success")); 
        
        } else {
          
            Serial.println(String("unknowncmd ") + command); 
          
        }
        
    } else {
      
         // Unknown
         Serial.println(String("invalidcmd")); 
         
    }
}


void setup() {
    // Init serial connection
    Serial.begin(BAUD_RATE);
}

void loop() {
    // Wait until we are ready to parse a line
    if (_ready) {
        _parseLine();
        _ready = false;
    } else while (Serial.available()) {
        // Read from serial until the line is ended or buffer is full
        char c = Serial.read();
        _buffer[_index++] = c;
        if ((c == '\n') || (_index == sizeof(_buffer)-1)) {
            _buffer[_index] = '\0';
            _index = 0;
            _ready = true;
        }
    }
}
