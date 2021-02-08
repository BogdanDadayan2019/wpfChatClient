using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Net.Sockets;

namespace WPFchatCLIENTtest
{

    public partial class MainWindow : Window
    {
        private string userName;
        private const string host = "127.0.0.1";
        private const int port = 8888;
        private TcpClient client;
        private NetworkStream stream;



        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            userName = tbName.Text;
            client = new TcpClient();
            client.Connect(host, port);
            stream = client.GetStream();

            string message = userName;
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);

            Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
            receiveThread.Start(); //старт потока
            Console.WriteLine("Добро пожаловать, {0}", userName);
            //SendMessage();
        }

        private void SendMessage()
        {
            Console.WriteLine("Введите сообщение: ");

            string message = tbMessage.Text;
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);

        }

        private void ReceiveMessage()
        {
            try
            {
                byte[] data = new byte[64]; // буфер для получаемых данных
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (stream.DataAvailable);

                string message = builder.ToString();
               // lbMessage.Items.Add(message);//вывод сообщения
            }
            catch
            {
                Console.WriteLine("Подключение прервано!"); //соединение было прервано
                Console.ReadLine();
                Disconnect();
            }
        }
        //private void Disconnect()
        //{
        //    // if (client != null)
        //    //  {

        //    stream.Close();
        //    //     client.Dispose();
        //    client.Close();//отключение клиента
                
        //   // }
        //    // НУЖНО ЗАКРЫТЬ СТРИМ
        //  //  if (stream != null)
        //  //  {
        //       // stream.Dispose();
        //  //      stream.Close();//отключение потока
                
        //  //  }

        //  Environment.Exit(0); //завершение процесса
        //}

        private void Disconnect()
        {
            client.Close();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            Disconnect();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }
    }



}
