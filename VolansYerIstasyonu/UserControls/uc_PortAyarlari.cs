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

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir Hata Meydana Geldi!: {ex.Message}", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
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

        private void button1_Click(object sender, EventArgs e)
        {

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

        
    }
}
