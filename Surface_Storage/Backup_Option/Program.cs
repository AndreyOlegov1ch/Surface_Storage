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

        }
    }
}
