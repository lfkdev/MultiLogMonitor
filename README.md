# MultiLogMonitor
A lightweight program (12KB) written in C# for linux to view multiple logs simultaneously in one terminal.

<p align="center">
  <img src="https://raw.githubusercontent.com/lfkdev/MultiLogMonitor/master/mlogm_screenshot.jpg">
</p>

### Usage:
```
$ mlogm /var/log/auth.log /var/log/syslog /var/log/kern.log
```
Add as many logs as you want. The more logs are added the more space is needed in the terminal.
Beside this there are two more arguments possible:
```
$ MlogM  --about
$ MlogM  -h
```

### Info
MultiLogMonitor is written in C# on .net Core and tested on Linux only. Theoretically it would work on Windows and Mac too, but I did not test this.

### Requirements
Only [.NET Core runtime](https://dotnet.microsoft.com/download/linux-package-manager/ubuntu16-04/runtime-2.2.0) Version 2.0 or newer.
(Choose your distro and only install .NET Core runtime. ~ 30MB Space required)

### Installation
```
$ wget github.com/lfkdev/MultiLogMonitor/releases/latest/download/mlogm.zip /usr/local/bin/mlogm.zip
$ unzip /usr/local/bin/mlogm.zip
$ sudo chmod +x /usr/local/bin/mlogm
```
testing:
```
$ mlogm --about
```

