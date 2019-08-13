<p align="center">
  <img src="https://raw.githubusercontent.com/lfkdev/MultiLogMonitor/master/mlogm_logo.png">
</p>

<div id="badges" align="center">
 
  [![Size](https://img.shields.io/github/size/lfkdev/MultiLogMonitor/release/0.3/mlogm.dll)](https://github.com/lfkdev/mlogm/releases)
[![Lizenz](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://github.com/lfkdev/MultiLogMonitor)
[![Version](https://img.shields.io/github/release-pre/lfkdev/MultiLogMonitor.svg)](https://github.com/lfkdev/MultiLogMonitor)

</div>

<p align="center">
  A lightweight program (12KB) written in C# for linux to view multiple logs simultaneously - in one terminal.
</p>

<p align="center">
  <img src="https://raw.githubusercontent.com/lfkdev/MultiLogMonitor/master/mlogmpreview.gif">
</p>

### Usage:
```
$ dotnet mlogm /var/log/auth.log /var/log/syslog /var/log/kern.log
```
Add as many logs as you want. The more logs are added the more space is needed in the terminal.
Beside this there are two more arguments possible:
```
$ dotnet mlogm  --about
$ dotnet mlogm  -h / -H
```

### Info
MultiLogMonitor is written in C# on .net Core and tested on Linux only. Theoretically it should work on Windows and Mac too, but I did not test this.

### Requirements
Only [.NET Core runtime](https://dotnet.microsoft.com/download/linux-package-manager/ubuntu16-04/runtime-2.2.0) Version 2.0 or newer.
(Optionally wait for a self contained release where you dont need to install .NET Core)

### Installation
```
$ wget https://github.com/lfkdev/MultiLogMonitor/releases/download/3.0/MultiLogMonitor3-0.zip -O /usr/local/bin/mlogm.zip
$ unzip /usr/local/bin/mlogm.zip
$ sudo chmod +x /usr/local/bin/mlogm
```
Or just download the newest release from the release tab and move to what ever location you want.

testing:
```
$ dotnet mlogm --about
```

### RoadMap
- Performance optimizing
- rescale window without breaking mlogm
- config to parse different settings to mlogm (ex. a "grep" funktion")
- more / better syntax highlight
