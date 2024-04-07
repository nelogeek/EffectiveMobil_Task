using System.Globalization;

namespace EffectiveMobil_Task
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Waiting for command...");
                string command = Console.ReadLine();

                if (string.IsNullOrEmpty(command))
                    continue;

                string[] commandArgs = command.Split(' ');

                if (commandArgs[0] == "exit")
                {
                    Console.WriteLine("Exiting...");
                    break;
                }

                if (commandArgs.Length != 12)
                {
                    Console.WriteLine("Invalid command. \nUsage: --file-log <log_file_path> --file-output <output_file_path> --address-start <start_ip> --address-mask <mask> --time-start <start_time> --time-end <end_time>");
                    continue;
                }

                ProcessArguments(commandArgs);
            }
        }

        // Метод для обработки аргументов и вывода данных
        static void ProcessArguments(string[] args)
        {
            try
            {
                var arguments = ParseArguments(args);
                var filteredAddresses = FilterAddresses(arguments.LogFilePath, arguments.StartTime, arguments.EndTime, arguments.StartAddress, arguments.Mask);
                var ipCounts = CountIPAddresses(filteredAddresses);
                WriteResultsToFile(arguments.OutputFilePath, ipCounts);
                Console.WriteLine("Analysis completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        // Метод для парсинга аргументов командной строки
        static Arguments ParseArguments(string[] args)
        {
            var arguments = new Arguments();
            for (int i = 0; i < args.Length; i += 2)
            {
                switch (args[i])
                {
                    case "--file-log":
                        arguments.LogFilePath = args[i + 1];
                        break;
                    case "--file-output":
                        arguments.OutputFilePath = args[i + 1];
                        break;
                    case "--address-start":
                        arguments.StartAddress = args[i + 1];
                        break;
                    case "--address-mask":
                        arguments.Mask = int.Parse(args[i + 1]);
                        break;
                    case "--time-start":
                        arguments.StartTime = DateTime.ParseExact(args[i + 1], "dd.MM.yyyy", null);
                        break;
                    case "--time-end":
                        arguments.EndTime = DateTime.ParseExact(args[i + 1], "dd.MM.yyyy", null);
                        break;
                    default:
                        Console.WriteLine($"Unknown argument: {args[i]}");
                        break;
                }
            }
            return arguments;
        }

        // Метод для фильтрации адресов по времени и диапазону адресов
        static IEnumerable<string> FilterAddresses(string logFilePath, DateTime startTime, DateTime endTime, string startAddress, int? mask)
        {
            if (string.IsNullOrEmpty(logFilePath))
            {
                throw new ArgumentNullException(nameof(logFilePath), "The path to the log file is null or empty.");
            }

            var filteredAddresses = new List<string>();
            try
            {
                using (var reader = new StreamReader(logFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split(':');
                        var address = parts[0];
                        var timeString = string.Join(":", parts.Skip(1)); // Объединяем оставшиеся части строки, чтобы получить строку времени
                        var timeParts = timeString.Split(' '); // Разбиваем строку времени на дату и время
                        var datePart = timeParts[0];
                        var timePart = timeParts[1];
                        var fullTimeString = $"{datePart} {timePart}";
                        var time = DateTime.ParseExact(fullTimeString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                        if (time >= startTime && time <= endTime && IPInRange(address, startAddress, mask))
                        {
                            filteredAddresses.Add(address);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the log file: {ex.Message}");
                // Можно обработать исключение по вашему усмотрению
            }

            return filteredAddresses;
        }


        /// Метод для проверки, находится ли IP в указанном диапазоне
        static bool IPInRange(string ip, string startAddress, int? mask)
        {
            if (mask == null)
                return true;

            var ipAddress = System.Net.IPAddress.Parse(ip).GetAddressBytes(); // Получаем байты IP-адреса
            var startIpAddress = System.Net.IPAddress.Parse(startAddress).GetAddressBytes(); // Получаем байты стартового IP-адреса
                                                                                             // Преобразуем маску в байты сетевой части
            byte[] maskBytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                if (mask > 8)
                {
                    maskBytes[i] = 255;
                    mask -= 8;
                }
                else if (mask == 8)
                {
                    maskBytes[i] = 255;
                    mask = 0;
                }
                else
                {
                    maskBytes[i] = (byte)(255 - (Math.Pow(2, 8 - (int)mask) - 1));
                    mask = 0;
                }
            }

            // Применяем маску к стартовому IP-адресу
            for (int i = 0; i < startIpAddress.Length; i++)
            {
                startIpAddress[i] = (byte)(startIpAddress[i] & maskBytes[i]);
            }

            // Сравниваем IP-адрес с примененной маской и стартовым IP-адресом
            for (int i = 0; i < ipAddress.Length; i++)
            {
                if (ipAddress[i] != startIpAddress[i])
                    return false;
            }

            return true;
        }


        // Метод для подсчета количества обращений с каждого адреса
        static Dictionary<string, int> CountIPAddresses(IEnumerable<string> addresses)
        {
            var ipCounts = new Dictionary<string, int>();
            foreach (var address in addresses)
            {
                if (!ipCounts.ContainsKey(address))
                    ipCounts[address] = 0;
                ipCounts[address]++;
            }
            return ipCounts;
        }

        // Метод для записи результатов в файл
        static void WriteResultsToFile(string filePath, Dictionary<string, int> ipCounts)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath), "The path to the output file is null or empty.");
            }

            try
            {
                using (var writer = new StreamWriter(filePath, true)) // Установите append в true для добавления данных в конец файла
                {
                    foreach (var pair in ipCounts)
                    {
                        writer.WriteLine($"{pair.Key}: {pair.Value}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while writing to the output file: {ex.Message}");
            }
        }

    }
}




