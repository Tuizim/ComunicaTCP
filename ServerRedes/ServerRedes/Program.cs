using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class TcpServer
{
    private static TcpListener _listener;
    private static TcpClient _client1;
    private static TcpClient _client2;

    static void Main()
    {
        _listener = new TcpListener(IPAddress.Any, 5000);
        _listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        _listener.Start();
        Console.WriteLine("Servidor iniciado. Aguardando conexões...");

        _client1 = _listener.AcceptTcpClient();
        Console.WriteLine("Cliente 1 conectado.");

        _client2 = _listener.AcceptTcpClient();
        Console.WriteLine("Cliente 2 conectado.");

        Thread client1Thread = new Thread(() => HandleClient(_client1, _client2));
        Thread client2Thread = new Thread(() => HandleClient(_client2, _client1));

        client1Thread.Start();
        client2Thread.Start();

        client1Thread.Join();
        client2Thread.Join();

        _client1.Close();
        _client2.Close();
        _listener.Stop();
    }

    private static void HandleClient(TcpClient client, TcpClient otherClient)
    {
        NetworkStream stream = client.GetStream();
        NetworkStream otherStream = otherClient.GetStream();

        byte[] buffer = new byte[1024];
        int bytesRead;

        try
        {
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                otherStream.Write(buffer, 0, bytesRead);
                Console.WriteLine("Cliente: "+Encoding.ASCII.GetString(buffer, 0, bytesRead));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro: " + e.Message);
        }
        finally
        {
            client.Close();
        }
    }
}
