Проект разработан в среде разработки Visual Studio 2022
Язык: C#.
Пакеты NuGet: MySQL.Data, GMap.Net, Guna;
При запуске на стороннем пк карта GMap может сломаться. Чтобы это исправить перенесите папку GMap.NET по пути:
C:\Users\...\AppData\Local
(Нажмите Win + R и пропишите в открывшемся окне %AppData% чтобы сразу перейти по этому пути).
Для запуска понадобится Visual Studio 2022 и OpenServer (или XAMPP) с импортированной базой данных "delivery_system.sql"