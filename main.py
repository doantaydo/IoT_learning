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
    temp = False
    pubTemp = False
    light = False
    pubLight = False
    humid = False
    pubHumid = False
    for d in splitData:
        if d == "TEMP": temp = True
        elif d == "LIGHT": light = True
        elif d == "HUMID": humid = True
        else:
            if pubTemp == False:
                if temp == True:
                    temp = d
                    pubTemp = True
            if pubLight == False:
                if light == True:
                    light = d
                    pubLight = True
            if pubHumid == False:
                if humid == True:
                    humid = d
                    pubHumid = True
        
    if (pubTemp == False and pubLight == False and pubHumid == False): return

    collect_data = {}
    if pubTemp: collect_data['temperature'] = temp
    if pubLight: collect_data['light'] = light
    if pubHumid: collect_data['humidity'] = humid
    client.publish('v1/devices/me/telemetry', json.dumps(collect_data), 1)


def recv_message(client, userdata, message):
    print("Received: ", message.payload.decode("utf-8"))
    temp_data = {}
    cmd = 1
    # TODO : Update the cmd to control 2 devices

    try:
        jsonobj = json.loads(message.payload)
        #temp_data['setValue'] = jsonobj['params']
        if jsonobj['method'] == "setLED":
            temp_data['value'] = jsonobj['params']
            client.publish('v1/devices/me/attributes', json.dumps(temp_data), 1)
        if jsonobj['method'] == "setFAN":
            temp_data['value'] = jsonobj['params']
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
# humi = 50
light_intensity = 100
# counter = 0
# longitude = 106.7
# latitude = 10.6

mess = ""
bbc_port = "COM4"
if len(bbc_port) > 0:
    ser = serial.Serial(port=bbc_port, baudrate=115200, timeout = 30000)

#client.publish('v1/devices/me/attributes', json.dumps(collect_data), 1)

while True:
    if len(bbc_port) > 0:
        readSerial()
    # temp = random.randrange(0,100)
    # light_intensity = random.randrange(0,100)
    # collect_data = {'temperature': temp, 'humidity':light_intensity}
    # client.publish('v1/devices/me/telemetry', json.dumps(collect_data), 1)
    # print(collect_data)
    # while ser.inWaiting():
    #     readSerial()
    time.sleep(1)