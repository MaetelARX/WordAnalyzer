using System;
using System.IO;
using System.Text;
using System.Text.Unicode;
using WordAnalyzer;
using WordAnalyzer.Non_Threaded_Version;
using WordAnalyzer.Threaded_Version;

public class Program
{
    public static void Main(string[] args)
    {
        var engineNT = new EngineNT();
        var engineT = new EngineT();

        // Стартиране на не-нишковата версия
        engineNT.Run();
        Console.WriteLine();
        // Стартиране на нишковата версия
        engineT.Run();
    }
}