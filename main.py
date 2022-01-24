import geocoder
print("Turn on ThingsBoard")
import random
import paho.mqtt.client as mqttclient
import time
import json

BROKER_ADDRESS = "demo.thingsboard.io"
PORT = 1883
THINGS_BOARD_ACCESS_TOKEN = "Z0NcbEWngaLorv8ETyiA"


def subscribed(client, userdata, mid, granted_qos):
    print("Subscribed...")


def recv_message(client, userdata, message):
    print("Received: ", message.payload.decode("utf-8"))
    temp_data = {'value': True}
    try:
        jsonobj = json.loads(message.payload)
        if jsonobj['method'] == "setValue":
            temp_data['value'] = jsonobj['params']
            client.publish('v1/devices/me/attributes', json.dumps(temp_data), 1)
    except:
        pass

def connected(client, usedata, flags, rc):
    if rc == 0:
        print("Thingsboard connected successfully!!")
        client.subscribe("v1/devices/me/rpc/request/+")
    else:
        print("Connection is failed")

def updateLongLat():
    g = geocoder.ip("me")
    myAddress = g.latlng

    longitude = myAddress[1]
    latitude = myAddress[0]

    return longitude, latitude

client = mqttclient.Client("Gateway_Thingsboard")
client.username_pw_set(THINGS_BOARD_ACCESS_TOKEN)

client.on_connect = connected
client.connect(BROKER_ADDRESS, 1883)
client.loop_start()

client.on_subscribe = subscribed
client.on_message = recv_message

temp = random.randrange(-30,150)
humi = random.randrange(0,100)
#light_intensity = 100
counter = 0
longitude = 106.6297
latitude = 10.8231
while True:
    collect_data = {'temperature': temp, 'humidity': humi, 'longitude': longitude, 'latitude': latitude}
    temp = random.randrange(-30,150)
    humi = random.randrange(0,100)
    #light_intensity += 1
    longitude, latitude = updateLongLat()
    client.publish('v1/devices/me/telemetry', json.dumps(collect_data), 1)
    time.sleep(10)
