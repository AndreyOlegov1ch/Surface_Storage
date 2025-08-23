using System;
using System.Diagnostics.Metrics;
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
                Console.Write("Введите количество поверхностей объекта.\nЕсли количество неизвестно - Нажмите Enter: ");
                string? input = Console.ReadLine();
                check_1 = int.TryParse(input, out int num);

                if (string.IsNullOrEmpty(input))
                {
                    break;
                }
                else if (check_1 == false || num <= 0)
                {
                    Console.Write("Некорректное значение. Введите целое число или нажмите Enter, если количество поверхностей неизвестно\nНажмите на любую клавишу для продолжения");
                    Console.ReadKey();
                    Console.Clear();
                    check_1 = false;
                    continue;
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
            output.text[0] = $"Количество поверхностей с успешно заданными параметрами: {numberOfSurfaces}";
            Console.Clear();
            output.text.Add($"Список введенных поверхностей:\n[{string.Join(" , ", data.dataSurfaces.Select(x => x.Name).ToArray())}]");
            output.OutputText(output.text);

        }


        public static void AddingToData(ref int numberOfSurfaces, int descriptionMethod, Output output, List<Surfaces> data) // Метод, осуществляющий прием и фиксацию имени поверхности
        {
            int checkingNumSurfaces = 1; // Счетчик поверхностей
            output.temporaryNotification = new List<string>(output.text); // Шапка с временным содержанием (наименование поверхности, шероховатость, вектор ориентации в пространстве и т.д.)
            string? input;
            int count = numberOfSurfaces;
            int n = numberOfSurfaces == 0 ? numberOfSurfaces : numberOfSurfaces - 1;
            while (n >= 0) // Фиксированние наименования поверхности
            {
                Console.Write("Введите наименование поверхности\nВведите \"end\", если желаете прекратить заполнение параметров поверхностей" +
                    "\nДопускается ввод только первой буквы наименования или слова целиком: "); // !!! Что лучше, самостоятельно вводить имя, или выбирать из списка (1 - Сфера, 2 - Плоскость, 3 - Цилиндр)?
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
                        Console.Write("Введите корректное наименование поверхности (имя целиком или только первую букву наименования)\nНажмите на любую клавишу для продолжения");
                        Console.ReadKey();
                        Console.Clear();
                        output.OutputText(output.temporaryNotification);
                        continue;
                }
                Console.Clear();
                output.text[0] = numberOfSurfaces == 0 ? $"Количество поверхностей с заданными параметрами: {checkingNumSurfaces++}" : $"Количество поверхностей: Параметры заданы для {checkingNumSurfaces++} из {numberOfSurfaces} поверхностей";
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
                    Console.Write("Выберите номер, описывающий состояние расположения поверхности относительно детали.\n1 - Внутренняя\n2 - Наружная\nВаш выбор: ");
                    string? location = Console.ReadLine();
                    if (int.TryParse(location, out int num) && (num == 1 || num == 2))
                    {
                        surface.Location = num == 1 ? "Внутренняя" : "Наружная";
                        break;
                    }
                    else
                    {
                        Console.Write("Введите номер 1 или 2\nНажмите на любую клавишу для продолжения");
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
                    Console.Write("Выберите номер, соответствующий подходящему наименованию отклонения от формы поверхности и его значение через пробел (размерность - миллиметры).\n" +
                        "Доступные варианты:\n1 - Плоскостность\n2 - Прямолинейность\nДля прекращения ввода отклонений форм поверхностей и задания значений неуказанных отклонений по умолчанию (0,005 мм) - нажмите Enter: ");
                else if (form == "Цилиндр")
                    Console.Write("Выберите номер, соответствующий подходящему наименованию отклонения от формы поверхности и его значение через пробел (размерность - миллиметры).\n" +
                        "Доступные варианты:\n1 - Цилиндричность\n2 - Прямолинейность\n3 - Круглость\nДля прекращения ввода отклонений форм поверхностей и задания значений неуказанных отклонений по умолчанию (0,005 мм) - нажмите Enter: ");
                else if (form == "Сфера")
                    Console.Write("Выберите номер, соответствующий подходящему наименованию отклонения от формы поверхности и его значение через пробел (размерность - миллиметры).\n" +
                        "Доступные варианты:\n1 - Круглость\nДля прекращения ввода отклонений форм поверхностей и задания значений неуказанных отклонений по умолчанию (0,005 мм) - нажмите Enter: ");

                List<string> list = Console.ReadLine().Trim().Split(' ').ToList();

                if (string.IsNullOrEmpty(list[0])) // Отработка прерывания блока задания отклонений
                {
                    if (form == "Плоскость")
                    {
                        if (output.temporaryNotification.Where(x => x.StartsWith("Отклонение от Плоскостности:")).Count() == 0)
                        {
                            surface.Flatness = 0.005;
                            output.temporaryNotification.Add($"Отклонение от Плоскостности: 0,005 мм");
                        }

                        if (output.temporaryNotification.Where(x => x.StartsWith("Отклонение от Прямолинейности:")).Count() == 0)
                        {
                            surface.Flatness = 0.005;
                            output.temporaryNotification.Add($"Отклонение от Прямолинейности: 0,005 мм");
                        }
                    }
                    else if (form == "Цилиндр")
                    {
                        if (output.temporaryNotification.Where(x => x.StartsWith("Отклонение от Цилиндричности:")).Count() == 0)
                        {
                            surface.Flatness = 0.005;
                            output.temporaryNotification.Add($"Отклонение от Цилиндричности: 0,005 мм");
                        }

                        if (output.temporaryNotification.Where(x => x.StartsWith("Отклонение от Прямолинейности:")).Count() == 0)
                        {
                            surface.Flatness = 0.005;
                            output.temporaryNotification.Add($"Отклонение от Прямолинейности: 0,005 мм");
                        }

                        if (output.temporaryNotification.Where(x => x.StartsWith("Отклонение от Круглости:")).Count() == 0)
                        {
                            surface.Flatness = 0.005;
                            output.temporaryNotification.Add($"Отклонение от Круглости: 0,005 мм");
                        }
                    }
                    else if (form == "Сфера" && (output.temporaryNotification.Where(x => x.StartsWith("Отклонение от Круглости:")).Count() == 0))
                    {
                        surface.Flatness = 0.005;
                        output.temporaryNotification.Add($"Отклонение от Круглости: {surface.Flatness} мм");
                    }
                    break;
                }
                if (list.Count() == 2) // Выполнение блока при удовлетворении требований к входным данным
                {
                    if (double.TryParse(list[1], out double v) && v > 0 && (int.TryParse(list[0], out int name)) && name > 0)
                    {
                        int number_name = name;
                        double deviation_value = v;

                        if (form == "Плоскость") // Задание отклонений формы для плоскости
                        {
                            if (number_name == 1)
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
                            else if (number_name == 2)
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
                                Console.Write("Для Плоскости могут быть выбраны номера 1 (отклонение от Плоскостности) и 2 (отклонение от Прямолинейности)" +
                                    "\nНажмите на любую клавишу для продолжения");
                                Console.ReadKey();
                                Console.Clear();
                                output.OutputText(output.temporaryNotification);
                                continue;
                            }
                        }

                        else if (form == "Цилиндр") // Задание отклонений формы для цилиндра
                        {
                            if (number_name == 1)
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
                            else if (number_name == 2)
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
                            else if (number_name == 3)
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
                                Console.Write("Для Цилиндра могут быть выбраны номера 1 (отклонение от Цилиндричности), 2 (отклонение от Прямолинейности) и 3 (отклонение от Круглости)" +
                                        "\nНажмите на любую клавишу для продолжения");
                                Console.ReadKey();
                                Console.Clear();
                                output.OutputText(output.temporaryNotification);
                                continue;
                            }
                        }
                        else if (form == "Сфера") // Задание отклонений формы для сферы
                        {
                            if (number_name != 1)
                            {
                                Console.Write("Для Сферы может быть выбран номер 1 (отклонение от Круглости)" +
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

                    /*else if (!list[0].Equals("Плоскостность", StringComparison.CurrentCultureIgnoreCase) &&
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
                    }*/
                    else if (v < 0) // Отработка исключений
                    {
                        Console.Write("Значение отклонения не может быть отрицательным (меньше 0)." +
                                           "\nНажмите на любую клавишу для продолжения");
                        Console.ReadKey();
                        Console.Clear();
                        output.OutputText(output.temporaryNotification);
                        continue;
                    }
                    else // Отработка исключений
                    {
                        Console.Write("Номер с соответствующим именем должен быть положительным и целым.\nЗначение отклонения должно быть положительным и численным (целым или дробным, к примеру, 1 или 0,05)" +
                            "\nДробные значения указываются через запятую, а не точку." +
                                           "\nНажмите на любую клавишу для продолжения");
                        Console.ReadKey();
                        Console.Clear();
                        output.OutputText(output.temporaryNotification);
                        continue;
                    }
                }
                else if (list.Count() < 2)
                {
                    Console.Write("Вы ввели недостаточно данных либо ничего не указали. Необходимо выбрать номер с наименованием отклонения и через пробел указать его значение" +
                                            "\nНажмите на любую клавишу для продолжения");
                    Console.ReadKey();
                    Console.Clear();
                    output.OutputText(output.temporaryNotification);
                    continue;
                }
                else
                {
                    Console.Write("Вы ввели избыточное количество данных. Необходимо только ввести номер с наименованием отклонения и через пробел указать его значение" +
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
                                        //Console.Clear();
                                        //output.OutputText(output.temporaryNotification);
                                        break;
                                    }
                                case 2:
                                    {
                                        //Console.WriteLine("Шестимерный вектор для Плоскости задан.\nНажмите любую клавишу чтобы продолжить");
                                        surface.DegreesOfFreedom = new List<int>() { 0, 1, 0, 1, 0, 1 };
                                        output.temporaryNotification.Add($"Шестимерный вектор ориентации поверхности в пространстве: {string.Join(',', surface.DegreesOfFreedom)}");
                                        //Console.ReadKey();
                                        //Console.Clear();
                                        //output.OutputText(output.temporaryNotification);
                                        break;
                                    }
                                case 3:
                                    {
                                        //Console.WriteLine("Шестимерный вектор для Плоскости задан.\nНажмите любую клавишу чтобы продолжить");
                                        surface.DegreesOfFreedom = new List<int>() { 0, 0, 1, 1, 1, 0 };
                                        output.temporaryNotification.Add($"Шестимерный вектор ориентации поверхности в пространстве: {string.Join(',', surface.DegreesOfFreedom)}");
                                        //Console.ReadKey();
                                        //Console.Clear();
                                        //output.OutputText(output.temporaryNotification);
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
                            Console.Clear();
                            output.OutputText(output.temporaryNotification);
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
                                        //Console.Clear();
                                        //output.OutputText(output.temporaryNotification);
                                        break;
                                    }
                                case 2:
                                    {
                                        //Console.WriteLine("Шестимерный вектор для Плоскости задан.\nНажмите любую клавишу чтобы продолжить");
                                        surface.DegreesOfFreedom = new List<int>() { 0, 1, 1, 0, 1, 1 };
                                        output.temporaryNotification.Add($"Шестимерный вектор ориентации поверхности в пространстве: {string.Join(',', surface.DegreesOfFreedom)}");
                                        //Console.ReadKey();
                                        //Console.Clear();
                                        //output.OutputText(output.temporaryNotification);
                                        break;
                                    }
                                case 3:
                                    {
                                        //Console.WriteLine("Шестимерный вектор для Плоскости задан.\nНажмите любую клавишу чтобы продолжить");
                                        surface.DegreesOfFreedom = new List<int>() { 1, 0, 1, 1, 0, 1 };
                                        output.temporaryNotification.Add($"Шестимерный вектор ориентации поверхности в пространстве: {string.Join(',', surface.DegreesOfFreedom)}");
                                        //Console.ReadKey();
                                        //Console.Clear();
                                        //output.OutputText(output.temporaryNotification);
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
                            Console.Clear();
                            output.OutputText(output.temporaryNotification);
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
                                    case 3:
                                    case 4:
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
                                    Console.WriteLine($"{i + 1} - {temporaryStorage[i].Name} ({string.Join(',', temporaryStorage[i].DegreesOfFreedom)})");
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
                                                    Console.WriteLine($"{i + 1} - {surfacesWithoutParallel[i].Name} ({string.Join(',', surfacesWithoutParallel[i].DegreesOfFreedom)})");
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
                                                    Console.WriteLine(surfacesWithoutParallel.Count() == 1 ? "Необходимо ввести 1" :
                                                        $"Необходимо ввести целое число от 1 до {surfacesWithoutParallel.Count()} включительно.\nНажмите на любую клавишу для продолжения");
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
                                    string notification = temporaryStorage.Count() == 1 ? "Необходимо ввести 1" :
                                        $"Необходимо ввести целое число от 1 до {temporaryStorage.Count()} включительно.\nНажмите на любую клавишу для продолжения";

                                    Console.WriteLine(notification);
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
                        if (temporaryStorage.Count() == 0) // Ситуация, когда задаваемая поверхность первая или ранее заданные поверхности сферы (невозможно задать параллельность или перпендикулярность)
                        {
                            Console.Write("Выберите подходящий вариант." +
                                "\n1 - цилиндр перпендикулярен оси X (вектор ориентации - 0, 1, 1, 0, 1, 1)" +
                                "\n2 - цилиндр перпендикулярен оси Y (вектор ориентации - 1, 0, 1, 1, 0, 1)" +
                                "\n3 - цилиндр перпендикулярен оси Z (вектор ориентации - 1, 1, 0, 1, 1, 0)" +
                                "\n4 - задать ориентацию плоскости автоматизированно программой" +
                                "\nВвести нужно только подходящий номер: ");
                            string? choiceNum = Console.ReadLine();

                            if (int.TryParse(choiceNum, out int num))
                            {
                                switch (num)
                                {
                                    case 1:
                                        {
                                            surface.DegreesOfFreedom = new List<int>() { 0, 1, 1, 0, 1, 1 };
                                            break;
                                        }
                                    case 2:
                                        {
                                            surface.DegreesOfFreedom = new List<int>() { 1, 0, 1, 1, 0, 1 };
                                            break;
                                        }
                                    case 3:
                                    case 4:
                                        {
                                            surface.DegreesOfFreedom = new List<int>() { 1, 1, 0, 1, 1, 0 };
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
                                    Console.WriteLine($"{i + 1} - {temporaryStorage[i].Name} ({string.Join(',', temporaryStorage[i].DegreesOfFreedom)})");
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

                                        if (number == 2 && (temporaryStorage[choice - 1].Name.StartsWith("Плоскость"))) // Цилиндр перпендикулярен плоскости
                                        {
                                            surface.DegreesOfFreedom = new List<int>();
                                            for (int i = 0; i < 6; i++)
                                                surface.DegreesOfFreedom.Add(i > 2 ? surfaceDegreesOfFreedom[i] : (surfaceDegreesOfFreedom[i] == 0 ? 1 : 0));
                                            break;
                                        }

                                        else if (number == 1 && (temporaryStorage[choice - 1].Name.StartsWith("Цилиндр"))) // Цилиндр параллелен цилиндру
                                        {
                                            surface.DegreesOfFreedom = new List<int>(surfaceDegreesOfFreedom);
                                            break;
                                        }
                                        else if (number == 1 && (temporaryStorage.Count() == 1 || surfacesWithoutParallel.Count() == 0) && (temporaryStorage[choice - 1].Name.StartsWith("Плоскость"))) // Цилиндр параллелен плоскости, но других поверхностей больше нет
                                        {
                                            if (surfaceDegreesOfFreedom.SequenceEqual(new List<int> { 1, 0, 0, 0, 1, 1 }))
                                            {
                                                surface.DegreesOfFreedom = new List<int> { 1, 0, 1, 1, 0, 1 };
                                            }
                                            else if (surfaceDegreesOfFreedom.SequenceEqual(new List<int> { 0, 1, 0, 1, 0, 1 }))
                                            {
                                                surface.DegreesOfFreedom = new List<int> { 1, 1, 0, 1, 1, 0 };
                                            }
                                            else
                                            {
                                                surface.DegreesOfFreedom = new List<int> { 0, 1, 1, 0, 1, 1 };
                                            }
                                            break;
                                        }
                                        else if (number == 2 && (temporaryStorage.Count() == 1 || surfacesWithoutParallel.Count() == 0) && (temporaryStorage[choice - 1].Name.StartsWith("Цилиндр"))) // Цилиндр перпендикулярен цилиндру, но других поверхностей больше нет
                                        {
                                            if (surfaceDegreesOfFreedom.SequenceEqual(new List<int> { 0, 1, 1, 0, 1, 1 }))
                                            {
                                                surface.DegreesOfFreedom = new List<int> { 1, 0, 1, 1, 0, 1 };
                                            }
                                            else if (surfaceDegreesOfFreedom.SequenceEqual(new List<int> { 1, 0, 1, 1, 0, 1 }))
                                            {
                                                surface.DegreesOfFreedom = new List<int> { 1, 1, 0, 1, 1, 0 };
                                            }
                                            else
                                            {
                                                surface.DegreesOfFreedom = new List<int> { 0, 1, 1, 0, 1, 1 };
                                            }
                                            break;
                                        }

                                        else if ((number == 1 && (temporaryStorage[choice - 1].Name.StartsWith("Плоскость"))) ||
                                            (number == 2 && (temporaryStorage[choice - 1].Name.StartsWith("Цилиндр")))) // Если цилиндр параллелен плоскости или цилиндр перпендикулярен цилиндру
                                        {
                                            while (true)
                                            {
                                                Console.Clear();
                                                output.OutputText(output.temporaryNotification);

                                                Console.WriteLine($"Поверхность для построения связи: {temporaryStorage[choice - 1].Name} ({string.Join(',', surfaceDegreesOfFreedom)}). Тип сопряжения: " + (number == 1 ? "Параллельность" : "Перпендикулярность"));
                                                Console.WriteLine("\nПолученной информации недостаточно.\nНеобходимо выбрать дополнительную поверхность и взаимную ориантацию\n");
                                                for (int i = 0; i < surfacesWithoutParallel.Count(); i++) // Представление всех введенных ранее поверхностей кроме сфер
                                                    Console.WriteLine($"{i + 1} - {surfacesWithoutParallel[i].Name} ({string.Join(',', surfacesWithoutParallel[i].DegreesOfFreedom)})");
                                                Console.Write("Введите номер, соответствующий подходящей поверхности: ");
                                                string? choiceSurface2 = Console.ReadLine(); // Ввод пользователем значения

                                                if (int.TryParse(choiceSurface2, out int choice2) && choice2 <= surfacesWithoutParallel.Count() && choice2 > 0 && choice2 % 1 == 0) // Выбор и фиксирование индекса подходящей поверхности 
                                                {
                                                    Console.Write("Выберете способ взаимной ориентации.\n1 - Параллельность\n2 - Перпендикулярность\nВведите номер подходящего способа: ");
                                                    string? numberOfMethod2 = Console.ReadLine();

                                                    if (int.TryParse(numberOfMethod2, out int number2) && (number2 == 1 || number2 == 2)) // Выбор и фиксирование способа задания взаимной связи (параллельность или перпендикулярность)
                                                    {
                                                        if (number2 == 2 && (surfacesWithoutParallel[choice2 - 1].Name.StartsWith("Плоскость"))) // Цилиндр перпендикулярен плоскости
                                                        {
                                                            surface.DegreesOfFreedom = new List<int>();
                                                            for (int i = 0; i < 6; i++)
                                                                surface.DegreesOfFreedom.Add(i > 2 ? surfacesWithoutParallel[choice2 - 1].DegreesOfFreedom[i]
                                                                    : (surfacesWithoutParallel[choice2 - 1].DegreesOfFreedom[i] == 0 ? 1 : 0));
                                                            break;
                                                        }
                                                        else if (number2 == 1 && (surfacesWithoutParallel[choice2 - 1].Name.StartsWith("Цилиндр"))) // Цилиндр параллелен цилиндру
                                                        {
                                                            surface.DegreesOfFreedom = new List<int>(surfacesWithoutParallel[choice2 - 1].DegreesOfFreedom);
                                                            break;
                                                        }
                                                        else if (number2 == 1 && (temporaryStorage[choice - 1].Name.StartsWith("Плоскость")) && (surfacesWithoutParallel[choice2 - 1].Name.StartsWith("Плоскость"))) // Цилиндр параллелен плоскости и в прошлый раз было тоже самое
                                                        {
                                                            List<int> indicators = new List<int>();
                                                            for (int i = 0; i < 3; i++)
                                                                indicators.Add(surfacesWithoutParallel[choice2 - 1].DegreesOfFreedom[i] + surfaceDegreesOfFreedom[i]);
                                                            if (indicators[0] == 0)
                                                            {
                                                                surface.DegreesOfFreedom = new List<int> { 0, 1, 1, 0, 1, 1 };
                                                            }
                                                            else if (indicators[1] == 0)
                                                            {
                                                                surface.DegreesOfFreedom = new List<int> { 1, 0, 1, 1, 0, 1 };
                                                            }
                                                            else if (indicators[2] == 0)
                                                            {
                                                                surface.DegreesOfFreedom = new List<int> { 1, 1, 0, 1, 1, 0 };
                                                            }
                                                            break;
                                                        }
                                                        else if ((number2 == 2 && (surfacesWithoutParallel[choice2 - 1].Name.StartsWith("Цилиндр")) && (temporaryStorage[choice - 1].Name.StartsWith("Плоскость"))) ||
                                                            (number2 == 1 && (surfacesWithoutParallel[choice2 - 1].Name.StartsWith("Плоскость")) && (temporaryStorage[choice - 1].Name.StartsWith("Цилиндр")))) // Цилиндр перпендикулярен цилиндру в прошлый раз цилиндр параллелен плоскости. Также, Цилиндр параллелен плоскости, в прошлый раз цилиндр перпендикулярен цилиндру
                                                        {
                                                            List<int> indicators = new List<int>();
                                                            for (int i = 0; i < 3; i++)
                                                                indicators.Add(surfacesWithoutParallel[choice2 - 1].DegreesOfFreedom[i] + surfaceDegreesOfFreedom[i]);
                                                            if (indicators[0] == 1)
                                                            {
                                                                surface.DegreesOfFreedom = new List<int> { 0, 1, 1, 0, 1, 1 };
                                                            }
                                                            else if (indicators[1] == 1)
                                                            {
                                                                surface.DegreesOfFreedom = new List<int> { 1, 0, 1, 1, 0, 1 };
                                                            }
                                                            else if (indicators[2] == 1)
                                                            {
                                                                surface.DegreesOfFreedom = new List<int> { 1, 1, 0, 1, 1, 0 };
                                                            }
                                                            break;
                                                        }
                                                        else if (number2 == 2 && (surfacesWithoutParallel[choice2 - 1].Name.StartsWith("Цилиндр")) && (temporaryStorage[choice - 1].Name.StartsWith("Цилиндр"))) // Цилиндр перпендикулярен цилиндру, в прошлый раз было тоже самое
                                                        {
                                                            List<int> indicators = new List<int>();
                                                            for (int i = 0; i < 3; i++)
                                                                indicators.Add(surfacesWithoutParallel[choice2 - 1].DegreesOfFreedom[i] + surfaceDegreesOfFreedom[i]);
                                                            if (indicators[0] == 2)
                                                            {
                                                                surface.DegreesOfFreedom = new List<int> { 0, 1, 1, 0, 1, 1 };
                                                            }
                                                            else if (indicators[1] == 2)
                                                            {
                                                                surface.DegreesOfFreedom = new List<int> { 1, 0, 1, 1, 0, 1 };
                                                            }
                                                            else if (indicators[2] == 2)
                                                            {
                                                                surface.DegreesOfFreedom = new List<int> { 1, 1, 0, 1, 1, 0 };
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
                                                    string notification = surfacesWithoutParallel.Count() == 1 ? "Необходимо ввести 1" :
                                        $"Необходимо ввести целое число от 1 до {surfacesWithoutParallel.Count()} включительно.\nНажмите на любую клавишу для продолжения";

                                                    Console.WriteLine(notification);

                                                    //Console.WriteLine($"Необходимо ввести целое число от 1 до {surfacesWithoutParallel.Count()} включительно.\nНажмите на любую клавишу для продолжения");
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
                                    string notification = temporaryStorage.Count() == 1 ? "Необходимо ввести 1" :
                                        $"Необходимо ввести целое число от 1 до {temporaryStorage.Count()} включительно.\nНажмите на любую клавишу для продолжения";

                                    Console.WriteLine(notification);
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
                }
            }

            while (true) // Задание рельефа следа инструмента
            {
                List<int> copyOrientation = new List<int>(surface.DegreesOfFreedom); // Создание копии шестимерного вектора ориентации поверхности в пространстве
                List<string> directions = new List<string>() { "lx", "ly", "lz", "ax", "ay", "az" }; //Направления для выбора направляющей и образующей

                GuideOfSurface(surface, copyOrientation, directions, output, form);

                output.temporaryNotification.Add($"Направление образующей линии: {surface.Formative}");
                output.temporaryNotification.Add($"Направление направляющей линии: {surface.Guide}");
                output.temporaryNotification.Add($"Рельеф следа инструмента: {surface.SurfaceRelief}");
                Console.Clear();
                output.OutputText(output.temporaryNotification);
                break;
            }


        }

        public static void GuideOfSurface(Surfaces surface, List<int> copyOrientation, List<string> directions, Output output, string form) // Задание направляющей
        {
            while (true)
            {
                Console.WriteLine("Выберите направления для задания образующей линии, определяющей формирование и рельеф поверхности");
                if (form == "Сфера")
                    Console.WriteLine("Если направления образующей и направляющей не имеют значения - введите Enter");
                int counter = 1;
                string? one = "";
                string? two = "";
                string? three = "";

                for (int j = 0; j < 6; j++)
                {
                    if (copyOrientation[j] == 0)
                    {
                        Console.WriteLine($"{counter++} - {directions[j]}");
                        if (string.IsNullOrEmpty(one))
                        {
                            one = directions[j];
                        }
                        else if (string.IsNullOrEmpty(two))
                        {
                            two = directions[j];
                        }
                        else
                            three = directions[j];
                    }
                }
                Console.Write("Введите удовлетворяющий вас вариант: ");
                string? input = Console.ReadLine();

                if (form == "Сфера" && string.IsNullOrEmpty(input))
                {
                    surface.Formative = directions[3];
                    surface.Guide = directions[4];
                    surface.SurfaceRelief = "Кругообразный";
                }
                else if (form == "Цилиндр" && int.TryParse(input, out int n) && (n == 1 || n == 2))
                {
                    if (n == 1)
                    {
                        surface.Formative = one;
                        surface.Guide = two;
                    }
                    else
                    {
                        surface.Formative = two;
                        surface.Guide = one;
                    }

                    int indexOfFormative = directions.IndexOf(surface.Formative);

                    if (indexOfFormative > 2)
                        surface.SurfaceRelief = "Кругообразный";
                    else
                        surface.SurfaceRelief = "Прямолинейный";
                }
                else if (int.TryParse(input, out int num) && (num == 1 || num == 2 || num == 3))
                {
                    switch (num)
                    {
                        case 1:
                            {
                                surface.Formative = one;
                                break;
                            }
                        case 2:
                            {
                                surface.Formative = two;
                                break;
                            }
                        case 3:
                            {
                                surface.Formative = three;
                                break;
                            }
                    }
                    int indexOfFormative = directions.IndexOf(surface.Formative);
                    copyOrientation[indexOfFormative] = 1;
                    if (indexOfFormative > 2)
                        surface.SurfaceRelief = "Кругообразный";
                    else
                        surface.SurfaceRelief = "Прямолинейный";

                    while (true)
                    {
                        Console.WriteLine("Выберите направление для задания направляющей линии");
                        int counter2 = 1;
                        string? one2 = "";
                        string? two2 = "";
                        for (int i = 0; i < 6; i++)
                        {
                            if (copyOrientation[i] == 0)
                            {
                                Console.WriteLine($"{counter2++} - {directions[i]}");
                                if (string.IsNullOrEmpty(one2))
                                {
                                    one2 = directions[i];
                                }
                                else
                                    two2 = directions[i];
                            }
                        }
                        Console.Write("Введите удовлетворяющий вас вариант: ");
                        string? input2 = Console.ReadLine();
                        if (int.TryParse(input2, out int num2) && (num2 == 1 || num2 == 2))
                        {
                            surface.Guide = num2 == 1 ? one2 : two2;
                        }
                        else
                        {
                            Console.WriteLine("Необходимо ввести целое число 1 или 2.\nНажмите на любую клавишу для продолжения");
                            Console.ReadKey();
                            Console.Clear();
                            output.OutputText(output.temporaryNotification);
                            continue;
                        }
                        break;
                    }
                }
                else
                {
                    string text = form == "Цилиндр" ? "Необходимо ввести целое число 1 или 2." : "Необходимо ввести целое число 1, 2 или 3.";
                    Console.WriteLine(text);
                    Console.WriteLine("Нажмите на любую клавишу для продолжения");
                    Console.ReadKey();
                    Console.Clear();
                    output.OutputText(output.temporaryNotification);
                    continue;
                }
                break;
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
        public string? SurfaceRelief; // Тип образуемого рельефа
        public string? Formative; // Образующая (направляение)
        public string? Guide; // Направляющая (направление)


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