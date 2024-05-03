namespace VolansYerIstasyonu.UserControls
{
    partial class uc_PortAyarlari
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnLoraBaglan = new System.Windows.Forms.Button();
            this.btnLoraBaglantiKes = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboxLoraParity = new System.Windows.Forms.ComboBox();
            this.cboxLoraStopBit = new System.Windows.Forms.ComboBox();
            this.cboxLoraDataBit = new System.Windows.Forms.ComboBox();
            this.cboxLoraBaudRate = new System.Windows.Forms.ComboBox();
            this.cboxLoraSP = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnHYIBaglan = new System.Windows.Forms.Button();
            this.btnHYIBaglantiKes = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cboxHYIParity = new System.Windows.Forms.ComboBox();
            this.cboxHYIStopBit = new System.Windows.Forms.ComboBox();
            this.cboxHYIDataBit = new System.Windows.Forms.ComboBox();
            this.cboxHYIBaudRate = new System.Windows.Forms.ComboBox();
            this.cboxHYISP = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.cboxLoraSec = new System.Windows.Forms.ComboBox();
            this.btnLoraMesajGonder = new System.Windows.Forms.Button();
            this.txtboxLoraMesaj = new System.Windows.Forms.TextBox();
            this.btnLoraAgAyarlari = new System.Windows.Forms.Button();
            this.btnLoraPingGorevY = new System.Windows.Forms.Button();
            this.btnLoraPingRoket = new System.Windows.Forms.Button();
            this.btnLoraPingYedekAv = new System.Windows.Forms.Button();
            this.btnLoraPingPong = new System.Windows.Forms.Button();
            this.logSeriPort = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.btnLoraBaglan);
            this.groupBox1.Controls.Add(this.btnLoraBaglantiKes);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cboxLoraParity);
            this.groupBox1.Controls.Add(this.cboxLoraStopBit);
            this.groupBox1.Controls.Add(this.cboxLoraDataBit);
            this.groupBox1.Controls.Add(this.cboxLoraBaudRate);
            this.groupBox1.Controls.Add(this.cboxLoraSP);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(332, 344);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label11.Location = new System.Drawing.Point(27, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(133, 15);
            this.label11.TabIndex = 6;
            this.label11.Text = "Lora Port Ayarları";
            // 
            // btnLoraBaglan
            // 
            this.btnLoraBaglan.Location = new System.Drawing.Point(31, 261);
            this.btnLoraBaglan.Name = "btnLoraBaglan";
            this.btnLoraBaglan.Size = new System.Drawing.Size(107, 23);
            this.btnLoraBaglan.TabIndex = 5;
            this.btnLoraBaglan.Text = "Bağlan";
            this.btnLoraBaglan.UseVisualStyleBackColor = true;
            this.btnLoraBaglan.Click += new System.EventHandler(this.btnLoraBaglan_Click);
            // 
            // btnLoraBaglantiKes
            // 
            this.btnLoraBaglantiKes.Enabled = false;
            this.btnLoraBaglantiKes.Location = new System.Drawing.Point(187, 261);
            this.btnLoraBaglantiKes.Name = "btnLoraBaglantiKes";
            this.btnLoraBaglantiKes.Size = new System.Drawing.Size(113, 23);
            this.btnLoraBaglantiKes.TabIndex = 5;
            this.btnLoraBaglantiKes.Text = "Bağlantıyı Kes";
            this.btnLoraBaglantiKes.UseVisualStyleBackColor = true;
            this.btnLoraBaglantiKes.Click += new System.EventHandler(this.btnLoraBaglantiKes_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(184, 190);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Parity Metodu";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Data Bit";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(184, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Baud Hızı";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Stop Bit";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Seri Port";
            // 
            // cboxLoraParity
            // 
            this.cboxLoraParity.FormattingEnabled = true;
            this.cboxLoraParity.Location = new System.Drawing.Point(187, 206);
            this.cboxLoraParity.Name = "cboxLoraParity";
            this.cboxLoraParity.Size = new System.Drawing.Size(113, 21);
            this.cboxLoraParity.TabIndex = 1;
            // 
            // cboxLoraStopBit
            // 
            this.cboxLoraStopBit.FormattingEnabled = true;
            this.cboxLoraStopBit.Location = new System.Drawing.Point(31, 206);
            this.cboxLoraStopBit.Name = "cboxLoraStopBit";
            this.cboxLoraStopBit.Size = new System.Drawing.Size(107, 21);
            this.cboxLoraStopBit.TabIndex = 0;
            // 
            // cboxLoraDataBit
            // 
            this.cboxLoraDataBit.FormattingEnabled = true;
            this.cboxLoraDataBit.Location = new System.Drawing.Point(31, 146);
            this.cboxLoraDataBit.Name = "cboxLoraDataBit";
            this.cboxLoraDataBit.Size = new System.Drawing.Size(107, 21);
            this.cboxLoraDataBit.TabIndex = 1;
            this.cboxLoraDataBit.SelectedIndexChanged += new System.EventHandler(this.comboBox5_SelectedIndexChanged);
            // 
            // cboxLoraBaudRate
            // 
            this.cboxLoraBaudRate.FormattingEnabled = true;
            this.cboxLoraBaudRate.Location = new System.Drawing.Point(187, 146);
            this.cboxLoraBaudRate.Name = "cboxLoraBaudRate";
            this.cboxLoraBaudRate.Size = new System.Drawing.Size(113, 21);
            this.cboxLoraBaudRate.TabIndex = 1;
            // 
            // cboxLoraSP
            // 
            this.cboxLoraSP.FormattingEnabled = true;
            this.cboxLoraSP.Location = new System.Drawing.Point(31, 65);
            this.cboxLoraSP.Name = "cboxLoraSP";
            this.cboxLoraSP.Size = new System.Drawing.Size(212, 21);
            this.cboxLoraSP.TabIndex = 0;
            this.cboxLoraSP.SelectedIndexChanged += new System.EventHandler(this.cboxLoraSP_SelectedIndexChanged);
            this.cboxLoraSP.Click += new System.EventHandler(this.cboxLoraSP_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.btnHYIBaglan);
            this.groupBox2.Controls.Add(this.btnHYIBaglantiKes);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.cboxHYIParity);
            this.groupBox2.Controls.Add(this.cboxHYIStopBit);
            this.groupBox2.Controls.Add(this.cboxHYIDataBit);
            this.groupBox2.Controls.Add(this.cboxHYIBaudRate);
            this.groupBox2.Controls.Add(this.cboxHYISP);
            this.groupBox2.Location = new System.Drawing.Point(341, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(352, 344);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label12.Location = new System.Drawing.Point(28, 16);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(126, 15);
            this.label12.TabIndex = 6;
            this.label12.Text = "HYI Port Ayarları";
            // 
            // btnHYIBaglan
            // 
            this.btnHYIBaglan.Location = new System.Drawing.Point(31, 261);
            this.btnHYIBaglan.Name = "btnHYIBaglan";
            this.btnHYIBaglan.Size = new System.Drawing.Size(107, 23);
            this.btnHYIBaglan.TabIndex = 5;
            this.btnHYIBaglan.Text = "Bağlan";
            this.btnHYIBaglan.UseVisualStyleBackColor = true;
            this.btnHYIBaglan.Click += new System.EventHandler(this.btnHYIBaglan_Click);
            // 
            // btnHYIBaglantiKes
            // 
            this.btnHYIBaglantiKes.Location = new System.Drawing.Point(187, 261);
            this.btnHYIBaglantiKes.Name = "btnHYIBaglantiKes";
            this.btnHYIBaglantiKes.Size = new System.Drawing.Size(113, 23);
            this.btnHYIBaglantiKes.TabIndex = 5;
            this.btnHYIBaglantiKes.Text = "Bağlantıyı Kes";
            this.btnHYIBaglantiKes.UseVisualStyleBackColor = true;
            this.btnHYIBaglantiKes.Click += new System.EventHandler(this.btnHYIBaglantiKes_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(184, 190);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Parity Metodu";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 130);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Data Bit";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(184, 130);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Baud Hızı";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(28, 190);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Stop Bit";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(28, 46);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Seri Port";
            // 
            // cboxHYIParity
            // 
            this.cboxHYIParity.FormattingEnabled = true;
            this.cboxHYIParity.Location = new System.Drawing.Point(187, 206);
            this.cboxHYIParity.Name = "cboxHYIParity";
            this.cboxHYIParity.Size = new System.Drawing.Size(113, 21);
            this.cboxHYIParity.TabIndex = 1;
            // 
            // cboxHYIStopBit
            // 
            this.cboxHYIStopBit.FormattingEnabled = true;
            this.cboxHYIStopBit.Location = new System.Drawing.Point(31, 206);
            this.cboxHYIStopBit.Name = "cboxHYIStopBit";
            this.cboxHYIStopBit.Size = new System.Drawing.Size(107, 21);
            this.cboxHYIStopBit.TabIndex = 0;
            // 
            // cboxHYIDataBit
            // 
            this.cboxHYIDataBit.FormattingEnabled = true;
            this.cboxHYIDataBit.Location = new System.Drawing.Point(31, 146);
            this.cboxHYIDataBit.Name = "cboxHYIDataBit";
            this.cboxHYIDataBit.Size = new System.Drawing.Size(107, 21);
            this.cboxHYIDataBit.TabIndex = 1;
            this.cboxHYIDataBit.SelectedIndexChanged += new System.EventHandler(this.comboBox5_SelectedIndexChanged);
            // 
            // cboxHYIBaudRate
            // 
            this.cboxHYIBaudRate.FormattingEnabled = true;
            this.cboxHYIBaudRate.Location = new System.Drawing.Point(187, 146);
            this.cboxHYIBaudRate.Name = "cboxHYIBaudRate";
            this.cboxHYIBaudRate.Size = new System.Drawing.Size(113, 21);
            this.cboxHYIBaudRate.TabIndex = 1;
            // 
            // cboxHYISP
            // 
            this.cboxHYISP.FormattingEnabled = true;
            this.cboxHYISP.Location = new System.Drawing.Point(31, 65);
            this.cboxHYISP.Name = "cboxHYISP";
            this.cboxHYISP.Size = new System.Drawing.Size(212, 21);
            this.cboxHYISP.TabIndex = 0;
            this.cboxHYISP.SelectedIndexChanged += new System.EventHandler(this.cboxHYISP_SelectedIndexChanged);
            this.cboxHYISP.Click += new System.EventHandler(this.cboxHYISP_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.cboxLoraSec);
            this.groupBox3.Controls.Add(this.btnLoraMesajGonder);
            this.groupBox3.Controls.Add(this.txtboxLoraMesaj);
            this.groupBox3.Controls.Add(this.btnLoraAgAyarlari);
            this.groupBox3.Controls.Add(this.btnLoraPingGorevY);
            this.groupBox3.Controls.Add(this.btnLoraPingRoket);
            this.groupBox3.Controls.Add(this.btnLoraPingYedekAv);
            this.groupBox3.Controls.Add(this.btnLoraPingPong);
            this.groupBox3.Location = new System.Drawing.Point(3, 364);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(690, 176);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Lora Ağ Ayarları";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(123, 55);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(13, 13);
            this.label17.TabIndex = 10;
            this.label17.Text = "0";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(7, 55);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(85, 13);
            this.label16.TabIndex = 9;
            this.label16.Text = "Bağlantı Durumu";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(123, 26);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(80, 13);
            this.label15.TabIndex = 8;
            this.label15.Text = "Kanal 0 433 Hz";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(7, 26);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(83, 13);
            this.label14.TabIndex = 7;
            this.label14.Text = "Kanal / Frekans";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(449, 36);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(171, 13);
            this.label13.TabIndex = 6;
            this.label13.Text = "Mesaj Gönderilecek Lora\'yı Seçiniz";
            // 
            // cboxLoraSec
            // 
            this.cboxLoraSec.Enabled = false;
            this.cboxLoraSec.FormattingEnabled = true;
            this.cboxLoraSec.Location = new System.Drawing.Point(452, 55);
            this.cboxLoraSec.Name = "cboxLoraSec";
            this.cboxLoraSec.Size = new System.Drawing.Size(121, 21);
            this.cboxLoraSec.TabIndex = 5;
            // 
            // btnLoraMesajGonder
            // 
            this.btnLoraMesajGonder.Enabled = false;
            this.btnLoraMesajGonder.Location = new System.Drawing.Point(452, 113);
            this.btnLoraMesajGonder.Name = "btnLoraMesajGonder";
            this.btnLoraMesajGonder.Size = new System.Drawing.Size(103, 23);
            this.btnLoraMesajGonder.TabIndex = 4;
            this.btnLoraMesajGonder.Text = "Mesaj Gönder";
            this.btnLoraMesajGonder.UseVisualStyleBackColor = true;
            // 
            // txtboxLoraMesaj
            // 
            this.txtboxLoraMesaj.Enabled = false;
            this.txtboxLoraMesaj.Location = new System.Drawing.Point(452, 82);
            this.txtboxLoraMesaj.Name = "txtboxLoraMesaj";
            this.txtboxLoraMesaj.Size = new System.Drawing.Size(121, 20);
            this.txtboxLoraMesaj.TabIndex = 3;
            // 
            // btnLoraAgAyarlari
            // 
            this.btnLoraAgAyarlari.Enabled = false;
            this.btnLoraAgAyarlari.Location = new System.Drawing.Point(238, 113);
            this.btnLoraAgAyarlari.Name = "btnLoraAgAyarlari";
            this.btnLoraAgAyarlari.Size = new System.Drawing.Size(75, 23);
            this.btnLoraAgAyarlari.TabIndex = 2;
            this.btnLoraAgAyarlari.Text = "Ağ Ayarları";
            this.btnLoraAgAyarlari.UseVisualStyleBackColor = true;
            // 
            // btnLoraPingGorevY
            // 
            this.btnLoraPingGorevY.Enabled = false;
            this.btnLoraPingGorevY.Location = new System.Drawing.Point(335, 113);
            this.btnLoraPingGorevY.Name = "btnLoraPingGorevY";
            this.btnLoraPingGorevY.Size = new System.Drawing.Size(95, 23);
            this.btnLoraPingGorevY.TabIndex = 1;
            this.btnLoraPingGorevY.Text = "Ping Görev Y.";
            this.btnLoraPingGorevY.UseVisualStyleBackColor = true;
            // 
            // btnLoraPingRoket
            // 
            this.btnLoraPingRoket.Enabled = false;
            this.btnLoraPingRoket.Location = new System.Drawing.Point(335, 55);
            this.btnLoraPingRoket.Name = "btnLoraPingRoket";
            this.btnLoraPingRoket.Size = new System.Drawing.Size(95, 23);
            this.btnLoraPingRoket.TabIndex = 1;
            this.btnLoraPingRoket.Text = "Ping Roket";
            this.btnLoraPingRoket.UseVisualStyleBackColor = true;
            this.btnLoraPingRoket.Click += new System.EventHandler(this.btnLoraPingRoket_Click);
            // 
            // btnLoraPingYedekAv
            // 
            this.btnLoraPingYedekAv.Enabled = false;
            this.btnLoraPingYedekAv.Location = new System.Drawing.Point(335, 84);
            this.btnLoraPingYedekAv.Name = "btnLoraPingYedekAv";
            this.btnLoraPingYedekAv.Size = new System.Drawing.Size(95, 23);
            this.btnLoraPingYedekAv.TabIndex = 0;
            this.btnLoraPingYedekAv.Text = "Ping Yedek Av.";
            this.btnLoraPingYedekAv.UseVisualStyleBackColor = true;
            // 
            // btnLoraPingPong
            // 
            this.btnLoraPingPong.Enabled = false;
            this.btnLoraPingPong.Location = new System.Drawing.Point(335, 26);
            this.btnLoraPingPong.Name = "btnLoraPingPong";
            this.btnLoraPingPong.Size = new System.Drawing.Size(95, 23);
            this.btnLoraPingPong.TabIndex = 0;
            this.btnLoraPingPong.Text = "Ping Pong Test";
            this.btnLoraPingPong.UseVisualStyleBackColor = true;
            this.btnLoraPingPong.Click += new System.EventHandler(this.btnLoraPingPong_Click);
            // 
            // logSeriPort
            // 
            this.logSeriPort.BackColor = System.Drawing.SystemColors.Desktop;
            this.logSeriPort.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.logSeriPort.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.logSeriPort.Location = new System.Drawing.Point(703, 3);
            this.logSeriPort.Multiline = true;
            this.logSeriPort.Name = "logSeriPort";
            this.logSeriPort.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logSeriPort.Size = new System.Drawing.Size(283, 536);
            this.logSeriPort.TabIndex = 2;
            this.logSeriPort.Text = "Heir Kommt die Sonne";
            this.logSeriPort.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(187, 290);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Test Verisi Gönder";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // uc_PortAyarlari
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.logSeriPort);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "uc_PortAyarlari";
            this.Size = new System.Drawing.Size(986, 609);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboxLoraParity;
        private System.Windows.Forms.ComboBox cboxLoraStopBit;
        private System.Windows.Forms.ComboBox cboxLoraBaudRate;
        private System.Windows.Forms.ComboBox cboxLoraSP;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboxLoraDataBit;
        private System.Windows.Forms.Button btnLoraBaglantiKes;
        private System.Windows.Forms.Button btnLoraBaglan;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnHYIBaglan;
        private System.Windows.Forms.Button btnHYIBaglantiKes;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cboxHYIParity;
        private System.Windows.Forms.ComboBox cboxHYIStopBit;
        private System.Windows.Forms.ComboBox cboxHYIDataBit;
        private System.Windows.Forms.ComboBox cboxHYIBaudRate;
        private System.Windows.Forms.ComboBox cboxHYISP;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox logSeriPort;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cboxLoraSec;
        private System.Windows.Forms.Button btnLoraMesajGonder;
        private System.Windows.Forms.TextBox txtboxLoraMesaj;
        private System.Windows.Forms.Button btnLoraAgAyarlari;
        private System.Windows.Forms.Button btnLoraPingGorevY;
        private System.Windows.Forms.Button btnLoraPingRoket;
        private System.Windows.Forms.Button btnLoraPingYedekAv;
        private System.Windows.Forms.Button btnLoraPingPong;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button button1;
    }
}
