using System.Collections.Generic;

namespace DovizTakipUygulamasi
{
    public class CurrencyResponse
    {
        // DÜZELTME: Başlangıç değeri atandı.
        public string Base { get; set; } = string.Empty;

        // DÜZELTME: Başlangıçta boş bir liste (Dictionary) oluşturduk.
        // Böylece "Rates null geldi" hatası almayız.
        public Dictionary<string, decimal> Rates { get; set; } = new Dictionary<string, decimal>();
    }
}