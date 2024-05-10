using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StrodeloCompanionProto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IPAddress pairedDeviceAddress;
        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine("MainWindow initialized");
            Debug.WriteLine("hi");
        }

        private void PairButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Pairing button clicked");

            string ipAddressString = IpAddressBox.Text;

            if (IPAddress.TryParse(ipAddressString, out pairedDeviceAddress))
            {
                bool deviceExists = CheckDeviceExists(pairedDeviceAddress);

                if (deviceExists)
                {
                    Debug.WriteLine("Device found at IP address: " + ipAddressString);
                }
                else
                {
                    Debug.WriteLine("No device found at IP address: " + ipAddressString);
                }
            }
            else
            {
                Debug.WriteLine("Invalid IP address format: " + ipAddressString);
            }
        }

        private bool CheckDeviceExists(IPAddress ipAddress)
        {
            Ping ping = new Ping();
            PingReply reply;

            try
            {
                reply = ping.Send(ipAddress, 1000);
                return reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                return false;
            }
        }

        public void SendFile(string filePath, string host, int port)
        {
            var client = new TcpClient(host, port);
            var stream = client.GetStream();

            using (var fileStream = File.OpenRead(filePath))
            {
                fileStream.CopyTo(stream);
            }

            stream.Close();
            client.Close();
        }

        private void SendFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Title = "Open File...";
            openFileDialog.Filter = "3D Files (*.obj;*.glb;*.gltf;*.fbx)|*.obj;*.glb;*.gltf;*.fbx";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                Debug.WriteLine("Selected file: " + filePath);

                // Send the file to pairedDeviceAddress
                SendFile(filePath, pairedDeviceAddress.ToString(), 8111);

            }
        }
    }
}