//****************************************************************************
//* Практическая работа N15                                                  *
//* Выполнил: Матченко M.C., группа 2-ИСП-оКФ                                *
//* Вариант 3                                                                *
//* Задание: Выполнение операций с объектами файловой системы                *
//****************************************************************************
using System;
using System.IO;
using System.Text;
namespace PR_15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Настройка кодировки для корректного отображения текста на русском языке
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Title = "Практическая работа № 15";
            string answer; // Переменная для хранения ответа пользователя о повторе
            do
            {
                Console.Clear(); // Очищаем экран при каждом новом старте
                string filepath = string.Empty; // путь размещения файла
                int count = 0; // количество чисел
                Random rand = new Random();
                double range_min = -50.25;
                double range_max = 10.52;
                // // 1. Запись случайных чисел в файл
                bool writeOk = false;
                try
                {
                    Console.WriteLine("Здравствуйте.\nУкажите путь нахождения файла, \nнапример: D:\\Documents\\2ИСП\\PR17_Task3.txt");
                    filepath = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(filepath))// проверка на пустой путь к файлу
                    {
                        throw new Exception("Путь к файлу не может быть пустым.");
                    }
                    FileStream F1 = new FileStream(filepath, FileMode.Create);
                    StreamWriter writer = new StreamWriter(F1, Encoding.Default);
                    Console.Write("Введите количество случайных чисел (n): ");
                    count = Int32.Parse(Console.ReadLine());
                    if (count <= 0)
                    {
                        throw new Exception("Количество чисел должно быть больше нуля.");
                    }
                    for (int i = 0; i < count; i++)
                    {
                        // Генерация случайного вещественного числа в интервале [-50.25, 10.52]
                        double number = rand.NextDouble() * (range_max - range_min) + range_min;
                        writer.Write(Math.Round(number, 2) + " "); // запись числа в поток через пробел
                    }
                    writer.Close(); // ОБЯЗАТЕЛЬНО закрываем поток
                    writeOk = true;
                    Console.WriteLine("\nФайл успешно заполнен случайными числами.");
                }
                catch (Exception ex)
                {
                    HandleError(ex);
                }
                // Если запись не удалась, сразу переходим к вопросу о повторе (вместо return)
                if (writeOk)
                {
                    // // 2. Чтение из файла и обработка данных
                    double[] array = null;
                    string tempStr;
                    try
                    {
                        Array.Resize(ref array, count);
                        Console.WriteLine($"\nСодержимое файла {filepath}:");
                        FileStream F2 = new FileStream(filepath, FileMode.Open);
                        StreamReader reader = new StreamReader(F2, Encoding.Default);
                        int globalIndex = 0; // Индекс для правильного последовательного заполнения массива
                        while ((tempStr = reader.ReadLine()) != null) // пока не достигнут конец файла
                        {
                            tempStr = tempStr.Trim();
                            Console.WriteLine(tempStr); // вывод содержимого файла на экран
                            // разбиение строки на подстроки по пробелам
                            string[] text = tempStr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < text.Length; i++)
                            {
                                // Защита от выхода за границы массива
                                if (globalIndex < array.Length)
                                {
                                    array[globalIndex] = double.Parse(text[i]);
                                    globalIndex++;
                                }
                            }
                        }
                        reader.Close(); // ОБЯЗАТЕЛЬНО закрываем поток
                        // // 3. Поиск максимального по модулю и минимального элементов
                        int maxModIndex = 0;
                        int minModIndex = 0;
                        for (int i = 1; i < array.Length; i++)
                        {
                            if (Math.Abs(array[i]) > Math.Abs(array[maxModIndex]))
                            {
                                maxModIndex = i;
                            }
                            if (Math.Abs(array[i]) < Math.Abs(array[minModIndex]))
                            {
                                minModIndex = i;
                            }
                        }
                        Console.WriteLine($"\nМаксимальный по модулю элемент: {array[maxModIndex]} (Модуль: {Math.Abs(array[maxModIndex])})");
                        Console.WriteLine($"Минимальный по модулю элемент: {array[minModIndex]} (Модуль: {Math.Abs(array[minModIndex])})");
                        // Сохраняем значение минимального по модулю для замены
                        double minModValue = array[minModIndex];
                        // 5. Замена максимального по модулю на минимальный по модулю
                        array[maxModIndex] = minModValue;
                        // // 4. Вывод измененного массива на экран
                        Console.WriteLine("\nМассив после замены максимального по модулю элемента на минимальный по модулю:");
                        for (int i = 0; i < array.Length; i++)
                        {
                            Console.Write(array[i] + " ");
                        }
                        Console.WriteLine();
                    }
                    catch (Exception ex)
                    {
                        HandleError(ex);
                    }
                }
                // Запрос на повторное выполнение программы
                Console.WriteLine("\n=========================================");
                Console.Write("Хотите запустить программу еще раз? (1 - да, любая другая клавиша - выход): ");
                answer = Console.ReadLine();
            } while (answer == "1"); // Повторяем, только если ввели "1"
            Console.WriteLine("\nРабота завершена. До свидания!");
        }
        static void HandleError(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n[ОШИБКА ОБРАБОТКИ]");
            Console.WriteLine($"Тип исключения: {ex.GetType().Name}");
            Console.WriteLine($"Сообщение: {ex.Message}");
            Console.ForegroundColor = ConsoleColor.Black;
        }
    }
}
