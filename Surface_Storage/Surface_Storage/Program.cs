using System;

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

            while (check_1 == false)
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
            output.text.Add("Количество поверхностей: " + (numberOfSurfaces != 0 ? numberOfSurfaces : "Уточняется далее"));
            output.OutputText(output.text);

            while (true)
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

            AddingToData(ref numberOfSurfaces, descriptionMethod, output, data.dataSurfaces);
            output.text[0] = $"Количество поверхностей: {numberOfSurfaces}";
            Console.Clear();
            output.text.Add($"Список введенных поверхностей:\n[{string.Join(" , ", data.dataSurfaces.Select(x => x.Name).ToArray())}]");
            output.OutputText(output.text);

        }


        public static void AddingToData(ref int numberOfSurfaces, int descriptionMethod, Output output, List<Surfaces> data)
        {
            output.temporaryNotification = new List<string>(output.text);
            string? input;
            int count = numberOfSurfaces;
            int n = numberOfSurfaces == 0 ? numberOfSurfaces : numberOfSurfaces - 1;
            while (n >= 0)
            {
                Console.Write("Введите наименование поверхности\nВведите \"end\", если желаете прекратить" +
                    "\nДопускается ввод только первой буквы наименования или слова целиком: ");
                input = Console.ReadLine();
                input = String.IsNullOrEmpty(input) ? input : input.ToLower();
                if (input == "end")
                {
                    numberOfSurfaces = data.Count();
                    break;
                }
                switch (input)
                {
                    case "с":
                    case "сфера":
                        Surfaces sphere = new Sphere();
                        AddingSurfaceParameters(sphere, data, "Сфера", output, descriptionMethod);
                        Console.Clear();
                        output.OutputText(output.temporaryNotification);
                        Console.WriteLine("Успешно добавлено!\n");
                        break;
                    case "ц":
                    case "цилиндр":
                        Surfaces cylinder = new Cylinder();
                        AddingSurfaceParameters(cylinder, data, "Цилиндр", output, descriptionMethod);
                        Console.Clear();
                        output.OutputText(output.temporaryNotification);
                        Console.WriteLine("Успешно добавлено!\n");
                        break;
                    case "п":
                    case "плоскость":
                        Surfaces plane = new Plane();
                        AddingSurfaceParameters(plane, data, "Плоскость", output, descriptionMethod);
                        Console.Clear();
                        output.OutputText(output.temporaryNotification);
                        Console.WriteLine("Успешно добавлено!\n");
                        break;
                    default:
                        Console.Write("Введите корректное наименование поверхности\nНажмите на любую клавишу для продолжения");
                        Console.ReadKey();
                        Console.Clear();
                        output.OutputText(output.temporaryNotification);
                        continue;
                }
                output.temporaryNotification = new List<string>(output.text);
                if (count != 0)
                    n--;
            }
        }


        public static void AddingSurfaceParameters(Surfaces surface, List<Surfaces> data, string form, Output output, int descriptionMethod)
        {
            //double roughness = 0;
            int newnum = data.Where(s => s != null && s.Name.StartsWith(form)).Count() + 1;
            surface.Name = form + newnum;
            data.Add(surface);
            //Console.WriteLine(surface.Name);
            output.temporaryNotification.Add($"\nВводимая поверхность: {form}");
            while (true)
            {
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

            if (form == "Сфера" || form == "Цилиндр")
            {
                while (true)
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
                output.temporaryNotification.Add($"Диаметр поверхности: {surface.Diameter} мм");

                while (true)
                {
                    Console.Write("Введите состояние расположения поверхности относительно детали. Внутренняя или Наружная: ");
                    string? location = Console.ReadLine();
                    if (String.IsNullOrEmpty(location) != true && (location.Equals("внутренняя", StringComparison.CurrentCultureIgnoreCase) || location.Equals("наружная", StringComparison.CurrentCultureIgnoreCase)))
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
                output.temporaryNotification.Add($"Состояние расположения поверхности относительно детали: {surface.Location}");
            }

            while (true)
            {
                if (form == "Плоскость")
                    Console.WriteLine("Введите наименование отклонения от формы поверхности и его значение через пробел в миллиметрах.\n" +
                        "Доступные варианты:\nПлоскостность;\nПрямолинейность.\nЧтобы прекратить ввод отклонений форм поверхностей - введите 0:");
                else if (form == "Цилиндр")
                    Console.WriteLine("Введите наименование отклонения от формы поверхности и его значение через пробел в миллиметрах.\n" +
                        "Доступные варианты:\nЦилиндричность;\nПрямолинейность;\nКруглость.\nЧтобы прекратить ввод отклонений форм поверхностей - введите 0:");
                else if (form == "Сфера")
                    Console.WriteLine("Введите наименование отклонения от формы поверхности и его значение через пробел в миллиметрах.\n" +
                        "Доступные варианты:\nКруглость.\nЧтобы прекратить ввод отклонений форм поверхностей - введите 0:");

                List<string> list = Console.ReadLine().Trim().Split(' ').ToList();

                if (list[0] == "0")
                    break;
                if (list.Count() == 2)
                {
                    if (double.TryParse(list[1], out double v) && v > 0 && (list[0].Equals("Плоскостность", StringComparison.CurrentCultureIgnoreCase) ||
                        list[0].Equals("Цилиндричность", StringComparison.CurrentCultureIgnoreCase) || list[0].Equals("Прямолинейность", StringComparison.CurrentCultureIgnoreCase) ||
                        list[0].Equals("Круглость", StringComparison.CurrentCultureIgnoreCase)))
                    {
                        string? deviation_name = list[0];
                        double deviation_value = v;

                        if (form == "Плоскость") // Продолжить для остальных случаев аналогично
                        {
                            if (deviation_name.Equals("Плоскостность", StringComparison.CurrentCultureIgnoreCase))
                            {
                                surface.Flatness = deviation_value;
                                if (output.temporaryNotification.Where(x=>x.StartsWith("Отклонение от Плоскостности:")).ToList().Count()==1)
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
                                surface.Straightness = deviation_value;
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

                        else if (form == "Цилиндр")
                        {
                            if (deviation_name.Equals("Цилиндричность", StringComparison.CurrentCultureIgnoreCase))
                            {
                                surface.Cylindrical = deviation_value;
                            }
                            else if (deviation_name.Equals("Прямолинейность", StringComparison.CurrentCultureIgnoreCase))
                            {
                                surface.Straightness = deviation_value;
                            }
                            else if (deviation_name.Equals("Круглость", StringComparison.CurrentCultureIgnoreCase))
                            {
                                surface.Roughness = deviation_value;
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
                        else if (form == "Сфера")
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
                            surface.Flatness = deviation_value;
                        }
                    }
                    
                    else if(!list[0].Equals("Плоскостность", StringComparison.CurrentCultureIgnoreCase)&&
                        !list[0].Equals("Цилиндричность", StringComparison.CurrentCultureIgnoreCase) &&
                        !list[0].Equals("Прямолинейность", StringComparison.CurrentCultureIgnoreCase) &&
                        !list[0].Equals("Круглость", StringComparison.CurrentCultureIgnoreCase))
                    {
                        Console.Write("Вы ввели недопстимое наименование отклонения формы. Ознакомьтесь с доступными вариантами" +
                                           "\nНажмите на любую клавишу для продолжения");
                        Console.ReadKey();
                        Console.Clear();
                        output.OutputText(output.temporaryNotification);
                        continue;
                    }
                    else if (v < 0)
                    {
                        Console.Write("Значение отклонения не может быть меньше 0." +
                                           "\nНажмите на любую клавишу для продолжения");
                        Console.ReadKey();
                        Console.Clear();
                        output.OutputText(output.temporaryNotification);
                        continue;
                    }
                    else
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
                Console.WriteLine("Данные успешно зафиксированы!\nНажмите на любую клавишу чтобы продолжить");
                Console.ReadKey();
                Console.Clear();
                output.OutputText(output.temporaryNotification);
            }


        }
    }
    public abstract class Surfaces
    {
        public string Name;
        public List<int>? DegreesOfFreedom;
        public double Roughness = 6.3;
        public double Diameter;
        public string? Location;

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
        List<int> DegreesOfFreedom = new List<int>() { 1, 1, 1, 0, 0, 0 };
    }
    public class DataBase
    {
        public List<Surfaces> dataSurfaces = new List<Surfaces>();
        //public List<int> dataVectors = new List<int>();
        //public void Add(Surfaces surface) => dataSurfaces.Add(surface);
    }


    public class Output
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