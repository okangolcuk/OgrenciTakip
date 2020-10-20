using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;


namespace ÖğrenciTakip
{
    public partial class Form1 : Form
    {
        int secilenkisiid = -1;
        DataGridViewRow secilenkisininsatiri;
        public Form1()
        {
            InitializeComponent();
        }

        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=ÖğrenciKulüp.accdb");
        OleDbCommand komut = new OleDbCommand();
        OleDbCommand komut2 = new OleDbCommand();
        OleDbCommand komut3 = new OleDbCommand();
        OleDbCommand komut4 = new OleDbCommand();
        
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.CellClick += (object o, DataGridViewCellEventArgs k) => {
                if (k.RowIndex >= 0)
                {                   
                    string id = dataGridView1[0, k.RowIndex].Value.ToString();
                    textBox1.Text = dataGridView1[1, k.RowIndex].Value.ToString();
                    textBox2.Text = dataGridView1[2, k.RowIndex].Value.ToString();
                    comboBox1.SelectedIndex = comboBox1.FindString(dataGridView1[4, k.RowIndex].Value.ToString());
                    comboBox2.SelectedIndex = comboBox2.FindString(dataGridView1[3, k.RowIndex].Value.ToString());
                    comboBox3.SelectedIndex = comboBox3.FindString(dataGridView1[5, k.RowIndex].Value.ToString());
                    secilenkisiid = Int32.Parse(id);
                    secilenkisininsatiri = dataGridView1.Rows[k.RowIndex];
                }
                else {
                    MessageBox.Show("Hatalı Seçim yaptınız");                   
                }
            }; 

            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("Select kulup_ismi from kulup", baglanti);
            OleDbDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                comboBox1.Items.Add(dr[0]);
            }
            baglanti.Close();
        
            baglanti.Open();
            OleDbCommand komut2 = new OleDbCommand("Select bolum_ismi from bolum", baglanti);
            OleDbDataReader dra2 = komut2.ExecuteReader();
            while (dra2.Read())
            {
                comboBox2.Items.Add(dra2[0]);
            }
            baglanti.Close();

            baglanti.Open();
            OleDbCommand komut3 = new OleDbCommand("Select danisman_ismi from danisman", baglanti);
            OleDbDataReader dra3 = komut3.ExecuteReader();
            while (dra3.Read())
            {
                comboBox3.Items.Add(dra3[0]);
            }
            baglanti.Close();
            baglanti.Open();
            OleDbCommand komut5 = new OleDbCommand("Select * from Ogrenci", baglanti);
            OleDbDataReader dra5 = komut5.ExecuteReader();
            while (dra5.Read())
            {
                string id = dra5[0].ToString();
                string adi = dra5[1].ToString();
                string soyadi = dra5[2].ToString();
                string bolum = dra5[3].ToString();
                string kulup = dra5[4].ToString();
                string danisman = dra5[5].ToString();

                kulup = comboBox1.Items[Int32.Parse(kulup)-1].ToString();
                bolum = comboBox2.Items[Int32.Parse(bolum) - 1].ToString();
                danisman = comboBox3.Items[Int32.Parse(danisman) - 1].ToString();

                dataGridView1.Rows.Add(id,adi,soyadi,bolum,kulup,danisman);
             
            }
            baglanti.Close();
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglan();
        }
        void baglan()
        {
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            else
            {
                baglanti.Close();
                baglanti.Open();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "" && textBox2.Text.Trim() != "")
            {
                string Adi = textBox1.Text.Trim();
                string Soyadi = textBox2.Text.Trim();
                int Kulup = comboBox1.SelectedIndex+1;
                int Bolumu = comboBox2.SelectedIndex+1;
                int danisman = comboBox3.SelectedIndex+1;
                
                baglan();
                komut4.Connection = baglanti;
                komut4.CommandText = "INSERT INTO Ogrenci (ogr_adi,ogr_soyadi,bolumu,kulubu,danisman) VALUES ('" + Adi + "'," +
                    "'" + Soyadi + "','" + Bolumu + "','" + Kulup + "','" + danisman + "')";
                komut4.ExecuteNonQuery();
                komut4.Dispose();
                baglanti.Close();
                
                MessageBox.Show("Veri başarı ile eklendi.");
                guncelle();
            }   
        } 
        void guncelle()
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("SELECT TOP 1 * FROM Ogrenci ORDER BY id DESC", baglanti);
            OleDbDataReader dra6 = komut.ExecuteReader();
            while (dra6.Read())
            {
                string id = dra6[0].ToString();
                string adi = dra6[1].ToString();
                string soyadi = dra6[2].ToString();
                string bolum = dra6[3].ToString();
                string kulup = dra6[4].ToString();
                string danisman = dra6[5].ToString();

                kulup = comboBox1.Items[Int32.Parse(kulup) - 1].ToString();
                bolum = comboBox2.Items[Int32.Parse(bolum) - 1].ToString();
                danisman = comboBox3.Items[Int32.Parse(danisman) - 1].ToString();
                dataGridView1.Rows.Add(id, adi, soyadi, bolum, kulup, danisman);               
            }
            baglanti.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {           
            //OleDbCommand komut = new OleDbCommand(" DELETE FROM ogrenci WHERE id = alan_degeri", baglanti);
            // dataGridView1.Rows.Remove(alan_degeri);
            // dataGridView1.SelectedCells.Count.ToString();
            List<int> rid = new List<int>();
            foreach (DataGridViewCell drow in dataGridView1.SelectedCells)  //Seçili Satırlar
            {
                int id = Int32.Parse(dataGridView1[0, drow.RowIndex].Value.ToString());
                if (rid.Contains(id))
                {
                    continue;
                }
                dataGridView1.Rows.RemoveAt(drow.RowIndex);
                rid.Add(id);              
                KayıtSil(id);
            }
        }        
        void KayıtSil(int numara)
        {
            MessageBox.Show(numara.ToString());
            OleDbCommand komut = new OleDbCommand("DELETE FROM Ogrenci WHERE id=" + numara, baglanti);
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kayıt silindi");
        }

        private void button3_Click(object sender, EventArgs e)
        {
           if(textBox1.Text == string.Empty || textBox2.Text == string.Empty || comboBox1.Text == string.Empty || comboBox2.Text == string.Empty || comboBox3.Text == string.Empty)
            {
                MessageBox.Show("Düzenleme Yapılamadı.");
            }
           else
            {
                baglanti.Open();          
                string adi = "ogr_adi='" + textBox1.Text + "'";
                string soyadi = "ogr_soyadi='" + textBox2.Text + "'";
                string bolumu = "bolumu=" + (comboBox2.SelectedIndex + 1);
                string kulubu = "kulubu=" + (comboBox1.SelectedIndex + 1);
                string danisman = "danisman=" + (comboBox3.SelectedIndex + 1);

                string adsd = "UPDATE Ogrenci SET " + adi + ", " + soyadi + ", " + bolumu + ", " + kulubu + ", " + danisman;
                adsd += " WHERE id=" + secilenkisiid;
               
                OleDbCommand komut = new OleDbCommand(adsd, baglanti);
               
                komut.ExecuteNonQuery();
                komut.Dispose();
                baglanti.Close();

                secilenkisininsatiri.SetValues(secilenkisiid,textBox1.Text, textBox2.Text,comboBox2.SelectedItem , comboBox1.SelectedItem, comboBox3.SelectedItem);
                MessageBox.Show("Güncelleme başarı ile gerçekleşti.");
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string vara, cumle;
            vara = textBox3.Text.ToLower();

            List<int> kulupIDleri = new List<int>();
            List<int> bolumIDleri = new List<int>();
            List<int> danismanIDleri = new List<int>();
            
            for (int i = 0; i < comboBox1.Items.Count; ++i)
                if (comboBox1.Items[i].ToString().ToLower().Contains(vara))
                    kulupIDleri.Add(i + 1);

            for (int i = 0; i < comboBox2.Items.Count; ++i)
                if (comboBox2.Items[i].ToString().ToLower().Contains(vara))
                    bolumIDleri.Add(i + 1);

            for (int i = 0; i < comboBox3.Items.Count; ++i)
                if (comboBox3.Items[i].ToString().ToLower().Contains(vara))
                    danismanIDleri.Add(i + 1);

            cumle = "Select * from Ogrenci";
            cumle += " where ogr_adi like '%" + vara + "%'"; // Adını aradığımız yer
            cumle += " or ogr_soyadi like '%" + vara + "%'"; // Soyadını aradığımız yer

            // Tüm Kulüp ID'leri için, Access'te idleri göndermemiz lazım.
            for (int i = 0; i < kulupIDleri.Count; ++i)
            {
                cumle += " or kulubu like '%" + kulupIDleri[i] + "%'";
            }

            // Tüm Bölüm ID'leri için, Access'te idleri göndermemiz lazım.
            for (int i = 0; i < bolumIDleri.Count; ++i)
            {
                cumle += " or bolumu like '%" + bolumIDleri[i] + "%'";
            }

            // Tüm Danışman ID'leri için, Access'te idleri göndermemiz lazım.
            for (int i = 0; i < danismanIDleri.Count; ++i)
            {
                cumle += " or danisman like '%" + danismanIDleri[i] + "%'";
            }

            
            baglanti.Open();      
            OleDbCommand komut = new OleDbCommand(cumle, baglanti);
            OleDbDataReader dre = komut.ExecuteReader();
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            

            while (dre.Read()) {
                string id = dre[0].ToString();
                string adi = dre[1].ToString();
                string soyadi = dre[2].ToString();
                string bolum = dre[3].ToString();
                string kulup = dre[4].ToString();
                string danisman = dre[5].ToString();

                kulup = comboBox1.Items[Int32.Parse(kulup) - 1].ToString();
                bolum = comboBox2.Items[Int32.Parse(bolum) - 1].ToString();
                danisman = comboBox3.Items[Int32.Parse(danisman) - 1].ToString();
                dataGridView1.Rows.Add(id, adi, soyadi, bolum, kulup, danisman);
                
            }
            komut.Dispose(); 
            baglanti.Close();
            
        }
    }
  }
