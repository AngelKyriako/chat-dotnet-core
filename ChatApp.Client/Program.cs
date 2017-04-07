using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WebSocket4Net;

namespace ChatApp.Client {
    using Model;

    /// <summary>
    /// Chat demo console app
    /// </summary>
    public class Program {

        private const string WEB_SERVER_URL = "http://localhost:5000";
        private const string WS_SERVER_URL = "ws://localhost:5000/ws";

        private static WebSocket ws;
        private static Api api = new Api(WEB_SERVER_URL);

        private static UserAndToken session = null;
        
        static bool IsAuthenticated { get { return session != null; } }

        static void Authenticate(string username, string password, bool isRegister) {
            Task.Run(async () => {
                session = !isRegister ? await api.User.Login(new LocalCredentials() {
                    Username = username,
                    Password = password
                }) : await api.User.Register(new UserModel() {
                    Username = username,
                    Password = password
                });

                if (session != null) {
                    Console.Write("Welcome " + session.User.Username + ".");
                } else {
                    Console.Write("Registration failed.");
                }
            }).GetAwaiter().GetResult();
        }

        static void Logout() {
            session = null;
        }

        static void HandleInput(string input) {
            //TODO: 
            // print help menu on /help or when trying to send a message but is not authenticated
            // login on /login <username> <password>
            // register on /register <username> <password>
            // logout on /logout
        }

        static void Main(string[] args) {
            ws = new WebSocket(WS_SERVER_URL);

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
