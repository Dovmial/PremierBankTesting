# PremierBankApi

Доп. настройка для запуска.
1. создать файл **.env** возле Program.cs
2. прописать там переменную **POSTGRESS_CONNECTION_STRIG_PREMIERDB=**
3. заполнить ее значением строки подключения к постгресс. пример формата: **Host=localhost;Port=5432;Database=BankPremierDb;Username=postgres;Password=admin;**
4. создание базы и накат миграций автоматические при запуске.
