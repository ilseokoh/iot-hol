# Ubuntu + Azure IoT Edge - Advance Lab (60분)

Visual Studio Code (VSCode)는 개발자의 개발 속도를 향상시키는 파워풀한 크로스플랫폼 무료 개발환경입니다. 이번 실습에서는 IoT Edge의 개발 프로세스를 알아보면서 VSCode를 사용하여 IoT Edge 모듈을 배포해보겠습니다. 

이번 실습을 통해서 

- VSCode에서 IoT Edge 디바이스 생성
- Ubuntu 에 Azure IoT Edge 런타임 설치 
- Azure Container Registry (ACR) 만들기
- Azure IoT Edge Module의 다운로드와 빌드

## 사전준비

- [Visual Studio Code 설치](https://code.visualstudio.com/download)
- Visual Studio 용 Azure IoT Hub Toolkit, Azure IoT Edge
  ![Azure IoT Edge Extension](images/Register-device-vscode/extension.png)
- Lab 2에서 만든 Azure IoT Hub
- .Net Core 설치
    [https://dotnet.microsoft.com/download/thank-you/dotnet-sdk-2.2.107-windows-x64-installer](https://dotnet.microsoft.com/download/thank-you/dotnet-sdk-2.2.107-windows-x64-installer)

## Step 0: Ubuntu 가상머신 만들기

실습에서 Edge 디바이스로 사용될 Ubuntu 가상머신을 만듭니다. 

### Step 0-1 : Azure에 로그인

[Azure 포탈](https://portal.azure.com)에 로그인합니다.

### Step 0-2 : Ubuntu 서버 새로 만들기

포탈의 왼쪽 위에 "리소스 만들기" 선택하고 Ubuntu로 검색해서
Ubuntu Server 18.04 선택합니다.

![리소스 만들기](images/linux-lab/new-ubuntu.png)

### Step 0-3 : 리눅스 가상머신 만들기

가상머신 만들기 문서를 따라서 진행합니다. SSH 공개키에 익숙하지 않으면 암호를 선택하고 아이디와 비밀번호를 입력합니다. 

[가상머신 만들기](https://docs.microsoft.com/ko-kr/azure/virtual-machines/linux/quick-create-portal#create-virtual-machine) 내용을 참조하여 Ubuntu 가상머신을 만듭니다.

### Step 0-4 : 가상머신에 SSH 연결

SSH를 활용해서 가상머신에 연결해 봅니다.

[가상머신에 연결](https://docs.microsoft.com/ko-kr/azure/virtual-machines/linux/quick-create-portal#connect-to-virtual-machine)

[Putty를 사용하여 SSH 연결](https://archmond.net/?p=7932)할 수 있습니다.

## Step 1: 새로운 Azure IoT Edge device 만들기

이번 실습에서는 Visual Studio Code의 Azure 포탈 대신 Azure IoT Extension을 통해서 Azure IoT Edge 디바이스를 만들어보겠습니다. 

### Step 1.1 : IoT Hub 선택

VSCode의 Azure IoT Extension을 사용하여 IoT Hub에 연결할 수 있습니다. 연결을 위해서 Azure 구독에 로그인하고 IoT Hub를 선택해야 합니다. 

1. Visual Studio Code의 **Explorer** 를 선택합니다. 

1. Explorer 아래쪽에 **Azure IoT Hub Devices** 섹션을 펼칩니다.

1. **Select IoT Hub**.를 선택합니다. 

1. Azure 에 로그인 합니다. 
    Azure 로그인 창을 따라서 Azure에 로그인 합니다.

    ![Expand Azure IoT Hub Devices section](./images/Register-device-vscode/azure-iot-hub-devices.png)

1. 구독과 IoT Hub를 선택합니다. 
    VSCode의 상단에 표시되는 창을 통해서 구독과 이전 실습에서 만든 IoT Hub를 선택합니다. 

    ![SelectSubscriptionAndHub](images/Register-device-vscode/azure-iot-hub-devices2.png)

## Step 2 : 디바이스 생성

이번엔 VSCode를 이용해 Azure IoT Edge 디바이스를 만들어 봅니다. 

1. VSCode의 Explorer창에서, **AZURE IOT HUB** 섹션을 찾습니다. 

1. **...** 을 클릭하여 팝업 메뉴를 표시합니다. 

1. **Create IoT Edge Device**를 선택

    ![VSCode-IoTEdge1](images/IoTEnt-Lab/VSCode-IoTEdgeDevice1.png)

1. 텍스트 박스가 열리면 디바이스 아이디를 입력합니다.

    ![VSCode-IoTEdge2](images/IoTEnt-Lab/VSCode-IoTEdgeDevice2.png)

1. 새로운 IoT Edge 디바이스가 생성된 것을 **AZURE IOT HUB** 섹션에서 확인 합니다. 

    ![VSCode-IoTEdge3](images/IoTEnt-Lab/VSCode-IoTEdgeDevice3.png)

**output** 창에서 디바이스 ID와 디바이스 Connection String이 표시된 결과 메시지를 확인 할 수 있습니다. 

## Step 3 : 디바이스 Connection String 복사

Ubuntu 디바이스가 IoT Hub에 연결되기 위해서는 디바이스 Connection String이 필요합니다. 

1. **Azure IoT Hub Devices** 섹션의 새로 만든 디바이스에서 오른쪽 클릭을 합니다. 

2. **Copy Device Connection String** 선택

    ![VSCode-IoTEdge4](images/IoTEnt-Lab/VSCode-IoTEdgeDevice4.png)

디바이스 connection string 이 클립보드로 복사됩니다. 

**Get Device Info** 메뉴를 통해서 Connectoin String 외의 정보를 **Output** 윈도우에서 확인 할 수 있습니다.

## Step 4 : Ubuntu에 Azure IoT Edge 런타임 설치하기

이번엔 Ubuntu 가상머신에 Azure IoT Edge 런타임을 설치해보겠습니다. [Linux(x64)에서 Azure IoT Edge 런타임 설치](https://docs.microsoft.com/ko-kr/azure/iot-edge/how-to-install-iot-edge-linux) 문서를 따라서 진행합니다. 

### Step 4.1 : Microsoft 키 및 소프트웨어 리포지토리 피드 등록

```bash
$ curl https://packages.microsoft.com/config/ubuntu/18.04/prod.list > ./microsoft-prod.list
  % Total    % Received % Xferd  Average Speed   Time    Time     Time  Current
                                 Dload  Upload   Total   Spent    Left  Speed
100    77  100    77    0     0    235      0 --:--:-- --:--:-- --:--:--   235
```

### Step 4.2 : 생성된 목록에 복사

```bash
$ sudo cp ./microsoft-prod.list /etc/apt/sources.list.d/
```
### Step 4.3 : Microsoft GPG 공개 키를 설치

```bash
$ curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.gpg
  % Total    % Received % Xferd  Average Speed   Time    Time     Time  Current
                                 Dload  Upload   Total   Spent    Left  Speed
100   983  100   983    0     0   2445      0 --:--:-- --:--:-- --:--:--  2445
$ sudo cp ./microsoft.gpg /etc/apt/trusted.gpg.d/
```

### Step 4.4 : 컨테이너 런타임 설치

```bash
$ sudo apt-get update
$ sudo apt-get install moby-engine
$ sudo apt-get install moby-cli
```

### Step 4.5 : 모비 호환성을 위해 Linux 커널을 확인

```bash
$ curl -sSL https://raw.githubusercontent.com/moby/moby/master/contrib/check-config.sh -o check-config.sh
$ chmod +x check-config.sh
$ ./check-config.sh
```

### Step 4.6 : Azure IoT Edge 보안 디먼 설치

```bash
$ sudo apt-get update
$ sudo apt-get install iotedge
```

### Step 4.7 : Azure IoT Edge 보안 디먼 구성 - 수동구성 

디먼은 /etc/iotedge/config.yaml에 있는 구성 파일을 사용하여 구성할 수 있습니다. 이 파일은 기본적으로 쓰기 금지되어 있습니다 편집하려면 관리자 권한이 필요합니다. 

VSCode에서 복사해 놓은 디바이스 Connection String을 YAML파일에 입력해줍니다. 

```bash
$ sudo nano /etc/iotedge/config.yaml
```

```yaml
provisioning:
  source: "manual"
  device_connection_string: "<ADD DEVICE CONNECTION STRING HERE>"

# provisioning:
#   source: "dps"
#   global_endpoint: "https://global.azure-devices-provisioning.net"
#   scope_id: "{scope_id}"
#   registration_id: "{registration_id}"
```

파일을 저장하고 닫습니다.
CTRL + X, Y, Enter

### Step 4.8 : 데몬 다시시작

```bash
$ sudo systemctl restart iotedge
```

### Step 4.9 : 서비스 상태 확인  

```bash
$ systemctl status iotedge
● iotedge.service - Azure IoT Edge daemon
   Loaded: loaded (/lib/systemd/system/iotedge.service; enabled; vendor preset: enabled)
   Active: active (running) since Mon 2019-06-24 05:51:26 UTC; 13s ago
     Docs: man:iotedged(8)
 Main PID: 5932 (iotedged)
    Tasks: 9 (limit: 9513)
   CGroup: /system.slice/iotedge.service
           └─5932 /usr/bin/iotedged -c /etc/iotedge/config.yaml

Jun 24 05:51:28 UbuntuIoT iotedged[5932]: 2019-06-24T05:51:28Z [INFO] - Updating identity for module $edgeAgent
Jun 24 05:51:28 UbuntuIoT iotedged[5932]: 2019-06-24T05:51:28Z [INFO] - Pulling image mcr.microsoft.com/azureiotedge-agent:1.0...
Jun 24 05:51:35 UbuntuIoT iotedged[5932]: 2019-06-24T05:51:35Z [INFO] - Successfully pulled image mcr.microsoft.com/azureiotedge-agent:1.0
Jun 24 05:51:35 UbuntuIoT iotedged[5932]: 2019-06-24T05:51:35Z [INFO] - Creating module edgeAgent...
lines 1-19/19 (END) 
```
### Step 4.10 : 실행중인 모듈 확인 

```bash 
$ sudo iotedge list
NAME             STATUS           DESCRIPTION      CONFIG
edgeAgent        running          Up 2 minutes     mcr.microsoft.com/azureiotedge-agent:1.0
edgeHub          running          Up a minute      mcr.microsoft.com/azureiotedge-hub:1.0
```

## Step 5 : Azure Container Registry (ACR) 만들기

우리가 만든 도커이미지를 배포하기 위해서는 이미지를 컨테이너 레지스트리에 푸시 해야 합니다. 일단 이미지가 레지스트리에 올라가면 직접 IoT Edge 디바이스에 배포할 수 있습니다. Azure 에서는 Private 컨테이너 레지스트리인 Azure Container Registry(ACR)를 제공하는데 ACR을 만들어보겠습니다. 

### Step 5.1 : Azure Container Registry 만들기

1. [Azure Portal](https://portal.azure.com/)에 로그인 합니다. 

1. **Container Registry** 를 검색하여 만들기를 클릭합니다. 
    **Create a resource** -> **Containers** -> **Container Registry**

    ![AcrCreate1](images/IoTEnt-Lab/ACR-Create1.png)

### Step 5.2 : ACR 생성 파라미터를 입력

리소스그룹이름과 레지스트리 이름을 입력합니다.  

![AcrCreate2](images/IoTEnt-Lab/ACR-Create2.png)

|Parameter  |Description  |Example  |
|---------|---------|---------|
|Registry Name     | Unique name within Azure and 5-50 alphanumeric characters only | IoTHOL2019acr1         |
|Subscription     | Your subscription         |         |
|Resource Group     | Select the resource group used for your IoT Hub         | IoTHOL2019         |
|Location     | Select the same region as your IoT Hub         | Korea Central         |
|Admin user     | This enables "User Name" and "Password" to access ACR.  Set to **Enable** | Enabled          |
|SKU     | Tier of ACR.  Different tier gives different storage size limit, etc.  Set to **Basic** for this lab         | Basic        |

### Step 5.3 : ACR 배포를 시작

**Create** 를 클릭하여 배포를 시작합니다. 

### Step 5.4 : ACR에 로그인

후반부에 우리가 만든 컨테이너 이미지를 푸시 하기 위해서는 ACR에 로그인을 해야 합니다. 

1. VSCode open Terminal  

    터미널 윈도우가 없다면 메뉴에서 **Terminal** -> **New Terminal**를 선택합니다.

    ![AcrCreate2](images/IoTEnt-Lab/ACR-Create3.png)

1. ACR Access Key  
    포탈에서 ACR의 `Access Key`를 복사합니다. 

    ![AcrCreate4](images/IoTEnt-Lab/ACR-Create4.png)

1. ACR에 아래 명령으로 로그인

    ```bash
    docker login <Your ACR Login Server Name> -u <Your ACR user name> -p <Your ACR password>
    ```
  
    예시 :

    ![AcrCreate5](images/IoTEnt-Lab/ACR-Create5.png)

관련자료 :

- ACR Authentication : [https://docs.microsoft.com/en-us/azure/container-registry/container-registry-authentication](https://docs.microsoft.com/en-us/azure/container-registry/container-registry-authentication)

## Step 6 : IoT Edge 모듈 샘플코드 준비

이번엔 샘플 코드를 통해서 모듈을 빌드해보겠습니다. 

- Github에서 `Simulated Temperature Sensor` 샘플 코드를 다운로드
- 컴파일 / 빌드
- Azure Container Registry로 업로드
- 컨테이너를 배포

샘플코드 리파지토리 : [IoT Edge Samples](https://github.com/Azure/iotedge)

### Step 6.1 : 소스코드 다운로드

아래 명령으로 Github에서 `Simulated Temperature Sensor` 소스코드를 다운로드 합니다. 

1. Console (CMD)이나 VSCode terminal 을 열기
1. git 명령 실행
    예 : C:\Repo 위치로 클론
  
    ```powershell  
    cd C:\Repo
    git clone https://github.com/Azure/iotedge
    ```

    ![SimTempSensor1](images/IoTEnt-Lab/SimulatedTempSensor1.png)

### Step 6.2 : 샘플 빌드

.NET 코어를 이용하여 빌드 합니다. 

> [!NOTE]  
> 아래 명령을 통해서 dotnet 코어가 설치되어 있는지 확인 할 수 있습니다. 
>
> ```ps  
> PS C:\repo> dotnet --version  
> 2.2.204  
> PS C:\repo>  
> ```
>
> 이 명령은 .NET Core SDK의 버전을 보여줍니다. 

참조 : [https://dotnet.microsoft.com/download/thank-you/dotnet-sdk-2.2.107-windows-x64-installer](https://dotnet.microsoft.com/download/thank-you/dotnet-sdk-2.2.107-windows-x64-installer)

### Step 6.3 : Simulated Temperature Sensor 모듈 컴파일

컨테이너 모듈을 Azure IoT Edge에 배포하기 전에 컴파일하고 로컬에서 실행시켜봐야 합니다. 

1. `SimulatedTemperatureSensor` 디렉토리로 이동  
    `SimulatedTemperatureSensor` 코드가 있는 디렉토리로 이동합니다. 
    소스코드는 이런 위치에 있습니다.  `<SampleRoot>\edge-modules\SimulatedTemperatureSensor`

    예 : C:\repo\iotedge\edge-modules\SimulatedTemperatureSensor

    ```powershell
    PS C:\repo\iotedge\edge-modules\SimulatedTemperatureSensor> dir
  
        Directory: C:\repo\iotedge\edge-modules\SimulatedTemperatureSensor
  
    Mode                LastWriteTime         Length Name
    ----                -------------         ------ ----
    d-----        5/20/2019   8:01 PM                config
    d-----        5/20/2019   8:01 PM                docker
    d-----        5/20/2019   8:01 PM                src
    -a----        5/20/2019   8:01 PM           2958 SimulatedTemperatureSensor.csproj

    ```

1. 샘플코드 빌드
    dotnet 명령으로 빌드합니다. 

    ```ps
    dotnet publish -r ubuntu.18.04-x64
    ```

    바이너리는 아래 위치에 생성됩니다. 
    `C:\repo\iotedge\edge-modules\SimulatedTemperatureSensor\bin\Debug\netcoreapp2.1\win-x64`  

    예시 :
  
    ```powershell
    PS C:\repo> cd .\iotedge\edge-modules\SimulatedTemperatureSensor\
    PS C:\repo\iotedge\edge-modules\SimulatedTemperatureSensor> dotnet publish -r ubuntu.18.04-x64
    Microsoft (R) Build Engine version 16.0.450+ga8dc7f1d34 for .NET Core
    Copyright (C) Microsoft Corporation. All rights reserved.
  
      Restore completed in 6.21 sec for C:\repo\iotedge\edge-util\src\Microsoft.Azure.Devices.Edge.Util\Microsoft.Azure.Devices.Edge.Util.csproj.
      Restore completed in 8.48 sec for C:\repo\iotedge\edge-modules\SimulatedTemperatureSensor\SimulatedTemperatureSensor.csproj.
      Restore completed in 8.59 sec for C:\repo\iotedge\edge-modules\ModuleLib\Microsoft.Azure.Devices.Edge.ModuleUtil.csproj.
      Microsoft.Azure.Devices.Edge.Util -> C:\repo\iotedge\edge-util\src\Microsoft.Azure.Devices.Edge.Util\bin\Debug\netstandard2.0\Microsoft.Azure.Devices.Edge.Util.dll
      Microsoft.Azure.Devices.Edge.ModuleUtil -> C:\repo\iotedge\edge-modules\ModuleLib\bin\Debug\netcoreapp2.1\Microsoft.Azure.Devices.Edge.ModuleUtil.dll
      SimulatedTemperatureSensor -> C:\repo\iotedge\edge-modules\SimulatedTemperatureSensor\bin\Debug\netcoreapp2.1\win-x64\SimulatedTemperatureSensor.dll
      SimulatedTemperatureSensor -> C:\repo\iotedge\edge-modules\SimulatedTemperatureSensor\bin\Debug\netcoreapp2.1\win-x64\publish\
      ```

## Step 7: Simulated Temperature Sensor 컨테이너

온도센서 모듈이 Edge 모듈로 작동하기 위해서는 컨테이너 이미지로 만들어야 합니다. 

아래 내용에 주의를 기울여야 합니다.

1. Container Type
1. Container Image Name
1. Container Tag
1. Dockerfile  
    Dockerfile 은 컨테이너 이미지를 어떻게 만드는지에 대하여 기술되어 있습니다. 

컨테이너 이미지 이름은 두개의 파트로 이뤄져 있습니다. 

### Container Type

두가지 타입이 있습니다.

- Linux container
- Windows Container

여기서는 Linux Container를 사용합니다. 

### Container Image Name

이미지 이름은 슬래시로 분리되어 있는데 첫번째 파트는 레지스트리 호스트 이름입니다. 

예 : myazureregistry.azurecr.io/myimage

규칙 :

- Lowercase letters
- Digits
- Separators (`.`, `_`, `-`)

### Container Tag

태그로 특정 이미지를 구분합니다. 보통 버전번호와 함께 사용됩니다.

규칙 :

- 영문자 대소문자
- 숫자
- Underscores (`_`), Periods (`.`), and dashes (`-`)

### Step 7.1 : Container Type

Step 4에서 IoT Edge 런타임을 Linux 버전을 설치했기 때문에 이미지를 만드는 환경도 **Linux Container**가 되어야 합니다. 

1. Docker for Desktop 설정 열기  

    Taskbar에서 `Docker Desktop` 아이콘을 오른쪽 클릭하고 메뉴를 표시합니다. 

    ![Docker1](images/IoTEnt-Lab/Docker1.png)

1. **Switch to Windows containers...**  메뉴가 보이면 현재 Linux Container 입니다. 만약 Switch to Linux containers 가 표시되면 클릭 합니다. 
  
    ![Docker2](images/IoTEnt-Lab/Docker2.png)

1. Confirm Warning  

    ![Docker3](images/IoTEnt-Lab/Docker3.png)
  
1. Docker Desktop이 container type을 변경할 때 까지 기다립니다. 

### Step 7.2 : 컨테이너 이미지 빌드

아래 명령으로 컨테이너 이미지를 빌드 합니다. 

```ps
docker build .\bin\Debug\netcoreapp2.1\win-x64\publish -t <Tag> -f <Dockerfile>
```

이미지 이름 **SimulatedTemperatureSensor**

```ps
docker build .\bin\Debug\netcoreapp2.1\ubuntu.18.04-x64\publish\ -t simulatedtemperaturesensor -f .\docker\linux\amd64\Dockerfile
```

아래 명령으로 이미지가 생성되었는지 확인 합니다. 

```ps
docker images
```

예 :

```ps
PS C:\repo\iotedge\edge-modules\SimulatedTemperatureSensor> docker images
REPOSITORY                              TAG                      IMAGE ID            CREATED             SIZE
simulatedtemperaturesensor              latest                   088860719e39        4 seconds ago       398MB
mcr.microsoft.com/dotnet/core/runtime   2.1.10-nanoserver-1809   d6a221ed7eed        4 weeks ago         321MB
```

### Step 7.3 : 이미지 Tag

`docker tag` 커멘드를 이용해서 ACR에 푸시하기위한 태그이름으로 변경해줍니다. 

```ps
docker tag <Source Image>[:TAG] <Target Image>[:TAG]
```

예 : Using the image just built, tag them with **simtemp**:0.0.1

```ps
docker tag simulatedtemperaturesensor:latest iothol2019acr1.azurecr.io/simtemp:0.0.1
```

또는 이미지 아이디(IMAGE ID)를 이용해서 같은 명령을 수행할 수 있습니다. 

```ps
docker tag 088860719e39 iotbootcamp2019acr1.azurecr.io/simtemp:0.0.1
```

### Step 7.4 : 이미지 Push

이제 이미지를 빌드하고 태깅했으므로 업로드(push)할 준비가 다 되었습니다. 

`docker push` 명령으로 ACR에 push 합니다. 

```ps
docker push <Your Tagged Image Name>
```

Example :

```ps
docker push iotbootcamp2019acr1.azurecr.io/simtemp:0.0.1
```

Example Output :

```ps
PS C:\repo\iotedge\edge-modules\SimulatedTemperatureSensor> docker push iotbootcamp2019acr1.azurecr.io/simtemp:0.0.1
The push refers to repository [iotbootcamp2019acr1.azurecr.io/simtemp]
da499279ea2b: Pushed
416b2cdadff0: Pushed
b366e64214b3: Pushed
f354c3635730: Pushed
146252efae6c: Pushed
6eaf1cf63dfc: Pushed
a2bb3d322957: Pushed
761ff3ef5aab: Pushed
5c3e3ab9e119: Pushed
273db4b66a2d: Skipped foreign layer
0.0.1: digest: sha256:ce4d56e9a062f7a77e8008d1e57eceda455555f8083194fec30fffc302c56598 size: 2507
```

### Step 7.5 : ACR에 이미지가 Push 되었는지 확인

ACR에 이미지가 Push 되었는지 확인합니다. 

1. [Azure Portal](https://portal.azure.com)에 로그인 
1. ACR 로 이동
1. **Repositories** 메뉴로 이동  
1. 이미지가 있는지 태그와 함께 확인

예를들어 `simtemp` 이름과 태그 `0.0.1` 확인

![SimTempSensor2](images/IoTEnt-Lab/SimulatedTempSensor2.png)

## Step 8 : Deploy to your Ubuntu Linux

이제 Azure IoT Edge 디바이스인 Ubuntu 가상머신에 Edge 모듈을 배포할 준비가 완료되었습니다. 이미지를 배포하기 위해서는 `Deployment Manifest`가 필요합니다. 

여기에서는 Azure 포탈에서 `Deployment Manifest`를 만들어서 배포하겠습니다. 

### Step 8.1 : IoT Edge 상세 페이지

1. [Azure Portal](https://portal.azure.com)에 로그인
1. IoT Hub의 IoT Edge Device ID로 이동

    **IoT Hub** -> **IoT Edge** -> **Ubuntu Device ID**

    ![SimTempSensor3](images/IoTEnt-Lab/SimulatedTempSensor3.png)

    > [!NOTE]  
    > 이 단계에서는 런타임 에러를 볼 수 있습니다. 
    > 이는 아직 아무것도 배포를 하지 않았기 때문에 발생합니다. 

1. **Device details** 상세 페이지로 가서 **Set modules** 클릭

    ![SimTempSensor4](images/IoTEnt-Lab/SimulatedTempSensor4.png)

### Step 8.2 : 컨테이너 레지스트리 셋팅

IoT Edge 런타임이 ACR에서 이미지를 Pull 해야하기 때문에 아래 정보를 입력해줍니다. 

Refer to [Step 5.4 : Login to your ACR](#step-54--login-to-your-acr)

|Parameter  |Description  |Example  |
|---------|---------|---------|
|Name     | Name of this setting | BootCampACR   |
|Address     | Your ACR Login Server Address        | iotbootcamp2019acr1.azurecr.io         |
|User Name   | Your ACR Admin User Name       | iotbootcamp2019acr1        |
|Password     | Your ACR Password        |         |

![SimTempSensor5](images/IoTEnt-Lab/SimulatedTempSensor5.png)

### Step 8.3 : Add Module

다음은 배포할 모듈을 지정해 줍니다.

1. **+Add** 클릭
1. **IoT Edge Module** 선택
1. 모듈 이름 입력  
    이 이름은 Azure IoT Edge가 사용하는 이름입니다. (vs. Module name)
1. 이미지의 URL
    태그를 포함한 풀 이미지 이름을 입력합니다.
1. **Save** 클릭
1. **Next** 클릭

![SimTempSensor6](images/IoTEnt-Lab/SimulatedTempSensor6.png)

### Step 8.4 : 메시지 라우팅 설정

Lab 2 와 마찬가지로 모듈 메시지가 IoT Hub로 전달되도록 설정합니다. 

기본값으로 두고 **Next**를 클릭합니다. 

### Step 8.5 : Submit

포탈이 만든 `Deployment Manifest`를 검토합니다.

> [!WARNING]  
> Deployment Manifest는 ACR의 아이디 패스워드를 포함하고 있음. 

**Submit**을 클릭합니다. 

### Step 8.6 : 배포 모니터링

배포는 몇 분이 걸릴 수 있습ㄴ디ㅏ. **edgeAgent**의 로그를 통해서 진행상황을 체크할 ㅅ수 있습니다. 

Edge Agent 는 Azure IoT Edge 런타임의 일부로 Azure IoT Hub 와 통신하고 모듈 배포를 책임 집니다. 

`iotedge logs` 명령을 통해서 배포상태를 체크합니다. 

```ps
iotedge logs -f edgeAgent
```

Enter `Ctrl + c`를 눌러 모니터링을 끝낼 수 있습니다. `iotedge list` 명령을 통해서 모듈 리스트와 상태를 확인 할 수 있습니다. 

예 :

```ps
PS C:\repo\iotedge\edge-modules\SimulatedTemperatureSensor> iotedge logs -f edgeAgent
[06/09/2019 03:32:14.668 PM] Edge Agent Main()
2019-06-09 08:32:14.900 -07:00 [INF] - Starting module management agent.
2019-06-09 08:32:15.077 -07:00 [INF] - Version - 1.0.7.1.22377503 (f7c51d92be8336bc6be042e1f1f2505ba01679f3)
2019-06-09 08:32:15.078 -07:00 [INF] -
        █████╗ ███████╗██╗   ██╗██████╗ ███████╗
       ██╔══██╗╚══███╔╝██║   ██║██╔══██╗██╔════╝
       ███████║  ███╔╝ ██║   ██║██████╔╝█████╗
       ██╔══██║ ███╔╝  ██║   ██║██╔══██╗██╔══╝
       ██║  ██║███████╗╚██████╔╝██║  ██║███████╗
       ╚═╝  ╚═╝╚══════╝ ╚═════╝ ╚═╝  ╚═╝╚══════╝

 ██╗ ██████╗ ████████╗    ███████╗██████╗  ██████╗ ███████╗
 ██║██╔═══██╗╚══██╔══╝    ██╔════╝██╔══██╗██╔════╝ ██╔════╝
 ██║██║   ██║   ██║       █████╗  ██║  ██║██║  ███╗█████╗
 ██║██║   ██║   ██║       ██╔══╝  ██║  ██║██║   ██║██╔══╝
 ██║╚██████╔╝   ██║       ███████╗██████╔╝╚██████╔╝███████╗
 ╚═╝ ╚═════╝    ╚═╝       ╚══════╝╚═════╝  ╚═════╝ ╚══════╝

2019-06-09 08:32:15.200 -07:00 [INF] - Started operation refresh twin config
2019-06-09 08:32:15.226 -07:00 [INF] - Edge agent attempting to connect to IoT Hub via Amqp_Tcp_Only...
2019-06-09 08:32:15.822 -07:00 [INF] - Created persistent store at C:\Windows\TEMP\edgeAgent
2019-06-09 08:32:16.326 -07:00 [INF] - Edge agent connected to IoT Hub via Amqp_Tcp_Only.
2019-06-09 08:32:16.693 -07:00 [INF] - Obtained Edge agent twin from IoTHub with desired properties version 2 and reported properties version 4.
2019-06-09 08:32:17.398 -07:00 [INF] - Plan execution started for deployment 2
2019-06-09 08:32:17.432 -07:00 [INF] - Executing command: "Command Group: (
  [Create module mysimtempsensor]
  [Start module mysimtempsensor]
)"
2019-06-09 08:32:17.434 -07:00 [INF] - Executing command: "Create module mysimtempsensor"
2019-06-09 08:32:47.091 -07:00 [INF] - Executing command: "Start module mysimtempsensor"
2019-06-09 08:32:48.152 -07:00 [INF] - Executing command: "Command Group: (
  [Create module edgeHub]
  [Start module edgeHub]
)"
2019-06-09 08:32:48.152 -07:00 [INF] - Executing command: "Create module edgeHub"
2019-06-09 08:32:56.393 -07:00 [INF] - Executing command: "Start module edgeHub"
2019-06-09 08:32:57.177 -07:00 [INF] - Plan execution ended for deployment 2
2019-06-09 08:32:57.513 -07:00 [INF] - Updated reported properties
2019-06-09 08:33:02.740 -07:00 [INF] - Updated reported properties

PS C:\repo\iotedge\edge-modules\SimulatedTemperatureSensor> iotedge list
NAME             STATUS           DESCRIPTION      CONFIG
edgeAgent        running          Up 4 minutes     mcr.microsoft.com/azureiotedge-agent:1.0
edgeHub          running          Up 3 minutes     mcr.microsoft.com/azureiotedge-hub:1.0
mysimtempsensor  running          Up 3 minutes     iotbootcamp2019acr1.azurecr.io/simtemp:0.0.1
```

## Step 9 : 모듈 확인

아래 순서로 모듈의 상태를 확인 해 봅니다. [Lab 2](lab2-edge-basic.md#step-7--temperature-simulator-모듈-배포-확인) 

- `iotedge list` 
- `iotedge logs -f <You Module Name>` 
- `Device Explorer`