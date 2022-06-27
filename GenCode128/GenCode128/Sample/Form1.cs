// Decompiled with JetBrains decompiler
// Type: Sample.Form1
// Assembly: Sample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 141F30C3-2E7B-41CA-8965-2F8A35040425
// Assembly location: C:\Users\josep\Downloads\BarcodeGenerator (1)\Sample.exe

using GenCode128;
using Sample.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Sample
{
    public class Form1 : Form
    {
        private Label label1;
        private TextBox txtInput;
        private Button cmdMakeBarcode;
        private PictureBox pictBarcode;
        private TextBox txtWeight;
        private Label label2;
        private PrintDocument printDocument1;
        private Button cmdPrint;
        private TextBox finalWidthTxt;
        private Label label3;
        private TextBox finalHeightTxt;
        private Label label4;
        private TextBox topOffsetTxt;
        private Label label5;
        private TextBox leftOffSetTxt;
        private Label label6;
        private TextBox bar2LeftOffSetTxt;
        private Label label7;
        private TextBox bar2TopOffSetTxt;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label11;
        private NumericUpDown Amount;
        private Container components = (Container)null;

        public Form1()
        {
            this.InitializeComponent();
            this.finalHeightTxt.Text = Settings.Default.FinalHeight;
            this.finalWidthTxt.Text = Settings.Default.FinalWidth;
            this.leftOffSetTxt.Text = Settings.Default.LeftOffSet;
            this.topOffsetTxt.Text = Settings.Default.TopOffSet;
            this.bar2LeftOffSetTxt.Text = Settings.Default.Bar2LeftOffSet;
            this.bar2TopOffSetTxt.Text = Settings.Default.Bar2TopOffSet;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.cmdMakeBarcode = new System.Windows.Forms.Button();
            this.pictBarcode = new System.Windows.Forms.PictureBox();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.cmdPrint = new System.Windows.Forms.Button();
            this.finalWidthTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.finalHeightTxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.topOffsetTxt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.leftOffSetTxt = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.bar2LeftOffSetTxt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.bar2TopOffSetTxt = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.Amount = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.pictBarcode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Amount)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Text to encode";
            // 
            // txtInput
            // 
            this.txtInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInput.Location = new System.Drawing.Point(96, 8);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(436, 20);
            this.txtInput.TabIndex = 1;
            // 
            // cmdMakeBarcode
            // 
            this.cmdMakeBarcode.Location = new System.Drawing.Point(440, 151);
            this.cmdMakeBarcode.Name = "cmdMakeBarcode";
            this.cmdMakeBarcode.Size = new System.Drawing.Size(92, 23);
            this.cmdMakeBarcode.TabIndex = 2;
            this.cmdMakeBarcode.Text = "Make barcode";
            this.cmdMakeBarcode.Click += new System.EventHandler(this.cmdMakeBarcode_Click);
            // 
            // pictBarcode
            // 
            this.pictBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictBarcode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictBarcode.Location = new System.Drawing.Point(8, 180);
            this.pictBarcode.Name = "pictBarcode";
            this.pictBarcode.Size = new System.Drawing.Size(528, 119);
            this.pictBarcode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictBarcode.TabIndex = 3;
            this.pictBarcode.TabStop = false;
            // 
            // txtWeight
            // 
            this.txtWeight.Location = new System.Drawing.Point(96, 36);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.Size = new System.Drawing.Size(44, 20);
            this.txtWeight.TabIndex = 5;
            this.txtWeight.Text = "2";
            this.txtWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Bar weight";
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // cmdPrint
            // 
            this.cmdPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdPrint.Location = new System.Drawing.Point(460, 307);
            this.cmdPrint.Name = "cmdPrint";
            this.cmdPrint.Size = new System.Drawing.Size(75, 23);
            this.cmdPrint.TabIndex = 6;
            this.cmdPrint.Text = "Print";
            this.cmdPrint.Click += new System.EventHandler(this.cmdPrint_Click);
            // 
            // finalWidthTxt
            // 
            this.finalWidthTxt.Location = new System.Drawing.Point(96, 83);
            this.finalWidthTxt.Name = "finalWidthTxt";
            this.finalWidthTxt.Size = new System.Drawing.Size(44, 20);
            this.finalWidthTxt.TabIndex = 8;
            this.finalWidthTxt.Text = "50";
            this.finalWidthTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Final Width";
            // 
            // finalHeightTxt
            // 
            this.finalHeightTxt.Location = new System.Drawing.Point(96, 109);
            this.finalHeightTxt.Name = "finalHeightTxt";
            this.finalHeightTxt.Size = new System.Drawing.Size(44, 20);
            this.finalHeightTxt.TabIndex = 10;
            this.finalHeightTxt.Text = "50";
            this.finalHeightTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "Final Height";
            // 
            // topOffsetTxt
            // 
            this.topOffsetTxt.Location = new System.Drawing.Point(288, 80);
            this.topOffsetTxt.Name = "topOffsetTxt";
            this.topOffsetTxt.Size = new System.Drawing.Size(44, 20);
            this.topOffsetTxt.TabIndex = 12;
            this.topOffsetTxt.Text = "50";
            this.topOffsetTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(200, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 16);
            this.label5.TabIndex = 11;
            this.label5.Text = "Top OffSet";
            // 
            // leftOffSetTxt
            // 
            this.leftOffSetTxt.Location = new System.Drawing.Point(440, 77);
            this.leftOffSetTxt.Name = "leftOffSetTxt";
            this.leftOffSetTxt.Size = new System.Drawing.Size(44, 20);
            this.leftOffSetTxt.TabIndex = 14;
            this.leftOffSetTxt.Text = "50";
            this.leftOffSetTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(352, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 16);
            this.label6.TabIndex = 13;
            this.label6.Text = "Left OffSet";
            // 
            // bar2LeftOffSetTxt
            // 
            this.bar2LeftOffSetTxt.Location = new System.Drawing.Point(440, 110);
            this.bar2LeftOffSetTxt.Name = "bar2LeftOffSetTxt";
            this.bar2LeftOffSetTxt.Size = new System.Drawing.Size(44, 20);
            this.bar2LeftOffSetTxt.TabIndex = 18;
            this.bar2LeftOffSetTxt.Text = "50";
            this.bar2LeftOffSetTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(352, 113);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 16);
            this.label7.TabIndex = 17;
            this.label7.Text = "Left OffSet";
            // 
            // bar2TopOffSetTxt
            // 
            this.bar2TopOffSetTxt.Location = new System.Drawing.Point(288, 113);
            this.bar2TopOffSetTxt.Name = "bar2TopOffSetTxt";
            this.bar2TopOffSetTxt.Size = new System.Drawing.Size(44, 20);
            this.bar2TopOffSetTxt.TabIndex = 16;
            this.bar2TopOffSetTxt.Text = "50";
            this.bar2TopOffSetTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(200, 116);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 16);
            this.label8.TabIndex = 15;
            this.label8.Text = "Top OffSet";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(146, 99);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(111, 17);
            this.label9.TabIndex = 19;
            this.label9.Text = "Second Bar Code";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(146, 66);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(111, 17);
            this.label10.TabIndex = 20;
            this.label10.Text = "Gen Bar Code";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(149, 161);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(139, 16);
            this.label11.TabIndex = 21;
            this.label11.Text = "Amt to Generate";
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // Amount
            // 
            this.Amount.Location = new System.Drawing.Point(319, 157);
            this.Amount.Name = "Amount";
            this.Amount.Size = new System.Drawing.Size(49, 20);
            this.Amount.TabIndex = 23;
            this.Amount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(540, 337);
            this.Controls.Add(this.Amount);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.bar2LeftOffSetTxt);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.bar2TopOffSetTxt);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.leftOffSetTxt);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.topOffsetTxt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.finalHeightTxt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.finalWidthTxt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmdPrint);
            this.Controls.Add(this.txtWeight);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictBarcode);
            this.Controls.Add(this.cmdMakeBarcode);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Code128 Demo";
            ((System.ComponentModel.ISupportInitialize)(this.pictBarcode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Amount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        [STAThread]
        private static void Main() => Application.Run((Form)new Form1());

        private void cmdMakeBarcode_Click(object sender, EventArgs e)
        {
           
            int width = int.Parse(this.finalWidthTxt.Text);
            int height = int.Parse(this.finalHeightTxt.Text);
            float num1 = float.Parse(this.topOffsetTxt.Text);
            float num2 = float.Parse(this.leftOffSetTxt.Text);
            float num3 = float.Parse(this.bar2TopOffSetTxt.Text);
            float num4 = float.Parse(this.bar2LeftOffSetTxt.Text);
            
            try
            {
                for (int i = 0; i < Amount.Value; i++)
                {
                    var image3 = CreateImage(width, height, num2, num1, num4, num3, this.txtInput.Text);
                    this.pictBarcode.Image = image3;
                }

                MessageBox.Show("Complete");
            }
            catch (Exception ex)
            {
                int num5 = (int)MessageBox.Show((IWin32Window)this, ex.Message, this.Text);
            }
        }

        private Image CreateImage(int width, int height, float num2, float num1, float num4,
            float num3, string inputText)
        {

            Image image1 = Image.FromFile("Background.png");
            Image image2 = Image.FromFile("Bar2.png");

            if (string.IsNullOrEmpty(inputText))
                inputText = this.RandomDigits(7);
            Image image3 = Code128Rendering.MakeBarcodeImage(inputText, int.Parse(this.txtWeight.Text), true);
            using (image1)
            {
                using (Bitmap bitmap = new Bitmap(width, height))
                {
                    using (Graphics graphics = Graphics.FromImage((Image)bitmap))
                    {
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(image1, new Rectangle(0, 0, width, height),
                            new Rectangle(0, 0, image1.Width, image1.Height), GraphicsUnit.Pixel);
                        graphics.DrawImage(image3, (float)(bitmap.Width / 2 - image3.Width / 2) + num2,
                            (float)(bitmap.Height / 2 - image3.Height / 2) + num1);
                        graphics.Save();
                        graphics.DrawImage(image2, (float)(bitmap.Width / 2 - image2.Width / 2) + num4,
                            (float)(bitmap.Height / 2 - image2.Height / 2) + num3);
                        graphics.Save();
                    }

                    try
                    {
                        bitmap.Save(inputText + ".png", ImageFormat.Png);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            return image3;
        }

        public string RandomDigits(int length)
        {
            Random random = new Random();
            string empty = string.Empty;
            for (int index = 0; index < length; ++index)
                empty += random.Next(10).ToString();
            return empty;
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            using (Graphics graphics = e.Graphics)
            {
                using (Font font = new Font("Arial", 16f))
                {
                    string s1 = string.Format("Code128 barcode weight={0}", (object)this.txtWeight.Text);
                    graphics.DrawString(s1, font, Brushes.Black, 50f, 50f);
                    string s2 = string.Format("message='{0}'", (object)this.txtInput.Text);
                    graphics.DrawString(s2, font, Brushes.Black, 50f, 75f);
                    graphics.DrawImage(this.pictBarcode.Image, 50, 110);
                }
            }
        }

        private void cmdPrint_Click(object sender, EventArgs e) => this.printDocument1.Print();

        private void label9_Click(object sender, EventArgs e)
        {
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
    }
}
