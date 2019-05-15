using System;
using System.Linq;
using System.Windows.Forms;
using Sales_registration.Sales;
using System.Linq.Dynamic;
using System.Data.Entity;



namespace Sales_registration
{
    public partial class Form1 : Form
    {
        ISale db;
        Form4 mainForm = null;

        public Form1(Form4 main)
        {
            db = new SaleRepositopy();
            InitializeComponent();

            LoadData();

            mainForm = main;
            Manager manager = main.comboBox1.SelectedItem as Manager;    //Текущий менеджер
            label2.Text = manager.FirstName;
            if (manager.Roots == false)
                button5.Hide();
        }

        private void LoadData()                                //Загрузка всей таблицы
        {
            dataGridView1.DataSource = db.GetList().ToList();
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Дата";
            dataGridView1.Columns[2].HeaderText = "Организация";
            dataGridView1.Columns[3].HeaderText = "Город";
            dataGridView1.Columns[4].HeaderText = "Страна";
            dataGridView1.Columns[5].HeaderText = "Менеджер";
            dataGridView1.Columns[6].HeaderText = "Количество";
            dataGridView1.Columns[7].HeaderText = "Сумма";
        }

        private void button1_Click(object sender, EventArgs e)       //Динамический выбор полей таблицы
        {                                                            
            var query = SaleQuery.GroupByColumns(db.GetList() as IQueryable, checkBox1.Checked,
                 checkBox2.Checked, checkBox3.Checked, checkBox4.Checked,
                 checkBox5.Checked).ToListAsync().Result;

            dataGridView1.DataSource = query.ToList();
            
            checkBox1.Enabled = false;
            checkBox2.Enabled = false;
            checkBox3.Enabled = false;
            checkBox4.Enabled = false;
            checkBox5.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)      //Кнопка сбросить фильтр
        {
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;

            checkBox1.Enabled = true;
            checkBox2.Enabled = true;
            checkBox3.Enabled = true;
            checkBox4.Enabled = true;
            checkBox5.Enabled = true;

            dataGridView1.Columns.Clear();
            LoadData();
        }

        private void button3_Click(object sender, EventArgs e)    //Выход 
        {
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)     //Форма редактирования таблицы менеджеров
        {
            Form2 fm2 = new Form2();
            fm2.FormClosed += fm2_FormClosed;
            fm2.Show();
            this.Hide();
        }

        void fm2_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }

        private void button4_Click(object sender, EventArgs e)        //Форма авторизации
        {
            Form4 fm4 = new Form4();
            fm4.Show();
            this.Close();
        }

        public void button6_Click(object sender, EventArgs e)              //Добавление записи
        {
            try
            {
                Form5 edit = new Form5();
                edit.Text = "Форма регистрации отгрузки";
                edit.label7.Text = label2.Text;
                edit.label9.Text = DateTime.Now.ToShortDateString();
                DialogResult res = edit.ShowDialog(this);

                string man = edit.label7.Text;

                if (res == DialogResult.Cancel)
                    return;

                if (String.IsNullOrEmpty(edit.textBox1.Text) || String.IsNullOrEmpty(edit.textBox2.Text) ||   
                String.IsNullOrEmpty(edit.textBox4.Text) || String.IsNullOrEmpty(edit.textBox5.Text))
                {
                    MessageBox.Show("Поля со * обязательны к заполнению!");
                    return;
                }

                Sale sale = new Sale();
                sale.Date = DateTime.Now;
                sale.Organization = edit.textBox1.Text;
                sale.City = edit.textBox2.Text;
                sale.Country = edit.textBox3.Text;
                sale.Manager = edit.label7.Text;
                sale.Quantity = Convert.ToInt32(edit.textBox4.Text);
                sale.Sum = Convert.ToDecimal(edit.textBox5.Text);

                var qThis = from data in db.GetList().ToList()
                        where data.Manager.Contains(man)
                        where data.Date.Month == DateTime.Today.Month
                        group data by new { data.Date }
                            into result
                        select new
                        {
                            result.Key.Date
                        };
                var qQThis = qThis.Count();

                var qSThis = from data in db.GetList().ToList()
                         where data.Manager.Contains(man)
                         where data.Date.Month == DateTime.Today.Month
                         group data by new { data.Manager }
                         into result
                         select new
                         {
                             S = result.Sum(i => i.Sum)
                         };

                decimal countThis = Convert.ToDecimal(qQThis);
                string qSumThis = qSThis.First().ToString();
                qSumThis = qSumThis.Substring(5, qSumThis.Length - 6);
                decimal sumThis = Decimal.Parse(qSumThis);
                decimal middleSumThis = sumThis / countThis;
                middleSumThis = middleSumThis + 500;

                var qThat = from data in db.GetList().ToList()
                            where data.Manager.Contains(man)
                            where data.Date.Month == DateTime.Today.AddMonths(-1).Month
                            group data by new { data.Date }
                            into result
                            select new
                            {
                                result.Key.Date
                            };
                var qQThat = qThat.Count();

                var qSThat = from data in db.GetList().ToList()
                             where data.Manager.Contains(man)
                             where data.Date.Month == DateTime.Today.AddMonths(-1).Month
                             group data by new { data.Manager }
                           into result
                             select new
                             {
                                 S = result.Sum(i => i.Sum)
                             };

                decimal countThat = Convert.ToDecimal(qQThat);
                string qSumThat = qSThat.First().ToString();
                qSumThat = qSumThat.Substring(5, qSumThat.Length - 6);
                decimal sumThat = Decimal.Parse(qSumThat);
                decimal middleSumThat = sumThat / countThat;
                middleSumThat = middleSumThat + 500;

                if ((middleSumThis < sale.Sum || middleSumThat < sale.Sum) && middleSumThis != 0 && middleSumThat != 0)
                {
                    MessageBox.Show("Превышен лимит суммы отгрузки!");
                    return;
                }

                db.Create(sale);
                db.Save();
                LoadData();
            }
            catch
            {
                MessageBox.Show("Что-то пошло не так! Попробуйте сначала");
            }
        }

    }

}

