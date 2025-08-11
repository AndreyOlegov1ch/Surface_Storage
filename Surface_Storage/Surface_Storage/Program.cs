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
            output.OutputText();

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
                output.OutputText();
            }
            Console.Clear();
            output.text.Add("Способ описания поверхностей объектов: " + (descriptionMethod == 1 ? "Шестимерные векторы поверхностей" : "Взаимное расположение поверхностей"));
            output.OutputText();

            AddingToData(ref numberOfSurfaces, descriptionMethod, output, data.dataSurfaces);
            output.text[0] = $"Количество поверхностей: {numberOfSurfaces}";
            Console.Clear();
            output.text.Add($"Список введенных поверхностей:\n[{string.Join(" , ",data.dataSurfaces.Select(x=>x.Name).ToArray())}]");
            output.OutputText();

        }


        public static void AddingToData(ref int numberOfSurfaces, int descriptionMethod, Output output, List<Surfaces> data)
        {
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
                        AddSurfacesName(sphere, data, "Сфера");
                        Console.Clear();
                        output.OutputText();
                        Console.WriteLine("Успешно добавлено!\n");
                        break;
                    case "ц":
                    case "цилиндр":
                        Surfaces cylinder = new Cylinder();
                        AddSurfacesName(cylinder, data, "Цилиндр");
                        Console.Clear();
                        output.OutputText();
                        Console.WriteLine("Успешно добавлено!\n");
                        break;
                    case "п":
                    case "плоскость":
                        Surfaces plane = new Plane();
                        AddSurfacesName(plane, data, "Плоскость");
                        Console.Clear();
                        output.OutputText();
                        Console.WriteLine("Успешно добавлено!\n");
                        break;
                    default:
                        Console.Write("Введите корректное наименование поверхности\nНажмите на любую клавишу для продолжения");
                        Console.ReadKey();
                        Console.Clear();
                        output.OutputText();
                        continue;
                }
                if (count!=0)
                   n--; 
            }
        }
    public static void AddSurfacesName(Surfaces surface, List<Surfaces> data, string form)
        {
            int newnum = data.Where(s => s != null && s.Name.StartsWith(form)).Count() + 1;
            surface.Name = form + newnum;
            data.Add(surface);
            //Console.WriteLine(surface.Name);
        }
    }
    public abstract class Surfaces
    {
        public string Name;
        List <int> DegreesOfFreedom;
        double Roughness;
    }
    public class Plane : Surfaces
    {
        
    }
    public class Cylinder : Surfaces
    {
        double Diameter;
    }
    public class Sphere : Surfaces
    {
        double Diameter;
        List <int> DegreesOfFreedom = [ 1, 1, 1, 0, 0, 0 ];
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
        public void OutputText()
        {
            for (int i = 0; i < text.Count; i++)
                Console.WriteLine(text[i]);
            Console.WriteLine();
        }
    }


}