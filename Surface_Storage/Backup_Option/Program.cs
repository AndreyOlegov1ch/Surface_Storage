namespace Backup_Option
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //sphere.Name = data.dataSurfaces.Contains(sphere) ? ($"Сфера{(int.Parse(data.dataSurfaces.FindLast(x => x == sphere).Name.Substring(5).ToString()) + 1)}") : "Сфера1";
            /*
            int newnum = 1;
            while (data.dataSurfaces.FirstOrDefault(x => x.Name == $"Сфера{++newnum}") != null) ;
            sphere.Name = $"Сфера{newnum}";
            */

            /*int newnum = data.Where(s => s != null && s.Name.StartsWith("Сфера")).Count() + 1;
                        sphere.Name = "Сфера" + newnum;
                        data.Add(sphere);*/

            //Console.WriteLine(int.Parse(data.FindLast(x => x == sphere).Name.Substring(5).ToString()));


            //if (r != 0)
            //    surface.Roughness = r;
            //Console.WriteLine(surface.Roughness);
            //break;



            //if (form == "Плоскость" && (deviation_name.Equals("Плоскостность", StringComparison.CurrentCultureIgnoreCase)
            //                || deviation_name.Equals("Прямолинейность", StringComparison.CurrentCultureIgnoreCase)))
            //{
            //    if (deviation_name.Equals("Плоскостность", StringComparison.CurrentCultureIgnoreCase))
            //        surface.Flatness = deviation_value;
            //    else if (deviation_name.Equals("Прямолинейность", StringComparison.CurrentCultureIgnoreCase))
            //        surface.Straightness = deviation_value;
            //    else
            //    {
            //        Console.Write("Для Плоскости могут быть назначены только отклонения от Плоскостности и/или Прямолинейности" +
            //            "\nНажмите на любую клавишу для продолжения");
            //        Console.ReadKey();
            //        Console.Clear();
            //        output.OutputText(output.temporaryNotification);
            //        continue;
            //    }
            //}

            //else if (form == "Цилиндр" && (deviation_name.Equals("Цилиндричность", StringComparison.CurrentCultureIgnoreCase)
            //    || deviation_name.Equals("Прямолинейность", StringComparison.CurrentCultureIgnoreCase)
            //    || deviation_name.Equals("Круглость", StringComparison.CurrentCultureIgnoreCase)))
            //{
            //    if (deviation_name.Equals("Цилиндричность", StringComparison.CurrentCultureIgnoreCase))
            //    {
            //        surface.Cylindrical = deviation_value;
            //    }
            //    else if (deviation_name.Equals("Прямолинейность", StringComparison.CurrentCultureIgnoreCase))
            //    {
            //        surface.Straightness = deviation_value;
            //    }
            //    else if (deviation_name.Equals("Круглость", StringComparison.CurrentCultureIgnoreCase))
            //    {
            //        surface.Roughness = deviation_value;
            //    }
            //    else
            //    {
            //        Console.Write("Для Цилиндра могут быть назначены только отклонения от Цилиндричности и/или Прямолинейности и/или Круглости" +
            //                "\nНажмите на любую клавишу для продолжения");
            //        Console.ReadKey();
            //        Console.Clear();
            //        output.OutputText(output.temporaryNotification);
            //        continue;
            //    }
            //}
            //else if (form == "Сфера" && deviation_name.Equals("Круглость", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    if (deviation_name.Equals("Круглость", StringComparison.CurrentCultureIgnoreCase) == false)
            //    {
            //        Console.Write("Для Сферы может быть назначено только отклонение от Круглости" +
            //                    "\nНажмите на любую клавишу для продолжения");
            //        Console.ReadKey();
            //        Console.Clear();
            //        output.OutputText(output.temporaryNotification);
            //        continue;
            //    }
            //    surface.Flatness = deviation_value;
            //}


        }
    }
}
