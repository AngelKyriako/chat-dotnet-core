using System;
using System.Threading;
using WebSocket4Net;

namespace ChatApp.Client {
    public class Program {

        private static string WEB_SOCKET_SERVER = "ws://localhost:5000/ws";

        private static WebSocket ws;

        static void Main(string[] args) {

            ws = new WebSocket(WEB_SOCKET_SERVER);

            ws.Opened += (sender, args2) => {
                Console.WriteLine("On socket open: " + args2);
                ws.Send("YOLO");
            };
            ws.Closed += (sender, args2) => { Console.WriteLine("On socket closed: " + args2); };
            ws.DataReceived += (sender, args2) => { Console.WriteLine("On socket data received: " + args2.Data); };
            ws.MessageReceived += (sender, args2) => { Console.WriteLine("On socket message received: " + args2.Message); };
            ws.Error += (sender, args2) => { Console.WriteLine("On socket error: " + args2.Exception); };

            Console.WriteLine("WebSocket4Net initialized");

            ws.Open();

            Thread.Sleep(1000);
            while (ws.State == WebSocketState.Open) {
                Thread.Sleep(1000);
                ws.Send("YOLO");
            }

            int input = Console.Read();
        }
    }
}
