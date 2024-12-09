using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace App
{
    public partial class Form1 : Form
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

        public Form1()
        {
            InitializeComponent();
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Добавляем продукт в список
            products.Add(new Product("Прибор 1", "11.01.24", "00.00.01"));

            // Создаем DataTable и добавляем колонки
            DataTable table = new DataTable();
            table.Columns.Add("Название", typeof(string));
            table.Columns.Add("Дата проверки", typeof(string));
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

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, изменена ли дата проверки или периодичность проверки
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2) // Индексы колонок "Дата проверки" и "Периодичность"
            {
                UpdateNextCheckDate(e.RowIndex);
            }
        }

        private void UpdateNextCheckDate(int rowIndex)
        {
            var table = (DataTable)dataGridView1.DataSource;

            // Получаем значения из ячеек
            string proverkaStr = table.Rows[rowIndex][1].ToString();
            string periodStr = table.Rows[rowIndex][2].ToString();

            DateTime proverkaDate;
            if (DateTime.TryParse(proverkaStr, out proverkaDate))
            {
                // Предположим, что периодичность вводится в формате "дд.чч.мм"
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
            // Ваш код обработки клика по ячейке
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
    }
}