using System;
using System.Linq;

namespace Surface_Storage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Output output = new Output();
            DataBase data = new DataBase();
            int numberOfSurfaces = 0;
            bool check_1 = false;
            int descriptionMethod = 0;

            while (check_1 == false) // Блок задания количества поверхностей объекта
            {
                Console.Write("Введите количество поверхностей объекта.\nЕсли количество неизвестно - введите 0: ");
                check_1 = int.TryParse(Console.ReadLine(), out int num);
                if (check_1 == false || num < 0)
                {
                    Console.Write("Некорректное значение. Введите целое число\nНажмите на любую клавишу для продолжения");
                    Console.ReadKey();
                    Console.Clear();
                    check_1 = false;
                }
                numberOfSurfaces = num;
            }
            Console.Clear();
            output.text.Add("Количество поверхностей: " + (numberOfSurfaces != 0 ? numberOfSurfaces : "Уточняется далее")); // Добавление текста в шапку
            output.OutputText(output.text); // Отображение шапки

            while (true) // Блок фиксирования способа описания положения поверхности в пространства
            {
                Console.Write("Введи способ описания поверхностей\n1 - Описание через шестимерные векторы поверхностей;" +
                    "\n2 - Описание через взаимное расположение поверхностей (параллельность или перпендиулярность)\nВведите значение 1 или 2: ");
                if (int.TryParse(Console.ReadLine(), out int num) && (num == 1 || num == 2))
                {
                    descriptionMethod = num;
                    break;
                }
                Console.Write("Некорректное значение. Введите 1 или 2\nНажмите на любую клавишу для продолжения");
                Console.ReadKey();
                Console.Clear();
                output.OutputText(output.text);
            }
            Console.Clear();
            output.text.Add("Способ описания поверхностей объектов: " + (descriptionMethod == 1 ? "Шестимерные векторы поверхностей" : "Взаимное расположение поверхностей"));
            output.OutputText(output.text);

            AddingToData(ref numberOfSurfaces, descriptionMethod, output, data.dataSurfaces); // Отправление информации в метод, осуществляющий прием и фиксацию всех параметров каждой поверхности
            output.text[0] = $"Количество поверхностей: {numberOfSurfaces}";
            Console.Clear();
            output.text.Add($"Список введенных поверхностей:\n[{string.Join(" , ", data.dataSurfaces.Select(x => x.Name).ToArray())}]");
            output.OutputText(output.text);

        }


        public static void AddingToData(ref int numberOfSurfaces, int descriptionMethod, Output output, List<Surfaces> data) // Метод, осуществляющий прием и фиксацию имени поверхности
        {
            output.temporaryNotification = new List<string>(output.text); // Шапка с временным содержанием (наименование поверхности, шероховатость, вектор ориентации в пространстве и т.д.)
            string? input;
            int count = numberOfSurfaces;
            int n = numberOfSurfaces == 0 ? numberOfSurfaces : numberOfSurfaces - 1;
            while (n >= 0) // Фиксированние наименования поверхности
            {
                Console.Write("Введите наименование поверхности\nВведите \"end\", если желаете прекратить" +
                    "\nДопускается ввод только первой буквы наименования или слова целиком: ");
                input = Console.ReadLine();
                input = String.IsNullOrEmpty(input) ? input : input.ToLower();
                if (input == "end") // Прерывание метода
                {
                    numberOfSurfaces = data.Count();
                    break;
                }
                switch (input)
                {
                    case "с":
                    case "сфера":
                        Surfaces sphere = new Sphere();
                        AddingSurfaceParameters(sphere, data, "Сфера", output, descriptionMethod); // Метод, осуществляющий прием и фиксацию всех параметров каждой поверхности
                        break;
                    case "ц":
                    case "цилиндр":
                        Surfaces cylinder = new Cylinder();
                        AddingSurfaceParameters(cylinder, data, "Цилиндр", output, descriptionMethod); // Метод, осуществляющий прием и фиксацию всех параметров каждой поверхности
                        break;
                    case "п":
                    case "плоскость":
                        Surfaces plane = new Plane();
                        AddingSurfaceParameters(plane, data, "Плоскость", output, descriptionMethod); // Метод, осуществляющий прием и фиксацию всех параметров каждой поверхности
                        break;
                    default:
                        Console.Write("Введите корректное наименование поверхности\nНажмите на любую клавишу для продолжения");
                        Console.ReadKey();
                        Console.Clear();
                        output.OutputText(output.temporaryNotification);
                        continue;
                }
                Console.Clear();
                output.OutputText(output.temporaryNotification);
                Console.WriteLine("Успешно добавлено!\nНажмите любую клавишу чтобы продолжить"); // Оповещение об успешном фиксировании параметров поверхности
                Console.ReadKey();
                Console.Clear();
                output.OutputText(output.text);

                output.temporaryNotification = new List<string>(output.text); // Сброс содержания временной шапки (перед заданием новой поверхности)
                if (count != 0) // Счетчик поверхностей, если изначально определено их количество
                    n--;
            }
        }


        public static void AddingSurfaceParameters(Surfaces surface, List<Surfaces> data, string form, Output output, int descriptionMethod) // Метод, осуществляющий прием и фиксацию всех параметров каждой поверхности
        {
            //double roughness = 0;
            int newnum = data.Where(s => s != null && s.Name.StartsWith(form)).Count() + 1; // Поиск следующего идентификатора для каждого типа поверхности (если есть Сфера1, то новой поверхности будет присвоен идентификатор 2. Для других аналогично)
            surface.Name = form + newnum;
            List<Surfaces> temporaryStorage = new List<Surfaces>(data.Where(s => !s.Name.StartsWith("Сфера"))); // Создание временного хранилища с ранее добавленными поверхностями кроме сфер

            data.Add(surface); // Добавление поверхности в постоянное (во время выполнения программы) хранилище

            //Console.WriteLine(surface.Name);
            Console.Clear();
            output.temporaryNotification.Add($"\nВводимая поверхность: {form}");
            output.OutputText(output.temporaryNotification);
            while (true) // Блок задани шероховатости поверхности
            {
                //Console.WriteLine($"Временное хранилище, объем: {temporaryStorage.Count()}"); // Для проверки
                Console.Write("Введите среднее арифметическое отклонение профиля поверхности (шероховатость Ra) в мкм." +
                        "\nПо-умолчанию (при отсутствии вводимого значения) поверхности присваивается Ra 6.3: ");
                string? rough = Console.ReadLine();
                if (String.IsNullOrEmpty(rough))
                    break;
                else if (double.TryParse(rough, out double r) && r > 0)
                {
                    surface.Roughness = r;
                    break;
                }
                else
                {
                    Console.Write("Введите корректное значение шероховатости поверхности\nНажмите на любую клавишу для продолжения");
                    Console.ReadKey();
                    Console.Clear();
                    output.OutputText(output.temporaryNotification);
                    continue;
                }
            }
            Console.Clear();
            output.temporaryNotification.Add($"Шероховатость поверхности: Ra {surface.Roughness}");
            output.OutputText(output.temporaryNotification);

            if (form == "Сфера" || form == "Цилиндр") // Блок с заданием диаметра поверхности и внутренним/наружным её состоянием. Только для сферы и цилиндра 
            {
                while (true) // Блок задания диаметра поверхности
                {
                    Console.Write("Введите диаметр поверхности в миллиметрах: ");
                    string? diameter = Console.ReadLine();
                    if (double.TryParse(diameter, out double d) && d > 0)
                    {
                        surface.Diameter = d;
                        break;
                    }
                    else
                    {
                        Console.Write("Введите корректное значение диаметра поверхности\nНажмите на любую клавишу для продолжения");
                        Console.ReadKey();
                        Console.Clear();
                        output.OutputText(output.temporaryNotification);
                        continue;
                    }
                }
                Console.Clear();
                output.temporaryNotification.Add($"Диаметр поверхности: {surface.Diameter} мм");
                output.OutputText(output.temporaryNotification);

                while (true) // Блок задания состояния расположения поверхности (внутреннее/наружное)
                {
                    Console.Write("Введите состояние расположения поверхности относительно детали. Внутренняя или Наружная: ");
                    string? location = Console.ReadLine();
                    if (!String.IsNullOrEmpty(location) && (location.Equals("внутренняя", StringComparison.CurrentCultureIgnoreCase) || location.Equals("наружная", StringComparison.CurrentCultureIgnoreCase)))
                    {
                        surface.Location = location.ToLower() == "внутренняя" ? "Внутренняя" : "Наружная";
                        break;
                    }
                    else
                    {
                        Console.Write("Введите корректное состояние расположения поверхности относительно детали\nНажмите на любую клавишу для продолжения");
                        Console.ReadKey();
                        Console.Clear();
                        output.OutputText(output.temporaryNotification);
                        continue;
                    }
                }
                Console.Clear();
                output.temporaryNotification.Add($"Состояние расположения поверхности относительно детали: {surface.Location}");
                output.OutputText(output.temporaryNotification);
            }

            while (true) // Блок задания отклонений формы поверхности
            {
                if (form == "Плоскость")
                    Console.Write("Введите наименование отклонения от формы поверхности и его значение через пробел в миллиметрах.\n" +
                        "Доступные варианты:\nПлоскостность;\nПрямолинейность.\nЧтобы прекратить ввод отклонений форм поверхностей - введите 0:");
                else if (form == "Цилиндр")
                    Console.Write("Введите наименование отклонения от формы поверхности и его значение через пробел в миллиметрах.\n" +
                        "Доступные варианты:\nЦилиндричность;\nПрямолинейность;\nКруглость.\nЧтобы прекратить ввод отклонений форм поверхностей - введите 0:");
                else if (form == "Сфера")
                    Console.Write("Введите наименование отклонения от формы поверхности и его значение через пробел в миллиметрах.\n" +
                        "Доступные варианты:\nКруглость.\nЧтобы прекратить ввод отклонений форм поверхностей - введите 0:");

                List<string> list = Console.ReadLine().Trim().Split(' ').ToList();

                if (list[0] == "0") // Отработка прерывания блока задания отклонений
                    break;
                if (list.Count() == 2) // Выполнение блока при удовлетворении требований к входным данным
                {
                    if (double.TryParse(list[1], out double v) && v > 0 && (list[0].Equals("Плоскостность", StringComparison.CurrentCultureIgnoreCase) ||
                        list[0].Equals("Цилиндричность", StringComparison.CurrentCultureIgnoreCase) || list[0].Equals("Прямолинейность", StringComparison.CurrentCultureIgnoreCase) ||
                        list[0].Equals("Круглость", StringComparison.CurrentCultureIgnoreCase)))
                    {
                        string? deviation_name = list[0];
                        double deviation_value = v;

                        if (form == "Плоскость") // Задание отклонений формы для плоскости
                        {
                            if (deviation_name.Equals("Плоскостность", StringComparison.CurrentCultureIgnoreCase))
                            {
                                surface.Flatness = deviation_value;
                                if (output.temporaryNotification.Where(x => x.StartsWith("Отклонение от Плоскостности:")).ToList().Count() == 1)
                                {
                                    int index = output.temporaryNotification.IndexOf(output.temporaryNotification.Where(x => x.StartsWith("Отклонение от Плоскостности:")).FirstOrDefault());
                                    output.temporaryNotification[index] = $"Отклонение от Плоскостности: {surface.Flatness} мм";
                                }
                                else
                                {
                                    output.temporaryNotification.Add($"Отклонение от Плоскостности: {surface.Flatness} мм");
                                }
                            }
                            else if (deviation_name.Equals("Прямолинейность", StringComparison.CurrentCultureIgnoreCase))
                            {
                                surface.Straightness = deviation_value;
                                if (output.temporaryNotification.Where(x => x.StartsWith("Отклонение от Прямолинейности:")).ToList().Count() == 1)
                                {
                                    int index = output.temporaryNotification.IndexOf(output.temporaryNotification.Where(x => x.StartsWith("Отклонение от Прямолинейности:")).FirstOrDefault());
                                    output.temporaryNotification[index] = $"Отклонение от Прямолинейности: {surface.Straightness} мм";
                                }
                                else
                                {
                                    output.temporaryNotification.Add($"Отклонение от Прямолинейности: {surface.Straightness} мм");
                                }
                            }
                            else
                            {
                                Console.Write("Для Плоскости могут быть назначены только отклонения от Плоскостности и/или Прямолинейности" +
                                    "\nНажмите на любую клавишу для продолжения");
                                Console.ReadKey();
                                Console.Clear();
                                output.OutputText(output.temporaryNotification);
                                continue;
                            }
                        }

                        else if (form == "Цилиндр") // Задание отклонений формы для цилиндра
                        {
                            if (deviation_name.Equals("Цилиндричность", StringComparison.CurrentCultureIgnoreCase))
                            {
                                surface.Cylindrical = deviation_value;
                                if (output.temporaryNotification.Where(x => x.StartsWith("Отклонение от Цилиндричности:")).ToList().Count() == 1)
                                {
                                    int index = output.temporaryNotification.IndexOf(output.temporaryNotification.Where(x => x.StartsWith("Отклонение от Цилиндричности:")).FirstOrDefault());
                                    output.temporaryNotification[index] = $"Отклонение от Цилиндричности: {surface.Cylindrical} мм";
                                }
                                else
                                {
                                    output.temporaryNotification.Add($"Отклонение от Цилиндричности: {surface.Cylindrical} мм");
                                }
                            }
                            else if (deviation_name.Equals("Прямолинейность", StringComparison.CurrentCultureIgnoreCase))
                            {
                                surface.Straightness = deviation_value;
                                if (output.temporaryNotification.Where(x => x.StartsWith("Отклонение от Прямолинейности:")).ToList().Count() == 1)
                                {
                                    int index = output.temporaryNotification.IndexOf(output.temporaryNotification.Where(x => x.StartsWith("Отклонение от Прямолинейности:")).FirstOrDefault());
                                    output.temporaryNotification[index] = $"Отклонение от Прямолинейности: {surface.Straightness} мм";
                                }
                                else
                                {
                                    output.temporaryNotification.Add($"Отклонение от Прямолинейности: {surface.Straightness} мм");
                                }
                            }
                            else if (deviation_name.Equals("Круглость", StringComparison.CurrentCultureIgnoreCase))
                            {
                                surface.Roughness = deviation_value;
                                if (output.temporaryNotification.Where(x => x.StartsWith("Отклонение от Круглости:")).ToList().Count() == 1)
                                {
                                    int index = output.temporaryNotification.IndexOf(output.temporaryNotification.Where(x => x.StartsWith("Отклонение от Круглости:")).FirstOrDefault());
                                    output.temporaryNotification[index] = $"Отклонение от Круглости: {surface.Roughness} мм";
                                }
                                else
                                {
                                    output.temporaryNotification.Add($"Отклонение от Круглости: {surface.Roughness} мм");
                                }
                            }
                            else
                            {
                                Console.Write("Для Цилиндра могут быть назначены только отклонения от Цилиндричности и/или Прямолинейности и/или Круглости" +
                                        "\nНажмите на любую клавишу для продолжения");
                                Console.ReadKey();
                                Console.Clear();
                                output.OutputText(output.temporaryNotification);
                                continue;
                            }
                        }
                        else if (form == "Сфера") // Задание отклонений формы для сферы
                        {
                            if (deviation_name.Equals("Круглость", StringComparison.CurrentCultureIgnoreCase) == false)
                            {
                                Console.Write("Для Сферы может быть назначено только отклонение от Круглости" +
                                            "\nНажмите на любую клавишу для продолжения");
                                Console.ReadKey();
                                Console.Clear();
                                output.OutputText(output.temporaryNotification);
                                continue;
                            }
                            surface.Roughness = deviation_value;
                            if (output.temporaryNotification.Where(x => x.StartsWith("Отклонение от Круглости:")).ToList().Count() == 1)
                            {
                                int index = output.temporaryNotification.IndexOf(output.temporaryNotification.Where(x => x.StartsWith("Отклонение от Круглости:")).FirstOrDefault());
                                output.temporaryNotification[index] = $"Отклонение от Круглости: {surface.Roughness} мм";
                            }
                            else
                            {
                                output.temporaryNotification.Add($"Отклонение от Круглости: {surface.Roughness} мм");
                            }
                        }
                    }

                    else if (!list[0].Equals("Плоскостность", StringComparison.CurrentCultureIgnoreCase) &&
                        !list[0].Equals("Цилиндричность", StringComparison.CurrentCultureIgnoreCase) &&
                        !list[0].Equals("Прямолинейность", StringComparison.CurrentCultureIgnoreCase) &&
                        !list[0].Equals("Круглость", StringComparison.CurrentCultureIgnoreCase)) // Отработка исключений
                    {
                        Console.Write("Вы ввели недопстимое наименование отклонения формы. Ознакомьтесь с доступными вариантами" +
                                           "\nНажмите на любую клавишу для продолжения");
                        Console.ReadKey();
                        Console.Clear();
                        output.OutputText(output.temporaryNotification);
                        continue;
                    }
                    else if (v < 0) // Отработка исключений
                    {
                        Console.Write("Значение отклонения не может быть меньше 0." +
                                           "\nНажмите на любую клавишу для продолжения");
                        Console.ReadKey();
                        Console.Clear();
                        output.OutputText(output.temporaryNotification);
                        continue;
                    }
                    else // Отработка исключений
                    {
                        Console.Write("Значение отклонения должно быть положительным и численным (целым или дробным, к примеру, 1 или 0,05) " +
                            "Дробные значения указываются через запятую, а не точку." +
                                           "\nНажмите на любую клавишу для продолжения");
                        Console.ReadKey();
                        Console.Clear();
                        output.OutputText(output.temporaryNotification);
                        continue;
                    }
                }
                else if (list.Count() < 2)
                {
                    Console.Write("Вы ввели недостаточно данных либо ничего не указали. Необходимо ввести наименование отклонения и через пробел указать его значение" +
                                            "\nНажмите на любую клавишу для продолжения");
                    Console.ReadKey();
                    Console.Clear();
                    output.OutputText(output.temporaryNotification);
                    continue;
                }
                else
                {
                    Console.Write("Вы ввели избыточное количество данных. Необходимо только ввести наименование отклонения и через пробел указать его значение" +
                                            "\nНажмите на любую клавишу для продолжения");
                    Console.ReadKey();
                    Console.Clear();
                    output.OutputText(output.temporaryNotification);
                    continue;
                }
                //Console.WriteLine("Данные успешно зафиксированы!\nНажмите на любую клавишу чтобы продолжить");
                //Console.ReadKey();
                Console.Clear();
                output.OutputText(output.temporaryNotification);
            }
            Console.Clear();
            output.OutputText(output.temporaryNotification);



            if (descriptionMethod == 1) // Блок задания ориентации поверхностей через шестимерный вектор
            {
                while (true)
                {
                    if (form == "Сфера") // Ориентация сферы
                    {
                        Console.WriteLine("Шестимерный вектор для Сферы задан - 1, 1, 1, 0, 0, 0.\nНажмите любую клавишу чтобы продолжить");
                        surface.DegreesOfFreedom = new List<int>() { 1, 1, 1, 0, 0, 0 };
                        output.temporaryNotification.Add($"Шестимерный вектор ориентации поверхности в пространстве: {string.Join(',', surface.DegreesOfFreedom)}");
                        Console.ReadKey();
                        Console.Clear();
                        output.OutputText(output.temporaryNotification);
                        break;
                    }
                    else if (form == "Плоскость") // Ориентация плоскости
                    {
                        Console.WriteLine("Выберите шестимерный вектор, характеризующий ориентацию поверхности в пространстве");

                        Console.Write("Доступные варианты:\n1 - 1,0,0,0,1,1\n2 - 0,1,0,1,0,1\n3 - 0,0,1,1,1,0\nВведите цифру, которая соответствует подходящему варианту: ");
                        string? choice = Console.ReadLine();
                        if (int.TryParse(choice, out int num))
                        {
                            switch (num)
                            {
                                case 1:
                                    {
                                        //Console.WriteLine("Шестимерный вектор для Плоскости задан.\nНажмите любую клавишу чтобы продолжить");
                                        surface.DegreesOfFreedom = new List<int>() { 1, 0, 0, 0, 1, 1 };
                                        output.temporaryNotification.Add($"Шестимерный вектор ориентации поверхности в пространстве: {string.Join(',', surface.DegreesOfFreedom)}");
                                        //Console.ReadKey();
                                        Console.Clear();
                                        output.OutputText(output.temporaryNotification);
                                        break;
                                    }
                                case 2:
                                    {
                                        //Console.WriteLine("Шестимерный вектор для Плоскости задан.\nНажмите любую клавишу чтобы продолжить");
                                        surface.DegreesOfFreedom = new List<int>() { 0, 1, 0, 1, 0, 1 };
                                        output.temporaryNotification.Add($"Шестимерный вектор ориентации поверхности в пространстве: {string.Join(',', surface.DegreesOfFreedom)}");
                                        //Console.ReadKey();
                                        Console.Clear();
                                        output.OutputText(output.temporaryNotification);
                                        break;
                                    }
                                case 3:
                                    {
                                        //Console.WriteLine("Шестимерный вектор для Плоскости задан.\nНажмите любую клавишу чтобы продолжить");
                                        surface.DegreesOfFreedom = new List<int>() { 0, 0, 1, 1, 1, 0 };
                                        output.temporaryNotification.Add($"Шестимерный вектор ориентации поверхности в пространстве: {string.Join(',', surface.DegreesOfFreedom)}");
                                        //Console.ReadKey();
                                        Console.Clear();
                                        output.OutputText(output.temporaryNotification);
                                        break;
                                    }
                                default:
                                    {
                                        Console.WriteLine("Введено некорректное значение выбора. Введите 1, 2 или 3\nНажмите на любую клавишу для продолжения");
                                        Console.ReadKey();
                                        Console.Clear();
                                        output.OutputText(output.temporaryNotification);
                                        continue;
                                    }
                            }
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Необходимо ввести целое число 1, 2 или 3.\nНажмите на любую клавишу для продолжения");
                            Console.ReadKey();
                            Console.Clear();
                            output.OutputText(output.temporaryNotification);
                            continue;
                        }

                    }
                    else if (form == "Цилиндр") // Ориентация цилиндра
                    {
                        Console.WriteLine("Выберите шестимерный вектор, характеризующий ориентацию поверхности в пространстве");

                        Console.Write("Доступные варианты:\n1 - 1,1,0,1,1,0\n2 - 0,1,1,0,1,1\n3 - 1,0,1,1,0,1\nВведите цифру, которая соответствует подходящему варианту: ");
                        string? choice = Console.ReadLine();
                        if (int.TryParse(choice, out int num))
                        {
                            switch (num)
                            {
                                case 1:
                                    {
                                        //Console.WriteLine("Шестимерный вектор для Плоскости задан.\nНажмите любую клавишу чтобы продолжить");
                                        surface.DegreesOfFreedom = new List<int>() { 1, 1, 0, 1, 1, 0 };
                                        output.temporaryNotification.Add($"Шестимерный вектор ориентации поверхности в пространстве: {string.Join(',', surface.DegreesOfFreedom)}");
                                        //Console.ReadKey();
                                        Console.Clear();
                                        output.OutputText(output.temporaryNotification);
                                        break;
                                    }
                                case 2:
                                    {
                                        //Console.WriteLine("Шестимерный вектор для Плоскости задан.\nНажмите любую клавишу чтобы продолжить");
                                        surface.DegreesOfFreedom = new List<int>() { 0, 1, 1, 0, 1, 1 };
                                        output.temporaryNotification.Add($"Шестимерный вектор ориентации поверхности в пространстве: {string.Join(',', surface.DegreesOfFreedom)}");
                                        //Console.ReadKey();
                                        Console.Clear();
                                        output.OutputText(output.temporaryNotification);
                                        break;
                                    }
                                case 3:
                                    {
                                        //Console.WriteLine("Шестимерный вектор для Плоскости задан.\nНажмите любую клавишу чтобы продолжить");
                                        surface.DegreesOfFreedom = new List<int>() { 1, 0, 1, 1, 0, 1 };
                                        output.temporaryNotification.Add($"Шестимерный вектор ориентации поверхности в пространстве: {string.Join(',', surface.DegreesOfFreedom)}");
                                        //Console.ReadKey();
                                        Console.Clear();
                                        output.OutputText(output.temporaryNotification);
                                        break;
                                    }
                                default:
                                    {
                                        Console.WriteLine("Введено некорректное значение выбора. Введите 1, 2 или 3\nНажмите на любую клавишу для продолжения");
                                        Console.ReadKey();
                                        Console.Clear();
                                        output.OutputText(output.temporaryNotification);
                                        continue;
                                    }
                            }
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Необходимо ввести целое число 1, 2 или 3.\nНажмите на любую клавишу для продолжения");
                            Console.ReadKey();
                            Console.Clear();
                            output.OutputText(output.temporaryNotification);
                            continue;
                        }
                    }
                }
            }

            else if (descriptionMethod == 2) // Блок задания ориентации поверхности через взаимное расположение (перпендикулярность и параллельность)
            {
                while (true)
                {
                    if (form == "Сфера") // Ориентация сферы
                    {
                        Console.WriteLine("Шестимерный вектор ориентации для Сферы задан - 1, 1, 1, 0, 0, 0.\nНажмите любую клавишу чтобы продолжить");
                        surface.DegreesOfFreedom = new List<int>() { 1, 1, 1, 0, 0, 0 };
                        output.temporaryNotification.Add($"Шестимерный вектор ориентации поверхности в пространстве: {string.Join(',', surface.DegreesOfFreedom)}");
                        Console.ReadKey();
                        Console.Clear();
                        output.OutputText(output.temporaryNotification);
                        break;
                    }
                    else if (form == "Плоскость") // Ориентация плоскости
                    {
                        if (temporaryStorage.Count() == 0) // Ситуация, когда задаваемая поверхность первая или ранее заданные поверхности сферы (невозможно задать параллельность или перпендикулярность)
                        {
                            Console.Write("Выберите подходящий вариант." +
                                "\n1 - плоскость перпендикулярна оси X (вектор ориентации - 1, 0, 0, 0, 1, 1)" +
                                "\n2 - плоскость перпендикулярна оси Y (вектор ориентации - 0, 1, 0, 1, 0, 1)" +
                                "\n3 - плоскость перпендикулярна оси Z (вектор ориентации - 0, 0, 1, 1, 1, 0)" +
                                "\n4 - задать ориентацию плоскости автоматизированно программой" +
                                "\nВвести нужно только подходящий номер: ");
                            string? choiceNum = Console.ReadLine();

                            if (int.TryParse(choiceNum, out int num))
                            {
                                switch (num)
                                {
                                    case 1:
                                        {
                                            surface.DegreesOfFreedom = new List<int>() { 1, 0, 0, 0, 1, 1 };
                                            break;
                                        }
                                    case 2:
                                        {
                                            surface.DegreesOfFreedom = new List<int>() { 0, 1, 0, 1, 0, 1 };
                                            break;
                                        }
                                    case 3: case 4:
                                        {
                                            surface.DegreesOfFreedom = new List<int>() { 0, 0, 1, 1, 1, 0 };
                                            break;
                                        }
                                    default:
                                        {
                                            Console.WriteLine("Введено некорректное значение выбора. Введите целое число от 1 до 4 включительно" +
                                                "\nНажмите на любую клавишу для продолжения");
                                            Console.ReadKey();
                                            Console.Clear();
                                            output.OutputText(output.temporaryNotification);
                                            continue;
                                        }

                                }
                                output.temporaryNotification.Add($"Шестимерный вектор ориентации поверхности в пространстве: {string.Join(',', surface.DegreesOfFreedom)}");
                                Console.Clear();
                                output.OutputText(output.temporaryNotification);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Необходимо ввести целое число 1, 2, 3 или 4.\nНажмите на любую клавишу для продолжения");
                                Console.ReadKey();
                                Console.Clear();
                                output.OutputText(output.temporaryNotification);
                                continue;
                            }
                        }

                        else // Ситуация наличия 1 и более ранее занесенных поверхностей кроме сферы
                        {
                            while (true)
                            {
                                Console.WriteLine("Выберите поверхность, относительно которой желаете задать параллельность или перпендикулярность.");
                                for (int i = 0; i < temporaryStorage.Count(); i++) // Представление всех введенных ранее поверхностей кроме сфер
                                    Console.WriteLine($"{i + 1} - {temporaryStorage[i].Name}");
                                Console.Write("Введите номер, соответствующий подходящей поверхности: ");
                                string? choiceSurface = Console.ReadLine(); // Ввод пользователем значения

                                if (int.TryParse(choiceSurface, out int choice) && choice <= temporaryStorage.Count() && choice > 0 && choice % 1 == 0) // Выбор и фиксирование индекса подходящей поверхности (не забыть отнять 1)
                                {
                                    Console.Write("Выберете способ взаимной ориентации.\n1 - Параллельность\n2 - Перпендикулярность\nВведите номер подходящего способа: ");
                                    string? numberOfMethod = Console.ReadLine();

                                    if (int.TryParse(numberOfMethod, out int number) && (number == 1 || number == 2)) // Выбор и фиксирование способа задания взаимной связи (параллельность или перпендикулярность)
                                    {
                                        List<int> surfaceDegreesOfFreedom = temporaryStorage[choice - 1].DegreesOfFreedom; // Присваивание переменной значений вектора поверхности, с которой выстраиваются взамоотношения

                                        List<int> perpendicularSurface = new List<int>(); // Шестимерный вектор перпендикулярной поверхности
                                        for (int i = 0; i < 6; i++) // Заполнение вектора
                                            perpendicularSurface.Add(i > 2 ? surfaceDegreesOfFreedom[i] : (surfaceDegreesOfFreedom[i] == 0 ? 1 : 0));

                                        List<Surfaces> surfacesWithoutParallel = temporaryStorage.Where(degrees => !degrees.DegreesOfFreedom.SequenceEqual(surfaceDegreesOfFreedom) && !degrees.DegreesOfFreedom.SequenceEqual(perpendicularSurface)).ToList(); // Набор поверхностей, которые отличаются шестимерной таблицей от выбранной поверхности и перпендикулярной ей плоскости/цилиндру

                                        if (number == 2 && (temporaryStorage[choice - 1].Name.StartsWith("Цилиндр"))) // Плоскость перпендикулярна цилиндру
                                        {
                                            surface.DegreesOfFreedom = new List<int>();
                                            for (int i = 0; i < 6; i++)
                                                surface.DegreesOfFreedom.Add(i > 2 ? surfaceDegreesOfFreedom[i] : (surfaceDegreesOfFreedom[i] == 0 ? 1 : 0));
                                            break;
                                        }

                                        else if (number == 1 && (temporaryStorage[choice - 1].Name.StartsWith("Плоскость"))) // Плоскость параллельна плоскости
                                        {
                                            surface.DegreesOfFreedom = new List<int>(surfaceDegreesOfFreedom);
                                            break;
                                        }
                                        else if (number == 1 && (temporaryStorage.Count() == 1 || surfacesWithoutParallel.Count() == 0) && (temporaryStorage[choice - 1].Name.StartsWith("Цилиндр"))) // Плоскость параллельна цилиндру, но других поверхностей больше нет
                                        {
                                            if (surfaceDegreesOfFreedom.SequenceEqual(new List<int> { 1, 1, 0, 1, 1, 0 }))
                                            {
                                                surface.DegreesOfFreedom = new List<int> { 1, 0, 0, 0, 1, 1 };
                                            }
                                            else if (surfaceDegreesOfFreedom.SequenceEqual(new List<int> { 0, 1, 1, 0, 1, 1 }))
                                            {
                                                surface.DegreesOfFreedom = new List<int> { 0, 1, 0, 1, 0, 1 };
                                            }
                                            else
                                            {
                                                surface.DegreesOfFreedom = new List<int> { 0, 0, 1, 1, 1, 0 };
                                            }
                                            break;
                                        }
                                        else if (number == 2 && (temporaryStorage.Count() == 1 || surfacesWithoutParallel.Count() == 0) && (temporaryStorage[choice - 1].Name.StartsWith("Плоскость"))) // Плоскость перпендикулярна плоскости, но других поверхностей больше нет
                                        {
                                            if (surfaceDegreesOfFreedom.SequenceEqual(new List<int> { 0, 1, 0, 1, 0, 1 }))
                                            {
                                                surface.DegreesOfFreedom = new List<int> { 1, 0, 0, 0, 1, 1 };
                                            }
                                            else if (surfaceDegreesOfFreedom.SequenceEqual(new List<int> { 0, 0, 1, 1, 1, 0 }))
                                            {
                                                surface.DegreesOfFreedom = new List<int> { 0, 1, 0, 1, 0, 1 };
                                            }
                                            else
                                            {
                                                surface.DegreesOfFreedom = new List<int> { 0, 0, 1, 1, 1, 0 };
                                            }
                                            break;

                                        }

                                        else if ((number == 1 && (temporaryStorage[choice - 1].Name.StartsWith("Цилиндр"))) ||
                                            (number == 2 && (temporaryStorage[choice - 1].Name.StartsWith("Плоскость")))) // Если плоскость перпендикулярна плоскости или плоскость параллельна цилиндру
                                        {
                                            while (true)
                                            {
                                                Console.Clear();
                                                output.OutputText(output.temporaryNotification);

                                                Console.WriteLine($"Поверхность для построения связи: {temporaryStorage[choice - 1].Name} ({string.Join(',', surfaceDegreesOfFreedom)}). Тип сопряжения: " + (number == 1 ? "Параллельность" : "Перпендикулярность"));
                                                Console.WriteLine("\nПолученной информации недостаточно.\nНеобходимо выбрать дополнительную поверхность и взаимную ориантацию\n");
                                                for (int i = 0; i < surfacesWithoutParallel.Count(); i++) // Представление всех введенных ранее поверхностей кроме сфер
                                                    Console.WriteLine($"{i+1} - {surfacesWithoutParallel[i].Name}");
                                                Console.Write("Введите номер, соответствующий подходящей поверхности: ");
                                                string? choiceSurface2 = Console.ReadLine(); // Ввод пользователем значения

                                                if (int.TryParse(choiceSurface2, out int choice2) && choice2 <= surfacesWithoutParallel.Count() && choice2 > 0 && choice2 % 1 == 0) // Выбор и фиксирование индекса подходящей поверхности 
                                                {
                                                    Console.Write("Выберете способ взаимной ориентации.\n1 - Параллельность\n2 - Перпендикулярность\nВведите номер подходящего способа: ");
                                                    string? numberOfMethod2 = Console.ReadLine();

                                                    if (int.TryParse(numberOfMethod2, out int number2) && (number2 == 1 || number2 == 2)) // Выбор и фиксирование способа задания взаимной связи (параллельность или перпендикулярность)
                                                    {
                                                        if (number2 == 2 && (surfacesWithoutParallel[choice2 - 1].Name.StartsWith("Цилиндр"))) // Плоскость перпендикулярна цилиндру
                                                        {
                                                            surface.DegreesOfFreedom = new List<int>();
                                                            for (int i = 0; i < 6; i++)
                                                                surface.DegreesOfFreedom.Add(i > 2 ? surfacesWithoutParallel[choice2 - 1].DegreesOfFreedom[i]
                                                                    : (surfacesWithoutParallel[choice2 - 1].DegreesOfFreedom[i] == 0 ? 1 : 0));
                                                            break;
                                                        }
                                                        else if (number2 == 1 && (surfacesWithoutParallel[choice2 - 1].Name.StartsWith("Плоскость"))) // Плоскость параллельна плоскости
                                                        {
                                                            surface.DegreesOfFreedom = new List<int>(surfacesWithoutParallel[choice2 - 1].DegreesOfFreedom);
                                                            break;
                                                        }
                                                        else if (number2 == 2 && (temporaryStorage[choice - 1].Name.StartsWith("Плоскость")) && (surfacesWithoutParallel[choice2 - 1].Name.StartsWith("Плоскость"))) // Плоскость перпендикулярна плоскости и в прошлый раз было тоже самое
                                                        {
                                                            List<int> indicators = new List<int>();
                                                            for (int i = 0; i < 3; i++)
                                                                indicators.Add(surfacesWithoutParallel[choice2 - 1].DegreesOfFreedom[i] + surfaceDegreesOfFreedom[i]);
                                                            if (indicators[0] == 0)
                                                            {
                                                                surface.DegreesOfFreedom = new List<int> { 1, 0, 0, 0, 1, 1 };
                                                            }
                                                            else if (indicators[1] == 0)
                                                            {
                                                                surface.DegreesOfFreedom = new List<int> { 0, 1, 0, 1, 0, 1 };
                                                            }
                                                            else if (indicators[2] == 0)
                                                            {
                                                                surface.DegreesOfFreedom = new List<int> { 0, 0, 1, 1, 1, 0 };
                                                            }
                                                            break;
                                                        }
                                                        else if ((number2 == 1 && (surfacesWithoutParallel[choice2 - 1].Name.StartsWith("Цилиндр")) && (temporaryStorage[choice - 1].Name.StartsWith("Плоскость"))) ||
                                                            (number2 == 2 && (surfacesWithoutParallel[choice2 - 1].Name.StartsWith("Плоскость")) && (temporaryStorage[choice - 1].Name.StartsWith("Цилиндр")))) // Плоскость параллельна цилиндру, в прошлый раз плоскость перпендикулярна плоскости. Также, Плоскость перпендикулярна плоскости, в прошлый раз плоскость параллельна цилиндру
                                                        {
                                                            List<int> indicators = new List<int>();
                                                            for (int i = 0; i < 3; i++)
                                                                indicators.Add(surfacesWithoutParallel[choice2 - 1].DegreesOfFreedom[i] + surfaceDegreesOfFreedom[i]);
                                                            if (indicators[0] == 1)
                                                            {
                                                                surface.DegreesOfFreedom = new List<int> { 1, 0, 0, 0, 1, 1 };
                                                            }
                                                            else if (indicators[1] == 1)
                                                            {
                                                                surface.DegreesOfFreedom = new List<int> { 0, 1, 0, 1, 0, 1 };
                                                            }
                                                            else if (indicators[2] == 1)
                                                            {
                                                                surface.DegreesOfFreedom = new List<int> { 0, 0, 1, 1, 1, 0 };
                                                            }
                                                            break;
                                                        }
                                                        else if (number2 == 1 && (surfacesWithoutParallel[choice2 - 1].Name.StartsWith("Цилиндр")) && (temporaryStorage[choice - 1].Name.StartsWith("Цилиндр"))) // Плоскость параллельна цилиндру, в прошлый раз было тоже самое
                                                        {
                                                            List<int> indicators = new List<int>();
                                                            for (int i = 0; i < 3; i++)
                                                                indicators.Add(surfacesWithoutParallel[choice2 - 1].DegreesOfFreedom[i] + surfaceDegreesOfFreedom[i]);
                                                            if (indicators[0] == 2)
                                                            {
                                                                surface.DegreesOfFreedom = new List<int> { 1, 0, 0, 0, 1, 1 };
                                                            }
                                                            else if (indicators[1] == 2)
                                                            {
                                                                surface.DegreesOfFreedom = new List<int> { 0, 1, 0, 1, 0, 1 };
                                                            }
                                                            else if (indicators[2] == 2)
                                                            {
                                                                surface.DegreesOfFreedom = new List<int> { 0, 0, 1, 1, 1, 0 };
                                                            }
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Необходимо ввести целое число 1 или 2.\nНажмите на любую клавишу для продолжения");
                                                        Console.ReadKey();
                                                        Console.Clear();
                                                        output.OutputText(output.temporaryNotification);
                                                        continue;
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine($"Необходимо ввести целое число от 1 до {surfacesWithoutParallel.Count()} включительно.\nНажмите на любую клавишу для продолжения");
                                                    Console.ReadKey();
                                                    Console.Clear();
                                                    output.OutputText(output.temporaryNotification);
                                                    continue;
                                                }
                                                break;
                                            }
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Необходимо ввести целое число 1 или 2.\nНажмите на любую клавишу для продолжения");
                                        Console.ReadKey();
                                        Console.Clear();
                                        output.OutputText(output.temporaryNotification);
                                        continue;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Необходимо ввести целое число от 1 до {temporaryStorage.Count()} включительно.\nНажмите на любую клавишу для продолжения");
                                    Console.ReadKey();
                                    Console.Clear();
                                    output.OutputText(output.temporaryNotification);
                                    continue;
                                }
                                break;
                            }
                        }
                        output.temporaryNotification.Add($"Шестимерный вектор ориентации поверхности в пространстве: {string.Join(',', surface.DegreesOfFreedom)}");
                        Console.Clear();
                        output.OutputText(output.temporaryNotification);
                        break;
                    }
                    else if (form == "Цилиндр") // Ориентация цилиндра
                    {

                    }
                }
            }


        }
    }
    public abstract class Surfaces // Класс для описания поверхностей
    {
        public string Name; // Имя поверхности
        public List<int>? DegreesOfFreedom; // Шестимерный вектор ориентации поверхности в пространстве
        public double Roughness = 6.3; // Шероховатость (есть значение по-умолчанию)
        public double Diameter; // Диаметр (свойственен для цилиндра и сферы)
        public string? Location; // Состояние расположения (внутреннее/наружное, для сефры или цилиндра)

        public double Flatness; // Отклонение от Плоскостности
        public double Cylindrical; // Отклонение от Цилиндричности
        public double Straightness; // Отклонение от Прямолинейности
        public double Roundness; // Отклонение от Круглости
    }
    public class Plane : Surfaces
    {

    }
    public class Cylinder : Surfaces
    {

    }
    public class Sphere : Surfaces
    {
        //public List<int> DegreesOfFreedom = new List<int>() { 1, 1, 1, 0, 0, 0 };
    }
    public class DataBase // Хранилище всех вводимых поверхностей
    {
        public List<Surfaces> dataSurfaces = new List<Surfaces>();
        //public List<int> dataVectors = new List<int>();
        //public void Add(Surfaces surface) => dataSurfaces.Add(surface);
    }


    public class Output // Метод отображения постоянной и пременной шапок с информацией
    {
        public List<string> text = new List<string>();
        public List<string> temporaryNotification = new List<string>();
        public void OutputText(List<string> text)
        {
            for (int i = 0; i < text.Count; i++)
                Console.WriteLine(text[i]);
            Console.WriteLine();
        }
    }


}