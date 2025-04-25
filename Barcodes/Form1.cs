using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using Barcodes.Model;

namespace Barcodes
{
    public partial class Form1 : Form
    {
        private DefaultValue DefaultVal = new DefaultValue();
        public string Prefix = "";
        public string Suffix = "";
        public string Code = "";
        public string Example = "";
        public int WidthCode = 120;
        public int HeightCode = 40;

        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

        private void SetWriterOptions(int width, int height)
        {
            writer.Options.Width = width;
            writer.Options.Height = height;
        }

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
                var shortCodeFormats = new HashSet<string> { "EAN8", "UPS E" };
                var fullHeightFormats = new HashSet<string> { "AZTEC", "DATAMATRIX", "QRCODE" };
                var formatMap = new Dictionary<string, BarcodeFormat>
                {
                    { "AZTEC", BarcodeFormat.AZTEC },
                    { "CODABAR", BarcodeFormat.CODABAR },
                    { "CODE128", BarcodeFormat.CODE_128 },
                    { "CODE39", BarcodeFormat.CODE_39 },
                    { "CODE93", BarcodeFormat.CODE_93 },
                    { "DATAMATRIX", BarcodeFormat.DATA_MATRIX },
                    { "EAN13", BarcodeFormat.EAN_13 },
                    { "EAN8", BarcodeFormat.EAN_8 },
                    { "ITF", BarcodeFormat.ITF },
                    { "MSI", BarcodeFormat.MSI },
                    { "PDF417", BarcodeFormat.PDF_417 },
                    { "PLESSEY", BarcodeFormat.PLESSEY },
                    { "QRCODE", BarcodeFormat.QR_CODE },
                    { "UPC A", BarcodeFormat.UPC_A },
                    { "UPS E", BarcodeFormat.UPC_E }
                };

                if (!formatMap.TryGetValue(itemCode, out var format))
                {
                    MessageBox.Show("Encoding method not selected!\nНе выбран способ кодирования!");
                    return;
                }

                writer.Format = format;
                Example = shortCodeFormats.Contains(itemCode) ? DefaultVal.ShortCode : DefaultVal.DefaultCode;

                int height = fullHeightFormats.Contains(itemCode) ? pictureBox2.Height : pictureBox2.Height / 3;
                SetWriterOptions(pictureBox2.Width, height);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool CreateCode(string code)
        {
            try
            {
                System.Drawing.Image barcodeImage = writer.Write(Prefix + code + Suffix);
                barcodeImage.Save(Path.Combine(folderBrowserDialog.SelectedPath, $"{Prefix}{code}{Suffix}.png"), ImageFormat.Png);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool CreateCodeAsync(string code)
        {
            return CreateCode(code);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            codeComboBox.SelectedIndex = 2;
            comboBox1.SelectedIndex = 2;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && !Char.IsControl(number))
            {
                e.Handled = true;
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            SelectedCode(codeComboBox.SelectedItem.ToString());
            pictureBox1.Image = writer.Write(Example);
        }

        private async void generationButton2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                generationButton2.Enabled = false;
                Prefix = prefixTextBox.Text;
                Suffix = suffixTextBox.Text;
                SelectedCode(codeComboBox.SelectedItem.ToString());

                int fromNumber = Convert.ToInt32(toTextBox.Text);
                int toNumber = Convert.ToInt32(doTextBox.Text);
                int step = Convert.ToInt32(stepTextBox.Text);

                progressBar1.Minimum = 0;
                progressBar1.Maximum = ((toNumber - fromNumber) / step) + 1;
                progressBar1.Value = 0;

                if (int.TryParse(textBox6.Text, out int customWidth))
                    writer.Options.Width = customWidth;
                if (int.TryParse(textBox7.Text, out int customHeight))
                    writer.Options.Height = customHeight;

                int total = 0;
                object lockObj = new object();

                await Task.Run(() =>
                {
                    Parallel.For(fromNumber, toNumber + 1, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, i =>
                    {
                        if (!CreateCodeAsync(i.ToString()))
                            return;

                        lock (lockObj)
                        {
                            total++;
                            Invoke(new Action(() =>
                            {
                                progressBar1.Value = total;
                            }));
                        }
                    });
                });

                MessageBox.Show("Штрих коды успешно сгенерирован!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                generationButton2.Enabled = true;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string value = dataTextBox.Text;
                    if (int.TryParse(widthTextBox1.Text, out int w)) writer.Options.Width = w;
                    if (int.TryParse(heighTextBox1.Text, out int h)) writer.Options.Height = h;

                    CreateCode(value);
                    MessageBox.Show("Штрих код успешно сгенерирован!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedCode(comboBox1.SelectedItem.ToString());
            pictureBox2.Image = writer.Write(Example);
        }
    }
}
