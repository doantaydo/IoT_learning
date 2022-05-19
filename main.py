print("Turn on ThingsBoard")
#from operator import truediv
import random
import paho.mqtt.client as mqttclient
import time
import json
import serial.tools.list_ports
import serial

BROKER_ADDRESS = "demo.thingsboard.io"
PORT = 1883
THINGS_BOARD_ACCESS_TOKEN = "Z0NcbEWngaLorv8ETyiA"

def subscribed(client, userdata, mid, granted_qos):
    print("Subscribed...")

def processData(data):
    data = data.replace("!", "")
    data = data.replace("#", "")
    splitData = data.split(":")
    print(splitData)
    # TODO : Add your source code to publish data to Thingsboard
    # var is storage value
    # pubVar is flag to mark this var has value
    if (splitData[1] == "TEMP"):
        collect_data = {'temperature':splitData[2]}
    else:
        collect_data = {'light':splitData[2]}
    client.publish('v1/devices/me/telemetry', json.dumps(collect_data), 1)



def recv_message(client, userdata, message):
    print("Received: ", message.payload.decode("utf-8"))
    temp_data = {}
    cmd = 1
    # TODO : Update the cmd to control 2 devices
    # 0: turn off the led
    # 1: turn on the led
    # 2: turn off the fan
    # 3: turn on the fan
    try:
        jsonobj = json.loads(message.payload)
        #temp_data['setValue'] = jsonobj['params']
        if jsonobj['method'] == "setLED":
            if jsonobj['params'] == True:
                cmd = 1
            else:
                cmd = 0
            temp_data['LED'] = jsonobj['params']
            temp_data['setLED'] = jsonobj['params']
            client.publish('v1/devices/me/attributes', json.dumps(temp_data), 1)
        if jsonobj['method'] == "setFAN":
            if jsonobj['params'] == True:
                cmd = 3
            else:
                cmd = 2
            temp_data['FAN'] = jsonobj['params']
            temp_data['setFAN'] = jsonobj['params']
            client.publish('v1/devices/me/attributes', json.dumps(temp_data), 1)
    except:
        pass
    if len(bbc_port) > 0:
        ser.write((str(cmd) + "#").encode())

def readSerial():
    bytesToRead = ser.inWaiting()
    if (bytesToRead > 0):
        global mess
        mess = mess + ser.read(bytesToRead).decode("UTF-8")
        while ("#" in mess) and ("!" in mess):
            start = mess.find("!")
            end = mess.find("#")
            processData(mess[start:end + 1])
            if (end == len(mess)):
                mess = ""
            else:
                mess = mess[end+1:]

def connected(client, usedata, flags, rc):
    if rc == 0:
        print("Thingsboard connected successfully!!")
        client.subscribe("v1/devices/me/rpc/request/+")
    else:
        print("Connection is failed")


client = mqttclient.Client("Gateway_Thingsboard")
client.username_pw_set(THINGS_BOARD_ACCESS_TOKEN)

client.on_connect = connected
client.connect(BROKER_ADDRESS, 1883)
client.loop_start()

client.on_subscribe = subscribed
client.on_message = recv_message

temp = 30
light_intensity = 100

mess = ""
bbc_port = "COM3"
if len(bbc_port) > 0:
    ser = serial.Serial(port=bbc_port, baudrate=115200, timeout = 30000)

#client.publish('v1/devices/me/attributes', json.dumps(collect_data), 1)

while True:
    if len(bbc_port) > 0:
        readSerial()
    time.sleep(1)