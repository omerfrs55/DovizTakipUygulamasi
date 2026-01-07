using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DovizTakipUygulamasi
{
    public class DovizService
    {
        // DÜZELTME: Listeyi tanımlarken hemen "new" diyerek oluşturuyoruz.
        // Constructor içinde yapmasak bile burası garantiye alır.
        private List<Currency> _dovizListesi = new List<Currency>();

        public DovizService()
        {
            // Burası boş kalabilir veya _dovizListesi = new List<Currency>(); kalabilir.
            // Yukarıda eşitlediğimiz için sorun çözüldü.
        }

        public async Task VerileriYukleAsync()
        {
            string url = "https://api.frankfurter.app/latest?from=TRY";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string jsonVerisi = await client.GetStringAsync(url);

                    // DÜZELTME: Deserialize işleminde null gelme ihtimaline karşı "?" koyabiliriz
                    // Ama aşağıdaki if kontrolü zaten bunu yapıyor.
                    var response = JsonConvert.DeserializeObject<CurrencyResponse>(jsonVerisi);

                    if (response != null && response.Rates != null)
                    {
                        _dovizListesi = response.Rates.Select(k => new Currency
                        {
                            Code = k.Key,
                            Rate = k.Value
                        }).ToList();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Veri çekilirken hata oluştu: " + ex.Message);
                }
            }
        }

        public List<Currency> TumunuGetir()
        {
            return _dovizListesi;
        }

        public List<Currency> Ara(string kod)
        {
            // DÜZELTME: "kod" parametresi null gelirse hata vermesin diye kontrol ekledik.
            if (string.IsNullOrEmpty(kod)) return new List<Currency>();

            return _dovizListesi
                .Where(x => x.Code.ToUpper().Contains(kod.ToUpper()))
                .ToList();
        }

        public List<Currency> DegerdenBuyukleriGetir(decimal sinirDeger)
        {
            return _dovizListesi
                .Where(x => x.Rate > sinirDeger)
                .ToList();
        }

        public List<Currency> Sirala(bool artanMi)
        {
            if (artanMi)
            {
                return _dovizListesi.OrderBy(x => x.Rate).ToList();
            }
            else
            {
                return _dovizListesi.OrderByDescending(x => x.Rate).ToList();
            }
        }

        public string IstatistikGetir()
        {
            if (_dovizListesi.Count == 0) return "Veri yok.";

            int toplamSayi = _dovizListesi.Count;

            // FirstOrDefault kullanıyoruz, eğer liste boşsa null döner, patlamaz.
            var enYuksek = _dovizListesi.OrderByDescending(x => x.Rate).FirstOrDefault();
            var enDusuk = _dovizListesi.OrderBy(x => x.Rate).FirstOrDefault();
            decimal ortalama = _dovizListesi.Average(x => x.Rate);

            // Eğer enYuksek null ise (veri yoksa) hata vermesin diye kontrol (?)
            // Ama yukarıda Count == 0 kontrolü olduğu için buraya veri varsa gelir.

            // Garanti olsun diye null kontrolü yapalım (CS8602 uyarısını önlemek için):
            if (enYuksek == null || enDusuk == null) return "Hesaplanamadı.";

            return $"Toplam Döviz Sayısı: {toplamSayi}\n" +
                   $"En Yüksek Kur: {enYuksek.Code} ({enYuksek.Rate})\n" +
                   $"En Düşük Kur: {enDusuk.Code} ({enDusuk.Rate})\n" +
                   $"Ortalama Kur: {ortalama:F4}";
        }
    }
}