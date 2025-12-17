using System;

namespace Game.ApiHost
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Game API Host";
            Console.WriteLine("🧩 Game API başlatılıyor...");

            GameApiServer.Start(9000); // 9000 portunda API açar

            Console.WriteLine("✅ API sunucusu aktif! http://localhost:9000");
            Console.WriteLine("Kapatmak için Enter'a basın...");
            Console.ReadLine();
        }
    }
}
