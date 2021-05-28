using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Traffic_Simulation_Server
{
    class Program
    {
        static Socket sListener;
        static Socket client;
        //static List<Client> clients = new List<Client>();

        Task task;
        Random random = new Random(2);

        static void Main(string[] args)
        {
            // Устанавливаем для сокета локальную конечную точку
            IPHostEntry ipHost = Dns.GetHostEntry("127.0.0.1"); //Выбор Ip хоста
            IPAddress ipAddr = IPAddress.Parse("127.0.0.1"); // ipHost.AddressList.FirstOrDefault((a) => a.AddressFamily == AddressFamily.InterNetwork);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 1001);

            Console.Write($"[{DateTime.Now:HH:mm:ss}] Лог сервера:\r\n");

            //Создаем сокет Tcp/Ip
            sListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Program program = new Program();

            try
            {
                sListener.Bind(ipEndPoint); //Связывает сокет с локальной конечной точкой

                // Начинаем слушать новые соединения
                sListener.Listen(10);

                //Программа приостанавливается, ожидая входящее соединение
                client = sListener.Accept(); //Создание нового клиента

                Task.Factory.StartNew(() =>
                {
                    program.ReceiveAndSend();
                }); //Создание и запуск нового потока

                Console.Write($"Соединение с [{client.RemoteEndPoint}] установлено");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message + "\r\n" + ex.StackTrace + "\r\n");
                //File.AppendAllText(Directory.GetCurrentDirectory() + "\\Логи\\" + DateTime.Now.ToString("dd.MM.yyyy") + ".txt", ex.Message + "\r\n" + ex.StackTrace + "\r\n");
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static IPHostEntry getIpHost()
        {
            Console.Write(
                "Выберите локальную конечную точку для сокета:\r\n" +
                "1. 127.0.0.1\r\n" +
                "2. 192.168.0.101\r\n" +
                "3. 192.168.0.105\r\n" +
                "или введите вручную\r\n");

            string ipHostString = Console.ReadLine();
            IPHostEntry ipHost = new IPHostEntry();

            switch (ipHostString)
            {
                case "1":
                    ipHost = Dns.GetHostEntry("127.0.0.1");
                    break;
                case "2":
                    ipHost = Dns.GetHostEntry("192.168.0.101");
                    break;
                case "3":
                    ipHost = Dns.GetHostEntry("192.168.0.105");
                    break;
                default:
                    ipHost = Dns.GetHostEntry(ipHostString);
                    break;
            }

            return ipHost;
        }

        public void ReceiveAndSend()
        {
            //Вечно проверям, пришло ли что-то
            while (true)
            {
                //Иначе...
                try
                {
                    //...получаем пакет из буфера входящего потока
                    Packet packet = GetPacket();

                    //В зависимости от команды, выполняется то или иное действие
                    switch (packet.Command)
                    {
                        case Command.Start:
                            StartInfo info = JsonConvert.DeserializeObject<StartInfo>(packet.data);
                            CrossroadsGrid[,] crossroadsArray = DrawHelper.DrawGrid(info.countRow, info.countColumm, info.useTrafficLight);
                            Queue<CarGrid> carsQueue = new Queue<CarGrid>();

                            for (int i = 0; i < info.countCar; i++)
                            {
                                CrossroadsGrid crossroads = crossroadsArray[0, i % info.countRow];
                                CarGrid car = new CarGrid(crossroads, random.Next(info.minSpeed, info.maxSpeed), 1);
                                carsQueue.Enqueue(car);
                            }

                            task = new Task(() =>
                            {
                                WorkHelper.WorkGrid(carsQueue, crossroadsArray);
                            });

                            task.Start();

                            break;
                        case Command.GetCrossroadses:
                            Send(Command.SendCrossroadses, Data.GetInstance().crossroadsArray);
                            break;
                        case Command.GetCars:
                            
                            break;
                        default:
                            WorkHelper.isWorkedTask = false;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message + "\r\n" + ex.StackTrace + "\r\n");
                    //File.AppendAllText(Directory.GetCurrentDirectory() + "\\Логи\\" + DateTime.Now.ToString("dd.MM.yyyy") + ".txt", ex.Message + "\r\n" + ex.StackTrace + "\r\n");
                    Main(null);
                    return;
                }

            }
        }

        public void Send(Command command, object messageObject)
        {
            string messageString = JsonConvert.SerializeObject(messageObject);

            Packet packet = new Packet();
            packet.Command = command;
            packet.data = messageString;

            string packetString = JsonConvert.SerializeObject(packet);
            byte[] packetBytes = Encoding.GetEncoding("Unicode").GetBytes(packetString);

            try
            {
                client.Send(packetBytes);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static Packet GetPacket()
        {
            byte[] bytes = new byte[1024];
            int bytesRec = client.Receive(bytes); //Забираем массив байт из буфера входящего потока
            string data = Encoding.GetEncoding("Unicode").GetString(bytes, 0, bytesRec); //Декодируем в текст (Json)
            Packet packet = JsonConvert.DeserializeObject<Packet>(data); //Десериализуем из Json в Packet

            return packet;
        }
    }
}
