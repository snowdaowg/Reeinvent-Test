namespace SynonymSearchApp.Models
{
    public class Synonym
    {
        public string Word { get; set; }
        public List<string> Synonyms { get; set; }

        public Synonym(string word)
        {
            Word = word;
            Synonyms = new List<string>();
        }
    }
}
