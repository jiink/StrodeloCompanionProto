using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

Console.WriteLine("Hello, World!");

string savePath = "savedFile.jpg";
int port = 8111;

var listener = new TcpListener(IPAddress.Any, port);
listener.Start();

var client = listener.AcceptTcpClient();
var stream = client.GetStream();
using (var output = File.Create(savePath))
{
    stream.CopyTo(output);
}
stream.Close();
client.Close();
listener.Stop();
Console.WriteLine("The end");