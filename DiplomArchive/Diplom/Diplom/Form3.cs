using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Diplom
{
    public partial class Form3 : Form
    {
        private string connectionString = "Server=localhost;Database=delivery_system;Uid=root;Pwd=;";
        private DataTable ordersTable;
        private bool isEditMode = false;
        private DataRow currentOrderData;
        private int currentOrderId = -1;

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            SetFieldsReadOnly(true);
            guna2Button1.Visible = false;
            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT o.id, 
                               COALESCE(u.full_name,'-') as courier, 
                               CASE 
                                   WHEN o.products_list IS NOT NULL AND o.products_list != '' THEN o.products_list
                                   WHEN p.name IS NOT NULL THEN p.name
                                   ELSE '-' 
                               END as product,
                               o.address, 
                               o.lat, 
                               o.lng, 
                               o.status
                        FROM orders o
                        LEFT JOIN couriers c ON o.courier_id = c.id
                        LEFT JOIN users u ON c.user_id = u.id
                        LEFT JOIN products p ON o.product_id = p.id
                        ORDER BY o.id DESC";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    ordersTable = new DataTable();
                    adapter.Fill(ordersTable);

                    guna2DataGridView1.DataSource = ordersTable;

                    // Настройка заголовков столбцов
                    if (guna2DataGridView1.Columns["id"] != null)
                    {
                        guna2DataGridView1.Columns["id"].HeaderText = "ID";
                        guna2DataGridView1.Columns["id"].Visible = false;
                    }
                    if (guna2DataGridView1.Columns["courier"] != null)
                        guna2DataGridView1.Columns["courier"].HeaderText = "КУРЬЕР";
                    if (guna2DataGridView1.Columns["product"] != null)
                        guna2DataGridView1.Columns["product"].HeaderText = "ТОВАРЫ";
                    if (guna2DataGridView1.Columns["address"] != null)
                        guna2DataGridView1.Columns["address"].HeaderText = "АДРЕС ДОСТАВКИ";
                    if (guna2DataGridView1.Columns["lat"] != null)
                        guna2DataGridView1.Columns["lat"].HeaderText = "ШИРОТА";
                    if (guna2DataGridView1.Columns["lng"] != null)
                        guna2DataGridView1.Columns["lng"].HeaderText = "ДОЛГОТА";
                    if (guna2DataGridView1.Columns["status"] != null)
                        guna2DataGridView1.Columns["status"].HeaderText = "СТАТУС";

                    guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка загрузки: {ex.Message}");
            }
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];
                currentOrderId = Convert.ToInt32(row.Cells["id"].Value);
                LoadFullOrderData(currentOrderId);
            }
        }

        private void LoadFullOrderData(int orderId)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT o.id, 
                               COALESCE(u.full_name,'-') as courier, 
                               CASE 
                                   WHEN o.products_list IS NOT NULL AND o.products_list != '' THEN o.products_list
                                   WHEN p.name IS NOT NULL THEN p.name
                                   ELSE '-' 
                               END as product,
                               o.address, 
                               o.lat, 
                               o.lng, 
                               o.status
                        FROM orders o
                        LEFT JOIN couriers c ON o.courier_id = c.id
                        LEFT JOIN users u ON c.user_id = u.id
                        LEFT JOIN products p ON o.product_id = p.id
                        WHERE o.id = @id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", orderId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                currentOrderData = ordersTable.NewRow();
                                currentOrderData["id"] = reader["id"];
                                currentOrderData["courier"] = reader["courier"];
                                currentOrderData["product"] = reader["product"];
                                currentOrderData["address"] = reader["address"];
                                currentOrderData["lat"] = reader["lat"];
                                currentOrderData["lng"] = reader["lng"];
                                currentOrderData["status"] = reader["status"];

                                FillFieldsFromData();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка загрузки заказа: {ex.Message}");
            }
        }

        private void FillFieldsFromData()
        {
            if (currentOrderData == null)
            {
                ClearFields();
                return;
            }

            guna2TextBox2.Text = currentOrderData["courier"]?.ToString() ?? "";      // Курьер
            guna2TextBox3.Text = currentOrderData["product"]?.ToString() ?? "";      // Товар
            guna2TextBox4.Text = currentOrderData["address"]?.ToString() ?? "";      // Адрес
            guna2TextBox5.Text = currentOrderData["lat"]?.ToString() ?? "";          // Широта
            guna2TextBox6.Text = currentOrderData["lng"]?.ToString() ?? "";          // Долгота
            guna2TextBox7.Text = currentOrderData["status"]?.ToString() ?? "";       // Статус
        }

        private void ClearFields()
        {
            guna2TextBox2.Text = "";
            guna2TextBox3.Text = "";
            guna2TextBox4.Text = "";
            guna2TextBox5.Text = "";
            guna2TextBox6.Text = "";
            guna2TextBox7.Text = "";
        }

        private void SetFieldsReadOnly(bool readOnly)
        {
            guna2TextBox2.ReadOnly = readOnly;
            guna2TextBox3.ReadOnly = readOnly;
            guna2TextBox4.ReadOnly = readOnly;
            guna2TextBox5.ReadOnly = readOnly;
            guna2TextBox6.ReadOnly = readOnly;
            guna2TextBox7.ReadOnly = readOnly;

            Color color = readOnly ? Color.FromArgb(220, 220, 220) : Color.FromArgb(240, 240, 240);
            guna2TextBox2.FillColor = color;
            guna2TextBox3.FillColor = color;
            guna2TextBox4.FillColor = color;
            guna2TextBox5.FillColor = color;
            guna2TextBox6.FillColor = color;
            guna2TextBox7.FillColor = color;
        }

        // КНОПКА "РЕДАКТИРОВАТЬ" / "СОХРАНИТЬ"
        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            if (!isEditMode)
            {
                if (currentOrderId == -1)
                {
                    MessageBox.Show("❌ Сначала выберите заказ!");
                    return;
                }

                isEditMode = true;
                SetFieldsReadOnly(false);
                guna2GradientButton1.Text = "СОХРАНИТЬ";
                guna2Button1.Visible = true;
            }
            else
            {
                SaveOrderChanges();
                isEditMode = false;
                SetFieldsReadOnly(true);
                guna2GradientButton1.Text = "РЕДАКТИРОВАТЬ";
                guna2Button1.Visible = false;
                LoadOrders();                    // Обновляем таблицу
                LoadFullOrderData(currentOrderId); // Обновляем поля
            }
        }

        // КНОПКА "ОТМЕНА"
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            isEditMode = false;
            SetFieldsReadOnly(true);
            guna2GradientButton1.Text = "РЕДАКТИРОВАТЬ";
            guna2Button1.Visible = false;

            if (currentOrderData != null)
                FillFieldsFromData();
            else
                ClearFields();
        }

        private void SaveOrderChanges()
        {
            if (currentOrderId == -1)
            {
                MessageBox.Show("❌ Выберите заказ!");
                return;
            }

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        UPDATE orders SET 
                            address = @address,
                            lat = @lat,
                            lng = @lng,
                            status = @status,
                            products_list = @products_list
                        WHERE id = @id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", currentOrderId);
                        cmd.Parameters.AddWithValue("@address", guna2TextBox4.Text);
                        cmd.Parameters.AddWithValue("@lat", double.Parse(string.IsNullOrEmpty(guna2TextBox5.Text) ? "0" : guna2TextBox5.Text));
                        cmd.Parameters.AddWithValue("@lng", double.Parse(string.IsNullOrEmpty(guna2TextBox6.Text) ? "0" : guna2TextBox6.Text));
                        cmd.Parameters.AddWithValue("@status", guna2TextBox7.Text);
                        cmd.Parameters.AddWithValue("@products_list", guna2TextBox3.Text);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("✅ Заказ успешно обновлен!");
                        }
                        else
                            MessageBox.Show("❌ Заказ не найден!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка сохранения: {ex.Message}");
            }
        }

        // Кнопка ОБНОВИТЬ
        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            LoadOrders();
            isEditMode = false;
            SetFieldsReadOnly(true);
            guna2GradientButton1.Text = "РЕДАКТИРОВАТЬ";
            guna2Button1.Visible = false;
            currentOrderData = null;
            currentOrderId = -1;
            ClearFields();
        }

        private void guna2TileButton4_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void guna2TileButton3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}