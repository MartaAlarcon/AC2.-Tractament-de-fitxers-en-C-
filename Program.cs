using CsvHelper;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using AC2;

public class Program
{
    static List<Comarca> comarques = new List<Comarca>();

    public static void Main()
    {
        const string MsgOption = "Escull una opción (1-5):\n\t1. Identificar comarques amb població superior a 200000\n\t2. Calcular consum domèstic mitjà per comarca\n\t3. Mostrar comarques amb consum domèstic per càpita més alt\n\t4. Mostrar comarques amb consum domèstic per càpita més baix\n\t5. Filtrar comarques per nom o codi";
        const string MsgDefault = "Opció no vàlida";
        string path = "dades.csv";
        int option;
        LlegirDadesCSV(path);

        Console.WriteLine(MsgOption);
        option = Convert.ToInt32(Console.ReadLine());

        switch (option)
        {
            case 1:
                IdentificarComarquesPoblacioSuperior();
                break;
            case 2:
                CalcularConsumMitjaPerComarca();
                break;
            case 3:
                MostrarComarquesConsumPerCapitaMesAlt();
                break;
            case 4:
                MostrarComarquesConsumPerCapitaMesBaix();
                break;
            case 5:
                FiltrarPerNomOCodi();
                break;
            default:
                Console.WriteLine(MsgDefault);
                break;
        }
    }

    public static void LlegirDadesCSV(string path)
    {

        //llegim el csv
        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                var any = csv.GetField<int>("Any");
                var codiComarca = csv.GetField<int>("Codi comarca");
                var nomComarca = csv.GetField("Comarca");
                var poblacio = csv.GetField<int>("Població");
                var domesticXarxa = csv.GetField<int>("Domèstic xarxa");
                var activitatsEconomiques = csv.GetField<int>("Activitats econòmiques i fonts pròpies");
                var total = csv.GetField<int>("Total");
                var consumDomesticPerCapita = csv.GetField<double>("Consum domèstic per càpita");

                var comarca = new Comarca
                {
                    Any = any,
                    CodiComarca = codiComarca,
                    NomComarca = nomComarca,
                    Poblacio = poblacio,
                    DomesticXarxa = domesticXarxa,
                    ActivitatsEconomiques = activitatsEconomiques,
                    Total = total,
                    ConsumDomesticPerCapita = consumDomesticPerCapita
                };

                comarques.Add(comarca);
            }
        }

        //fem un xml amb les dades
        var comarquesXml = new ComarquesXml
        {
            Comarques = comarques.Select(c => new Comarca
            {
                Any = c.Any,
                CodiComarca = c.CodiComarca,
                NomComarca = c.NomComarca,
                Poblacio = c.Poblacio,
                DomesticXarxa = c.DomesticXarxa,
                ActivitatsEconomiques = c.ActivitatsEconomiques,
                Total = c.Total,
                ConsumDomesticPerCapita = c.ConsumDomesticPerCapita
            }).ToList()
        };

        // serializar a XML
        XmlSerializer serializer = new XmlSerializer(typeof(ComarquesXml));
        using (StreamWriter sw = new StreamWriter("comarques.xml"))
        {
            serializer.Serialize(sw, comarquesXml);
        }
    }

    public static void IdentificarComarquesPoblacioSuperior()
    {
        const int MAX = 200000;
        var comarquesPoblacioSuperior = comarques.Where(c => c.Poblacio > MAX);
        ImprimirComarques(comarquesPoblacioSuperior);
    }

    public static void CalcularConsumMitjaPerComarca()
    {
        var consumMitjaPerComarca = comarques.GroupBy(c => c.NomComarca)
            .Select(g => new { Comarca = g.Key, ConsumMitja = g.Average(c => c.Total) });
        foreach (var item in consumMitjaPerComarca)
        {
            Console.WriteLine($"Comarca: {item.Comarca}, Consum Mitjà: {item.ConsumMitja}");
        }
    }

    public static void MostrarComarquesConsumPerCapitaMesAlt()
    {
        var comarquesConsumPerCapitaMesAlt = comarques.Where(c => c.ConsumDomesticPerCapita == comarques.Max(x => x.ConsumDomesticPerCapita));
        ImprimirComarques(comarquesConsumPerCapitaMesAlt);
    }

    public static void MostrarComarquesConsumPerCapitaMesBaix()
    {
        var comarquesConsumPerCapitaMesBaix = comarques.Where(c => c.ConsumDomesticPerCapita == comarques.Min(x => x.ConsumDomesticPerCapita));
        ImprimirComarques(comarquesConsumPerCapitaMesBaix);
    }

    public static void FiltrarPerNomOCodi()
    {
        const string MsgComarca = "Introdueix el nom o codi de la comarca: ";
        Console.WriteLine(MsgComarca);
        string consulta = Console.ReadLine();
        var comarquesFiltrades = comarques.Where(c => c.NomComarca.Contains(consulta.ToUpper()) || c.CodiComarca.ToString().Equals(consulta));
        ImprimirComarques(comarquesFiltrades);
    }

    public static void ImprimirComarques(IEnumerable<Comarca> comarques)
    {
        foreach (var comarca in comarques)
        {
            Console.WriteLine($"Any: {comarca.Any}, Codi Comarca: {comarca.CodiComarca}, Nom Comarca: {comarca.NomComarca}, Poblacio: {comarca.Poblacio}, Domestic Xarxa: {comarca.DomesticXarxa}, Activitats Economiques: {comarca.ActivitatsEconomiques}, Total: {comarca.Total}, Consum Domèstic Per Càpita: {comarca.ConsumDomesticPerCapita}\n");
        }
    }
}
