# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for
# full license information.

import time
import sys
import os
import requests
import json
from azure.iot.device import IoTHubModuleClient, Message

# global counters
SENT_IMAGES = 0

# global client
CLIENT = None

# Send a message to IoT Hub
# Route output1 to $upstream in deployment.template.json
def send_to_hub(strMessage):
    message = Message(bytearray(strMessage, 'utf8'))
    CLIENT.send_message_to_output(message, "output1")
    global SENT_IMAGES
    SENT_IMAGES += 1
    print( "Total images sent: {}".format(SENT_IMAGES) )

# Send an image to the image classifying server
# Return the JSON response from the server with the prediction result
def sendFrameForProcessing(imagePath, imageProcessingEndpoint):
    headers = {'Content-Type': 'application/octet-stream'}

    with open(imagePath, mode="rb") as test_image:
        try:
            response = requests.post(imageProcessingEndpoint, headers = headers, data = test_image)
            print("Response from classification service: (" + str(response.status_code) + ") " + json.dumps(response.json()) + "\n")
        except Exception as e:
            print(e)
            print("No response from classification service")
            return None

    return json.dumps(response.json())

def main(imagePath, imageProcessingEndpoint):
    try:
        print ( "Simulated camera module for Azure IoT Edge. Press Ctrl-C to exit." )

        try:
            global CLIENT
            CLIENT = IoTHubModuleClient.create_from_edge_environment()
        except Exception as iothub_error:
            print ( "Unexpected error {} from IoTHub".format(iothub_error) )
            return

        print ( "The sample is now sending images for processing and will indefinitely.")

        while True:
            classification = sendFrameForProcessing(imagePath, imageProcessingEndpoint)
            if classification:
                send_to_hub(classification)
            time.sleep(10)

    except KeyboardInterrupt:
        print ( "IoT Edge module sample stopped" )

if __name__ == '__main__':
    try:
        # Retrieve the image location and image classifying server endpoint from container environment
        IMAGE_PATH = os.getenv('IMAGE_PATH', "")
        IMAGE_PROCESSING_ENDPOINT = os.getenv('IMAGE_PROCESSING_ENDPOINT', "")
    except ValueError as error:
        print ( error )
        sys.exit(1)

    if ((IMAGE_PATH and IMAGE_PROCESSING_ENDPOINT) != ""):
        main(IMAGE_PATH, IMAGE_PROCESSING_ENDPOINT)
    else: 
        print ( "Error: Image path or image-processing endpoint missing" )