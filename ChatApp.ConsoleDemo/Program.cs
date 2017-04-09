using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSocket4Net;

namespace ChatApp.ConsoleDemo {
    using Common;
    using Model;
    using Client;

    /// <summary>
    /// Chat demo console app.
    /// </summary>
    public class Program {

        private const string WEB_SERVER_URL = "http://localhost:5000";
        private const string WS_SERVER_URL = "ws://localhost:5000/ws";

        private const string SLASH_CMD_HELP = "help";
        private const string SLASH_CMD_REGISTER = "register";
        private const string SLASH_CMD_LOGIN = "login";
        private const string SLASH_CMD_LOGOUT = "logout";
        private const string SLASH_CMD_ME = "me";
        private const string SLASH_CMD_USERS = "users";
        private const string SLASH_CMD_HISTORY = "history";
        private const string SLASH_CMD_QUIT = "quit";

        private const int HISTORY_SIZE = 1000;

        private static WebSocket ws;
        private static ApiClient api = new ApiClient(WEB_SERVER_URL, "v1");
        private static Queue<MessageModel> history;

        static string ConsoleReadPassword() {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter) {
                if (info.Key != ConsoleKey.Backspace) {
                    Console.Write("*");
                    password += info.KeyChar;
                } else if (info.Key == ConsoleKey.Backspace) {
                    if (!string.IsNullOrEmpty(password)) {
                        // remove one character from the list of password characters
                        password = password.Substring(0, password.Length - 1);
                        // get the location of the cursor
                        int pos = Console.CursorLeft;
                        // move the cursor to the left by one character
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        // replace it with space
                        Console.Write(" ");
                        // move the cursor to the left by one character again
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }
            // add a new line because user pressed enter at the end of their password
            Console.WriteLine();
            return password;
        }

        static void PrintHelp() {
            Console.WriteLine("Chat net core");
            Console.WriteLine();
            Console.WriteLine("Write a message to send, or hit one of the following commands.");
            Console.WriteLine();
            Console.WriteLine("/" + SLASH_CMD_HELP + "\t\t| to print this again.");
            Console.WriteLine("/" + SLASH_CMD_REGISTER + "\t\t| to register a new user.");
            Console.WriteLine("/" + SLASH_CMD_LOGIN + "\t\t| to login with an existing user.");
            Console.WriteLine("/" + SLASH_CMD_LOGOUT + "\t\t| to logout from the app.");
            Console.WriteLine("/" + SLASH_CMD_ME + "\t\t| to print your username.");
            Console.WriteLine("/" + SLASH_CMD_USERS + "\t\t| to print all the registered users.");
            Console.WriteLine("/" + SLASH_CMD_HISTORY + "\t\t| to print the messaging history.");
            Console.WriteLine("/" + SLASH_CMD_QUIT + "\t\t| to close the app.");
        }

        static void PrintUser(UserModel user) {
            Console.WriteLine(user.Id + " :: " + user.Username + " :: " + user.CreatedAt);
        }

        static void PrintMe() {
            if (!api.IsAuthenticated) {
                Console.WriteLine("Whoops, you are a nobody right now.");
            } else {
                PrintUser(api.Session.User);
            }
        }

        static void PrintUsers() {
            if (!api.IsAuthenticated) {
                Console.WriteLine("No can do babydoll, register or login to be able to get the users.");
            } else {
                List<UserModel> users = null;
                Task.Run(async () => {
                    users = await api.User.Get();

                    if (users != null) {
                        foreach (var user in users) {
                            PrintUser(user);
                        }
                    }

                }).GetAwaiter().GetResult();
            }
        }

        static void Connect(bool isRegister) {
            string username = null;
            string password = null;

            Console.Write("Enter username: ");
            username = Console.ReadLine();

            Console.Write("Enter password: ");
            password = ConsoleReadPassword();

            Task.Run(async () => {
                if (isRegister) {
                    await api.User.Register(new UserModel() {
                        Username = username,
                        Password = password
                    });
                } else {
                    await api.User.Login(new LocalCredentials() {
                        Username = username,
                        Password = password
                    });
                }

                if (api.IsAuthenticated) {
                    Console.WriteLine("Welcome " + api.Session.User.Username + ".");
                }  else {
                    Console.WriteLine("Failed to " + (isRegister ? "register" : "login") + ".");
                    Console.WriteLine("Come on mate, you can do this.");
                }
            }).GetAwaiter().GetResult();
        }

        static void Disconnect() {
            if (api.IsAuthenticated) {
                api.User.Logout();
                history.Clear();
                Console.WriteLine("Logout completed, you are a nobody now.");
            } else {
                Console.WriteLine("Already Logged out.");
            }
        }

        static void SendMessage(string text) {
            if (!api.IsAuthenticated) {
                Console.WriteLine("No can do babydoll, register or login to be able to send a message.");
            } else {
                MessageModel message = null;
                Task.Run(async () => {
                    message = await api.Message.Post(new MessageModel() {
                        CreatorId = api.Session.User.Id,
                        Body = text
                    });

                    if (message == null) {
                        Console.WriteLine("Message sent failed");
                    }

                }).GetAwaiter().GetResult();
            }
        }

        static void PrintMessage(MessageModel message) {
            Console.WriteLine(message.Creator.Username + ": " + message.Body + " | " + message.CreatedAt);
        }

        static void PrintHistory() {
            foreach(MessageModel message in history) {
                PrintMessage(message);
            }
        }

        static void AddToHistory(MessageModel message) {
            if (history.Count == HISTORY_SIZE) {
                history.Dequeue();
            }
            history.Enqueue(message);
        }

        static bool ParseCommand() {
            string cmd = Console.ReadLine();

            if (cmd.Length > 0) {

                string[] words = cmd.Split(' ');

                if (words[0].Length > 1 && words[0][0] == '/') {
                    switch (words[0].Substring(1, words[0].Length - 1)) {
                        case SLASH_CMD_HELP:
                            PrintHelp();
                            break;
                        case SLASH_CMD_ME:
                            PrintMe();
                            break;
                        case SLASH_CMD_USERS:
                            PrintUsers();
                            break;
                        case SLASH_CMD_HISTORY:
                            PrintHistory();
                            break;
                        case SLASH_CMD_REGISTER:
                            Connect(true);
                            break;
                        case SLASH_CMD_LOGIN:
                            Connect(false);
                            break;
                        case SLASH_CMD_LOGOUT:
                            Disconnect();
                            break;
                        case SLASH_CMD_QUIT:
                            return false;
                        default:
                            SendMessage(cmd);
                            break;
                    }
                } else {
                    SendMessage(cmd);
                }
            }

            return true;
        }

        static void Main(string[] args) {
            history = new Queue<MessageModel>();

            ws = new WebSocket(WS_SERVER_URL);

            ws.Opened += (sender, args2) => {
                Console.WriteLine("Connected to server.");
            };
            ws.Closed += (sender, args2) => {
                Console.WriteLine("Disconnected from server.");
            };
            ws.DataReceived += (sender, args2) => {
                Console.WriteLine("Something went wrong: " + args2.Data);
            };
            ws.MessageReceived += (sender, data) => {
                string json = data.Message;
                WSMessage<MessageModel> message = JsonSerializer.Deserialize<WSMessage<MessageModel>>(json);

                if (message != null) {
                    PrintMessage(message.Payload);
                    AddToHistory(message.Payload);
                }
            };
            ws.Error += (sender, args2) => {
                Console.WriteLine("Something went wrong: " + args2.Exception);
            };

            ws.Open();

            PrintHelp();

            while (ParseCommand()) {
                if (ws.State != WebSocketState.Open) {
                    ws.Open();
                }
            }
        }
        
    }
}
