namespace DovizTakipUygulamasi
{
    public class Currency
    {
        // DÜZELTME: = string.Empty ekleyerek varsayılan değer atadık.
        // Artık "null" hatası vermez.
        public string Code { get; set; } = string.Empty;

        public decimal Rate { get; set; }

        public override string ToString()
        {
            return $"{Code}: {Rate}";
        }
    }
}