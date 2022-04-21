/* 
 * Copyright(C)AIOI SYSTEMS CO., LTD.
 * All rights reserved.
 */

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

namespace SampleForAcr1252
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private const string READER_NAME_ACR1252_PICC = "ACS ACR1252 1S CL Reader PICC";
 
        private int _context = 0;
        private SmartCard _smartCard;

        /// <summary>
        /// Initializes the application when starting.
        /// </summary>
        private void Init()
        {
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

                    AddLog("ACR1252 is ready to use.");
                    lblMessage.Text = "Push the function button.";
                }
                catch (Win32Exception err)
                {
                    lblMessage.Text = "r/w open error.";
                    AddLog(string.Format("r/w open error. code={0}", err.NativeErrorCode));
                }
            }
            else
            {
                lblMessage.Text = "r/w was not found.";
            }

            lblIdm.Text = "";
        }

        
        private ImageInfo CreateImage()
        {
            if (rb27inch.Checked)
            {
                return CreateImageFor27inch();
            }
            else if (rb29inch.Checked)
            {
                return CreateImageFor29inch();
            }
            else
            {
                return CreateImageFor2inch();
            }
        }

        /// <summary>
        /// Creates the sample image.
        /// </summary>
        private ImageInfo CreateImageFor2inch()
        {
            DisplayPainter display = new DisplayPainter(DisplayPainter.DisplaySizeType.Size200x96, false);

            //Draws text
            display.PutText("Smart-Tag", new Font("Arial", 9), 0, 0, true);
            display.PutText("Sample Program", new Font("Arial", 12, FontStyle.Bold), 0, 14, false);

            //Draws line
            display.PutLine(0, 35, 130, 35, 1, false);

            //Draws barcode
            Code39 code39 = new Code39();
            code39.BarcodeData = "123456";
            code39.Height = 40;
            display.PutBarcode(code39, 0, 40);
           
            //Draws rectangle
            display.PutRectangle(132, 2, 66, 90, 1, false);

            //Draws image
            display.PutImage(Application.StartupPath + "\\image1.png", 135, 6, 60, 80, true, false);

            return display.GetLocalDisplayImage();
        }

        /// <summary>
        /// Creates the sample image.(for 2.7inch)
        /// </summary>
        /// <returns></returns>
        private ImageInfo CreateImageFor27inch()
        {
            DisplayPainter display = new DisplayPainter(DisplayPainter.DisplaySizeType.Size264x176, false);

            //Draws text
            display.PutText("Smart-Tag", new Font("Arial", 9), 0, 0, true);
            display.PutText("Sample Program", new Font("Arial", 12, FontStyle.Bold), 8, 14, false);

            //Draws line
            display.PutLine(8, 35, 256, 35, 1, false);

            //Draws barcode
            Code39 code39 = new Code39();
            code39.BarcodeData = "123456";
            code39.Height = 60;

            code39.RotateFlip = RotateFlipType.Rotate270FlipNone;
            display.PutBarcode(code39, 182, 45);

            //Draws image
            display.PutImage(Application.StartupPath + "\\image2.png", 8, 51, 156, 110, true, false);

            return display.GetLocalDisplayImage();
        }

        /// <summary>
        /// Creates the sample image.(for 2.9inch)
        /// </summary>
        /// <returns></returns>
        private ImageInfo CreateImageFor29inch()
        {
            DisplayPainter display = new DisplayPainter(DisplayPainter.DisplaySizeType.Size300x200, false);

            //Draws text
            display.PutText("SmartCard", new Font("Arial", 9), 0, 0, true);
            display.PutText("Sample Program", new Font("Arial", 18, FontStyle.Bold), 8, 23, false);

            //Draws line
            display.PutLine(8, 52, 292, 52, 1, false);

            //Draws barcode
            Code39 code39 = new Code39();
            code39.BarcodeData = "123456";
            code39.Height = 60;
            code39.RotateFlip = RotateFlipType.Rotate270FlipNone;
            display.PutBarcode(code39, 220, 65);
            
            //Draws image
            display.PutImage(Application.StartupPath + "\\image2.png", 8, 60, 203, 135, true, false);

            return display.GetLocalDisplayImage();
        }

        private ImageInfo CreatePartialImage()
        {
            return DisplayPainter.GetTextImage(DateTime.Now.ToString("HH:mm"), new Font("Consolas", 10));
        }

        /// <summary>
        /// Adds the message to the log.
        /// </summary>
        private void AddLog(string line)
        {
            lstLog.SelectedIndex = lstLog.Items.Add(line);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_smartCard != null && _smartCard.Adapter != null)
                _smartCard.Adapter.CloseAdapter();

            if (_context != 0)
            {
                PcScModules.ReleaseContext(_context);
            }
        }

        private void btnShowImage_Click(object sender, EventArgs e)
        {
            if (StartPolling())
            {
                _smartCard.SelectedFunction = SmartCard.SmartCardFunctions.ShowImage;
                _smartCard.SetImageData(CreateImage());
            }
        }

        private void btnShowPartial_Click(object sender, EventArgs e)
        {
            if (StartPolling())
            {
                _smartCard.SelectedFunction = SmartCard.SmartCardFunctions.ShowPartial;
                _smartCard.SetImageData(CreatePartialImage());
            }
        }

        private void btnClearDisplay_Click(object sender, EventArgs e)
        {
            if (StartPolling())
            {
                _smartCard.SelectedFunction = SmartCard.SmartCardFunctions.ClearDisplay;
            }
        }
        
        private void btnWriteData_Click(object sender, EventArgs e)
        {
            if (StartPolling())
            {
                _smartCard.SetUserData(CreateTestDataForWrite(3070));
                _smartCard.SelectedFunction = SmartCard.SmartCardFunctions.WriteUserArea;
            }
        }

        private void btnReadData_Click(object sender, EventArgs e)
        {
            if (StartPolling())
            {
                _smartCard.SelectedFunction = SmartCard.SmartCardFunctions.ReadUserArea;
            }
        }

        private void btnWriteData2_Click(object sender, EventArgs e)
        {
            if (StartPolling())
            {
                _smartCard.SetUserData(CreateTestDataForWrite(27 * 16));
                _smartCard.SelectedFunction = SmartCard.SmartCardFunctions.WriteToCardArea;
            }
        }

        private void btnReadData2_Click(object sender, EventArgs e)
        {
            if (StartPolling())
            {
                _smartCard.SelectedFunction = SmartCard.SmartCardFunctions.ReadFromCardArea;
            }
        }

        private void btnSaveLayout_Click(object sender, EventArgs e)
        {
            if (StartPolling())
            {
                _smartCard.LayoutNo = 1;
                _smartCard.SelectedFunction = SmartCard.SmartCardFunctions.SaveLayout;
            }
        }

        private void btnLoadLayout_Click(object sender, EventArgs e)
        {
            if (StartPolling())
            {
                _smartCard.LayoutNo = 1;
                _smartCard.SelectedFunction = SmartCard.SmartCardFunctions.LoadLayout;
            }
        }

        /// <summary>
        /// Starts polling for smart-tag.
        /// </summary>
        private bool StartPolling()
        {
            if (_smartCard == null)
                return false;

            lblMessage.Text = "Touch your Smart-tag!";
            lblIdm.Text = "";

            
            _smartCard.TargetDisplay = GetSelectedDisplay();
            timer1.Interval = 250;
            timer1.Enabled = true;

            return true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            /*
             * This sample using main-thread, but usually 
             * uses sub-thread to communicate with the smart-tag.
             */
            if (_smartCard.Adapter.Poll())
            {
                timer1.Enabled = false;

                try
                {
                    _smartCard.Adapter.OpenCard();
                }
                catch (Exception err)
                {
                    AddLog(err.ToString());
                    lblMessage.Text = "Open failed.";
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                bool isError = false;
                try
                {
                    byte[] idm = _smartCard.Adapter.GetIdm();
                    lblIdm.Text = BitConverter.ToString(idm);
                    
                    if (SmartCard.IsSmartCard(idm))
                    {
                        lblMessage.Text = "Processing...";
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

                        lblMessage.Text = "Done.";

                        if (_smartCard.SelectedFunction == SmartCard.SmartCardFunctions.ReadUserArea
                            || _smartCard.SelectedFunction == SmartCard.SmartCardFunctions.ReadFromCardArea)
                        {
                            ShowReadData(_smartCard.GetUserData());
                        }
                    }
                }
                catch (Win32Exception err)
                {
                    lblMessage.Text = "Error";
                    AddLog(string.Format("Communication error. code={0}", err.NativeErrorCode));
                    isError = true;
                }
                catch (Exception err)
                {
                    lblMessage.Text = "Error";
                    AddLog(err.ToString());
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

        private SmartCard.DisplayType GetSelectedDisplay()
        {
            if (rb27inch.Checked)
                return SmartCard.DisplayType.SmartTag27;
            else if (rb29inch.Checked)
                return SmartCard.DisplayType.SmartCard29;
            else
                return SmartCard.DisplayType.SmartTag20;
        }

        

        private byte[] CreateTestDataForWrite(int size)
        {
            byte[] data = new byte[size];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)((i % 10) + 0x30);
            }
            return data;
        }

        private void ShowReadData(byte[] data)
        {
            string text = Encoding.ASCII.GetString(data);
            Form2 form = new Form2();
            form.SetText(text);
            form.ShowDialog(this);
        }

        /// <summary>
        /// Starts polling for tag released.
        /// </summary>
        private void DetectRelease()
        {
            timer2.Interval = 200;
            timer2.Enabled = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            if (_smartCard.Adapter.Poll())
            {
                timer2.Enabled = true;
            }
            else
            {
                lblMessage.Text = "Push the function button.";
                lblIdm.Text = "";
            }
        }

        private void rb29inch_CheckedChanged(object sender, EventArgs e)
        {
            if (rb29inch.Checked)
            {
                btnWriteData2.Enabled = true;
                btnReadData2.Enabled = true;
            }
            else
            {
                btnWriteData2.Enabled = false;
                btnReadData2.Enabled = false;
            }
        }
    }

}
