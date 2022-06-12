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
    collect_data = {splitData[1]:splitData[2]}
    client.publish('v1/devices/me/telemetry', json.dumps(collect_data), 1)
    # if (splitData[1][:-1] == "TEMP"):
    #     collect_data = {'temperature' + splitData[1][-1:]:splitData[2]}
    # else:
    #     collect_data = {'light':splitData[2]}
    # client.publish('v1/devices/me/telemetry', json.dumps(collect_data), 1)

def setJson(json, jsonobj, id):
    temp_data = {}
    cmd = 0
    if jsonobj['method'] == "setLED" + str(id):
        if jsonobj['params'] == True:
            cmd = 1 + id * 10
        else:
            cmd = 0 + id * 10
        temp_data['LED'  + str(id)] = jsonobj['params']
        temp_data['setLED'  + str(id)] = jsonobj['params']
        client.publish('v1/devices/me/attributes', json.dumps(temp_data), 1)
    if jsonobj['method'] == "setFAN"  + str(id):
        if jsonobj['params'] == True:
            cmd = 3 + id * 10
        else:
            cmd = 2 + id * 10
        temp_data['FAN' + str(id)] = jsonobj['params']
        temp_data['setFAN' + str(id)] = jsonobj['params']
        client.publish('v1/devices/me/attributes', json.dumps(temp_data), 1)
    return cmd

def recv_message(client, userdata, message):
    print("Received: ", message.payload.decode("utf-8"))
    cmd = -1
    try:
        jsonobj = json.loads(message.payload)
        temp_cmd = setJson(json, jsonobj, 1)
        if temp_cmd != 0: cmd = temp_cmd

        temp_cmd = setJson(json, jsonobj, 2)
        if temp_cmd != 0: cmd = temp_cmd

    except:
        pass
    if len(bbc_port) > 0:
        if cmd != -1:
            print("Cmd : " + str(cmd))
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

# collect_data = {"TEMP1":0,"TEMP2":1,"LIGHT1":2,"LIGHT2":3}
# print(collect_data)
# client.publish('v1/devices/me/telemetry', json.dumps(collect_data), 1)

mess = ""
bbc_port = "COM3"
if len(bbc_port) > 0:
    ser = serial.Serial(port=bbc_port, baudrate=115200, timeout = 30000)

while True:
    if len(bbc_port) > 0:
        readSerial()
    time.sleep(1)