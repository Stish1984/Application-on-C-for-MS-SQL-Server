using System;
using System.Linq;
using System.Windows.Forms;
using Sales_registration.Managers;

namespace Sales_registration
{
    public partial  class Form4 : Form
    {
        int i = 0;
        IRepository db;

        public   Form4()
        {
            db = new SQLRepository();
           
                InitializeComponent();
            try
            {
                object table = db.GetList().ToList();      //Загрузка базы менеджеров
                comboBox1.DataSource = table;
                comboBox1.DisplayMember = "Login";
                comboBox1.ValueMember = "Password";
            }
            catch
            {
                MessageBox.Show("Не удалось подключиться к базе данных");
            }
        }

        public void button1_Click(object sender, EventArgs e)        //Проверка соответствия логина-пароля
        {
            i++;

            Manager manager = (Manager)comboBox1.SelectedItem;
            string pass = GetPass(comboBox1.SelectedValue.ToString());  //Дешифровка пароля

            while (i <= 3)
            {
                if (pass != textBox2.Text && i<3)
                {
                    MessageBox.Show($"Не верный логин или пароль! Осталось попыток:{3-i}! ");
                    textBox2.Clear();
                    return;
                }

                else if (pass != textBox2.Text && i==3)
                {
                    manager.Blocking = true;
                    db.Save();
                }
                break;
            }

            if (manager.Blocking == true)                          //Блокировка учётной записи
            {
                MessageBox.Show("Ваша учетная запись заблокирована! Пожалуйста, обратитесь к администратору.");
                Application.Exit();
            }

            if (manager.Roots == true)
                MessageBox.Show("Вы вошли как администратор");
            else
                MessageBox.Show("Вы вошли как пользователь");

            Form1 fm1 = new Form1(this);
            fm1.Show();
            this.Hide();
        }

        internal string GetPass(string password)
        {
            return Crypter.Decrypt(password);
        }

        private void button2_Click(object sender, EventArgs e)     //Выход
        {
            Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)   //Показать-скрыть пароль
        {
            if (textBox2.UseSystemPasswordChar == false)
                textBox2.UseSystemPasswordChar = true;
            else
                textBox2.UseSystemPasswordChar = false;
        }
    }
}
