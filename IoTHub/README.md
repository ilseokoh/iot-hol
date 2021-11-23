## Reference Arch 
 1. https://aka.ms/AzureIoTArch
 1. IoT와 관련된 서비스(IoT Hub / DPS)

## Portal에서 IoT Hub 만들면서 

 1. Tire S vs B, Unit, 가격표 
 1. 파티션 갯수는 만들때만
 1. SAS vs RBAC 

## 디바이스 시뮬레이션 부터 시작 

 1. Device SDK 설명, C, C#, Javascript, Python, Java. 디바이스가 보통은 MCU 라서 C를 많이 쓰는데. 
 1. 가끔 리눅스가 있다. 가끔 RPi를 쓰는 경우가 있다. 이런 경우에는 Javascript, Python이 좋은 옵션  (https://github.com/Azure/azure-iot-sdks)
 1. 3가지 프로토콜 지원. 우선 알 필요없다. SDK를 쓰면 된다. 
 1. Javascript SDK 를 이용한 디바이스 시뮬레이션 작성 
   1) D2C 메시지 보내기 / Connection String
   2) Explorer에서 받기 

## DPS의 등장 
 1. 왜 필요한지? 필수임 
 1. 작동 방식은? 
 1. DPS 만들기 / IoT Hub 연결 / 그룹 Enrollment / Individual Enrollment
 1. 샘플코드 추가해서 Provisioning 
 1. Kefico 사례

## Warm Path의 구현 
 1.  Function으로 트리거 받아서 Warm Path (Azure function IoT Hub trigger 로 검색)
 1. DB 만들어서 Insert
 1. DB는 어떤것이 가능한가? NoSQL, SQL Database, MySQL 

## Message Routing을 통한 Cold Path 
 1. Storage 또는 Data Lake 로 바로 넣어주는 Route, 메시지를 동시에 Built-in endpoint로 넣으려면 하나더 만들어줘야 한다. 
 1. Event Hub 거쳐서 ADX / TSI 로 넣어주거나 
 1. 그대로 말고 파싱을 해서 정리해서 주고 싶다면 Function 에서 파싱하고 Event Hub에 넣어줌. 
 1. 참고자료: https://docs.microsoft.com/ko-kr/azure/iot-hub/tutorial-routing

## Stream Analytics 를 통한 Hot Path 
 1. ASA 만들기 
 1. Input / output / Query 
 1. 참고자료: https://docs.microsoft.com/ko-kr/azure/stream-analytics/stream-analytics-get-started-with-azure-stream-analytics-to-process-data-from-iot-devices

## IoT Hub의 통신 방식
 1. 그림 
 1. C2D Callback 을 시뮬레이터에서 만들고 Explorer 에서 보내기 
 1. Direct Method를 시뮬레이터에 만들고 Explorer에서 보내기 
 1. Device Twin의 개념
 1. Device Twin 콜백 만들어서 Explorer에 보내기 

## 서버 코드로 
 1. C2D 
 1. Direct Method 
 1. Desired Properties 

## Connected / Disconnected 이벤트 

 1. Event Grid 설정 및 Function Code 
 1. 이벤트를 받을때까지 시간이 걸림