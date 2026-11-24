using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Diplom
{
    public partial class Form1 : Form
    {
        private string connectionString = "Server=localhost;Database=delivery_system;Uid=root;Pwd=;";
        private int currentUserRole = 0;
        private int selectedCourierId = 0;
        private string selectedCourierName = "";

        public Form1()
        {
            InitializeComponent();
        }

        public void SetUserRole(int roleId)
        {
            currentUserRole = roleId;
            SetButtonsAccessibility();
        }

        private void SetButtonsAccessibility()
        {
            if (currentUserRole == 3) // Курьер
            {
                // Отключаем все кнопки
                guna2Button1.Enabled = false;
                guna2GradientButton1.Enabled = false;
                guna2GradientButton2.Enabled = false;
                guna2GradientButton3.Visible = false;

                // Отключаем клики по DataGridView для курьера
                guna2DataGridView2.Enabled = false;
                guna2DataGridView1.Enabled = false;
            }
            else // Для всех остальных ролей
            {
                // Включаем все кнопки
                guna2Button1.Enabled = true;
                guna2GradientButton1.Enabled = true;
                guna2GradientButton2.Enabled = true;
                guna2GradientButton3.Visible = false;

                // Включаем клики по DataGridView
                guna2DataGridView2.Enabled = true;
                guna2DataGridView1.Enabled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
            // Подписываемся на событие клика только если не курьер
            if (currentUserRole != 3)
            {
                guna2DataGridView2.CellClick += guna2DataGridView2_CellClick;
            }
        }

        // Обработчик клика по ячейке в DataGridView с занятыми курьерами
        private void guna2DataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Дополнительная проверка на роль (на всякий случай)
            if (currentUserRole == 3)
            {
                return;
            }

            // Проверяем, что клик не по заголовку и есть строки
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = guna2DataGridView2.Rows[e.RowIndex];

                // Получаем имя курьера (первая колонка)
                if (selectedRow.Cells[0].Value != null)
                {
                    selectedCourierName = selectedRow.Cells[0].Value.ToString();
                    GetCourierIdByName(selectedCourierName);

                    // Показываем кнопку
                    guna2GradientButton3.Visible = true;
                    guna2GradientButton3.Text = $"Освободить курьера";
                }
            }
        }

        private void GetCourierIdByName(string fullName)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT c.id 
                        FROM couriers c 
                        JOIN users u ON c.user_id = u.id 
                        WHERE u.full_name = @fullName";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fullName", fullName);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            selectedCourierId = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения ID курьера: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            if (selectedCourierId == 0)
            {
                MessageBox.Show("Выберите курьера для освобождения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show($"Освободить курьера '{selectedCourierName}'?\nОн сможет принимать новые заказы.",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        string updateQuery = "UPDATE couriers SET is_busy = 0 WHERE id = @courierId";
                        using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@courierId", selectedCourierId);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show($"Курьер '{selectedCourierName}' освобожден!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                guna2GradientButton3.Visible = false;
                                selectedCourierId = 0;
                                selectedCourierName = "";

                                LoadData();
                            }
                            else
                            {
                                MessageBox.Show("Не удалось освободить курьера!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при освобождении курьера: {ex.Message}", "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadData()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string freeQuery = @"
                        SELECT u.full_name 'Курьер', 
                               COALESCE((SELECT COUNT(*) FROM orders o WHERE o.courier_id = c.id AND o.status IN ('New','InProgress','pending')), 0) 'Заказов',
                               'Свободен' , 
                               c.vehicle_info 'Транспорт'
                        FROM couriers c 
                        JOIN users u ON c.user_id = u.id
                        WHERE c.is_active = 1 AND c.is_busy = 0";

                    DataTable dtFree = new DataTable();
                    new MySqlDataAdapter(freeQuery, conn).Fill(dtFree);
                    guna2DataGridView1.DataSource = dtFree;
                    guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    string busyQuery = @"
                        SELECT u.full_name 'Курьер', 
                               COALESCE((SELECT COUNT(*) FROM orders o WHERE o.courier_id = c.id AND o.status IN ('New','InProgress','pending')), 0) 'Заказов',
                               'Занят', 
                               c.vehicle_info 'Транспорт'
                        FROM couriers c 
                        JOIN users u ON c.user_id = u.id
                        WHERE c.is_active = 1 AND c.is_busy = 1";

                    DataTable dtBusy = new DataTable();
                    new MySqlDataAdapter(busyQuery, conn).Fill(dtBusy);
                    guna2DataGridView2.DataSource = dtBusy;
                    guna2DataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    
                    label3.Text = new MySqlCommand("SELECT COUNT(*) FROM orders WHERE status='InProgress'", conn).ExecuteScalar().ToString();

                    
                    label4.Text = new MySqlCommand("SELECT COUNT(*) FROM orders WHERE status='Delivered'", conn).ExecuteScalar().ToString();

                   
                    label5.Text = new MySqlCommand("SELECT COUNT(*) FROM orders WHERE status='pending'", conn).ExecuteScalar().ToString();

                    label6.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка БД: {ex.Message}\nПроверь подключение!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadData();
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            LoadData();
            MessageBox.Show("Данные обновлены!", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.Show();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }

        private void guna2TileButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2TileButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void guna2Panel1_Paint(object sender, PaintEventArgs e) { }
        private void guna2ShadowPanel2_Paint(object sender, PaintEventArgs e) { }
        private void guna2DataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e) { }

        private void guna2GradientButton2_MouseEnter(object sender, EventArgs e)
        {
            guna2GradientButton2.Image = Properties.Resources.refresh_red;
            guna2GradientButton2.FillColor = Color.FromArgb(255, 235, 235);
            guna2GradientButton2.FillColor2 = Color.FromArgb(255, 235, 235);
        }

        private void guna2GradientButton2_MouseLeave(object sender, EventArgs e)
        {
            guna2GradientButton2.Image = Properties.Resources.refresh;
            guna2GradientButton2.FillColor = Color.White;
            guna2GradientButton2.FillColor2 = Color.White;
        }
 private void guna2Button1_MouseEnter(object sender, EventArgs e)
        {
            guna2Button1.Image = Properties.Resources.task_red;
            guna2Button1.FillColor = Color.FromArgb(255, 235, 235);
           
        }

        private void guna2Button1_MouseLeave(object sender, EventArgs e)
        {
            guna2Button1.Image = Properties.Resources.task;
            guna2Button1.FillColor = Color.White;
           
        }
        private void guna2GradientButton1_MouseEnter(object sender, EventArgs e)
        {
            guna2GradientButton1.Image = Properties.Resources.add_red;
            guna2GradientButton1.FillColor = Color.FromArgb(255, 235, 235);
            guna2GradientButton1.FillColor2 = Color.FromArgb(255, 235, 235);
        }

        private void guna2GradientButton1_MouseLeave(object sender, EventArgs e)
        {
            guna2GradientButton1.Image = Properties.Resources.add;
            guna2GradientButton1.FillColor = Color.White;
            guna2GradientButton1.FillColor2 = Color.White;
        }

        private void guna2DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}