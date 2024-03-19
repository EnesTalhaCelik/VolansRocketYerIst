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
namespace VolansYerIstasyonu.UserControls
{
    public partial class uc_PortAyarlari : UserControl
    {

        //seri portları burada tanımlıyorum
        SerialPort loraSerialPort = new SerialPort();
        SerialPort HYISerialPort = new SerialPort();



        static string[] ports = SerialPort.GetPortNames();
        static int[] boudRates = { 9600, 19200 };
        static System.IO.Ports.Parity[] pairityTypes = { Parity.None, Parity.Odd, Parity.Even, Parity.Mark, Parity.Space };
        public uc_PortAyarlari()
        {
            InitializeComponent();
         
            
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
                btnLoraBağlantiKes.Enabled = true;
                cboxLoraSP.Enabled = false;
                cboxLoraBaudRate.Enabled = false;
                cboxLoraDataBit.Enabled = false;
                cboxLoraStopBit.Enabled = false;
                cboxLoraParity.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir Hata Meydana Geldi.: {ex.Message}", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboxLoraSP_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
    }
}
