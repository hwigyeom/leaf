# Leaf web business application platform

## Prerequisite

### 기본 환경 구성하기

#### ASP.NET Core

* Back-end 환경의 기반이 되는 플랫폼입니다.
* 2.1 버전을 이용합니다. 
* 설치하기: https://www.microsoft.com/net/download
* 사용하는 OS를 선택한 후 `Download .NET Core SDK` 를 이용하여 설치 프로그램을 다운로드하고 설치를 수행합니다.

#### Node.js

* Front-end 개발 환경을 구성하는 플랫폼으로 실제 운영환경에서는 필요하지 않습니다. 
* 설치하기: https://nodejs.org/ko/
* 다운로드 부분에서 `8.XX.X LTS` 로 설치 프로그램을 다운로드하고 설치를 수행합니다. 

#### Visual Studio

* 설치하기: https://visualstudio.microsoft.com/ko/downloads/

* Visual Studio IDE
  * Back-end 플랫폼 부분을 개발하기 위해 사용합니다.
  * Visual Studio 2017 버전을 기본으로 사용합니다.

* Visual Studio Code
  * Front-end 플랫폼 부분을 개발하기 위해 사용합니다.
  * Front-end 개발툴은 취향에 따라 선택할 수 있으며, Visual Studio IDE 를 이용할 수 도 있습니다.

#### Git

* 소스 현상 관리를 위해 git 을 이용합니다.
* 설치하기: https://git-scm.com/download/

---

### 개발 환경 구성하기

#### 소스 가져오기

각 개발 툴의 기능을 이용하여 가져올 수 있지만 여기서는 기본적인 `git` 명령을 콘솔에서 입력하여 소스를 가져옵니다.  
Leaf 플랫폼 개발을 수행할 새로운 폴더를 원하는 곳에 생성한 후 콘솔 화면에서 생성한 경로로 이동합니다.  
콘솔은 `git` 설치 시 기본 콘솔로 지정한 항목을 사용하는 것이 좋습니다.

```bash
$> git clone https://github.com/hwigyeom/leaf.git
```

##### 프론트엔드 디펜던시 설치 및 초기 빌드

콘솔에서 프론트엔드 개발 홈 폴더로 이동합니다.

```bash
$> cd src/frontend/leaf-platform/
```

Node.js 설치 시 함께 설치된 NPM(Node Package Manager)의 기능을 이용하여 프론트엔드 플랫폼 개발을 위한 라이브러리를 설치합니다.

```bash
$> npm install
```

벡엔드 서버에도 프론트엔드 플랫폼의 빌드 결과를 넣기 위해서 초기 빌드를 수행합니다.

```bash
$> npm run build:dev
``` 

##### 백엔드 프로젝트 초기 빌드

VisualStudio 2017 버전에서 프로젝트 루트 폴더의 `Leaf.sln` 솔루션 파일을 엽니다.

자동으로 프로젝트의 종속성 라이브러리가 설치되지 않을 때는 솔루션에 포함된 프로젝트의 디펜던시를 설치해줍니다.

VisualStudio 의 빌드 기능을 이용하여 프로젝트를 빌드합니다.

빌드 시 솔루션을 선택하여 전체를 빌드하도록 합니다.

#### 실행해보기

##### 백엔드 서버 실행

백엔드 서버는 VisualStudio 에서 `실행` 을 이용하거나 `디버그` 기능을 이용하여 실행할 수 있습니다.  
개발 시에는 5000 번 포트를 이용하게 됩니다.  
브라우저에서 `http://localhost:5000` 를 입력하여 접속이 되는지 확인합니다.

##### 프론트엔드 개발 환경 실행

프론트엔드 개발 환경은 프론트엔드 플랫폼을 개발하기 위한 환경으로 TypeScript 를 이용하여 개발을 수행하게 되며, TypeScript 로 작성한 소스가 빌드 과정을 거쳐 JavaScript 파일로 변환된 후에 백엔드 서버로 배치되는 형태로 개발을 수행하게 됩니다.

결과물을 백엔드 서버로 배치하기 전에 개발 시점에 테스트 및 디버깅을 위해 프론트엔드 자체 개발 환경을 갖추고 있으며, JavaScript 로 빌드된 결과물을 생성하지 않고도 개발에 대한 결과를 브라우저에서 확인할 수 있도록 되어있습니다.

새로운 콘솔 창을 열어 프론트엔드 개발 폴더로 이동합니다.

```bash
$> cd src/frontend/leaf-platform/
```

`NPM START` 명령으로 프론트엔드 개발 서버를 실행합니다.

```bash
$> npm start
```

프론트엔드 개발서버는 백엔드 서버와는 별개의 포트(8800)를 이용합니다.

브라우저에서 `http://localhost:8800` 주소를 입력하여 백엔드 서버에 접속했을 때와 동일한 화면이 나타나는지 확인합니다.

백엔드 서버와는 달리 프론트엔드 개발 서버는 프론트엔드 소스를 변경하였을 경우 자동으로 브라우저가 새로고침되어 결과를 즉시 확인할 수 있도록 되어있습니다.

## Leaf 플랫폼 개발하기

### [Leaf 프론트엔드 플랫폼 개발](doc/frontend/README.md)

### [Leaf 백엔드 플랫폼 개발](doc/backend/README.md)

## Leaf 플랫폼 기반의 어플리케이션 개발하기

...준비중...
