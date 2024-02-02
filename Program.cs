namespace KekTura_struct;
public struct Section
{
    public string kiindulopont;
    public string vegpont;
    public double hossz;
    public int emelkedes;
    public int lejtes;
    public char pecsetelohely;

    // A gyári konstruktor 0 illetve "", '' kezdő értékekkel látta el a mezőket. A konstruktor neve mindig a típus nevével azonos!
    // A gyári konstruktort itt felülírtuk:
    public Section(string line)
    {
        string[] tempLine = line.Trim().Split(";");
        this.kiindulopont = tempLine[0];
        this.vegpont = tempLine[1];
        this.hossz = Convert.ToDouble(tempLine[2]);
        this.emelkedes = Convert.ToInt32(tempLine[3]);
        this.lejtes = Convert.ToInt32(tempLine[4]);
        this.pecsetelohely = Convert.ToChar(tempLine[5]);
    }

    // Ez itt már egy metódus (olyan függvény, amelyet a struktúrában benn készítek el.)
    // A metódus a struct-nak egy képessége. Analógia: otthon, ahol benn főzök, mosok, takarítok...
    public bool IsIncompleteName()
    {
        return !this.vegpont.Contains("pecsetelohely") && this.pecsetelohely == 'i';
    }

    public int StatusCalculate()
    {
        return this.emelkedes - this.lejtes;
    }
}
class Program
{
    static void Main(string[] args)
    {
        List<Section> textDatas = new List<Section>();
        string[] textFile = File.ReadAllLines("./kektura.csv");
        for (int i = 1; i < textFile.Length; i++)
        {
            Section line = new Section(textFile[i]);
            textDatas.Add(line);
        }

        // A túra teljes hossza:
        double summa = 0;
        foreach (Section line in textDatas)
        {
            summa += line.hossz;
        }
        System.Console.WriteLine($"A túra teljes hossza: {summa:f2} km");

        // A túra legrövidebb szakaszának kiinduló pontja, hossza:
        double shortestRoute = textDatas[0].hossz;
        string startPoint = textDatas[0].kiindulopont;
        for (int i = 1; i < textDatas.Count; i++) 
        {
            if (shortestRoute > textDatas[i].hossz) {
                shortestRoute = textDatas[i].hossz;
                startPoint = textDatas[i].kiindulopont;
            }
        }
        System.Console.WriteLine($"A legrövidebb szakasz kezdőpontja: {startPoint}, hossza: {shortestRoute} km");

        // Hiányos végpontok nevei:
        foreach (Section line in textDatas)
        {
            if (line.IsIncompleteName())
            {
                Console.WriteLine($"\t{line.vegpont}");
            }
        }

        // A túra legmagasabb végpontja + magassága:
        int currentHeight = 192;
        string endPoint = "";
        int maximumHeight = 0;
        foreach (Section row in textDatas)
        {
            int status = row.StatusCalculate();
            currentHeight += status;
            if (currentHeight > maximumHeight)
            {
                maximumHeight = currentHeight;
                endPoint = row.vegpont;
            }

        }
        System.Console.WriteLine($"Legmagasabb végpont: {endPoint},  magassága: {maximumHeight} m");
    }
}
