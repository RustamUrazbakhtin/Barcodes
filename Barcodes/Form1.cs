using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace Barcodes
{
    public partial class Form1 : Form
    {
        public string _prefix = "";
        public string _suffix = "";

        public string _code = "";
        public string _example = "123456789012";

        public int _widthCode = 120;
        public int _heightCode = 40;

        private int _toNamber;
        private int _doNamber;
        private int _step;

        private readonly FolderBrowserDialog _folderBrowserDialog = new FolderBrowserDialog();

        public Form1()
        {
            InitializeComponent();
        }

        public BarcodeWriter writer = new BarcodeWriter()
        {
            Options = new ZXing.Common.EncodingOptions { Width = 120, Height = 40 }
        };

        public void SelectedCode(string itemCode)
        {
            try
            {
                switch (itemCode)
                {
                    case "AZTEC":
                        writer.Format = BarcodeFormat.AZTEC;
                        _example = "123456789012";
                        writer.Options.Width = pictureBox2.Width;
                        writer.Options.Height = pictureBox2.Height;
                        break;

                    case "CODABAR":
                        writer.Format = BarcodeFormat.CODABAR;
                        _example = "123456789012";
                        writer.Options.Width = pictureBox2.Width;
                        writer.Options.Height = pictureBox2.Height / 3;
                        break;

                    case "CODE128":
                        writer.Format = BarcodeFormat.CODE_128;
                        _example = "123456789012";
                        writer.Options.Width = pictureBox2.Width;
                        writer.Options.Height = pictureBox2.Height / 3;
                        break;

                    case "CODE39":
                        writer.Format = BarcodeFormat.CODE_39;
                        _example = "123456789012";
                        writer.Options.Width = pictureBox2.Width;
                        writer.Options.Height = pictureBox2.Height / 3;
                        break;
                    case "CODE93":
                        writer.Format = BarcodeFormat.CODE_93;
                        _example = "123456789012";
                        writer.Options.Width = pictureBox2.Width;
                        writer.Options.Height = pictureBox2.Height / 3;
                        break;

                    case "DATAMATRIX":
                        writer.Format = BarcodeFormat.DATA_MATRIX;
                        _example = "123456789012";
                        writer.Options.Width = pictureBox2.Width;
                        writer.Options.Height = pictureBox2.Height;
                        break;

                    case "EAN13":
                        writer.Format = BarcodeFormat.EAN_13;
                        _example = "123456789012";
                        writer.Options.Width = pictureBox2.Width;
                        writer.Options.Height = pictureBox2.Height / 3;
                        break;

                    case "EAN8":
                        writer.Format = BarcodeFormat.EAN_8;
                        _example = "1234567";
                        writer.Options.Width = pictureBox2.Width;
                        writer.Options.Height = pictureBox2.Height / 3;
                        break;

                    case "ITF":
                        writer.Format = BarcodeFormat.ITF;
                        _example = "123456789012";
                        writer.Options.Width = pictureBox2.Width;
                        writer.Options.Height = pictureBox2.Height / 3;
                        break;

                    case "MSI":
                        writer.Format = BarcodeFormat.MSI;
                        _example = "123456789012";
                        writer.Options.Width = pictureBox2.Width;
                        writer.Options.Height = pictureBox2.Height / 3;
                        break;

                    case "PDF417":
                        writer.Format = BarcodeFormat.PDF_417;
                        _example = "123456789012";
                        writer.Options.Width = pictureBox2.Width;
                        writer.Options.Height = pictureBox2.Height / 3;
                        break;
                    case "PLESSEY":
                        writer.Format = BarcodeFormat.PLESSEY;
                        _example = "123456789012";
                        writer.Options.Width = pictureBox2.Width;
                        writer.Options.Height = pictureBox2.Height / 3;
                        break;

                    case "QRCODE":
                        writer.Format = BarcodeFormat.QR_CODE;
                        _example = "123456789012";
                        writer.Options.Width = pictureBox2.Width;
                        writer.Options.Height = pictureBox2.Height;
                        break;
                    case "UPC A":
                        writer.Format = BarcodeFormat.UPC_A;
                        _example = "123456789012";
                        writer.Options.Width = pictureBox2.Width;
                        writer.Options.Height = pictureBox2.Height / 3;
                        break;

                    case "UPS E":
                        writer.Format = BarcodeFormat.UPC_E;
                        _example = "1234567";
                        writer.Options.Width = pictureBox2.Width;
                        writer.Options.Height = pictureBox2.Height / 3;
                        break;

                    default:
                        MessageBox.Show("Encoding method not selected!\nНе выбран способ кодирования!");
                        break;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void CreateCode(string code)
        {
            //BarcodeLib.Barcode barcode = new BarcodeLib.Barcode();
            //System.Drawing.Image barcodeImage = barcode.Encode(BarcodeLib.TYPE.CODE128, toNamber.ToString(), Color.Black, Color.White, 120, 20);
            try
            {
                System.Drawing.Image barcodeImage = writer.Write(_prefix + code + _suffix);
                //pictureBox1.Image = barcodeImage;
                barcodeImage.Save(_folderBrowserDialog.SelectedPath + "\\" + _prefix + code + _suffix + ".png", ImageFormat.Png);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        public void CreateCodeAsync()
        {
            //BarcodeLib.Barcode barcode = new BarcodeLib.Barcode();
            //System.Drawing.Image barcodeImage = barcode.Encode(BarcodeLib.TYPE.CODE128, toNamber.ToString(), Color.Black, Color.White, 120, 20);
            try
            {
                for (int i = _toNamber; i <= _doNamber; i += _step)
                {
                    System.Drawing.Image barcodeImage = writer.Write(_prefix + i + _suffix);
                    //pictureBox1.Image = barcodeImage;
                    barcodeImage.Save(_folderBrowserDialog.SelectedPath + "\\" + _prefix + i + _suffix + ".png", ImageFormat.Png);
                    progressBar1.PerformStep();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            codeComboBox.SelectedIndex = 2;
            comboBox1.SelectedIndex = 2;
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if(!Char.IsDigit(number) && !Char.IsControl(number))
            {
                e.Handled = true;
            }
        }

        private void ComboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            SelectedCode(codeComboBox.SelectedItem.ToString());
            System.Drawing.Image image = writer.Write(_example); ;
            pictureBox1.Image = image;
        }

        private async void GenerationButton_Click(object sender, EventArgs e)
        {
            if (_folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _prefix = prefixTextBox.Text;
                    _suffix = suffixTextBox.Text;
                    // Выбор кода
                    SelectedCode(codeComboBox.SelectedItem.ToString());


                    _toNamber = Convert.ToInt32(toTextBox.Text);
                    _doNamber = Convert.ToInt32(doTextBox.Text);
                    _step = Convert.ToInt32(stepTextBox.Text);

                    // Установка ограничений в ProgressBar
                    progressBar1.Value = _toNamber;
                    progressBar1.Minimum = _toNamber;
                    progressBar1.Maximum = _doNamber;
                    progressBar1.Step = _step;

                    if (textBox6.Text != "")
                    {
                        writer.Options.Width = Convert.ToInt32(textBox6.Text);
                    }

                    if (textBox7.Text != "")
                    {
                        writer.Options.Height = Convert.ToInt32(textBox7.Text);
                    }

                    await Task.Run(() => CreateCodeAsync());
                        
                    MessageBox.Show("Штрих коды успешно сгенерирован!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private async void Button_Click(object sender, EventArgs e)
        {
            if (_folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string value = dataTextBox.Text;
                    if (widthTextBox1.Text != "")
                    {
                        writer.Options.Width = Convert.ToInt32(widthTextBox1.Text);
                    }

                    if (heighTextBox1.Text != "")
                    {
                        writer.Options.Height = Convert.ToInt32(heighTextBox1.Text);
                    }

                    await Task.Run(() => CreateCode(value));

                    CreateCode(value);
                    MessageBox.Show("Штрих код успешно сгенерирован!");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedCode(comboBox1.SelectedItem.ToString());
            System.Drawing.Image image = writer.Write(_example); ;
            pictureBox2.Image = image;
        }
    }
}
