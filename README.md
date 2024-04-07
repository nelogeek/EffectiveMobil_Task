# Программа для анализа журнала доступа по IP-адресам

Дан  файл, содержащий список IP-адресов протокола IPv4 из журнала доступа. На каждой строке записан адрес и время, в которое с него пришёл запрос.

Необходимо разработать консольное приложение, которое  выводит в файл список IP-адресов из файла журнала, входящих в указанный диапазон с количеством обращений с этого адреса в указанный интервал времени. Адрес и время доступа разделено двоеточием. 
Дата в журнале доступа записана в формате: yyyy-MM-dd HH:mm:ss

Все параметры передаются приложению через параметры командной строки:

- --file-log — путь к файлу с логами
- --file-output — путь к файлу с результатом
- --address-start —  нижняя граница диапазона адресов, необязательный параметр, по умолчанию обрабатываются все адреса
- --address-mask — маска подсети, задающая верхнюю границу диапазона десятичное число. Необязательный параметр. В случае, если он не указан, обрабатываются все адреса, начиная с нижней границы диапазона. Параметр нельзя использовать, если не задан address-start
- --time-start —  нижняя граница временного интервала
- --time-end — верхняя граница временного интервала.

Даты в параметрах задаются в формате dd.MM.yyyy

## Дополнительные возможности
По возможности, кроме передачи параметров через командную строку, предусмотреть возможность частичной/полной передачи параметров через файлы конфигурации или переменные среды

## Обработка ошибок
Программа не должна ломаться от некорректных входных данных, ошибок ввода-вывода и прочим причинам, которые можно предусмотреть.

## Оптимальность и читаемость кода
Код должен быть оптимальным и читаемым, при разработке желательно использовать общераспространённые практики (паттерны проектирования, тесты...)

##
Примеры команд:

--file-log C:\Users\lokot\Desktop\file.log --file-output C:\Users\lokot\Desktop\output.txt --address-start 192.168.1.100 --address-mask 32 --time-start 01.02.2024 --time-end 01.05.2024

--file-log C:\Users\lokot\Desktop\file.log --file-output C:\Users\lokot\Desktop\output.txt --address-start 192.168.1.112 --address-mask 32 --time-start 01.02.2024 --time-end 01.05.2024

--file-log C:\Users\lokot\Desktop\file.log --file-output C:\Users\lokot\Desktop\output.txt --address-start 192.168.1.124 --address-mask 32 --time-start 01.02.2024 --time-end 01.05.2024


##
P.s. пример журнала лежит в файле file.log



