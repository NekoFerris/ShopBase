namespace ShopBase
{
    public enum Geschlecht
    {
        NichtGesetzt,
        Maenlich,
        Weiblich,
        Divers,
    }

    public class Person
    {
        public string? Vorname { get; set; }
        public DateTime? GebDat { get; set; }

        //public readonly int Alter;
        public Geschlecht? Geschlecht { get; set; }

        public string? Name { get; set; }
        public string? Adresse { get; set; }
    }
}