using System;
using System.Linq;
using System.Windows.Forms;
using Sales_registration.Managers;


namespace Sales_registration
{
    public partial class Form2 : Form
    {
        string password;
        IRepository db;

        public Form2()
        {
            db = new SQLRepository();

            InitializeComponent();

            dataGridView1.DataSource = db.GetList().ToList();

            dataGridView1.Columns[1].HeaderText = "Фамилия";
            dataGridView1.Columns[2].HeaderText = "Имя";
            dataGridView1.Columns[3].HeaderText = "Отчество";
            dataGridView1.Columns[4].HeaderText = "Логин";
            dataGridView1.Columns[5].HeaderText = "Пароль";
            dataGridView1.Columns[6].HeaderText = "Права администратора";
            dataGridView1.Columns[7].HeaderText = "Блокировка аккаунта";
        }

        private void button3_Click(object sender, EventArgs e)      // Удаление записи из базы данных менеджеров
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Невозможно удалить запись!");
                return;
            }

            int ID = Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value);
            Manager manager = db.GetManager(ID);
            Form3 edit = new Form3();
            edit.Text = "Удаление записи";

            edit.textBox1.Text = manager.FirstName;
            edit.textBox2.Text = manager.LastName;
            edit.textBox3.Text = manager.Patronymic;
            edit.textBox4.Text = manager.Login;
            edit.checkBox1.Checked = manager.Roots;
            edit.checkBox2.Checked = manager.Blocking;

            edit.textBox5.Text = GetPass(manager.Password);
            edit.button1.Text = "Удалить";
            MessageBox.Show("Вы точно хотите удалить запись?");
            DialogResult result = edit.ShowDialog(this);

            if (result == DialogResult.Cancel)
                return;

            db.Delete(ID);
            db.Save();
            dataGridView1.DataSource = db.GetList().ToList();
            MessageBox.Show("Запись удалена");
        }

        public void button1_Click(object sender, EventArgs e)       //Добавление записи в базу данных менеджеров
        {
            Form3 edit = new Form3();
            edit.Text = "Добавление менеджера";
            DialogResult result = edit.ShowDialog(this);

            if (result == DialogResult.Cancel)
                return;

            Manager manager = new Manager();
            
            manager.FirstName = edit.textBox1.Text;
            manager.LastName = edit.textBox2.Text;
            manager.Patronymic = edit.textBox3.Text;
            manager.Login = edit.textBox4.Text;
            manager.Roots = edit.checkBox1.Checked;
            manager.Blocking = edit.checkBox2.Checked;

            SetPass(edit.textBox5.Text);
            manager.Password = password;
          
            if (String.IsNullOrEmpty(edit.textBox1.Text) || String.IsNullOrEmpty(edit.textBox2.Text) ||   //Проверка заполнения полей 
             String.IsNullOrEmpty(edit.textBox4.Text) || String.IsNullOrEmpty(edit.textBox5.Text))
            {
                MessageBox.Show("Поля со * обязательны к заполнению!");
                return;
            }

            for (int i = 0; i < dataGridView1.RowCount; i++)          //Проверка уникальности вводимого логина
            {
                if (dataGridView1[4, i].Value.ToString() == edit.textBox4.Text)
                {
                    MessageBox.Show("Данный логин уже используется!");
                    return;
                }
            }

            db.Create(manager);
            db.Save();

            MessageBox.Show("Новый менеджер добавлен");
            dataGridView1.DataSource = db.GetList().ToList();
        }

        private void button2_Click(object sender, EventArgs e)   //Редактирование записи в базе данных менеджеров
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Запись для редактирования не выбрана!");
                return;
            }

            int ID = Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value);
            Manager manager = db.GetManager(ID);
            Form3 edit = new Form3();
            edit.Text = "Редактирование записи";

            edit.textBox1.Text = manager.FirstName;
            edit.textBox2.Text = manager.LastName;
            edit.textBox3.Text = manager.Patronymic;
            edit.textBox4.Text = manager.Login;
            edit.checkBox1.Checked = manager.Roots;
            edit.checkBox2.Checked = manager.Blocking;

            edit.textBox5.Text = GetPass(manager.Password);

            DialogResult result = edit.ShowDialog(this);

            if (result == DialogResult.Cancel)
                return;

            manager.FirstName = edit.textBox1.Text;
            manager.LastName = edit.textBox2.Text;
            manager.Patronymic = edit.textBox3.Text;
            manager.Login = edit.textBox4.Text;
            manager.Roots = edit.checkBox1.Checked;
            manager.Blocking = edit.checkBox2.Checked;

            SetPass(edit.textBox5.Text);
            manager.Password = password;

            if (String.IsNullOrEmpty(edit.textBox1.Text) || String.IsNullOrEmpty(edit.textBox2.Text) ||   //Проверка заполнения полей 
                 String.IsNullOrEmpty(edit.textBox4.Text) || String.IsNullOrEmpty(edit.textBox5.Text))
            {
                MessageBox.Show("Поля со * обязательны к заполнению!");
                return;
            }

            db.Update(manager);
            db.Save();

            MessageBox.Show("Данные о менеджере обновлены");
            dataGridView1.DataSource = db.GetList().ToList();
        }

        internal string GetPass(string password)              //Шифрование и дешифровка пароля
        {
            return Crypter.Decrypt(password);
        }

        internal void SetPass(string pass)
        {
            password = Crypter.Encrypt(pass);
        }

        private void button4_Click(object sender, EventArgs e)    
        {
              this.Close();
        }

    }
}