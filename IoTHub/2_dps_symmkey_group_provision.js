'use strict';

var iotHubTransport = require('azure-iot-device-mqtt').Mqtt;
var Client = require('azure-iot-device').Client;
var Message = require('azure-iot-device').Message;
var crypto = require('crypto');


var ProvisioningTransport = require('azure-iot-provisioning-device-mqtt').Mqtt;
// Feel free to change to using any of the following if you would like to try another protocol.
// var ProvisioningTransport = require('azure-iot-provisioning-device-http').Http;
// var ProvisioningTransport = require('azure-iot-provisioning-device-amqp').Amqp;
// var ProvisioningTransport = require('azure-iot-provisioning-device-amqp').AmqpWs;
// var ProvisioningTransport = require('azure-iot-provisioning-device-mqtt').MqttWs;

var SymmetricKeySecurityClient = require('azure-iot-security-symmetric-key').SymmetricKeySecurityClient;
var ProvisioningDeviceClient = require('azure-iot-provisioning-device').ProvisioningDeviceClient;

var provisioningHost = "global.azure-devices-provisioning.net";
var idScope = "0ne0043EA7B";
// 아이디는 디바이스마다 유일한 값을 찾아서 적용 ex) MAC주소, 시리얼번호, 기타 관리번호
var registrationId = "mzdev-dps-001";
var symmetricKey = "SNcyzk5oZEsD+u2v8ssUZFVsh5uKG5NtwSdRqIdQSQ4hSYF/tl9j5zoYeRlrCPm0D9OKRWxv48+7jzr2XpZJqw==";

function computeDerivedSymmetricKey(masterKey, regId) {
  return crypto.createHmac('SHA256', Buffer.from(masterKey, 'base64'))
    .update(regId, 'utf8')
    .digest('base64');
}
var symmetricKey = computeDerivedSymmetricKey(symmetricKey, registrationId);

var provisioningSecurityClient = new SymmetricKeySecurityClient(registrationId, symmetricKey);

var provisioningClient = ProvisioningDeviceClient.create(provisioningHost, idScope, new ProvisioningTransport(), provisioningSecurityClient);
// Register the device.
provisioningClient.register(function(err, result) {
  if (err) {
    console.log("error registering device: " + err);
  } else {
    console.log('registration succeeded');
    console.log('assigned hub=' + result.assignedHub);
    console.log('deviceId=' + result.deviceId);

    var connectionString = 'HostName=' + result.assignedHub + ';DeviceId=' + result.deviceId + ';SharedAccessKey=' + symmetricKey;
    var hubClient = Client.fromConnectionString(connectionString, iotHubTransport);

    hubClient.open(function(err) {
      if (err) {
        console.error('Could not connect: ' + err.message);
      } else {
        console.log('Client connected');

        // Create a message and send it to the IoT Hub every two seconds
        setInterval(function () {
            var temperature = 20 + (Math.random() * 10); // range: [20, 30]
            var humidity = 60 + (Math.random() * 20); // range: [60, 80]
            var data = JSON.stringify({ deviceId: 'myFirstDevice', temperature: temperature, humidity: humidity });
            var message = new Message(data);

            message.properties.add('temperatureAlert', (temperature > 28) ? 'true' : 'false');

            console.log('Sending message: ' + message.getData());
            
            hubClient.sendEvent(message, printResultFor('send'));
        }, 2000);
      }
    });
  }
});

// Helper function to print results in the console
function printResultFor(op) {
    return function printResult(err, res) {
      if (err) console.log(op + ' error: ' + err.toString());
      if (res) console.log(op + ' status: ' + res.constructor.name);
    }
  }
