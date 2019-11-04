# Raspberry PI + Azure IoT Edge (45분)

라즈베리파이에 Azure IoT Edge를 설치하고 Azure IoT Hub를 통해서 모듈을 배포해보겠습니다. 특히 Azure 마켓플레이스에 등록된 IoT Edge 모듈을 사용하는 방법도 살펴보겠습니다.

이번 실습을 통해서 

1. Azure IoT Hub 만들기
2. IoT Hub에 Azure IoT Edge 디바이스 생성
3. 라즈베리파이에 Azure IoT Edge 런타임 설치
4. Azure 마켓플레이스에 있는 모듈 설치하기

## 사전준비

- Azure 구독  

  구독이 없으면 [무료 체험계정 만들기](https://azure.microsoft.com/ko-kr/free/)

## Step 0 : 라즈베리파이 실습용 기기 준비

Azure IoT Edge가 작동될 라즈베리파이 기기를 준비합니다. 라즈베리파이에는 Rasbian 리눅스가 설치되어 있어야 합니다. 설치 방법은 [Setting up your Raspberry Pi](https://projects.raspberrypi.org/en/projects/raspberry-pi-setting-up)를 참조 바랍니다.

### Step 0-1 : 라즈베리파이 IP 확인

라즈베리파이에 모니터와 키보드, 마우스를 연결하여 무선 네트워크를 연결하고 IP를 확인합니다.

![WiFi 설정](images/rpi-lab/rpi-wifi-setup.png)

![IP 확인](images/rpi-lab/rpi-wifi-setup-ip.png)

### Step 0-2 : SSH 설정 및 접속

라즈베리파이에 SSH를 연결할 수 있도록 설정합니다. 초기 아이디와 비밀번호는 pi/raspberry 입니다.

![SSH 설정](images/rpi-lab/raspberry_pi_configuration.png)

[Putty](https://www.putty.org/) 등의 툴을 이용하여 SSH에 접속합니다.

![Putty로 SSH 연결](images/rpi-lab/putty.jpg)

![Putty로 SSH 연결](images/rpi-lab/putty_connected.png)

## Step 1 : IoT Hub 만들기

[Azure 포탈](https://portal.azure.com)에서 Azure IoT Hub를 만듭니다.

### Step 1.1 : [Azure 포탈](https://portal.azure.com)에 로그인

웹브라우저로 [http://portal.azure.com](http://portal.azure.com)에 접속한 후 로그인 합니다. 

### Step 1.2 : Azure IoT Hub 만들기

**Create a resource** -> **Internet of Things** -> **IoT Hub** 를 선택합니다. 

![CreateIoTHub](images/IoTHub-Lab/CreateIoTHub.png)

### Step 1.3 : IoT Hub 생성

4가지 파라미터를 입력하여 IoT Hub를 생성합니다. 

| 파라미터        | 설명                                     | 예                       |
| -------------- | ---------------------------------------- | ------------------------ |
| 구독           | Subscription to use for the new IoT Hub  | Azure Free Account       |
| Resource Group | Create a new Resource Group for this lab | IoTHOLGroup              |
| 지역           | Data center region nearest to you        | Korea Central            |
| IoT Hub 이름   | Provide a name that is globally unique   | MsIoTBootCamp1234        |

1. 4가지 값을 입력하여 IoT Hub를 만듭니다. 
2. IoT Hub 이름은 유일해야 합니다. 녹색 체크박스 확인  
3. **Next: Size and scale>>** 선택

![CreateIoTHub2](images/IoTHub-Lab/CreateIoTHub2.png)

### Step 1.4 : Size and Scale 선택

IoT Hub는 가격과 관련된 Scale과 크기가 있습니다. 각 Scale tier는 서로 다른 한도와 기능제약을 가지고 있습니다. 실제 시나리오에 맡는 크기와 Scale을 선택해야 합니다.

여기에서는 무료를 선택하고 `F1: Free tier for Pricing and scale tier` **Review + create** 를 클릭합니다.

> [!NOTE]  
> 무료 IoT Hub는 구독당 하나만 만들수 있습니다. 이미 F1 IoT Hub를 가지고 있다면 Standard 1 (S1)을 선택합니다.

![CreateIoTHub3](images/IoTHub-Lab/CreateIoTHub3.png)

참조 : [https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-scaling](https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-scaling)

### Step 1.5 : IoT Hub 생성 시작

**Create** 버튼을 눌러 IoT Hub 인스턴스를 생성합니다. 

![CreateIoTHub4](images/IoTHub-Lab/CreateIoTHub4.png)

### Step 1.6 : 배포가 완료될 때까지 대기

배포가 완료될 때까지 기다립니다. 

![CreateIoTHub5](images/IoTHub-Lab/CreateIoTHub5.png)

> [!TIP]  
> `Notifications` 알림창을 통해서도 배포 진행상황을 확인 할 수 있습니다. 
>  
> ![CreateIoTHub6](images/IoTHub-Lab/CreateIoTHub6.png)


## Step 2 : 새로운 Azure IoT Edge 디바이스 등록

이번엔 라즈베리파이를 위한 새로운 Azure IOT Edge Device를 등록합니다.

### Step 2.1 : 생성한 IoT Hub에 들어갑니다.

`Go to resource` 버튼을 눌러 IoT Hub에 들어갑니다.

![CreateIoTEdge1.png](images/IoTHub-Lab/CreateIoTEdge1.png)

### Step 2.2 : IoT Edge 메뉴

IoT Edge 메뉴를 선택합니다

![CreateIoTEdge2.png](images/IoTHub-Lab/CreateIoTEdge2.png)

### Step 2.3 : IoT Edge 디바이스 추가

새로운 IoT Edge 디바이스를 추가합니다.

Click **Add an IoT Edge device**  

![CreateIoTEdge3.png](images/IoTHub-Lab/CreateIoTEdge3.png)

### Step 2.4 : Device ID

디바이스 아이디는 IoT Edge 디바이스를 구분하는 ID로 사용됩니다. 

1. 유일한 이름을 사용합니다.
  e.g. IoTHOLRpi

1. **Save** 클릭

![CreateIoTEdge4.png](images/IoTHub-Lab/CreateIoTEdge4.png)

### Step 2.5 : 새로운 IoT Edge 디바이스 확인

새로운 IoT Edge 디바이스를 **Refresh** 버튼을 눌러 확인합니다.

![CreateIoTEdge5.png](images/IoTHub-Lab/CreateIoTEdge5.png)

## Step 3 : Connection string

IoT Edge 디바이스를 IoT Hub에 연결하려면 **Connection String**이 필요합니다. 디바이스 Connection String은 IoT Hub가 디바이스를 인증하는데 사용됩니다. 

향후 사용을 위해서 복사해 놓습니다. 

참고문서 : [https://devblogs.microsoft.com/iotdev/understand-different-connection-strings-in-azure-iot-hub/](https://devblogs.microsoft.com/iotdev/understand-different-connection-strings-in-azure-iot-hub/)

### Step 3.1 : 디바이스 상세 보기

**IoT Edge** 메뉴에서 디바이스 이름을 클릭하면 **Device Details** 페이지를 통해 IoT Edge 디바이스의 상세 정보를 볼 수 있습니다.

> [!TIP]  
> 각 디바이스마다 두개의 Connection String을 제공합니다.

![CreateIoTEdge6.png](images/IoTHub-Lab/CreateIoTEdge6.png)

### Step 3.2 : Connection String 복사

Connection String 두개 중에 하나를 복사해 둡니다.

1. **Copy button** ![Copy](images/IoTHub-Lab/Copy-Icon.png) 을 누르면 클립보드에 복사됩니다.
  ![CreateIoTEdge7.png](images/IoTHub-Lab/CreateIoTEdge7.png)
1. 텍스트 파일에 복사해 둡니다.

## Step 4 : 라즈베리파이 연결하기

> [!IMPORTANT]  
> Step 0에서 라즈베리파이에 SSH 또는 Desktop에 연결할 수 있어야 합니다.

IoT Hub연결을 위한 설정을 위해 라즈베리파이에 접속합니다.

## Step 5 : IoT Edge 런타임 설정

이번엔 라즈베리파이에 Azure IoT Edge 런타임을 설치하고 설정해 보겠습니다.

### Step 5.1 : 준비

- repository configuration 설치
- generated list 복사
- Microsoft GPG public key 설치

```bash
curl https://packages.microsoft.com/config/debian/stretch/multiarch/prod.list > ./microsoft-prod.list

sudo cp ./microsoft-prod.list /etc/apt/sources.list.d/

curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.gpg
sudo cp ./microsoft.gpg /etc/apt/trusted.gpg.d/
```

### Step 5.2 : 컨테이너 런타임 설치

Azure IoT Edge는 OCI 호환 컨테이너 런타임이 필요하고 Moby 엔진을 추천합니다. 

apt 업데이트
```bash
sudo apt-get update
```
Moby 엔진 설치 
```bash
sudo apt-get install moby-engine
```
Moby command-line interface (CLI) 설치
```bash
sudo apt-get install moby-cli
```

### Step 5.3 : IoT Edge 런타임 설치

```bash
sudo apt-get update

sudo apt-get install iotedge
```

### Step 5.4 : 디바이스 Connection String을 입력합니다.

3.2 단계에서 복사해 놓은 Connection String을 /etc/iotedge/config.yaml에 붙여 넣습니다.

```bash
sudo nano /etc/iotedge/config.yaml
```

```yaml
# Manual provisioning configuration
provisioning:
  source: "manual"
  device_connection_string: "<ADD DEVICE CONNECTION STRING HERE>"

# DPS TPM provisioning configuration
# provisioning:
#   source: "dps"
#   global_endpoint: "https://global.azure-devices-provisioning.net"
#   scope_id: "{scope_id}"
#   attestation:
#     method: "tpm"
#     registration_id: "{registration_id}"
```

### Step 5.4 : iotedge 서비스 다시시작 

Azure IoT Edge 서비스를 다시시작합니다. 
```bash
sudo systemctl restart iotedge
```

## Step 6 : 'Simulated Temperature Sensor'를 마켓플레이스에서 Windows Server로 배포하기

Azure 마켓플레이스에는 마이크로소프트가 검증해 놓은 엔터프라이즈 환경에서 사용할 수 있는 다양한 Azure의 애플리케이션과 서비스가 있는 온라인 마켓입니다.  여기에는 미리 많들어 놓은 여러가지 [IoT Edge 모듈](https://aka.ms/iot-edge-marketplace)을 찾아볼 수 있습니다. 이중에서 온도센서 시뮬레이터를 사용해 보겠습니다. 

### Step 6.1 :  Azure IoT Edge 마켓플레이스 열기

Azure 포탈에서도 마켓플레이스를 제공합니다. 

![MarketPlace](images/IoTHub-Lab/SearchMarketPlace.png)

### Step 6.2 : **Simulated Temperature Sensor** 검색

마켓플레이스에서 *simulated* 를 입력하여 검색한 후 **Simulated Temperature Sensor**를 클릭 합니다. 

![SearchSimTempSensor1](images/IoTHub-Lab/SearchSimTempSensor1.png)

### Step 6.3 : Simulated Temperature Sensor 모듈 생성

**Create**를 클릭합니다.

![CreateSimTempSensor](images/IoTHub-Lab/CreateSimTempSensor.png)

### Step 6.4 : IoT Edge 디바이스를 선택

이 단계에서는 어떤 디바이스에 모듈을 배포할지 선택합니다. 이전에 만든 IoT Edge 디바이스를 선택합니다. 

| Parameter            | Description                                                                                                    | Example                 |
| -------------------- | -------------------------------------------------------------------------------------------------------------- | ----------------------- |
| 구독         | 구독을 선택합니다.                                                                                       | Azure Free Account         |
| IoT Hub              | 이전단계에서 만든 IoT Hub를 선택합니다.  | IoTHOLHub       |
| IoT Edge Device 이름 | 이전단계에서 만든 IoT Edge 디바이스를 선택합니다.                          | IoTHOLRpi |

 **Create** 를 클릭하여 다음 단계로 넘어갑니다. 

![SimTempSensor1](images/IoTHub-Lab/SimulatedTempSensor1.png)

### Step 6.5 : 모듈 추가

어떤 경우에는 추가적인 정보를 제공하기도 합니다.

**Next** 을 클릭하여 넘어갑니다.

![SimTempSensor2](images/IoTHub-Lab/SimulatedTempSensor2.png)

### Step 6.6 : 라우팅 설정

IoT Edge 모듈에서 나가고 들어오는 메시지에 대한 라우팅 설정을 해줄 수 있습니다. 여기에서는 모든 메시지를 클라우드 ($upstream)로 보내도록 설정합니다.

```json
{
  "routes": {
    "route": "FROM /messages/* INTO $upstream",
    "upstream": "FROM /messages/* INTO $upstream"
  }
}
```

 **Next** 을 클릭하여 넘어갑니다.

Reference : [https://docs.microsoft.com/en-us/azure/iot-edge/module-composition#declare-routes](https://docs.microsoft.com/en-us/azure/iot-edge/module-composition#declare-routes)

### Step 6.7 : 배포를 전송

**Submit** 버튼을 눌러 temperature simulator를 IoT Edge 디바이스(Windows Server 2019)에 배포합니다. 

![SimTempSensor3](images/IoTHub-Lab/SimulatedTempSensor3.png)

## Step 7 : Temperature Simulator 모듈 배포 확인

 simulated temperature sensor 모듈은 테스트를 위한 온도데이터를 생성합니다. 이런 센서는 서버실, 공장, 풍력발전기 등에 설치 되어 온도, 습도, 압력 등의 값을 발생 시킵니다. 

### Step 7.1 : 모듈 배포 및 작동 확인 

모듈이 클라우드로 부터 IoT Hub를 통해 디바이스까지 배포가 되었는지 확인 합니다. 

```bash
sudo iotedge list
```

### Step 7.2 : 메시지 전송 확인

온도센서에서 클라우드로 보내는 메시지를 모듈 로그를 통해서 확인합니다. 

```bash
sudo iotedge logs SimulatedTemperatureSensor -f
```

### Step 7.3 : IoT Hub에서 메시지 수신 확인 

이번에는 **Device Explorer**를 통해서 IoT Hub가 받은 메시지를 확인해 보겠습니다. **Device Explorer**는 현재 Windows OS에서만 작동합니다. **Device Explorer**의 설치는 [Device Explorer 설치링크](http://aka.ms/deviceexplorer)에서 다운로드 받아서 설치합니다. 

IoT Hub에 접근하기 위해서는 다시 **Connection String**이 필요합니다. 이번에는 IoT Hub Connection String입니다. Device Connection String과 헷갈리면 안됩니다.

1. Device Explorer 실행
    ![DeviceExplorer1](images/WinServer-Lab/DeviceExplorer1.png)
1. **Shared Access Policies**  
    Shared Access Policies 메뉴를 클릭합니다.  
1. **iothubowner** 클릭
    iothubowner를 클릭합니다. 
    ![DeviceExplorer2](images/WinServer-Lab/DeviceExplorer2.png)
1. Connection String 복사
    ![DeviceExplorer3](images/WinServer-Lab/DeviceExplorer3.png)
1. Device Explorer에 Connection String를 복사하여 입력
1. **Update** 클릭
    ![DeviceExplorer4](images/WinServer-Lab/DeviceExplorer4.png)
1. **Data** 탭 선택
1. Device ID를 선택
1. **Monitor** 클릭
1. 메시지 수식 확인
    ![DeviceExplorer4](images/WinServer-Lab/DeviceExplorer5.png)
