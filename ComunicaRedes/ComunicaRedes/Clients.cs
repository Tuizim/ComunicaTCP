using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class TcpClientProgram
{
    private static TcpClient _client;
    private static NetworkStream _stream;

    static void Main()
    {
        try
        {
            _client = new TcpClient("127.0.0.1", 5000);
            _stream = _client.GetStream();
            Console.WriteLine("Conectado ao servidor. Você pode começar a enviar mensagens.");

            Thread readThread = new Thread(ReadMessages);
            readThread.Start();

            while (true)
            {
                string message = Console.ReadLine();
                if (message == "exit")
                {
                    break;
                }

                byte[] data = Encoding.ASCII.GetBytes(message);
                _stream.Write(data, 0, data.Length);
            }

            _client.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro: " + e.Message);
        }
    }

    private static void ReadMessages()
    {
        byte[] buffer = new byte[1024];
        int bytesRead;

        try
        {
            while ((bytesRead = _stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                Console.WriteLine("Recebido: "+Encoding.ASCII.GetString(buffer, 0, bytesRead));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro: " + e.Message);
        }
    }
}
