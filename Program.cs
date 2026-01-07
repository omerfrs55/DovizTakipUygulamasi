using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DovizTakipUygulamasi
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Currency Tracker v1.0";

            DovizService service = new DovizService();

            RenkliYaz("Veriler API'den çekiliyor, lütfen bekleyiniz...", ConsoleColor.Yellow);

            try
            {
                await service.VerileriYukleAsync();
                RenkliYaz("Veriler başarıyla yüklendi! Menüye geçiliyor...", ConsoleColor.Green);
            }
            catch (Exception)
            {
                RenkliYaz("HATA: İnternet bağlantınızı kontrol ediniz!", ConsoleColor.Red);
                Console.ReadLine();
                return;
            }

            System.Threading.Thread.Sleep(1000);

            while (true)
            {
                Console.Clear();

                // --- MENÜ TASARIMI ---
                RenkliYaz("===============================", ConsoleColor.Magenta);
                RenkliYaz("      CURRENCY TRACKER         ", ConsoleColor.Magenta);
                RenkliYaz("===============================", ConsoleColor.Magenta);

                Console.WriteLine("1. Tüm dövizleri listele");
                Console.WriteLine("2. Koda göre döviz ara");
                Console.WriteLine("3. Belirli bir değerden büyükleri listele");
                Console.WriteLine("4. Dövizleri değere göre sırala");
                Console.WriteLine("5. İstatistiksel özet göster");

                RenkliYaz("0. Çıkış", ConsoleColor.Red);
                RenkliYaz("-------------------------------", ConsoleColor.Magenta);

                Console.Write("Seçiminiz: ");
                string secim = Console.ReadLine() ?? string.Empty;

                // Kullanıcı seçim yapıp vazgeçerse diye "İşlem İptal" kontrolü ekliyoruz.
                // "continue" komutu, döngünün en başına (menüye) direkt zıplar.

                switch (secim)
                {
                    case "1":
                        // Listelemede iptale gerek yok, direkt listeler.
                        var tumListe = service.TumunuGetir();
                        TabloYazdir("TÜM DÖVİZLER", tumListe);
                        break;

                    case "2":
                        // UX Geliştirmesi: İptal seçeneği
                        Console.Write("Aranacak Kodu Girin (Örnek: USD, EUR, GBP vb.) (İptal için BOŞ bırakıp ENTER'a basın): ");
                        string kod = Console.ReadLine() ?? string.Empty;

                        // Eğer boşsa iptal et ve menüye dön
                        if (string.IsNullOrWhiteSpace(kod))
                        {
                            RenkliYaz("İşlem iptal edildi.", ConsoleColor.DarkGray);
                            System.Threading.Thread.Sleep(800); // 0.8 saniye bekle ki mesaj okunsun
                            continue; // While döngüsünün başına döner
                        }

                        var aramaSonucu = service.Ara(kod);
                        TabloYazdir($"ARAMA SONUCU: {kod.ToUpper()}", aramaSonucu);
                        break;

                    case "3":
                        Console.Write("Alt Sınır Değeri (İptal için BOŞ bırakıp ENTER'a basın): ");
                        string girilenDeger = Console.ReadLine() ?? string.Empty;

                        // İptal Kontrolü
                        if (string.IsNullOrWhiteSpace(girilenDeger))
                        {
                            RenkliYaz("İşlem iptal edildi.", ConsoleColor.DarkGray);
                            System.Threading.Thread.Sleep(800);
                            continue;
                        }

                        if (decimal.TryParse(girilenDeger.Replace('.', ','), out decimal sinir))
                        {
                            var filtreSonucu = service.DegerdenBuyukleriGetir(sinir);
                            TabloYazdir($"{sinir} ₺ ÜZERİNDEKİLER", filtreSonucu);
                        }
                        else
                        {
                            RenkliYaz("\nHATA: Geçersiz sayı formatı!", ConsoleColor.Red);
                        }
                        break;

                    case "4":
                        Console.WriteLine("1: Artan Sıralama");
                        Console.WriteLine("2: Azalan Sıralama");
                        Console.Write("Seçim (İptal için BOŞ bırakıp ENTER): ");

                        string siraSecim = Console.ReadLine() ?? string.Empty;

                        // İptal Kontrolü
                        if (string.IsNullOrWhiteSpace(siraSecim))
                        {
                            RenkliYaz("İşlem iptal edildi.", ConsoleColor.DarkGray);
                            System.Threading.Thread.Sleep(800);
                            continue;
                        }

                        bool artanMi = (siraSecim == "1");
                        var siraliListe = service.Sirala(artanMi);

                        string baslik = artanMi ? "SIRALI (ARTAN)" : "SIRALI (AZALAN)";
                        TabloYazdir(baslik, siraliListe);
                        break;

                    case "5":
                        // İstatistik hemen gelir, iptale gerek yok.
                        Console.WriteLine();
                        RenkliYaz("--- İSTATİSTİKLER ---", ConsoleColor.Yellow);
                        Console.WriteLine(service.IstatistikGetir());
                        break;

                    case "0":
                        RenkliYaz("Program kapatılıyor...", ConsoleColor.DarkGray);
                        return;

                    default:
                        RenkliYaz("Geçersiz seçim!", ConsoleColor.Red);
                        // Hatalı tuşlamada hemen menüye dönmesin, hatayı görsün diye bekletiyoruz.
                        break;
                }

                if (secim != "0")
                {
                    Console.WriteLine("\nAna menü için bir tuşa basınız...");
                    Console.ReadKey();
                }
            }
        }

        static void TabloYazdir(string baslik, List<Currency> liste)
        {
            Console.WriteLine();
            RenkliYaz($"--- {baslik} ---", ConsoleColor.Yellow);

            if (liste == null || liste.Count == 0)
            {
                RenkliYaz("Kayıt bulunamadı.", ConsoleColor.Red);
                return;
            }

            Console.WriteLine("{0,-10} {1,-15}", "DÖVİZ", "KUR DEĞERİ");
            Console.WriteLine(new string('-', 25));

            foreach (var item in liste)
            {
                Console.WriteLine("{0,-10} {1,-15}", item.Code, $"{item.Rate:F4} ₺");
            }
            Console.WriteLine(new string('-', 25));
            RenkliYaz($"Toplam {liste.Count} kayıt listelendi.", ConsoleColor.DarkGray);
        }

        static void RenkliYaz(string mesaj, ConsoleColor renk)
        {
            Console.ForegroundColor = renk;
            Console.WriteLine(mesaj);
            Console.ResetColor();
        }
    }
}