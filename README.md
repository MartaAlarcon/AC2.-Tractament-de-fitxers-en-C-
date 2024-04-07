# AC2

##Bibliografia
Gewarren. (2023, 18 marzo). Examples of XML Serialization - .NET. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/standard/serialization/examples-of-xml-serialization 

Llamas, L. (2023, 22 abril). Cómo trabajar con CSV en C# con CsvHelper. Luis Llamas. https://www.luisllamas.es/csv-helper/ 

Reading CSV files using C#. (s. f.). Stack Overflow. https://stackoverflow.com/questions/3507498/reading-csv-files-using-c-sharp

##IA
Como poner la path relativa a un archivo que esta en la misma carpeta
ChatGPT
Si deseas especificar una ruta relativa para un archivo que se encuentra en la misma carpeta que tu aplicación, puedes hacerlo utilizando la clase Path del espacio de nombres System.IO. Aquí tienes cómo podrías hacerlo en el código:

Supongamos que tu archivo CSV está en la misma carpeta que tu aplicación. Puedes usar el método Combine de la clase Path para construir la ruta relativa al archivo:

string nomFitxer = "dades.csv"; // nombre del archivo
string rutaRelativa = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nomFitxer);
