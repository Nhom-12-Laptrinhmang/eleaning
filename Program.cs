using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class TcpClientDemo
{
    public static async Task Main()
    {
        using TcpClient client = new TcpClient("127.0.0.1", 13000);
        client.NoDelay = true;
        client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
        client.ReceiveBufferSize = 64 * 1024;
        client.SendBufferSize = 64 * 1024;
        client.ReceiveTimeout = 10000;
        client.SendTimeout = 10000;

        using NetworkStream stream = client.GetStream();

        while (true)
        {
            Console.Write("Nhap tin nhan: ");
            string message = Console.ReadLine();
            if (message?.ToLower() == "exit") break;

            byte[] data = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(data, 0, data.Length);

            byte[] buffer = new byte[1024];
            int bytes = await stream.ReadAsync(buffer, 0, buffer.Length);
            Console.WriteLine($"Server tra loi: {Encoding.UTF8.GetString(buffer, 0, bytes)}");
        }
    }
}
