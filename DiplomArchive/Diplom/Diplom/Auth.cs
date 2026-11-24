using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Diplom
{
    public partial class Auth : Form
    {
        private string connectionString = "Server=localhost;Database=delivery_system;Uid=root;Pwd=;";

        public Auth()
        {
            InitializeComponent();
            guna2TextBox2.PasswordChar = '*'; // Скрыть пароль
            guna2TextBox2.UseSystemPasswordChar = true;
        }

        private void guna2Button1_Click(object sender, EventArgs e) // КНОПКА ВХОД
        {
            string login = guna2TextBox1.Text.Trim();
            string password = guna2TextBox2.Text.Trim();

            // Проверка пустых полей
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                label5.Text = "Заполни логин и пароль!";
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // ПОИСК ПО ЛОГИНУ
                    string checkQuery = "SELECT id, password_hash, full_name, role_id FROM users WHERE login = @login AND is_active = 1";
                    using (MySqlCommand cmd = new MySqlCommand(checkQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@login", login);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string dbPassword = reader.GetString("password_hash");
                                string fullName = reader.GetString("full_name");
                                int roleId = reader.GetInt32("role_id");

                                // ✅ ПРОВЕРКА ПАРОЛЯ
                                if (password == dbPassword)
                                {
                                    // УСПЕХ! Переход по роли
                                    Form1 mainForm = new Form1();
                                    mainForm.Tag = fullName; // Имя пользователя
                                    mainForm.SetUserRole(roleId); // Передаем роль
                                    mainForm.Show();

                                    this.Hide(); // Скрыть авторизацию
                                    return;
                                }
                                else
                                {
                                    // ❌ НЕВЕРНЫЙ ПАРОЛЬ (логин правильный)
                                    label5.Text = "Неверный пароль!";
                                    guna2TextBox2.Clear(); // Очистить ТОЛЬКО пароль
                                    guna2TextBox2.Focus();
                                }
                            }
                            else
                            {
                                // ❌ ЛОГИН НЕ НАЙДЕН
                                label5.Text = "Неверный логин или пароль!";
                                guna2TextBox1.Clear(); // Очистить ЛОГИН
                                guna2TextBox2.Clear(); // Очистить ПАРОЛЬ
                                guna2TextBox1.Focus();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                label5.Text = "❌ Ошибка подключения к БД!";
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Получить ID курьера по логину
        private int GetCourierId(MySqlConnection conn, string login)
        {
            string query = @"SELECT c.id FROM couriers c 
                           JOIN users u ON c.user_id = u.id 
                           WHERE u.login = @login";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@login", login);
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        private void guna2TileButton4_Click(object sender, EventArgs e) // КНОПКА ВЫХОД
        {
            Application.Exit();
        }

        // ENTER в логине → вход
        private void guna2TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                guna2Button1_Click(null, null);
        }

        // ENTER в пароле → вход
        private void guna2TextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                guna2Button1_Click(null, null);
        }

        private void Auth_Load(object sender, EventArgs e)
        {

        }

        private void guna2TileButton3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2ShadowPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void guna2ShadowPanel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2ShadowPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2TileButton2_Click(object sender, EventArgs e)
        {

        }

        private void guna2TileButton1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click_1(object sender, EventArgs e)
        {

        }
    }
}