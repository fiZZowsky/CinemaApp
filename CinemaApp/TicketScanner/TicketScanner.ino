#include <SPI.h>
#include <MFRC522.h>
#include <WiFiS3.h>

#include "wifi_secrets.h"
#include "env.h"

#define duration 5000

MFRC522 rfid(10, 9);
MFRC522::MIFARE_Key key;
boolean state = false;
unsigned long time;

char ssid[] = SECRET_SSID;
char pass[] = SECRET_PASS;

IPAddress serverIPAddress(SERVER_IP_ADDRESS);
String host = serverIPAddress + ":" + PORT;

int keyIndex = 0;

int status = WL_IDLE_STATUS;
WiFiClient client;

unsigned long lastConnectionTime = 0;
const unsigned long postingInterval = 5L * 1000L;

String cardUID = "";

void setup() {
  Serial.begin(9600);
  SPI.begin();
  rfid.PCD_Init();
  pinMode(2, OUTPUT);
  pinMode(4, OUTPUT);

  while (!Serial);

  if (WiFi.status() == WL_NO_MODULE) {
    Serial.println("Communication with WiFi module failed!");
    while (true);
  }

  String fv = WiFi.firmwareVersion();
  if (fv < WIFI_FIRMWARE_LATEST_VERSION) {
    Serial.println("Please upgrade the firmware");
  }

  while (status != WL_CONNECTED) {
    Serial.print("Attempting to connect to SSID: ");
    Serial.println(ssid);
    status = WiFi.begin(ssid, pass);
  }

  printWifiStatus();
}

void loop() {
  if(status = WL_CONNECTED){
    if(rfid.PICC_IsNewCardPresent() && rfid.PICC_ReadCardSerial())
    {
      for (byte i = 0; i < rfid.uid.size; i++) {
        cardUID += (rfid.uid.uidByte[i] < 0x10 ? "0x0" : "0x");
        cardUID += String(rfid.uid.uidByte[i], HEX);
      }
      
      Serial.print("UID: {");
      Serial.print(cardUID);
      Serial.println("}");

      rfid.PICC_HaltA();
      rfid.PCD_StopCrypto1();
    }

    if(state && time < millis())
      state = false;

    if(cardUID.length() != 0) {
      if (millis() - lastConnectionTime > postingInterval) {
        httpRequest();
      }
    }

    digitalWrite(2, state);
    digitalWrite(4, !state);
  }
}

void httpRequest() {
  client.stop();

  Serial.println("connecting...");
  
  if (client.connect(serverIPAddress, PORT)) {
    // Tworzenie pełnego żądania HTTP
    String request = "POST /Ticket/ScannedTicketCheck HTTP/1.1\r\n";
    request += "Host: " + host + "\r\n";
    request += "Content-Type: application/x-www-form-urlencoded\r\n";
    request += "Content-Length: " + String(cardUID.length()) + "\r\n";
    request += "Connection: close\r\n\r\n";
    request += cardUID;

    Serial.println("HTTP Request:");
    Serial.print(request);

    client.print(request);

    while (!client.available()) {
      delay(10);
    }

    processResponse();

    client.stop();
    lastConnectionTime = millis();
  } else {
    Serial.println("connection failed");
  }
}

void processResponse() {
  Serial.println("\n---------------Response---------------");
  String response = "";
  bool isStatus200 = false;

  while (client.available()) {
    char c = client.read();
    response += c;

    // Jeśli odpowiedź zawiera "HTTP/1.1 200 OK", ustaw flagę isStatus200 na true
    if (response.endsWith("HTTP/1.1 200 OK\r\n")) {
      isStatus200 = true;
    }
  }

  // Wypisz całą odpowiedź na Serial Monitor
  Serial.print(response);

  Serial.println("---------------Received data---------------");
  if(strstr(response.c_str(), "success") != NULL)
  {
    Serial.println("Correct ticket");
    state = true;
    time = millis() + duration;
  }
  else{
    Serial.println("Incorrect ticket");
    state = false;
      for(int i = 0; i < 5; i++){
        digitalWrite(4, HIGH);
        delay(500);
        digitalWrite(4, LOW);
        delay(500);
      }
  }
  
  cardUID = "";
}

void printWifiStatus() {
  Serial.println("Connected to WIFI");
  Serial.println("SSID: ");
  Serial.println(WiFi.SSID());

  IPAddress ip = WiFi.localIP();
  Serial.print("IP Address: ");
  Serial.println(ip);

  long rssi = WiFi.RSSI();
  Serial.print("Signal Strength (RSSI): ");
  Serial.print(rssi);
  Serial.println(" dBm");
}