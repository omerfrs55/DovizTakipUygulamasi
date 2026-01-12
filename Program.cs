using System;
using System.Collections.Generic;
using System.Text; // UTF-8 Encoding için gerekli
using System.Threading.Tasks;

namespace DovizTakipUygulamasi
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // ÖNEMLİ: Konsolun Türkçe karakterleri (₺, €, £) doğru göstermesi için bu ayar şart.
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "Currency Tracker v1.0";

            // Servis sınıfımızı başlatıyoruz.
            DovizService service = new DovizService();

            RenkliYaz("Veriler API'den çekiliyor, lütfen bekleyiniz...", ConsoleColor.Yellow);

            try
            {
                // API isteği yapılıyor.
                await service.VerileriYukleAsync();
                RenkliYaz("Veriler başarıyla yüklendi! Menüye geçiliyor...", ConsoleColor.Green);
            }
            catch (Exception)
            {
                RenkliYaz("HATA: İnternet bağlantınızı kontrol ediniz!", ConsoleColor.Red);
                Console.ReadLine();
                return; // Hata varsa programı durdur.
            }

            // Kullanıcı mesajı görsün diye 1 saniye bekletiyorum.
            System.Threading.Thread.Sleep(1000);

            // SONSUZ DÖNGÜ (Menü sürekli döner)
            while (true)
            {
                Console.Clear();

                // --- MENÜ TASARIMIM ---
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

                // Null kontrolü (CS8600 hatasını önler)
                string secim = Console.ReadLine() ?? string.Empty;

                switch (secim)
                {
                    case "1":
                        // Tüm listeyi getir ve tabloya bas
                        var tumListe = service.TumunuGetir();
                        TabloYazdir("TÜM DÖVİZLER", tumListe);
                        break;

                    case "2":
                        // Arama İşlemi + İptal Koruması
                        Console.Write("Aranacak Kodu Girin (İptal için BOŞ bırakıp ENTER): ");
                        string kod = Console.ReadLine() ?? string.Empty;

                        // Eğer boşsa başa dön
                        if (string.IsNullOrWhiteSpace(kod))
                        {
                            RenkliYaz("İşlem iptal edildi.", ConsoleColor.DarkGray);
                            System.Threading.Thread.Sleep(800);
                            continue;
                        }

                        var aramaSonucu = service.Ara(kod);
                        TabloYazdir($"ARAMA SONUCU: {kod.ToUpper()}", aramaSonucu);
                        break;

                    case "3":
                        // Filtreleme İşlemi + İptal Koruması
                        Console.Write("Alt Sınır Değeri (İptal için BOŞ bırakıp ENTER): ");
                        string girilenDeger = Console.ReadLine() ?? string.Empty;

                        if (string.IsNullOrWhiteSpace(girilenDeger))
                        {
                            RenkliYaz("İşlem iptal edildi.", ConsoleColor.DarkGray);
                            System.Threading.Thread.Sleep(800);
                            continue;
                        }

                        // Nokta/Virgül dönüşümü yaparak sayıya çeviriyorum
                        if (decimal.TryParse(girilenDeger.Replace('.', ','), out decimal sinir))
                        {
                            var filtreSonucu = service.DegerdenBuyukleriGetir(sinir);
                            TabloYazdir($"{sinir} DEĞERİNDEN BÜYÜKLER", filtreSonucu);
                        }
                        else
                        {
                            RenkliYaz("\nHATA: Geçersiz sayı formatı!", ConsoleColor.Red);
                        }
                        break;

                    case "4":
                        // Sıralama İşlemi + İptal Koruması
                        Console.WriteLine("1: Artan Sıralama (Ucuz -> Pahalı)");
                        Console.WriteLine("2: Azalan Sıralama (Pahalı -> Ucuz)");
                        Console.Write("Seçim (İptal için BOŞ bırakıp ENTER): ");

                        string siraSecim = Console.ReadLine() ?? string.Empty;

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
                        // İstatistik Gösterimi
                        Console.WriteLine();
                        RenkliYaz("--- İSTATİSTİKLER ---", ConsoleColor.Yellow);
                        Console.WriteLine(service.IstatistikGetir());
                        break;

                    case "0":
                        RenkliYaz("Program kapatılıyor...", ConsoleColor.DarkGray);
                        return; // Programdan çıkış

                    default:
                        RenkliYaz("Geçersiz seçim!", ConsoleColor.Red);
                        break;
                }

                // Seçim 0 değilse, kullanıcının ekranı görebilmesi için bekle
                if (secim != "0")
                {
                    Console.WriteLine("\nAna menü için bir tuşa basınız...");
                    Console.ReadKey();
                }
            }
        }

        // --- YARDIMCI METOTLAR ---

        // Listeyi şık bir tablo halinde yazdıran metot
        static void TabloYazdir(string baslik, List<Currency> liste)
        {
            Console.WriteLine();
            RenkliYaz($"--- {baslik} ---", ConsoleColor.Yellow);

            if (liste == null || liste.Count == 0)
            {
                RenkliYaz("Kayıt bulunamadı.", ConsoleColor.Red);
                return;
            }

            // Tablo Başlıkları
            // {0,-10} -> Sola yasla, 10 karakter yer ayır
            Console.WriteLine("{0,-10} {1,-15}", "DÖVİZ", "KUR DEĞERİ");
            Console.WriteLine(new string('-', 30));

            foreach (var item in liste)
            {
                // Para birimi koduna göre özel simgeyi getir (Örn: USD -> $)
                string sembol = SembolGetir(item.Code);

                // Ekrana yazdırma: "USD34.5000 $"
                Console.WriteLine("{0,-10} {1,-15}", item.Code, $"{item.Rate:F4} {sembol}");
            }
            Console.WriteLine(new string('-', 30));
            RenkliYaz($"Toplam {liste.Count} kayıt listelendi.", ConsoleColor.DarkGray);
        }

        // Konsola renkli yazı yazmayı sağlayan yardımcı metot
        static void RenkliYaz(string mesaj, ConsoleColor renk)
        {
            Console.ForegroundColor = renk;
            Console.WriteLine(mesaj);
            Console.ResetColor(); // Rengi sıfırla ki sonraki yazılar bozulmasın
        }

        // Para birimi koduna göre simge döndüren kapsamlı metot
        // Frankfurter API'sinde bulunan yaygın tüm para birimleri ekledim
        static string SembolGetir(string kod)
        {
            switch (kod.ToUpper())
            {
                case "TRY": return "₺";  // Türk Lirası
                case "USD": return "$";  // Amerikan Doları
                case "EUR": return "€";  // Euro
                case "GBP": return "£";  // İngiliz Sterlini
                case "JPY": return "¥";  // Japon Yeni
                case "CNY": return "¥";  // Çin Yuanı
                case "AUD": return "A$"; // Avustralya Doları
                case "CAD": return "C$"; // Kanada Doları
                case "CHF": return "Fr"; // İsviçre Frangı
                case "SEK": return "kr"; // İsveç Kronu
                case "DKK": return "kr"; // Danimarka Kronu
                case "NOK": return "kr"; // Norveç Kronu
                case "RUB": return "₽";  // Rus Rublesi
                case "INR": return "₹";  // Hindistan Rupisi
                case "BRL": return "R$"; // Brezilya Reali
                case "ZAR": return "R";  // Güney Afrika Randı
                case "MXN": return "$";  // Meksika Pesosu
                case "SGD": return "S$"; // Singapur Doları
                case "HKD": return "HK$";// Hong Kong Doları
                case "NZD": return "NZ$";// Yeni Zelanda Doları
                case "KRW": return "₩";  // Güney Kore Wonu
                case "PLN": return "zł"; // Polonya Zlotisi
                case "THB": return "฿";  // Tayland Bahtı
                case "IDR": return "Rp"; // Endonezya Rupiahı
                case "HUF": return "Ft"; // Macar Forinti
                case "CZK": return "Kč"; // Çek Korunası
                case "ILS": return "₪";  // İsrail Şekeli
                case "PHP": return "₱";  // Filipin Pesosu
                case "MYR": return "RM"; // Malezya Ringgiti
                case "RON": return "lei";// Rumen Leyi
                case "BGN": return "лв"; // Bulgar Levası
                case "ISK": return "kr"; // İzlanda Kronu
                case "HRK": return "kn"; // Hırvat Kunası

                // Eğer listede olmayan bir kod gelirse, kodun kendisini döndür (Örn: XYZ)
                default: return kod;
            }
        }
    }
}