using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AioiSystems.SmartCard;
using AioiSystems.DotModule;
using AioiSystems.DotModule.Barcode;
using System.IO;
using System.Data.SqlClient;

namespace SampleForAcr1252
{
    public partial class Form3 : Form
    {
        private const string READER_NAME_ACR1252_PICC = "ACS ACR1252 1S CL Reader PICC";
        private const string QRCodeData = "ACS ACR1252 1S CL Reader PICC";

        private int _context = 0;
        private Bitmap BMP;
        private SmartCard _smartCard;

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void Init()
        {
            //testconnection();
            this.ActiveControl = txt_DataInput;
            btn_Generate.Enabled = false;
            this.btn_Generate.BackColor = System.Drawing.Color.FromArgb(190, 194, 191);
            string adapterName = "";
            int ret = PcScModules.GetContext(out _context);
            if (ret == PcScModules.SCARD_S_SUCCESS)
            {
                string[] names = PcScModules.GetAdapters(_context);
                if (names != null)
                {
                    foreach (string name in names)
                    {
                        if (name.StartsWith(READER_NAME_ACR1252_PICC))
                        {
                            adapterName = name;
                            break;
                        }
                    }
                }
            }

            if (adapterName != "")
            {
                NfcAdapter adapter = new NfcAdapter();
                adapter.Context = _context;
                adapter.Name = adapterName;
                try
                {
                    adapter.OpenAdapter();
                    _smartCard = new SmartCard(adapter);

                    lbl_LogData.Text = "Push the function button.";
                }
                catch (Win32Exception err)
                {
                    lbl_LogData.Text = "r/w open error.";
                }
            }
            else
            {
                lbl_LogData.Text = "r/w was not found.";
            }
        }

        private bool StartPolling()
        {
            if (_smartCard == null)
                return false;

            lbl_LogData.Text = "No Smart Card Detected.";
            lbl_LogData.ForeColor = Color.FromArgb(219, 50, 35);

            _smartCard.TargetDisplay = GetSelectedDisplay();
            time_pollingTimer.Interval = 250;
            time_pollingTimer.Enabled = true;

            return true;
        }

        private SmartCard.DisplayType GetSelectedDisplay()
        {
            return SmartCard.DisplayType.SmartCard29;
        }

        private ImageInfo CreateImage()
        {
            DisplayPainter display = new DisplayPainter(DisplayPainter.DisplaySizeType.Size300x200, false);
            display.PutText("DENSO", new Font("Arial", 9), 0, 0, true);
            display.PutImage(BMP, 60, 17, 180, 180, true, false);
            return display.GetLocalDisplayImage();
        }

        // EVENTS BUTTONS
        private void btn_Generate_Click(object sender, EventArgs e)
        {
            this.ActiveControl = txt_DataInput;
            if (StartPolling())
            {
                _smartCard.SelectedFunction = SmartCard.SmartCardFunctions.ShowImage;
                _smartCard.SetImageData(CreateImage());

                DisplayPainter display = new DisplayPainter(DisplayPainter.DisplaySizeType.Size300x200, false);

                btn_Generate.Enabled = false;
                btn_LoadData.Enabled = false;
                this.btn_LoadData.BackColor = System.Drawing.Color.FromArgb(190, 194, 191);
                this.btn_Generate.BackColor = System.Drawing.Color.FromArgb(190, 194, 191);
            }
            
        }

        private void btn_ClearCard_Click(object sender, EventArgs e)
        {
            this.ActiveControl = txt_DataInput;
            if (StartPolling())
            {
                _smartCard.SelectedFunction = SmartCard.SmartCardFunctions.ClearDisplay;
                btn_Generate.Enabled = false;
                qrCodeImage.Image = null;
                btn_LoadData.Enabled = true;
                txt_DataInput.Text = null;
                this.btn_LoadData.BackColor = System.Drawing.Color.FromArgb(128, 255, 255);
            }
        }

        private void btn_LoadData_Click(object sender, EventArgs e)
        {
            if(txt_DataInput.Text != null && txt_DataInput.Text != "")
            {
                this.ActiveControl = txt_DataInput;
                DisplayPainter display = new DisplayPainter(DisplayPainter.DisplaySizeType.Size300x200, false);
                string inputData = txt_DataInput.Text;
                Zen.Barcode.CodeQrBarcodeDraw qrcode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                qrCodeImage.Image = qrcode.Draw(inputData, 70);

                BMP = (Bitmap)qrCodeImage.Image;
                btn_Generate.Enabled = true;
                this.btn_Generate.BackColor = System.Drawing.Color.FromArgb(128, 255, 128);
            }
            else
            {
                this.ActiveControl = txt_DataInput;
                lbl_LogData.Text = "No data to generate.";
            }
        }

        // TIMER
        private void time_pollingTimer_Tick(object sender, EventArgs e)
        {
            if (_smartCard.Adapter.Poll())
            {
                time_pollingTimer.Enabled = false;

                try
                {
                    _smartCard.Adapter.OpenCard();
                }
                catch (Exception err)
                {
                    lbl_LogData.Text = "Open failed.";
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                bool isError = false;
                try
                {
                    byte[] idm = _smartCard.Adapter.GetIdm();

                    if (SmartCard.IsSmartCard(idm))
                    {
                        lbl_LogData.ForeColor = System.Drawing.SystemColors.ControlLightLight; ;
                        lbl_LogData.Text = "Processing...";
                        this.Refresh();

                        _smartCard.SetIdm(idm);
                        if (_smartCard.SelectedFunction == SmartCard.SmartCardFunctions.WriteToCardArea)
                        {
                            _smartCard.WriteToCardArea(0, _smartCard.GetUserData());
                        }
                        else if (_smartCard.SelectedFunction == SmartCard.SmartCardFunctions.ReadFromCardArea)
                        {
                            _smartCard.SetUserData(_smartCard.ReadFromCardArea(0, 27));
                        }
                        else
                        {
                            _smartCard.StartProcess();
                        }

                        lbl_LogData.ForeColor = Color.FromArgb(71, 181, 78);
                        lbl_LogData.Text = "Done.";

                        if (_smartCard.SelectedFunction == SmartCard.SmartCardFunctions.ReadUserArea
                            || _smartCard.SelectedFunction == SmartCard.SmartCardFunctions.ReadFromCardArea)
                        {
                            ShowReadData(_smartCard.GetUserData());
                        }
                    }
                }
                catch (Win32Exception err)
                {
                    lbl_LogData.Text = "Error";
                    isError = true;
                }
                catch (Exception err)
                {
                    lbl_LogData.Text = "Error";
                    isError = true;
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                    _smartCard.Adapter.Close();
                }
                if (!isError)
                    DetectRelease();
            }

        }

        private void time_pollingTimer2_Tick(object sender, EventArgs e)
        {
            time_pollingTimer2.Enabled = false;
            if (_smartCard.Adapter.Poll())
            {
                time_pollingTimer2.Enabled = true;
            }
            else
            {
                lbl_LogData.Text = "Push the function button.";
            }
        }

        private void DetectRelease()
        {
            time_pollingTimer2.Interval = 200;
            time_pollingTimer2.Enabled = true;
        }

        private void ShowReadData(byte[] data)
        {
            string text = Encoding.ASCII.GetString(data);
            lbl_LogData.Text = text;
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void testconnection()
        {
            string inputData = txt_DataInput.Text;

            try
            {
                using (SqlConnection conn = new SqlConnection("User ID=sa;password=LogMeIn@SQLDB;server=172.168.200.3\\SQL2019EXPRESS;database=KedicaDB; connection timeout=30"))
                {
                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        cmdSql.CommandType = CommandType.StoredProcedure;
                        cmdSql.CommandText = "spSmartCard_GetPartNumberInfo";
                        cmdSql.Parameters.Clear();
                        cmdSql.Parameters.AddWithValue("@PartNumberInput", inputData);

                        using (SqlDataReader sdr = cmdSql.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                //model data here 
                            }
                        }
                    } 
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void txt_DataInput_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine("hello");
        }
    }
}
