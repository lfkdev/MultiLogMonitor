# MultiLogMonitor
A lightweight program written in C# for linux to view multiple logs simultaneously in one terminal.

<p align="center">
  <img src="https://s17.directupload.net/images/190623/u7n6qv67.gif">
</p>

### Usage:
```
$ ./MlogM  /var/log/auth.log /var/log/syslog /var/log/kern.log
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
only [.NET Core runtime](https://dotnet.microsoft.com/download/linux-package-manager/ubuntu16-04/runtime-2.2.0)

### Installation
```
$ wget [MultiLogMonitor](/lfkdev/MultiLogMonitor/releases/latest/download/mlogm.zip) /usr/local/bin/mlogm
$ sudo chmod +x /usr/local/bin/mlogm
```
