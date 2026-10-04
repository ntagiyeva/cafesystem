using System;
using System.Globalization;
using System.Windows.Forms;

namespace cafesystem
{
    public partial class Form1 : Form
    {
        private decimal? yekunHesab;

        private class Yemek
        {
            public string Ad { get; }
            public decimal Qiymet { get; }

            public Yemek(string ad, decimal qiymet)
            {
                Ad = ad;
                Qiymet = qiymet;
            }

            public override string ToString()
            {
                return $"{Ad} — {Qiymet:0.00} AZN";
            }
        }

        public Form1()
        {
            InitializeComponent();

            // Hesab və qalıq yalnız proqram tərəfindən yazılır.
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;

            button1.Click += Hesabla_Click;
            button2.Click += Temizle_Click;
            button3.Click += SebetdenSil_Click;
            button4.Click += Yenile_Click;
            button5.Click += YekunHesab_Click;

            // Şəklə hər klik səbətə bir məhsul əlavə edir.
            MenyuyaBagla(pictureBox2, "Tort", 4.00m);
            MenyuyaBagla(pictureBox3, "Kola", 2.00m);
            MenyuyaBagla(pictureBox4, "Pizza", 6.00m);
            MenyuyaBagla(pictureBox5, "Burger", 5.00m);
            MenyuyaBagla(pictureBox6, "Sendviç", 3.50m);
            MenyuyaBagla(pictureBox7, "Şirə", 2.50m);
            MenyuyaBagla(pictureBox8, "Keks", 3.00m);
            MenyuyaBagla(pictureBox9, "Hot-doq", 4.00m);
            MenyuyaBagla(pictureBox10, "Peçenye", 1.50m);

            // Məbləğ dəyişəndə əvvəlki qalıq silinir.
            textBox2.TextChanged += (sender, e) => textBox3.Clear();
        }

        private void MenyuyaBagla(
            PictureBox sekil, string ad, decimal qiymet)
        {
            sekil.Cursor = Cursors.Hand;

            sekil.Click += (sender, e) =>
            {
                listBox1.Items.Add(new Yemek(ad, qiymet));
                HesabiSifirla();
            };
        }

        private void HesabiSifirla()
        {
            // Səbət dəyişəndə yekun hesab yenidən çıxarılmalıdır.
            yekunHesab = null;
            textBox4.Clear();
            textBox3.Clear();
        }

        private void SebetdenSil_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is not Yemek yemek)
            {
                MessageBox.Show("Silmək üçün səbətdən yemək seçin!");
                return;
            }

            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            HesabiSifirla();

            MessageBox.Show($"{yemek.Ad} səbətdən silindi");
        }

        private void Yenile_Click(object sender, EventArgs e)
        {
            DialogResult cavab = MessageBox.Show(
                "Xanalar sıfırlansınmı?",
                "Yenilə",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (cavab != DialogResult.Yes)
                return;

            listBox1.Items.Clear();
            textBox2.Clear();
            HesabiSifirla();

            // textBox1 — Müddət: tapşırığa əsasən dəyişdirilmir.
        }

        private void YekunHesab_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("Səbətdə yemək yoxdur!");
                return;
            }

            decimal cem = 0;

            foreach (Yemek yemek in listBox1.Items)
                cem += yemek.Qiymet;

            yekunHesab = cem;
            textBox4.Text = cem.ToString("0.00");
            textBox3.Clear();
        }

        private void Hesabla_Click(object sender, EventArgs e)
        {
            textBox3.Clear();

            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("Səbətdə yemək yoxdur!");
                return;
            }

            if (!yekunHesab.HasValue)
            {
                MessageBox.Show("Əvvəlcə “Yekun hesab” düyməsinə basın!");
                return;
            }

            // Həm 10,50, həm də 10.50 qəbul edilir.
            string daxilEdilen = textBox2.Text.Trim().Replace(',', '.');

            if (!decimal.TryParse(
                    daxilEdilen,
                    NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture,
                    out decimal mebleg))
            {
                MessageBox.Show("Məbləği düzgün daxil edin!");
                textBox2.Focus();
                return;
            }

            if (mebleg < yekunHesab.Value)
            {
                MessageBox.Show("Daxil edilən məbləğ hesabdan azdır");
                return;
            }

            decimal qaliq = mebleg - yekunHesab.Value;
            textBox3.Text = qaliq.ToString("0.00");
        }

        private void Temizle_Click(object sender, EventArgs e)
        {
            // Yalnız Məbləğ və Qalıq təmizlənir.
            textBox2.Clear();
            textBox3.Clear();
            textBox2.Focus();
        }

        // Designer faylında bu hadisəyə istinad var.
        private void label1_Click_1(object sender, EventArgs e)
        {
        }
    }
}