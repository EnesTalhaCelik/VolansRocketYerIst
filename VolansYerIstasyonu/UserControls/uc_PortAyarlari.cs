using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using SeriHaberlesme;
using static GMap.NET.Entity.OpenStreetMapGeocodeEntity;
using System.Runtime.Remoting.Channels;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DocumentFormat.OpenXml.Wordprocessing;
using TeknofestVeriler;
using System.Management;
namespace VolansYerIstasyonu.UserControls
{
    public partial class uc_PortAyarlari : UserControl
    {
        
        

        //seri portları burada tanımlıyorum

        
        
        static string[] ports = SerialPort.GetPortNames();
        static int[] boudRates = { 9600, 19200,115200 };
        static System.IO.Ports.Parity[] pairityTypes = { Parity.None, Parity.Odd, Parity.Even, Parity.Mark, Parity.Space };

        static public SerialPort loraSerialPort = new SerialPort();
        static public SerialPort HYISerialPort = new SerialPort(); 
        public uc_PortAyarlari()
        {
            InitializeComponent();
            Console.WriteLine("Mr. House always wins");

            loraSerialPort.RtsEnable = true;
            HYISerialPort.RtsEnable = true;
            loraSerialPort.DtrEnable = true;
            HYISerialPort.DtrEnable = true;

            loraSerialPort.DataReceived += loraSerialPort_DataReceived;
            HYISerialPort.DataReceived += HYISerialPort_DataReceived;
            foreach (int i in boudRates)            //sp dışında kalan yerleri hazırlıyoruz
            {
                cboxHYIBaudRate.Items.Add(i);
                cboxLoraBaudRate.Items.Add(i);
            }
            foreach (Parity i in pairityTypes)
            {
                cboxHYIParity.Items.Add(i);
                cboxLoraParity.Items.Add(i);
            }
            cboxLoraDataBit.Items.Add(8);
            cboxLoraStopBit.Items.Add(1);
            cboxLoraStopBit.Items.Add(2);
            cboxHYIDataBit.Items.Add(8);
            cboxHYIStopBit.Items.Add(1);
            cboxHYIStopBit.Items.Add(2);

            cboxLoraParity.SelectedIndex = 0;
            cboxLoraDataBit.SelectedIndex = 0;
            cboxLoraBaudRate.SelectedIndex = 0;
            cboxLoraStopBit.SelectedIndex = 0;

            cboxHYIParity.SelectedIndex = 0;
            cboxHYIDataBit.SelectedIndex = 0;
            cboxHYIBaudRate.SelectedIndex = 1;
            cboxHYIStopBit.SelectedIndex = 0;

        }
        private void displayToTxt(string Data)
        {
            BeginInvoke(new EventHandler(delegate
            {
                MessageBox.Show($"mesaj geldi"+ Data, "Port Başarıyla Açıldı" + Data, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }));
        }

        static int GetFirstTwoDigitsOfFractionalPart(float number)
        {
            string numberString = number.ToString("F");
            int decimalPointIndex = numberString.IndexOf('.');
            if (decimalPointIndex == -1 || decimalPointIndex + 2 >= numberString.Length)
            {
                return 0; // If there are no fractional digits, return 0
            }

            string fractionalPartString = numberString.Substring(decimalPointIndex + 1, 2);
            int firstTwoDigits = int.Parse(fractionalPartString);
            return firstTwoDigits;
        }

        static float RoundToDecimalPlaces(float number, int decimalPlaces)
        {
            float factor = (float)Math.Pow(10, decimalPlaces);
            return (float)Math.Round(number * factor) / factor;
        }

        private void loraSerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {



            SerialPort serialPort = (SerialPort)sender;


            //string[] responseContainer = receivedData.Split('/');
            try
            {
                string receivedData = serialPort.ReadLine();
               
            }
            catch (Exception ex)
            {
                
                MessageBox.Show($" bir hata meydana geldi: {ex.Message}", "Fuck this shit!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void HYISerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
                try
                {
                    string receivedData = HYISerialPort.ReadLine();


                    displayToTxt(receivedData);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"HYI veri geliyordu bro.: {ex.Message}", "HYI broo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
           

        }



        private void cboxLoraSP_Click(object sender, EventArgs e)
        {
            ports = SerialPort.GetPortNames();
            cboxLoraSP.Items.Clear();
            cboxLoraSP.Items.AddRange(ports);
        }
        private void cboxHYISP_Click(object sender, EventArgs e)
        {
            ports = SerialPort.GetPortNames();
            cboxHYISP.Items.Clear();
            cboxHYISP.Items.AddRange(ports);
        }

        private void btnLoraBaglan_Click(object sender, EventArgs e)
        {

            try
            {
                loraSerialPort.BaudRate = (int)cboxLoraBaudRate.SelectedItem;
                loraSerialPort.PortName = cboxLoraSP.Text;

                loraSerialPort.Parity = (Parity)cboxLoraParity.SelectedItem;
                loraSerialPort.DataBits = (int)cboxLoraDataBit.SelectedItem;
                loraSerialPort.StopBits = (StopBits)(cboxLoraStopBit.SelectedItem);

                loraSerialPort.Open();
                MessageBox.Show($"Port Başarıyla Açıldı: ", "Port Başarıyla Açıldı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnLoraBaglan.Enabled = false;
                btnLoraBaglantiKes.Enabled = true;
                cboxLoraSP.Enabled = false;
                cboxLoraBaudRate.Enabled = false;
                cboxLoraDataBit.Enabled = false;
                cboxLoraStopBit.Enabled = false;
                cboxLoraParity.Enabled = false;

                btnLoraAgAyarlari.Enabled = true;
                btnLoraPingGorevY.Enabled = true;
                btnLoraPingPong.Enabled = true;
                btnLoraPingRoket.Enabled = true;
                btnLoraPingYedekAv.Enabled = true;
                btnLoraMesajGonder.Enabled = true;
                cboxLoraSec.Enabled = true;
                txtboxLoraMesaj.Enabled = true;
                btnLoraParamGet.Enabled = true;
                btnLoraTstVeriGondr.Enabled = true;
                string cihaz = GetSerialPortDeviceName(loraSerialPort.PortName);
                if (logSeriPort.InvokeRequired)
                {
                    logSeriPort.Invoke(new MethodInvoker(delegate
                    {
                        logSeriPort.AppendText($"{Environment.NewLine} {DateTime.Now}: Seri" +
                                                $" Port Başarıyla açılmıştır.{Environment.NewLine}Bağlı Cihaz: {cihaz}");
                    }));
                }
                else
                {

                    logSeriPort.AppendText($"{Environment.NewLine} {DateTime.Now}: Seri" +
                        $" Port Başarıyla açılmıştır.{Environment.NewLine}Bağlı Cihaz: {cihaz}");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir Hata Meydana Geldi.: {ex.Message}", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnHYIBaglan_Click(object sender, EventArgs e)
        {
            try
            {
                HYISerialPort.BaudRate = (int)cboxHYIBaudRate.SelectedItem;
                HYISerialPort.PortName = cboxHYISP.Text;

                HYISerialPort.Parity = (Parity)cboxHYIParity.SelectedItem;
                HYISerialPort.DataBits = (int)cboxHYIDataBit.SelectedItem;
                HYISerialPort.StopBits = (StopBits)(cboxHYIStopBit.SelectedItem);

                HYISerialPort.Open();
                MessageBox.Show($"Port Başarıyla Açıldı: ", "Port Başarıyla Açıldı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnHYIBaglan.Enabled = false;
                btnHYIBaglantiKes.Enabled = true;
                cboxHYISP.Enabled = false;
                cboxHYIBaudRate.Enabled = false;
                cboxHYIDataBit.Enabled = false;
                cboxHYIStopBit.Enabled = false;
                cboxHYIParity.Enabled = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir Hata Meydana Geldi.: {ex.Message}", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnLoraBaglantiKes_Click(object sender, EventArgs e)
        {
            try
            {
                loraSerialPort.Close();
                btnLoraBaglan.Enabled = true;
                btnLoraBaglantiKes.Enabled = false;

                cboxLoraSP.Enabled = true;
                cboxLoraParity.Enabled = true;
                cboxLoraDataBit.Enabled = true;
                cboxLoraBaudRate.Enabled = true;
                cboxLoraStopBit.Enabled = true;

                btnLoraAgAyarlari.Enabled = false;
                btnLoraPingGorevY.Enabled = false;
                btnLoraPingPong.Enabled = false;
                btnLoraPingRoket.Enabled = false;
                btnLoraPingYedekAv.Enabled = false;
                btnLoraMesajGonder.Enabled = false;
                cboxLoraSec.Enabled = false;
                txtboxLoraMesaj.Enabled = false;
                btnLoraParamGet.Enabled = false;
                btnLoraTstVeriGondr.Enabled = false;
                
                if (logSeriPort.InvokeRequired)
                {
                    logSeriPort.Invoke(new MethodInvoker(delegate
                    {
                        logSeriPort.AppendText($"{ Environment.NewLine} {DateTime.Now}: Lora Serial port Başarıyla Kapatılmıştır. ");
                    }));
                }
                else
                {

                    logSeriPort.AppendText($"{Environment.NewLine} {DateTime.Now}: Lora Serial port Başarıyla Kapatılmıştır. ");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir Hata Meydana Geldi!: {ex.Message}", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private string GetSerialPortDeviceName(string portName)
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption LIKE '%(" + portName + ")%'"))
                {
                    foreach (var device in searcher.Get())
                    {
                        return device["Caption"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving device name: " + ex.Message);
            }
            return "Unknown device";
        }
        private void btnHYIBaglantiKes_Click(object sender, EventArgs e)
        {
            try
            {
                HYISerialPort.Close();
                btnHYIBaglan.Enabled = true;
                btnHYIBaglantiKes.Enabled = false;

                cboxHYISP.Enabled = true;
                cboxHYIParity.Enabled = true;
                cboxHYIDataBit.Enabled = true;
                cboxHYIBaudRate.Enabled = true;
                cboxHYIStopBit.Enabled = true;

                


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir Hata Meydana Geldi!: {ex.Message}", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }




        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TeknofestVeriler.Veriler.Aci = 15;
        }

        private void cboxLoraSP_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboxHYISP_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnLoraPingPong_Click(object sender, EventArgs e)
        {

            byte[] bytestosend = {  0xc1, 0xc1, 0xc1 };
            //byte[] bytestosend = {0xc0, 0x0, 0x3f, 0x1a, 0x17, 0xc7 };
            loraSerialPort.Write(bytestosend, 0, bytestosend.Length);

        }

        private void btnLoraPingRoket_Click(object sender, EventArgs e)
        {
            byte[] bytestosend = {0x0, 0x2c, 0x17, 0x76, 0x6F, 0x6C, 0x61, 0x6E, 0x73 };
                       loraSerialPort.Write(bytestosend, 0, bytestosend.Length);
            
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void btnLoraParamGet_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] bytestosend = { 0xc1, 0xc1, 0xc1 };
                loraSerialPort.Write(bytestosend, 0, bytestosend.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir Hata Meydana Geldi!: {ex.Message}", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void btnLoraTstVeriGondr_Click(object sender, EventArgs e)
        {


            try
            {
                byte[] bytestosend = { 0x0, 0x01, 0x17, 0x76, 0x6F, 0x6C, 0x61, 0x6E, 0x73 };
                loraSerialPort.Write(bytestosend, 0, bytestosend.Length);
            }
             catch (Exception ex)
            {
                MessageBox.Show($"Bir Hata Meydana Geldi!: {ex.Message}", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void button1_Click(object sender, EventArgs e)//patlatma kodu
        {
            try
            {
                byte[] bytestosend = { 0x00, 0x01, 0x17, 0x6D, 0x61, 0x68, 0x6D, 0x75, 0x74, 0x20, 0x63, 0x61, 0x6E };
                loraSerialPort.Write(bytestosend, 0, bytestosend.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir Hata Meydana Geldi!: {ex.Message}", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void uc_PortAyarlari_Load(object sender, EventArgs e)
        {

        }
    }
}
