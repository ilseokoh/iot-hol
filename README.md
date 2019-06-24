# Azure IoT 기술 실습(Hands On Lab) 자료

## 사전준비 내용

1. Azure 구독 : [체험계정 만들기](https://azure.microsoft.com/ko-kr/free/)

1. 실습용 노트북 준비: Windows 10 PC + 개발환경 설치 
    * [Visual Studio Community 2017 이상 설치](https://www.visualstudio.com/ko/downloads/) 
    * [Device Explorer설치](https://github.com/Azure/azure-iot-sdk-csharp/releases/download/2019-1-4/SetupDeviceExplorer.msi)
    * [Docker Community Edition설치](https://docs.docker.com/docker-for-windows/install/)
    * [Azure Storage Explore 설치 (권장)](https://azure.microsoft.com/en-us/features/storage-explorer/)

 1. SmartMeterSimulator 설치 - 첫째날 실습용
 텔레메트리 데이터를 생성하는 IoT 디바이스로 스마트 미터라는 사전 제작된 시뮬레이터를 사용하게 됩니다. 이를 위하여 개인 실습 PC에 아래 안내에 따라 사전 설치하여 와주시면 감사하겠습니다. 실제 동작하는 방법은 워크샵에서 같이 진행 할 예정이오니, Visual Studio 설치 및 패키지 다운로드만 사전 완료해주시기 바랍니다.
    1. SmartMeterSimulator 스타터 솔루션을 [다운로드](https://bit.ly/2wMSwsH)합니다. 
    1. 콘텐츠를 C:\SmartMeter\ 폴더에 압축 해제합니다. 
    1. Visual Studio 2017에서 SmartMeterSimulator.sln을 엽니다.
    1. 메시지가 나타나면 Visual Studio에 로그인하거나 계정을 생성합니다.
    1. SmartMeterSimulator를 위한 보안 경고 창이 나타나면 이 솔루션의 모든 프로젝트에 관해 물어보기를 선택 취소한 다음 확인을 선택합니다. 
    1. 이 시점에서 솔루션을 빌드하려고 할 경우 많은 오류가 발생합니다. 이는 의도적입니다. 본 과정에서 이러한 빌드 오류를 수정합니다.


## Lab 2: Azure IoT Edge - Basic

[Windows Server 2019 + Azure IoT Edge Lab](lab2-edge-basic.md)

## Lab 3: Azure IoT Edge - Advanced 

[Windows + Azure IoT Edge -Advanced Lab](lab3-edge-advanced.md)

## Lab 4: Azure IoT Edge - Intelligent

[Building Intelligent Edge Device](lab4-edge-intelligent.md)