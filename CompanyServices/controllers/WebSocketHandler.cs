using System.Net.WebSockets;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompanyServices
{
    public static class WebSocketHandler
    {
        private static readonly ConcurrentBag<WebSocket> _sockets = new();

        public static void AddSocket(WebSocket socket)
        {
            _sockets.Add(socket);
        }

        public static async Task BroadcastAsync(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(buffer);

            foreach (var socket in _sockets)
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

        public static async Task Receive(WebSocket socket)
        {
            var buffer = new byte[1024 * 4];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the WebSocketHandler", CancellationToken.None);
                }
            }
        }
    }
}