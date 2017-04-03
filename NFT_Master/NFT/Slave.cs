using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

class Slave
{
    public static Slave[] slaveList;
    public string address;
    public Socket socket;
    public bool isReady;

    public Slave() {}

    public void sendCommand(Command c) { }
    public void reconnect() { }
    public void disconnect() { }
}
