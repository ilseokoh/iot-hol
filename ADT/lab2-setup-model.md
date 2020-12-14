# Lab 2 - Azure Digital Twin Explorer 설정 (15분)

ADT 모델을 설정하는 방법은 여러가지가 있습니다. Azure CLI를 사용할 수도 있고 ADT REST API나 SDK를 사용할 수도 있습니다. 이 실습에서는 [Azure Digital Twin Explorer](https://github.com/Azure-Samples/digital-twins-explorer/tree/master/)을 사용하여 UI에서 모델을 업로드하고 설정을 하는 방법을 사용해보겠습니다. 

![ADT Explorer](images/adt-explorer.png)

ADT Explorer를 실행하려면 Node.js 10+ 이 설치되어 있어야 합니다. 버전을 확인합니다. 

아래 예제는 Windows 10 환경에서 PowerShell 을 사용했습니다. 자신의 쉘 환경에 맞춰 실행 합니다.

``` powershell
PS C:\Users\iloh\source\digital-twins-explorer> node --version
v12.19.0
PS C:\Users\iloh\source\digital-twins-explorer> npm --version
6.14.9
```

## ADT Explorer 설치 

먼저 github에서 Azure Digital Twin Explorer 소스코드를 클론 합니다. 

``` powershell
git clone https://github.com/Azure-Samples/digital-twins-explorer.git
```

client/src 폴더로 이동하여 npm install 을 실행합니다. 



``` powershell
cd .\digital-twins-explorer\client\src\
npm install
```

> found 104 vulnerabilities (103 low, 1 high) vulnerabilities 경고가 발생할 수 있으나 > 무시하고 진행합니다. 

## ADT Explorer 실행

npm run start 명령으로 애플리케이션을 실행합니다. 정상적으로 실행이 되었다면 웹 브라우저가 실행되면서 http://localhost:3000/ 에 접속합니다. 처음 실행할 때 시간이 걸릴 수 있습니다. 페이지가 정상적으로 표시될 때까지 기다립니다.

``` bash
npm run start
```

## ADT Explorer 인증 

ADT Explorer를 로컬에서 실행시키면 Azure Default credentials를 사용하여 로그인 합니다. 따라서 커멘트 프롬프트나 파워쉘 등에서 Azure CLI를 사용하여 로그인하거나 az login, Visual Studio Code 에서 Azure Sign in 을 하면 정상적으로 로그인 됩니다. 

또는 Azure 포탈에 로그인되어 있는 브라우저에서 http://localhost:3000 으로 접속해보세요.

## ADT Explorer 설정 

ADT Explorer(http://localhost:3000)가 실행되면 처음에 ADT URL 입력창이 표시됩니다. 여기에 Lab 1 에서 만들었던 ADT의 URL을 찾아서 입력합니다. 형식은 반드시 **https://** 를 붙여서 입력합니다. 

![ADT URL](images/adt-explorer-setup2.png)

![ADT URL](images/adt-explorer-setup.png)

## (옵션) 3000번 포트가 사용중이면 오류가 발생합니다.

만약 로컬 PC에서 3000번 포트를 사용중이라면 포트를 변경하여 실행해야 정상작동합니다. 예를들어 아래 명령으로 8080포트를 사용하도록 설정할 수 있습니다. 

윈도우 커멘트 프롬프트 (cmd)
``` cmd
set PORT=8080 && npm run start
```

Linux / Mac(bash)
``` bash
PORT=8080 npm run start
```

## [Lab 3 ADT 모델 만들기](lab3-adt-model.md)

## [실습 홈으로 가기](README.md)