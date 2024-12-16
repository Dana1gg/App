using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App
{
    public partial class app : Form
    {
        public struct Product
        {
            public string Name;
            public string Proverka;
            public string PeriodProverki;
            public string NextProverka;

            public Product(string _name, string _proverka, string _periodProverki)
            {
                Name = _name;
                Proverka = _proverka;
                PeriodProverki = _periodProverki;
                NextProverka = "00.00.00";
            }

            public Product(string _name)
            {
                Name = _name;
                Proverka = "00.00.00";
                PeriodProverki = "00.00.00";
                NextProverka = "00.00.00";
            }
        }

        List<Product> products = new List<Product>();

        public app()
        {
            InitializeComponent();
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            DataTable table = new DataTable();
            table.Columns.Add("Название", typeof(string));
            table.Columns.Add("Дата поверки", typeof(string));
            table.Columns.Add("Периодичность поверки", typeof(string));
            table.Columns.Add("Дата следующей поверки", typeof(string));

            // Заполняем DataTable данными из списка продуктов
            foreach (var product in products)
            {
                table.Rows.Add(product.Name, product.Proverka, product.PeriodProverki, product.NextProverka);
            }

            // Устанавливаем DataTable как источник данных для DataGridView
            dataGridView1.DataSource = table;

            // Разрешаем редактирование ячеек
            dataGridView1.ReadOnly = false; 
        }

        private async void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            if (await IsRowFilled(e.RowIndex))
            {
                UpdateNextCheckDate(e.RowIndex);
            }
        }

        private Task<bool> IsRowFilled(int rowIndex)
        {
            return Task.Run(() =>
            {
                var table = (DataTable)dataGridView1.DataSource;

                if (table == null || rowIndex < 0 || rowIndex >= table.Rows.Count)
                {
                    return false;
                }

                // Проверяем, заполнены ли ячейки "Дата проверки" и "Периодичность"
                return table.Rows[rowIndex][1] != DBNull.Value && table.Rows[rowIndex][2] != DBNull.Value;
            });
        }

        private void UpdateNextCheckDate(int rowIndex)
        {
            var table = (DataTable)dataGridView1.DataSource;


            if (table.Rows[rowIndex][1] == DBNull.Value || table.Rows[rowIndex][2] == DBNull.Value)
            {
                MessageBox.Show("Одно из значений (Дата проверки или Периодичность) отсутствует.");
                return;
            }

            // Получаем значения из ячеек
            string proverkaStr = table.Rows[rowIndex][1].ToString(); 
            string periodStr = table.Rows[rowIndex][2].ToString();



            DateTime proverkaDate;
            if (DateTime.TryParse(proverkaStr, out proverkaDate))
            {
                var periodParts = periodStr.Split('.');
                if (periodParts.Length == 3 &&
                    int.TryParse(periodParts[0], out int days) &&
                    int.TryParse(periodParts[1], out int months) &&
                    int.TryParse(periodParts[2], out int years))
                {
                    // Вычисляем следующую дату проверки
                    DateTime nextCheckDate = proverkaDate.AddDays(days).AddMonths(months).AddYears(years);
                    table.Rows[rowIndex][3] = nextCheckDate.ToString("dd.MM.yy"); // Обновляем дату следующей проверки
                }
                else
                {
                    MessageBox.Show("Неверный формат. Используйте 'дд.чч.мм'.");
                }
            }
            else
            {
                MessageBox.Show("Неверный формат даты проверки.");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CloseButton_MouseEnter(object sender, EventArgs e)
        {
        }

        Point lastPoint;
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void CloseButton_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_minimized_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}