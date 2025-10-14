using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class OptimizedTcpServer
{
    private const int Port = 13000;
    private static readonly SemaphoreSlim connectionLimit = new SemaphoreSlim(100); // gioi han 100 client

    public static async Task Main()
    {
        TcpListener server = new TcpListener(IPAddress.Loopback, Port);
        server.Start();
        Console.WriteLine($"TCP Server dang chay tren cong {Port}...");

        while (true)
        {
            TcpClient client = await server.AcceptTcpClientAsync();
            client.NoDelay = true;
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            client.ReceiveBufferSize = 64 * 1024;
            client.SendBufferSize = 64 * 1024;
            client.ReceiveTimeout = 10000;
            client.SendTimeout = 10000;

            Console.WriteLine($"Ket noi moi tu {client.Client.RemoteEndPoint}");

            await connectionLimit.WaitAsync();
            _ = Task.Run(async () =>
            {
                try { await HandleClientAsync(client); }
                finally { connectionLimit.Release(); }
            });
        }
    }

    private static async Task HandleClientAsync(TcpClient client)
    {
        var buffer = new byte[1024];
        var stream = client.GetStream();

        try
        {
            while (true)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Nhan tu {client.Client.RemoteEndPoint}: {received}");

                string response = received.ToUpper();
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                Console.WriteLine($"Gui lai: {response}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Loi client {client.Client.RemoteEndPoint}: {ex.Message}");
        }
        finally
        {
            Console.WriteLine($"Client {client.Client.RemoteEndPoint} da ngat ket noi.");
            stream.Close();
            client.Close();
        }
    }
}
