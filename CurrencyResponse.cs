using System.Collections.Generic;

namespace DovizTakipUygulamasi
{
    public class CurrencyResponse
    {
        // Başlangıç değeri atadım.
        public string Base { get; set; } = string.Empty;

        // Başlangıçta boş bir liste (Dictionary) oluşturdum.
        // Böylece "Rates null geldi" hatası almayız.
        public Dictionary<string, decimal> Rates { get; set; } = new Dictionary<string, decimal>();
    }
}