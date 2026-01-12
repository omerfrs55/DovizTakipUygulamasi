namespace DovizTakipUygulamasi
{
    public class Currency
    {
        // string.Empty ekleyerek varsayılan değer atadım.
        // Artık "null" hatası vermeyecek.
        public string Code { get; set; } = string.Empty;

        public decimal Rate { get; set; }

        public override string ToString()
        {
            return $"{Code}: {Rate}";
        }
    }
}