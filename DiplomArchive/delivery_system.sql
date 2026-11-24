-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Хост: 127.0.0.1
-- Время создания: Июн 16 2026 г., 11:07
-- Версия сервера: 10.4.32-MariaDB
-- Версия PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: `delivery_system`
--

-- --------------------------------------------------------

--
-- Структура таблицы `couriers`
--

CREATE TABLE `couriers` (
  `id` int(11) NOT NULL,
  `user_id` int(11) DEFAULT NULL,
  `vehicle_info` varchar(50) DEFAULT NULL,
  `is_active` tinyint(4) DEFAULT 1,
  `is_busy` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Дамп данных таблицы `couriers`
--

INSERT INTO `couriers` (`id`, `user_id`, `vehicle_info`, `is_active`, `is_busy`) VALUES
(6, 15, 'Газель', 1, 0),
(7, 16, 'Мотоцикл', 1, 0),
(8, 17, 'Chevrolet Corvette', 1, 0),
(9, 18, 'Велосипед', 1, 0),
(10, 19, 'Пеший', 1, 0),
(11, 20, 'Lada Granta', 1, 0),
(12, 21, 'Skoda Octavia', 1, 0),
(13, 22, 'Yamaha электросамокат', 1, 0),
(14, 23, 'Hyundai Solaris', 1, 0),
(15, 24, 'Peugeot велосипед', 1, 0),
(16, 25, 'ГАЗель грузовая', 1, 0),
(17, 26, 'Kawasaki мото', 1, 0),
(18, 27, 'Volkswagen Polo', 1, 0),
(19, 28, 'Xiaomi самокат', 1, 0),
(20, 29, 'Пешком с тележкой', 1, 0);

-- --------------------------------------------------------

--
-- Структура таблицы `customers`
--

CREATE TABLE `customers` (
  `id` int(11) NOT NULL,
  `full_name` varchar(100) DEFAULT NULL,
  `phone` varchar(20) DEFAULT NULL,
  `address` varchar(255) DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Дамп данных таблицы `customers`
--

INSERT INTO `customers` (`id`, `full_name`, `phone`, `address`, `created_at`) VALUES
(7, 'Иванов Иван Иванович', '+7-900-123-45-67', 'Краснодар, ул. Северная, 1', '2026-01-23 23:07:54'),
(8, 'Петров Петр Петрович', '+7-900-234-56-78', 'Краснодар, ул. Южная, 10', '2026-01-23 23:07:54'),
(9, 'Сидорова Анна Сергеевна', '+7-900-345-67-89', 'Краснодар, пр. Кубани, 25', '2026-01-23 23:07:54'),
(10, 'Козлов Дмитрий Алексеевич', '+7-900-456-78-90', 'Краснодар, ул. Заполярная, 5', '2026-01-23 23:07:54'),
(11, 'Морозова Elena Викторовна', '+7-900-567-89-01', 'Краснодар, ул. Рашпилевская, 12', '2026-01-23 23:07:54'),
(12, 'Волков Сергей Михайлович', '+7-900-678-90-12', 'Краснодар, ул. 40 лет Победы, 8', '2026-01-23 23:07:54'),
(13, 'Лебедева Мария Андреевна', '+7-900-789-01-23', 'Краснодар, ул. Ставропольская, 30', '2026-01-23 23:07:54'),
(14, 'Николаев Андрей Валерьевич', '+7-900-890-12-34', 'Краснодар, ул. Тургенева, 15', '2026-01-23 23:07:54'),
(15, 'Федорова Ольга Павловна', '+7-900-901-23-45', 'Краснодар, ул. Дзержинского, 7', '2026-01-23 23:07:54'),
(16, 'Смирнов Алексей Геннадьевич', '+7-900-012-34-56', 'Краснодар, ул. Московская, 20', '2026-01-23 23:07:54');

-- --------------------------------------------------------

--
-- Структура таблицы `orders`
--

CREATE TABLE `orders` (
  `id` int(11) NOT NULL,
  `customer_id` int(11) DEFAULT NULL,
  `customer_name` varchar(100) DEFAULT NULL,
  `customer_phone` varchar(20) DEFAULT NULL,
  `address` varchar(255) DEFAULT NULL,
  `amount` decimal(10,2) DEFAULT NULL,
  `status` varchar(20) DEFAULT NULL,
  `courier_id` int(11) DEFAULT NULL,
  `dispatcher_id` int(11) DEFAULT NULL,
  `planned_time` datetime DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `comment` text DEFAULT NULL,
  `products_list` text DEFAULT NULL,
  `lat` double(10,8) DEFAULT NULL,
  `lng` double(11,8) DEFAULT NULL,
  `product_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Дамп данных таблицы `orders`
--

INSERT INTO `orders` (`id`, `customer_id`, `customer_name`, `customer_phone`, `address`, `amount`, `status`, `courier_id`, `dispatcher_id`, `planned_time`, `created_at`, `updated_at`, `comment`, `products_list`, `lat`, `lng`, `product_id`) VALUES
(21, NULL, NULL, NULL, 'Клик по карте', NULL, 'InProgress', 8, NULL, NULL, '2026-03-08 00:01:25', '2026-06-04 11:07:10', NULL, NULL, 44.47430414, 39.71523285, NULL),
(22, NULL, NULL, NULL, 'Клик по карте', NULL, 'Delivered', 8, NULL, NULL, '2026-03-08 00:05:36', '2026-06-04 11:07:34', NULL, NULL, 44.47430032, 39.71522212, NULL),
(23, NULL, NULL, NULL, 'ул. Красная 15', NULL, 'pending', 8, NULL, NULL, '2026-03-08 00:07:08', '2026-03-08 00:07:08', NULL, NULL, 45.03547000, 38.97512000, NULL),
(24, NULL, NULL, NULL, 'ул. Красная 15', NULL, 'pending', 8, NULL, NULL, '2026-03-08 00:29:07', '2026-03-08 00:29:07', NULL, NULL, 45.03547000, 38.97512000, NULL),
(25, NULL, NULL, NULL, 'ул. Красная 15', NULL, 'pending', 8, NULL, NULL, '2026-03-08 00:32:51', '2026-03-08 00:32:51', NULL, NULL, 45.03547000, 38.97512000, NULL),
(26, NULL, NULL, NULL, 'ул. Красная 15', NULL, 'pending', 8, NULL, NULL, '2026-03-08 00:43:39', '2026-03-08 00:43:39', NULL, NULL, 45.03547000, 38.97512000, NULL),
(27, NULL, NULL, NULL, 'Клик по карте', NULL, 'pending', 8, NULL, NULL, '2026-03-08 00:47:05', '2026-03-08 00:47:05', NULL, NULL, 45.03386561, 38.97738934, NULL),
(28, NULL, NULL, NULL, 'Клик по карте', NULL, 'pending', 8, NULL, NULL, '2026-03-08 00:49:07', '2026-03-08 00:49:07', NULL, NULL, 45.03389593, 38.97674561, NULL),
(29, NULL, NULL, NULL, 'Клик по карте', NULL, 'pending', 8, NULL, NULL, '2026-03-08 00:50:34', '2026-03-08 00:50:34', NULL, NULL, 45.03304675, 38.97369862, NULL),
(30, NULL, NULL, NULL, 'Клик по карте', NULL, 'pending', 8, NULL, NULL, '2026-03-08 00:51:16', '2026-03-08 00:51:16', NULL, NULL, 45.03526067, 38.97524357, NULL),
(31, NULL, NULL, NULL, 'Клик по карте', NULL, 'pending', 8, NULL, NULL, '2026-03-08 00:56:22', '2026-03-08 00:56:22', NULL, NULL, 45.03171229, 38.98116589, NULL),
(32, NULL, NULL, NULL, 'Клик по карте', NULL, 'pending', 8, NULL, NULL, '2026-03-08 01:01:05', '2026-03-08 01:01:05', NULL, NULL, 45.03352442, 38.98033977, NULL),
(33, NULL, NULL, NULL, 'Клик по карте', NULL, 'pending', 8, NULL, NULL, '2026-03-08 01:04:14', '2026-03-08 01:04:14', NULL, NULL, 45.03214448, 38.98228705, NULL),
(34, NULL, NULL, NULL, 'Клик по карте', NULL, 'pending', 8, NULL, NULL, '2026-03-08 01:09:15', '2026-03-08 01:09:15', NULL, NULL, 44.47429266, 39.71521139, NULL),
(35, NULL, NULL, NULL, 'Клик по карте', NULL, 'accepted', 8, NULL, NULL, '2026-03-08 01:14:51', '2026-03-09 23:06:00', NULL, NULL, 44.99102675, 39.05158997, NULL),
(36, NULL, NULL, NULL, 'Клик по карте', NULL, 'pending', 8, NULL, NULL, '2026-03-08 01:17:52', '2026-03-08 01:17:52', NULL, NULL, 44.47429266, 39.71521139, NULL),
(37, NULL, NULL, NULL, 'Клик по карте', NULL, 'pending', 8, NULL, NULL, '2026-03-08 01:18:33', '2026-03-08 01:18:33', NULL, NULL, 44.47429266, 39.71521139, NULL),
(38, NULL, NULL, NULL, 'Клик по карте', NULL, 'pending', 8, NULL, NULL, '2026-03-08 01:20:54', '2026-03-08 01:20:54', NULL, NULL, 44.47430414, 39.71524358, NULL),
(39, NULL, NULL, NULL, 'Клик по карте', NULL, 'pending', 8, NULL, NULL, '2026-03-08 18:57:55', '2026-03-08 18:57:55', NULL, NULL, 45.00584345, 39.02232170, NULL),
(40, NULL, NULL, NULL, 'Клик по карте', NULL, 'pending', 8, NULL, NULL, '2026-03-08 19:00:00', '2026-03-08 19:00:00', NULL, NULL, 45.03271693, 38.96352768, NULL),
(41, NULL, NULL, NULL, 'ул. Красная 15', NULL, 'pending', 8, NULL, NULL, '2026-03-09 22:13:34', '2026-03-09 22:13:34', NULL, NULL, 45.03547000, 38.97512000, 4),
(42, NULL, NULL, NULL, 'ул. Красная 15', NULL, 'pending', 8, NULL, NULL, '2026-03-09 22:14:16', '2026-03-09 22:14:16', NULL, NULL, 45.03547000, 38.97512000, 5),
(43, NULL, NULL, NULL, 'ул. Красная 15', NULL, 'pending', 8, NULL, NULL, '2026-05-10 20:59:42', '2026-05-10 20:59:42', NULL, 'Свеча Океан - 4 шт; Свеча церковная - 163 шт; ', 45.03547000, 38.97512000, 0),
(44, NULL, NULL, NULL, 'ул. Зеленых карбюраторов 543', NULL, 'pending', 8, NULL, NULL, '2026-05-10 21:08:52', '2026-05-10 21:33:40', NULL, 'Свеча церковная - 1 шт; ', 45.03547000, 38.97512000, 0),
(45, NULL, NULL, NULL, 'ул. Репина 15', NULL, 'pending', 6, NULL, NULL, '2026-05-10 21:35:03', '2026-05-10 21:39:21', NULL, 'Свеча Сандал - 9 шт; Свеча церковная - 1 шт; ', 45.03547000, 38.97512000, 0),
(46, NULL, NULL, NULL, 'ул. Чапаева 32', NULL, 'pending', 8, NULL, NULL, '2026-05-10 21:36:30', '2026-05-10 21:51:06', NULL, 'Свеча Сандал - 9 шт; Свеча церковная - 42 шт; ', 45.04353935, 39.00245190, 0),
(47, NULL, NULL, NULL, 'ул. Красная 15', NULL, 'pending', 8, NULL, NULL, '2026-05-10 21:38:28', '2026-05-10 21:38:28', NULL, 'Свеча церковная - 675 шт; Свеча Океан - 45 шт; ', 45.03547000, 38.97512000, 0),
(48, NULL, NULL, NULL, 'ргрлгр', NULL, 'pending', 8, NULL, NULL, '2026-05-10 21:52:48', '2026-05-10 21:53:19', NULL, 'Свеча Океан - 98 шт; Свеча церковная - 6547 шт; ', 45.02379590, 38.99532795, 0),
(49, NULL, NULL, NULL, 'Координаты: 45,030992, 38,985200', NULL, 'pending', 8, NULL, NULL, '2026-05-10 22:00:05', '2026-05-10 22:00:05', NULL, 'Свеча церковная - 4 шт; ', 45.03099198, 38.98519993, 0),
(50, NULL, NULL, NULL, 'ул. Красная 15', NULL, 'pending', 8, NULL, NULL, '2026-05-10 22:05:23', '2026-05-10 22:05:23', NULL, 'Свеча церковная - 231 шт; ', 45.03547000, 38.97512000, 0),
(51, NULL, NULL, NULL, 'Клик по карте', NULL, 'pending', 8, NULL, NULL, '2026-05-21 13:05:59', '2026-05-21 13:05:59', NULL, 'Свеча Океан - 5 шт; Свеча церковная - 3 шт; ', 45.03171229, 38.98022175, 0),
(52, NULL, NULL, NULL, 'ул. Красная 15', NULL, 'pending', 8, NULL, NULL, '2026-05-21 13:15:52', '2026-05-21 13:15:52', NULL, 'Свеча церковная - 5 шт; ', 45.03547000, 38.97512000, 0),
(53, NULL, NULL, NULL, 'ул. Красная 15', NULL, 'pending', 8, NULL, NULL, '2026-05-21 13:17:52', '2026-05-21 13:17:52', NULL, 'Свеча Сандал - 2 шт; ', 45.03547000, 38.97512000, 0),
(54, NULL, NULL, NULL, 'Клик по карте', NULL, 'pending', 8, NULL, NULL, '2026-05-27 21:31:50', '2026-05-27 21:31:50', NULL, 'Свеча Океан - 2 шт; Свеча церковная - 54 шт; ', 45.03335003, 38.97112370, 0),
(55, NULL, NULL, NULL, 'ул. Красная 15', NULL, 'pending', 8, NULL, NULL, '2026-05-27 23:12:34', '2026-05-27 23:12:34', NULL, 'Свеча церковная - 1 шт; ', 45.03547000, 38.97512000, 0),
(56, NULL, NULL, NULL, 'ул. Красная 15', NULL, 'pending', 8, NULL, NULL, '2026-06-02 23:48:08', '2026-06-02 23:48:08', NULL, 'Свеча Сандал - 2 шт; Свеча церковная - 2 шт; ', 45.03547000, 38.97512000, 0),
(57, NULL, NULL, NULL, 'ул. Красная 15', NULL, 'pending', 8, NULL, NULL, '2026-06-02 23:53:24', '2026-06-02 23:53:24', NULL, 'Свеча церковная - 1 шт; ', 45.03547000, 38.97512000, 0),
(58, NULL, NULL, NULL, 'ул. Красная 15', NULL, 'pending', 8, NULL, NULL, '2026-06-02 23:55:07', '2026-06-02 23:55:07', NULL, 'Свеча церковная - 1 шт; ', 45.03547000, 38.97512000, 0),
(59, NULL, NULL, NULL, 'ул. Красная 15', NULL, 'pending', 8, NULL, NULL, '2026-06-04 10:24:00', '2026-06-04 10:24:00', NULL, 'Свеча церковная - 1 шт; ', 45.03547000, 38.97512000, 0),
(60, NULL, NULL, NULL, 'Клик по карте', NULL, 'InProgress', 8, NULL, NULL, '2026-06-06 19:02:02', '2026-06-06 19:02:02', NULL, 'Свеча церковная - 1 шт; ', 45.01603009, 38.98816109, 0),
(61, NULL, NULL, NULL, 'ул. Красная 15', NULL, 'InProgress', 8, NULL, NULL, '2026-06-06 19:05:06', '2026-06-06 19:05:06', NULL, 'Свеча церковная - 2 шт; ', 45.03547000, 38.97512000, 0),
(62, NULL, NULL, NULL, 'ул. Красная 15', NULL, 'InProgress', 8, NULL, NULL, '2026-06-06 19:06:13', '2026-06-06 19:06:13', NULL, 'Свеча церковная - 3 шт; ', 45.03547000, 38.97512000, 0),
(63, NULL, NULL, NULL, 'Клик по карте', NULL, 'InProgress', 8, NULL, NULL, '2026-06-06 19:07:01', '2026-06-06 19:07:01', NULL, 'Свеча Сандал - 4 шт; ', 45.03620080, 38.97983551, 0),
(64, NULL, NULL, NULL, '45,03029, 38,98850', NULL, 'InProgress', 8, NULL, NULL, '2026-06-06 19:14:45', '2026-06-06 19:14:45', NULL, 'Свеча церковная - 1 шт; ', 45.03028682, 38.98850441, 0),
(65, NULL, NULL, NULL, '45,03141, 38,98284', NULL, 'InProgress', 8, NULL, NULL, '2026-06-06 19:18:09', '2026-06-06 19:18:09', NULL, 'Свеча церковная - 4 шт; ', 45.03140900, 38.98283958, 0),
(66, NULL, NULL, NULL, 'ул. Красная 15', NULL, 'InProgress', 8, NULL, NULL, '2026-06-06 19:20:40', '2026-06-06 19:20:40', NULL, 'Свеча церковная - 321 шт; ', 45.03547000, 38.97512000, 0),
(67, NULL, NULL, NULL, 'Клик по карте', NULL, 'InProgress', 8, NULL, NULL, '2026-06-06 19:25:44', '2026-06-06 19:25:44', NULL, 'Свеча Океан - 43 шт; ', 45.03325905, 38.97116661, 0),
(68, NULL, NULL, NULL, 'Клик по карте', NULL, 'InProgress', 8, NULL, NULL, '2026-06-06 19:27:55', '2026-06-06 19:27:55', NULL, 'Свеча церковная - 312 шт; ', 45.03271314, 38.96876335, 0),
(69, NULL, NULL, NULL, 'Клик по карте', NULL, 'InProgress', 8, NULL, NULL, '2026-06-06 19:38:34', '2026-06-06 19:38:34', NULL, 'Свеча церковная - 1 шт; ', 45.03313774, 38.96884918, 0),
(70, NULL, NULL, NULL, 'Краснодар, ул. Красная 15', NULL, 'InProgress', 8, NULL, NULL, '2026-06-07 15:40:58', '2026-06-07 15:40:58', NULL, 'Свеча церковная - 1 шт; ', 45.03547000, 38.97512000, 0),
(71, NULL, NULL, NULL, 'Краснодар, ул. Красная 13267', NULL, 'Delivered', 8, NULL, NULL, '2026-06-16 12:01:12', '2026-06-16 12:05:14', NULL, 'Свеча церковная -88888 шт; ', 45.03547000, 38.97512000, 0);

-- --------------------------------------------------------

--
-- Структура таблицы `order_messages`
--

CREATE TABLE `order_messages` (
  `id` int(11) NOT NULL,
  `order_id` int(11) DEFAULT NULL,
  `sender_id` int(11) DEFAULT NULL,
  `message` text DEFAULT NULL,
  `sent_at` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Структура таблицы `order_status_history`
--

CREATE TABLE `order_status_history` (
  `id` int(11) NOT NULL,
  `order_id` int(11) DEFAULT NULL,
  `status` varchar(20) DEFAULT NULL,
  `changed_by` int(11) DEFAULT NULL,
  `changed_at` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Структура таблицы `products`
--

CREATE TABLE `products` (
  `id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Дамп данных таблицы `products`
--

INSERT INTO `products` (`id`, `name`) VALUES
(1, 'Свеча Лаванда'),
(2, 'Свеча церковная'),
(3, 'Свеча Сандал'),
(4, 'Свеча Океан'),
(5, 'Подставка');

-- --------------------------------------------------------

--
-- Структура таблицы `roles`
--

CREATE TABLE `roles` (
  `id` int(11) NOT NULL,
  `name` varchar(50) NOT NULL,
  `description` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Дамп данных таблицы `roles`
--

INSERT INTO `roles` (`id`, `name`, `description`) VALUES
(1, 'Admin', 'Администратор'),
(2, 'Dispatcher', 'Диспетчер свечей'),
(3, 'Courier', 'Курьер'),
(4, 'Warehouse', 'Складской работник'),
(5, 'Manager', 'Менеджер'),
(6, 'Accountant', 'Бухгалтер'),
(7, 'Driver', 'Водитель'),
(8, 'Operator', 'Оператор колл-центра'),
(9, 'Supervisor', 'Супервайзер'),
(10, 'Technician', 'Техник'),
(11, 'HR', 'Кадровик'),
(12, 'Security', 'Охрана'),
(13, 'Cleaner', 'Уборщица'),
(14, 'IT', 'Сисадмин'),
(15, 'Director', 'Директор'),
(16, 'Owner', 'Владелец');

-- --------------------------------------------------------

--
-- Структура таблицы `users`
--

CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `login` varchar(50) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `full_name` varchar(100) NOT NULL,
  `phone` varchar(20) DEFAULT NULL,
  `role_id` int(11) DEFAULT NULL,
  `is_active` tinyint(4) DEFAULT 1,
  `email` varchar(100) NOT NULL DEFAULT '',
  `is_busy` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Дамп данных таблицы `users`
--

INSERT INTO `users` (`id`, `login`, `password_hash`, `full_name`, `phone`, `role_id`, `is_active`, `email`, `is_busy`) VALUES
(1, 'admin', 'admin', 'Админ Системы', '+7(861)000-00-01', 1, 1, '', 0),
(10, 'disp1', 'disp123', 'Иванова И.А.', '+7(900)111-11-11', 2, 1, '', 0),
(11, 'disp2', 'disp123', 'Петров П.П.', '+7(900)222-22-22', 2, 1, '', 0),
(12, 'disp3', 'disp123', 'Сидорова С.С.', '+7(900)333-33-33', 2, 1, '', 0),
(13, 'disp4', 'disp123', 'Крамар Артем', '+7(900)444-44-44', 2, 1, 'nikolaskeje@gmail.com', 0),
(14, 'disp5', 'disp123', 'Смирнова М.М.', '+7(900)555-55-55', 2, 1, '', 0),
(15, 'courier1', 'courier123', 'Козлов Кирилл', '+7(900)601-11-11', 3, 1, 'kurier1@gmail.com', 0),
(16, 'courier2', 'courier123', 'Иванов Иван', '+7(900)602-22-22', 3, 1, 'kurier2@gmail.com', 0),
(17, 'courier3', 'courier123', 'Крамар Артем', '+7(900)603-33-33', 3, 1, 'nikolaskeje@gmail.com', 0),
(18, 'courier4', 'courier123', 'Смирнов Сергей', '+7(900)604-44-44', 3, 1, 'kurier4@gmail.com', 0),
(19, 'courier5', 'courier123', 'Морозова Мария', '+7(900)605-55-55', 3, 1, 'kurier5@gmail.com', 0),
(20, 'courier6', 'courier123', 'Васильев Василий', '+7(900)606-66-66', 3, 1, 'kurier6@gmail.com', 0),
(21, 'courier7', 'courier123', 'Новиков Николай', '+7(900)607-77-77', 3, 1, 'kurier7@gmail.com', 0),
(22, 'courier8', 'courier123', 'Федорова Фёкла', '+7(900)608-88-88', 3, 1, 'kurier8@gmail.com', 0),
(23, 'courier9', 'courier123', 'Егоров Егор', '+7(900)609-99-99', 3, 1, 'kurier9@gmail.com', 0),
(24, 'courier10', 'courier123', 'Зайцев Захар', '+7(900)610-00-11', 3, 1, 'kurier10@gmail.com', 0),
(25, 'courier11', 'courier123', 'Яковлева Юлия', '+7(900)611-11-22', 3, 1, '', 0),
(26, 'courier12', 'courier123', 'Киселёв Константин', '+7(900)612-22-33', 3, 1, '', 0),
(27, 'courier13', 'courier123', 'Лебедев Леонид', '+7(900)613-33-44', 3, 1, '', 0),
(28, 'courier14', 'courier123', 'Михайлова Марина', '+7(900)614-44-55', 3, 1, '', 0),
(29, 'courier15', 'courier123', 'Николаев Николай', '+7(900)615-55-66', 3, 1, '', 0);

--
-- Индексы сохранённых таблиц
--

--
-- Индексы таблицы `couriers`
--
ALTER TABLE `couriers`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `user_id` (`user_id`);

--
-- Индексы таблицы `customers`
--
ALTER TABLE `customers`
  ADD PRIMARY KEY (`id`);

--
-- Индексы таблицы `orders`
--
ALTER TABLE `orders`
  ADD PRIMARY KEY (`id`),
  ADD KEY `customer_id` (`customer_id`),
  ADD KEY `dispatcher_id` (`dispatcher_id`),
  ADD KEY `idx_orders_status` (`status`),
  ADD KEY `idx_orders_courier` (`courier_id`);

--
-- Индексы таблицы `order_messages`
--
ALTER TABLE `order_messages`
  ADD PRIMARY KEY (`id`),
  ADD KEY `order_id` (`order_id`),
  ADD KEY `sender_id` (`sender_id`);

--
-- Индексы таблицы `order_status_history`
--
ALTER TABLE `order_status_history`
  ADD PRIMARY KEY (`id`),
  ADD KEY `order_id` (`order_id`),
  ADD KEY `changed_by` (`changed_by`);

--
-- Индексы таблицы `products`
--
ALTER TABLE `products`
  ADD PRIMARY KEY (`id`);

--
-- Индексы таблицы `roles`
--
ALTER TABLE `roles`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `name` (`name`);

--
-- Индексы таблицы `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `login` (`login`),
  ADD KEY `role_id` (`role_id`);

--
-- AUTO_INCREMENT для сохранённых таблиц
--

--
-- AUTO_INCREMENT для таблицы `couriers`
--
ALTER TABLE `couriers`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT для таблицы `customers`
--
ALTER TABLE `customers`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT для таблицы `orders`
--
ALTER TABLE `orders`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=72;

--
-- AUTO_INCREMENT для таблицы `order_messages`
--
ALTER TABLE `order_messages`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT для таблицы `order_status_history`
--
ALTER TABLE `order_status_history`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT для таблицы `products`
--
ALTER TABLE `products`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT для таблицы `roles`
--
ALTER TABLE `roles`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT для таблицы `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=30;

--
-- Ограничения внешнего ключа сохраненных таблиц
--

--
-- Ограничения внешнего ключа таблицы `couriers`
--
ALTER TABLE `couriers`
  ADD CONSTRAINT `couriers_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`);

--
-- Ограничения внешнего ключа таблицы `orders`
--
ALTER TABLE `orders`
  ADD CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`customer_id`) REFERENCES `customers` (`id`),
  ADD CONSTRAINT `orders_ibfk_2` FOREIGN KEY (`courier_id`) REFERENCES `couriers` (`id`),
  ADD CONSTRAINT `orders_ibfk_3` FOREIGN KEY (`dispatcher_id`) REFERENCES `users` (`id`);

--
-- Ограничения внешнего ключа таблицы `order_messages`
--
ALTER TABLE `order_messages`
  ADD CONSTRAINT `order_messages_ibfk_1` FOREIGN KEY (`order_id`) REFERENCES `orders` (`id`),
  ADD CONSTRAINT `order_messages_ibfk_2` FOREIGN KEY (`sender_id`) REFERENCES `users` (`id`);

--
-- Ограничения внешнего ключа таблицы `order_status_history`
--
ALTER TABLE `order_status_history`
  ADD CONSTRAINT `order_status_history_ibfk_1` FOREIGN KEY (`order_id`) REFERENCES `orders` (`id`),
  ADD CONSTRAINT `order_status_history_ibfk_2` FOREIGN KEY (`changed_by`) REFERENCES `users` (`id`);

--
-- Ограничения внешнего ключа таблицы `users`
--
ALTER TABLE `users`
  ADD CONSTRAINT `users_ibfk_1` FOREIGN KEY (`role_id`) REFERENCES `roles` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
