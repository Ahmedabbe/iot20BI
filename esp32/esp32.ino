#include <WiFi.h>
#include <Esp32MQTTClient.h>
#include <DHT.h>
#include <ArduinoJson.h>

#define DHT_PIN 4
#define DHT_TYPE DHT11

char* ssid = "Ahmed";
char* pass = "KompIS123";
char *conn = "HostName=AhmedAlhasaniIOT20.azure-devices.net;DeviceId=esp32;SharedAccessKey=rEQbiEEJYH0BQW+Y0Y+9mbxcs27f1n16SQpEB49nP7o=";

bool messagePending = false;
int interval = 1000;
unsigned long prevMillis = 0;
time_t epochTime;

DHT dht(DHT_PIN,DHT_TYPE);
IOTHUB_CLIENT_LL_HANDLE deviceClient;

void setup() {
  initSerial();
  initWifi();
  initEpochTime();
  initDHT();
  initDevice();
}

void loop() {
  unsigned long currentMillis = millis();

  if(!messagePending) {
    if((currentMillis - prevMillis) >= interval) {
      prevMillis = currentMillis;

      epochTime = time(NULL);
      Serial.printf("Current Time: %lu. ", epochTime);

      float temperature = dht.readTemperature();
      float humidity = dht.readHumidity();

      if(!(std::isnan(temperature)) && !(std::isnan(humidity)))  {
        char payload[256];
        
        DynamicJsonDocument doc(sizeof(payload));
        doc["deviceId"] = "esp32";
        doc["ts"] = epochTime;
        doc["temp"] = temperature;
        doc["hum"] = humidity;
        serializeJson(doc, payload);
        
        sendMessage(payload);       
      }
    }
  }

  IoTHubClient_LL_DoWork(deviceClient);
  delay(10);
}
