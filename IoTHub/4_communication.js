"use strict";

var iotHubTransport = require("azure-iot-device-mqtt").Mqtt;
var Client = require("azure-iot-device").Client;
var Message = require("azure-iot-device").Message;
var crypto = require("crypto");

var ProvisioningTransport = require("azure-iot-provisioning-device-mqtt").Mqtt;
// Feel free to change to using any of the following if you would like to try another protocol.
// var ProvisioningTransport = require('azure-iot-provisioning-device-http').Http;
// var ProvisioningTransport = require('azure-iot-provisioning-device-amqp').Amqp;
// var ProvisioningTransport = require('azure-iot-provisioning-device-amqp').AmqpWs;
// var ProvisioningTransport = require('azure-iot-provisioning-device-mqtt').MqttWs;

var SymmetricKeySecurityClient =
  require("azure-iot-security-symmetric-key").SymmetricKeySecurityClient;
var ProvisioningDeviceClient =
  require("azure-iot-provisioning-device").ProvisioningDeviceClient;

var provisioningHost = "global.azure-devices-provisioning.net";
var idScope = "0ne0043EA7B";
// 아이디는 디바이스마다 유일한 값을 찾아서 적용 ex) MAC주소, 시리얼번호, 기타 관리번호
var registrationId = "mzdev-dps-001";
var symmetricKey =
  "SNcyzk5oZEsD+u2v8ssUZFVsh5uKG5NtwSdRqIdQSQ4hSYF/tl9j5zoYeRlrCPm0D9OKRWxv48+7jzr2XpZJqw==";

function computeDerivedSymmetricKey(masterKey, regId) {
  return crypto
    .createHmac("SHA256", Buffer.from(masterKey, "base64"))
    .update(regId, "utf8")
    .digest("base64");
}
var symmetricKey = computeDerivedSymmetricKey(symmetricKey, registrationId);

var provisioningSecurityClient = new SymmetricKeySecurityClient(
  registrationId,
  symmetricKey
);

var provisioningClient = ProvisioningDeviceClient.create(
  provisioningHost,
  idScope,
  new ProvisioningTransport(),
  provisioningSecurityClient
);
// Register the device.
provisioningClient.register(function (err, result) {
  if (err) {
    console.log("error registering device: " + err);
  } else {
    console.log("registration succeeded");
    console.log("assigned hub=" + result.assignedHub);
    console.log("deviceId=" + result.deviceId);

    var connectionString =
      "HostName=" +
      result.assignedHub +
      ";DeviceId=" +
      result.deviceId +
      ";SharedAccessKey=" +
      symmetricKey;
    var hubClient = Client.fromConnectionString(
      connectionString,
      iotHubTransport
    );

    hubClient.open(function (err) {
      if (err) {
        console.error("Could not connect: " + err.message);
      } else {
        console.log("Client connected");

        // 오류발생
        hubClient.on("error", function (err) {
          console.error(err.message);
        });
        // 연결끊김
        hubClient.on("disconnect", function () {
          client.open(connectCallback);
        });
        // C2D 메시지
        hubClient.on("message", function (msg) {
          console.log(
            "C2D Message:" + "Id: " + msg.messageId + " Body: " + msg.data
          );
          // When using MQTT the following line is a no-op.
          hubClient.complete(msg, printResultFor("completed"));
          // The AMQP and HTTP transports also have the notion of completing, rejecting or abandoning the message.
          // When completing a message, the service that sent the C2D message is notified that the message has been processed.
          // When rejecting a message, the service that sent the C2D message is notified that the message won't be processed by the device. the method to use is client.reject(msg, callback).
          // When abandoning the message, IoT Hub will immediately try to resend it. The method to use is client.abandon(msg, callback).
          // MQTT is simpler: it accepts the message by default, and doesn't support rejecting or abandoning a message.
        });
        // Direct Method
        hubClient.onDeviceMethod("opneDoor", function (req, res) {
          console.log(
            "Received method call for method '" + req.methodName + "'"
          );

          // if there's a payload just do a default console log on it
          if (!!req.payload) {
            console.log("Payload:\n" + JSON.stringify(req.payload));
          }

          // 응답
          res.send(200, '{"status": "success"}', function (err) {
            if (!!err) {
              console.error(
                "An error ocurred when sending a method response:\n" +
                  err.toString()
              );
            } else {
              console.log(
                "Response to method '" +
                  req.methodName +
                  "' sent successfully."
              );
            }
          });
        });

        // Create device Twin
        hubClient.getTwin(function (err, twin) {
          if (err) {
            console.error("could not get twin");
          } else {
            console.log("twin created");

            // 디바이스 트윈 (Desired)
            twin.on("properties.desired", function (delta) {
              console.log("new desired properties received:");
              console.log(JSON.stringify(delta));
            });

            twin.on("properties.desired.fanSpeed", function (delta) {
              console.log("fanSpeed desired properties received:");
              console.log(JSON.stringify(delta));
            });

            // 디바이스 트윈 (Reported)
            var patch = {
              firmwareVersion: "1.2.1",
              sensor: {
                temperature: 20 + Math.random() * 10,
                humidity: 60 + Math.random() * 20,
              },
            };
            twin.properties.reported.update(patch, function (err) {
              if (err) throw err;
              console.log("twin state reported");
            });
          }
        });

        // Create a message and send it to the IoT Hub every two seconds
        setInterval(function () {
          var temperature = 20 + Math.random() * 10; // range: [20, 30]
          var humidity = 60 + Math.random() * 20; // range: [60, 80]
          var data = JSON.stringify({
            deviceId: "myFirstDevice",
            temperature: temperature,
            humidity: humidity,
          });
          var message = new Message(data);

          message.properties.add(
            "temperatureAlert",
            temperature > 28 ? "true" : "false"
          );

          console.log("Sending message: " + message.getData());

          hubClient.sendEvent(message, printResultFor("send"));
        }, 60000);
      }
    });
  }
});

// Helper function to print results in the console
function printResultFor(op) {
  return function printResult(err, res) {
    if (err) console.log(op + " error: " + err.toString());
    if (res) console.log(op + " status: " + res.constructor.name);
  };
}
