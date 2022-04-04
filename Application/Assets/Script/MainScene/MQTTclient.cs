using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;

namespace M2MqttUnity.Examples
{
    public class MQTTclient: M2MqttUnityClient {
        public static MQTTclient instance;
        string topic_status = "/bkiot/1913123/status";
        string topic_led = "/bkiot/1913123/led";
        string topic_bump = "/bkiot/1913123/pump";
        public string msg_received_from_topic;
        public Text text_display;
        private List<string> eventMessages = new List<string>();
        void Awake() {
            if (instance == null) instance = this;
            this.brokerAddress = PlayerPrefs.GetString("bro_uri_txt", "mqttserver.tk");
            this.autoConnect = true;
            this.mqttUserName = PlayerPrefs.GetString("user_txt", "bkiot");
            this.mqttPassword = PlayerPrefs.GetString("pwd_txt", "12345678");
            //base.Awake();

            // this.brokerAddress = "mqttserver.tk";
            // this.mqttUserName = "bkiot";
            // this.mqttPassword = "12345678";

            // this.brokerAddress = "test.mosquitto.org";
            // this.autoConnect = true;
            // this.mqttUserName = "wildcard";
            // this.mqttPassword = "";
            // topic_status = "#";
        }
        // overide
        public void TestPublish() {
            string value = "{temperature:25,humidity:25}";
            client.Publish(topic_status, System.Text.Encoding.UTF8.GetBytes(value), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            AddUiMessage("Test message published.");
        }
        public void SetEncrypted(bool isEncrypted) {
            this.isEncrypted = isEncrypted;
        }
        public void AddUiMessage(string msg) {
            // Debug.Log(msg);
            // if (consoleInputField != null) {
            //    consoleInputField.text += msg + "\n";
            //    updateUI = true;
            // }
        }
        protected override void OnConnecting() {
            base.OnConnecting();
            //SetUiMessage("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
        }

        protected override void OnConnected() {
            base.OnConnected();
            //SetUiMessage("Connected to broker on " + brokerAddress + "\n");

            //if (autoTest)
            //{
            //    TestPublish();
            //}
            SubscribeTopics();
        }
        int count = 0;
        bool isFirst = true;
        protected override void SubscribeTopics() {
            if (topic_status != "")
            {
                if (isFirst) {
                    pubLed("OFF");
                    pubBump("OFF");
                    float curTemp = (float)((int)(UnityEngine.Random.Range(-50f, 100f) * 100)) / 100;
                    float curHumid = (float)((int)(UnityEngine.Random.Range(0f, 100f) * 100)) / 100;
                    pubStatus(curTemp, curHumid);
                    isFirst = false;
                }
                else {
                    if (count == 10)
                    {
                        Debug.Log("Pub");
                        float curTemp = (float)((int)(UnityEngine.Random.Range(-50f, 100f) * 100)) / 100;
                        float curHumid = (float)((int)(UnityEngine.Random.Range(0f, 100f) * 100)) / 100;
                        pubStatus(curTemp, curHumid);
                        count = 0;
                    }
                    
                    Debug.Log("Sub");
                    Debug.Log(count.ToString());
                    client.Subscribe(new string[] { topic_status }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                    client.Subscribe(new string[] { topic_led }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                    client.Subscribe(new string[] { topic_bump }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                }
                
                count++;
            }
        }

        protected override void UnsubscribeTopics() {
            // client.Unsubscribe(new string[] {topic_status });
            // client.Unsubscribe(new string[] { topic_led });
            client.Unsubscribe(new string[] { topic_bump });
        }

        protected override void OnConnectionFailed(string errorMessage) {
            AddUiMessage("CONNECTION FAILED! " + errorMessage);
        }

        protected override void OnDisconnected() {
            AddUiMessage("Disconnected.");
        }

        protected override void OnConnectionLost() {
            AddUiMessage("CONNECTION LOST!");
        }
        protected override void DecodeMessage(string topic, byte[] message) {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            msg_received_from_topic = msg;
            if (text_display != null) text_display.text = msg;
            Debug.Log("Received: " + msg);
            if (topic == topic_status) {
                updateStatus(msg);
            }
            else if (topic == topic_bump) {
                updateBump(msg);
            }
            else if (topic == topic_led) {
                updateLed(msg);
            }


            StoreMessage(msg);
            // if (topic == Topic_to_Subcribe)
            // {
            //     //if (autoTest)
            //     //{
            //     //    autoTest = false;
            //     //    Disconnect();
            //     //}
            // }
        }
        private void updateStatus(string msg) {
            string temp = "", humid = "";
            int i = 16;
            while(msg[i] != '"') {
                temp += msg[i++];
            }
            i += 14;
            while(msg[i] != '"') {
                humid += msg[i++];
            }
            ManagerData.instance.curTemp = float.Parse(temp);
            ManagerData.instance.curHumid = float.Parse(humid);
        }
        private void updateLed(string msg) {
            if (msg[27] == 'N') {
                ManagerData.instance.curLedState = true;
            }
            else {
                ManagerData.instance.curLedState = false;
            }
        }
        private void updateBump(string msg) {
            if (msg[28] == 'N')
            {
                ManagerData.instance.curBumpState = true;
            }
            else
            {
                ManagerData.instance.curBumpState = false;
            }
        }

        private void StoreMessage(string eventMsg) {
            eventMessages.Add(eventMsg);
        }

        private void ProcessMessage(string msg) {
            AddUiMessage("Received 2: " + msg);
        }

        protected override void Update() {
            base.Update(); // call ProcessMqttEvents()

            if (eventMessages.Count > 0) {
                foreach (string msg in eventMessages) {
                    ProcessMessage(msg);
                }
                eventMessages.Clear();
            }
            // if (updateUI) {
            //     UpdateUI();
            // }
        }

        private void OnDestroy() {
            Disconnect();
        }

        private void OnValidate() {
            //if (autoTest)
            //{
            //    autoConnect = true;
            //}
        }
        // publish
        public void pubStatus(float temp, float humid) {
            string msg = "{\"temperature\":\"" + temp.ToString() + "\",\"humidity\":\"" + humid.ToString() + "\"}";
            client.Publish(topic_status, System.Text.Encoding.UTF8.GetBytes(msg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        public void pubLed(string value) {
            string msg = "{\"device\":\"LED\",\"status\":\"" + value + "\"}";
            client.Publish(topic_led, System.Text.Encoding.UTF8.GetBytes(msg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        public void pubBump(string value) {
            string msg = "{\"device\":\"PUMP\",\"status\":\"" + value + "\"}";
            client.Publish(topic_bump, System.Text.Encoding.UTF8.GetBytes(msg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
    }
}
