print("Turn on ThingsBoard")
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

def readSerial():
    bytesToRead = ser.inWaiting()
    if (bytesToRead > 0):
        global mess
        mess = mess + ser.read(bytesToRead).decode("UTF-8")
        while ("#" in mess) and ("!" in mess):
            start = mess.find("!")
            end = mess.find("#")
            processData(mess[start:end + 1])
            if (end == len(mess)): mess = ""
            else: mess = mess[end + 1:]

def connected(client, usedata, flags, rc):
    if rc == 0:
        print("Thingsboard connected successfully!!")
        client.subscribe("v1/devices/me/rpc/request/+")
    else:
        print("Connection is failed")

def getTempLight(splitData):
    if (splitData[1] == "TEMP"): collect_data = {'temperature':splitData[2]}
    else: collect_data = {'light':splitData[2]}
    client.publish('v1/devices/me/telemetry', json.dumps(collect_data), 1)
    # SEND BACK TO MICROBIT
    if len(bbc_port) > 0: ser.write((str(splitData[1]) + "#").encode())

cmd_list = []
waiting_cmd = 10

def processData(data):
    data = data.replace("!", "")
    data = data.replace("#", "")
    splitData = data.split(":")
    print(splitData)
    if splitData[0] == 1: getTempLight(splitData)
    elif splitData[0] == 2 and splitData[1] == "cmd":
        cmd = splitData[2]
        if cmd == cmd_list[0]:
            cmd_list = cmd_list[1:]
            waiting_cmd = 10
            send_cmd_to_server(cmd)
        
def recv_message(client, userdata, message):
    print("Received: ", message.payload.decode("utf-8"))
    try:
        jsonobj = json.loads(message.payload)
        if jsonobj['method'] == "setLED":
            cmd_list.append(1 if jsonobj['params'] == True else 0)
        if jsonobj['method'] == "setFAN":
            cmd_list.append(3 if jsonobj['params'] == True else 2)
    except: pass

def get_queue_cmd():
    waiting_cmd += 1
    if len(cmd_list) != 0:
        if waiting_cmd > 5:
            if len(bbc_port) > 0:
                print("SEND CMD " + cmd_list[0] + " TO MICROBIT")
                ser.write((str(cmd_list[0]) + "#").encode())
            waiting_cmd = 0
    
def send_cmd_to_server(cmd):
    temp_data = {}
    try:
        if cmd == 0 or cmd == 1:
            temp_data['LED'] = cmd == 1
            temp_data['setLED'] = cmd == 1
            client.publish('v1/devices/me/attributes', json.dumps(temp_data), 1)
            print("LED: " + ("ON" if cmd == 1 else "OFF"))
        if cmd == 2 or cmd == 3:
            temp_data['FAN'] = cmd == 3
            temp_data['setFAN'] = cmd == 3
            client.publish('v1/devices/me/attributes', json.dumps(temp_data), 1)
            print("FAN: " + ("ON" if cmd == 1 else "OFF"))
    except: pass

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

while True:
    get_queue_cmd()
    if len(bbc_port) > 0: readSerial()
    time.sleep(1)