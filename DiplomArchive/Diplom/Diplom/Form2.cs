using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using Guna.UI2.WinForms;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.VisualBasic;
using MimeKit;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Diplom
{
    public partial class Form2 : Form
    {
        private GMapControl gmap;
        private GMarkerGoogle deliveryMarker;
        private string connectionString = "Server=localhost;Database=delivery_system;Uid=root;Pwd=;";
        private AutoCompleteStringCollection addressSuggestions = new AutoCompleteStringCollection();
        private double currentLat;
        private double currentLng;
        private string currentAddress;
        private Dictionary<string, int> selectedProductsWithQuantity = new Dictionary<string, int>();

        public Form2()
        {
            InitializeComponent();
        }

        private async void Form2_Load(object sender, EventArgs e)
        {
            labelCoords.ForeColor = Color.Black;
            //GMap.NET.MapProviders.GMapProvider.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36";


            LoadProductsToCheckedListBox();

            // Инициализация карты
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            gmap = new GMapControl();
            gmap.Dock = DockStyle.Fill;
            gmap.MapProvider = GMapProviders.OpenStreetMap;
            gmap.Position = new PointLatLng(45.035470, 38.975120);
            gmap.MinZoom = 1;
            gmap.MaxZoom = 18;
            gmap.Zoom = 15;
            gmap.MouseClick += Gmap_MouseClick;

            guna2Panel1.Controls.Add(gmap);

            AddDeliveryMarker(45.035470, 38.975120, "Краснодар, ул. Красная 15");
            LoadCouriersToComboBox();
            labelStatus.Text = "Готово!";
        }
        private async Task<string> GetAddressFromCoordinates(double lat, double lng)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = $"https://nominatim.openstreetmap.org/reverse?lat={lat.ToString(CultureInfo.InvariantCulture)}&lon={lng.ToString(CultureInfo.InvariantCulture)}&format=json&addressdetails=1";
                    var response = await client.GetStringAsync(url);
                    var json = JObject.Parse(response);

                    var address = json["address"];
                    string road = address?["road"]?.ToString() ?? "";
                    string houseNumber = address?["house_number"]?.ToString() ?? "";
                    string city = address?["city"]?.ToString() ?? address?["town"]?.ToString() ?? address?["village"]?.ToString() ?? "";

                    if (!string.IsNullOrEmpty(road) && !string.IsNullOrEmpty(city))
                    {
                        if (!string.IsNullOrEmpty(houseNumber))
                            return $"{city}, {road} {houseNumber}";
                        else
                            return $"{city}, {road}";
                    }
                }
            }
            catch { }

            return "Клик по карте";
        }
        private void LoadProductsToCheckedListBox()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT id, name FROM products ORDER BY name";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    checkedListBox1.Items.Clear();

                    foreach (DataRow row in dt.Rows)
                    {
                        string productName = row["name"].ToString();
                        checkedListBox1.Items.Add(productName, false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}", "Ошибка БД");
            }
        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkedListBox1.CheckedItems.Count == 0)
                {
                    MessageBox.Show("Пожалуйста, выберите хотя бы один товар!", "Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                foreach (var item in checkedListBox1.CheckedItems)
                {
                    string productName = item.ToString();

                    string quantityStr = Interaction.InputBox(
                        $"Введите количество для товара:\n{productName}",
                        "Количество товара",
                        "1");

                    if (string.IsNullOrEmpty(quantityStr))
                    {
                        continue;
                    }

                    int quantity;
                    if (!int.TryParse(quantityStr, out quantity) || quantity <= 0)
                    {
                        MessageBox.Show($"Некорректное количество для товара '{productName}'! Товар пропущен.",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue;
                    }

                    if (selectedProductsWithQuantity.ContainsKey(productName))
                    {
                        selectedProductsWithQuantity[productName] += quantity;
                    }
                    else
                    {
                        selectedProductsWithQuantity.Add(productName, quantity);
                    }
                }

                UpdateSelectedProductsTextBox();

                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, false);
                }

                if (selectedProductsWithQuantity.Count > 0)
                {
                    MessageBox.Show("Товары успешно добавлены!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении товаров: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSelectedProductsTextBox()
        {
            try
            {
                if (selectedProductsWithQuantity.Count == 0)
                {
                    guna2TextBox3.Text = "";
                    return;
                }

                string result = "";
                foreach (var item in selectedProductsWithQuantity)
                {
                    result += $"{item.Key} - {item.Value} шт;\r\n";
                }

                guna2TextBox3.Text = result.TrimEnd('\r', '\n');
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления списка: {ex.Message}", "Ошибка");
            }
        }

        private async Task SearchAddressesAsync(string query)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = $"https://nominatim.openstreetmap.org/search?q={HttpUtility.UrlEncode(query + ", Краснодар")}&format=json&limit=5&addressdetails=1";
                    var response = await client.GetStringAsync(url);
                    var json = JArray.Parse(response);

                    foreach (var result in json)
                    {
                        string displayName = result["display_name"]?.ToString() ?? "";
                        if (!string.IsNullOrEmpty(displayName))
                            addressSuggestions.Add(displayName);
                    }
                }
            }
            catch { }
        }

        private async void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            if (deliveryMarker == null || guna2ComboBox1.SelectedValue == null || selectedProductsWithQuantity.Count == 0)
            {
                labelStatus.Text = "❌ Выберите курьера, ТОВАР(Ы) и адрес!";
                MessageBox.Show("Пожалуйста, выберите курьера, добавьте товары и укажите адрес доставки!",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            labelStatus.Text = "📧 Отправляем...";
            currentLat = deliveryMarker.Position.Lat;
            currentLng = deliveryMarker.Position.Lng;
            currentAddress = deliveryMarker.ToolTipText.Replace("🚚 ", "");

            string courierId = guna2ComboBox1.SelectedValue.ToString();
            string courierEmail = await GetCourierEmailAsync(courierId);
            string courierName = guna2ComboBox1.Text;

            string productsString = "";
            foreach (var item in selectedProductsWithQuantity)
            {
                productsString += $"{item.Key} - {item.Value} шт; ";
            }

            if (string.IsNullOrEmpty(courierEmail))
            {
                labelStatus.Text = "❌ Email курьера!";
                return;
            }

            bool courierUpdated = await SetCourierBusyAsync(courierId);
            if (!courierUpdated) { labelStatus.Text = "❌ Статус курьера!"; return; }

            int orderId = await CreateOrderWithProductsAsync(courierId, productsString, currentLat, currentLng, currentAddress);
            if (orderId == 0)
            {
                await SetCourierFreeAsync(courierId);
                labelStatus.Text = "❌ Создание заказа!";
                return;
            }

            string htmlBody = GenerateOrderEmail(courierName, productsString, currentAddress, currentLat, currentLng, orderId);
            bool sent = await SendOrderEmailAsync(courierEmail, htmlBody);

            if (sent)
            {
                labelStatus.Text = $"✅ #{orderId} → {courierName}!";
                this.Text = $"🗺️ #{orderId} → {courierName}";

                selectedProductsWithQuantity.Clear();
                guna2TextBox3.Text = "";

                LoadCouriersToComboBox();

                MessageBox.Show($"✅ Заказ #{orderId} успешно отправлен курьеру {courierName}!",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                await SetCourierFreeAsync(courierId);
                labelStatus.Text = "❌ Отправка!";
            }
        }

        private async Task<int> CreateOrderWithProductsAsync(string courierId, string products, double lat, double lng, string address)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    string query = @"INSERT INTO orders (courier_id, product_id, lat, lng, address, status, products_list) 
                   VALUES (@courier_id, 0, @lat, @lng, @address, 'InProgress', @products_list);
                   SELECT LAST_INSERT_ID();";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@courier_id", courierId);
                        cmd.Parameters.AddWithValue("@lat", lat);
                        cmd.Parameters.AddWithValue("@lng", lng);
                        cmd.Parameters.AddWithValue("@address", address);
                        cmd.Parameters.AddWithValue("@products_list", products);

                        object result = await cmd.ExecuteScalarAsync();
                        return result != null ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка создания заказа: {ex.Message}");
                return 0;
            }
        }

        private async Task<string> GetCourierEmailAsync(string courierId)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    string query = @"SELECT u.email FROM couriers c 
                           JOIN users u ON c.user_id = u.id 
                           WHERE c.id = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", courierId);
                        return await cmd.ExecuteScalarAsync() as string;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Email курьера: {ex.Message}", "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private async Task<bool> SetCourierBusyAsync(string courierId)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    string query = "UPDATE couriers SET is_busy = 1 WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", courierId);
                        int rows = await cmd.ExecuteNonQueryAsync();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Статус курьера: {ex.Message}", "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private async Task SetCourierFreeAsync(string courierId)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    string query = "UPDATE couriers SET is_busy = 0 WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", courierId);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch { }
        }

        private string GenerateOrderEmail(string courierName, string productsString, string address, double lat, double lng, int orderId)
        {
            string coordsGoogle = $"{lat.ToString("F6", CultureInfo.InvariantCulture)},{lng.ToString("F6", CultureInfo.InvariantCulture)}";
            string mapsUrl = $"https://maps.google.com/?q={coordsGoogle}";

            return $@"
<!DOCTYPE html>
<html lang='ru'>
<head>
  <meta charset='utf-8'>
  <title>🚚 #{orderId}</title>
  <style>
    * {{ box-sizing: border-box; }}
    body {{
      margin: 0;
      padding: 20px;
      font-family: 'Segoe UI', Arial, sans-serif;
      background: radial-gradient(circle at top, #8B0000 0%, #000000 40%, #1a1a1a 100%);
      min-height: 100vh;
      display: flex;
      align-items: center;
      justify-content: center;
      color: #D0D0D0;
    }}
    .card {{
      background: linear-gradient(145deg, #2a1a1a 0%, #1f1f1f 40%, #4a0000 100%);
      color: #E0E0E0;
      border-radius: 32px;
      padding: 32px 40px;
      box-shadow: 0 20px 50px rgba(0,0,0,0.8);
      max-width: 550px;
      width: 100%;
      position: relative;
      overflow: hidden;
      border: 2px solid #CC0000;
    }}
    .card::before,
    .card::after {{
      content: '';
      position: absolute;
      width: 260px;
      height: 260px;
      border-radius: 160px;
      filter: blur(16px);
      opacity: 0.6;
      z-index: 0;
    }}
    .card::before {{
      background: #CC0000;
      top: -120px;
      right: -80px;
    }}
    .card::after {{
      background: #660000;
      bottom: -140px;
      left: -60px;
    }}
    .card-inner {{ position: relative; z-index: 1; }}
    .subtitle {{
      font-size: 13px;
      text-transform: uppercase;
      letter-spacing: 0.25em;
      color: #CC0000;
      margin-bottom: 22px;
    }}
    .title {{
      font-size: 26px;
      font-weight: 700;
      letter-spacing: 0.12em;
      text-transform: uppercase;
      color: #FF4444;
      margin-bottom: 4px;
      text-shadow: 0 2px 4px rgba(0,0,0,0.5);
    }}
    .products {{
      background: rgba(40,40,40,0.85);
      color: #FF6666;
      padding: 20px;
      font-size: 18px;
      text-align: left;
      border-radius: 15px;
      margin-bottom: 15px;
      border-top: 3px solid #CC0000;
    }}
    .product-item {{
      padding: 8px 0;
      border-bottom: 1px solid #660000;
      font-size: 16px;
    }}
    .content {{
      background: rgba(40,40,40,0.85);
      border-radius: 20px;
      padding: 35px;
      text-align: center;
      font-size: 28px;
      font-family: monospace;
      letter-spacing: 2px;
      color: #FFF;
      border-left: 4px solid #CC0000;
      cursor: pointer;
      margin: 0;
      transition: all 0.3s;
    }}
    .content:hover {{
      background: #CC0000;
      transform: scale(1.02);
    }}
    .maps-btn {{
      display: block;
      background: linear-gradient(45deg, #CC0000, #990000);
      color: #FFF;
      padding: 25px;
      text-decoration: none;
      text-align: center;
      font-size: 20px;
      font-weight: bold;
      margin: 25px auto;
      border-radius: 12px;
      box-shadow: 0 5px 15px rgba(204,0,0,0.4);
      max-width: 300px;
      transition: all 0.3s;
    }}
    .maps-btn:hover {{
      transform: translateY(-3px);
      box-shadow: 0 8px 20px rgba(204,0,0,0.6);
    }}
    .address {{
      background: rgba(40,40,40,0.85);
      color: #FF6666;
      padding: 25px;
      font-size: 18px;
      text-align: center;
      border-top: 3px solid #CC0000;
      border-bottom: 3px solid #CC0000;
      border-radius: 0 0 20px 20px;
    }}
    .footer {{
      margin-top: 18px;
      font-size: 14px;
      color: #999;
      text-align: center;
      padding-top: 20px;
      border-top: 2px solid #CC0000;
    }}
    .chip {{
      display: inline-block;
      padding: 4px 12px;
      border-radius: 999px;
      background: linear-gradient(45deg, #CC0000, #990000);
      color: #FFF;
      font-size: 12px;
      letter-spacing: 0.16em;
      text-transform: uppercase;
    }}
  </style>
</head>
<body>
  <div class='card'>
    <div class='card-inner'>
      <div class='subtitle'>delivery system</div>
      <div class='title'>Заказ #{orderId}</div>
      <div class='products'>
        <div style='font-weight: bold; margin-bottom: 10px;'>📋 Состав заказа:</div>
        {string.Join("", productsString.Split(';').Where(p => !string.IsNullOrWhiteSpace(p)).Select(p => $"<div class='product-item'>📦 {p.Trim()}</div>"))}
      </div>
      <div class='content' onclick='navigator.clipboard.writeText(`{coordsGoogle}`); alert(`✅ Координаты скопированы!`);'>{coordsGoogle}</div>
      <a href='{mapsUrl}' class='maps-btn'>🗺️ GOOGLE MAPS</a>
      <div class='address'>📍 {address}</div>
      <div class='footer'>
        <div class='chip'>{courierName}</div>
        DeliverySystem • {DateTime.Now:HH:mm dd.MM}
      </div>
    </div>
  </div>
</body>
</html>";
        }

        private async Task<bool> SendOrderEmailAsync(string toEmail, string htmlBody)
        {
            try
            {
                labelStatus.Text = "⏳ Отправка MailKit...";

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Delivery", "krasnodarcandles@gmail.com"));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = $"🚚 Новый заказ #{DateTime.Now:dd.MM HH:mm}";

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = htmlBody;
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync("krasnodarcandles@gmail.com", "btkjrboyknhnykar");
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                labelStatus.Text = $"Отправлено: {toEmail}";
                MessageBox.Show($"Email ДОСТАВЛЕН на {toEmail}!");
                return true;
            }
            catch (Exception ex)
            {
                labelStatus.Text = "MailKit ошибка";
                MessageBox.Show($"❌ {ex.Message}");
                return false;
            }
        }

        private void AddDeliveryMarker(double lat, double lng, string address)
        {
            currentLat = lat;
            currentLng = lng;
            currentAddress = address;

            if (gmap.Overlays.Count > 0)
                gmap.Overlays.Clear();

            deliveryMarker = new GMarkerGoogle(new PointLatLng(lat, lng), GMarkerGoogleType.red)
            {
                ToolTipText = $"{address}"
            };

            GMapOverlay overlay = new GMapOverlay("markers");
            overlay.Markers.Add(deliveryMarker);
            gmap.Overlays.Add(overlay);

            gmap.Position = new PointLatLng(lat, lng);

            labelCoords.Text = $"📍 {lat:F6}, {lng:F6}";
        }

        private async void Gmap_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PointLatLng point = gmap.FromLocalToLatLng(e.X, e.Y);
                string address = await GetAddressFromCoordinates(point.Lat, point.Lng);
                AddDeliveryMarker(point.Lat, point.Lng, address);
            }
        }

        private void LoadCouriersToComboBox()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT c.id, u.full_name 
                           FROM couriers c 
                           JOIN users u ON c.user_id = u.id 
                           WHERE c.is_active = 1 AND c.is_busy = 0";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    guna2ComboBox1.DataSource = null;
                    guna2ComboBox1.DataSource = dt;
                    guna2ComboBox1.DisplayMember = "full_name";
                    guna2ComboBox1.ValueMember = "id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Курьеры: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2ComboBox1.SelectedValue != null)
                this.Text = $"Курьер: {guna2ComboBox1.Text}";
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            selectedProductsWithQuantity.Clear();
            guna2TextBox3.Text = "";
            MessageBox.Show("Список товаров очищен!", "Успех",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void guna2TileButton4_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void guna2TileButton3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2ShadowPanel7_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
