using SynonymSearchApp.Models;

namespace SynonymSearchApp.Repositories
{
    public interface ISynonymRepository
    {
        void AddSynonym(string word, List<string> synonyms);
        List<string> GetSynonyms(string word);
    }

    public class SynonymRepository : ISynonymRepository
    {
        // In-memory storage for synonyms.
        private static readonly Dictionary<string, Synonym> _synonymDictionary = new();

        // Add synonyms to the word and ensure bidirectional synonym mapping
        public void AddSynonym(string word, List<string> synonyms)
        {
            // Ensure the word exists in the dictionary
            if (!_synonymDictionary.ContainsKey(word))
            {
                _synonymDictionary[word] = new Synonym(word);
            }

            // Add the provided synonyms to the word's list of synonyms
            foreach (var synonym in synonyms)
            {
                if (!_synonymDictionary[word].Synonyms.Contains(synonym))
                {
                    _synonymDictionary[word].Synonyms.Add(synonym);
                }

                // Ensure the reverse direction is added as well
                if (!_synonymDictionary.ContainsKey(synonym))
                {
                    _synonymDictionary[synonym] = new Synonym(synonym);
                }

                if (!_synonymDictionary[synonym].Synonyms.Contains(word))
                {
                    _synonymDictionary[synonym].Synonyms.Add(word);
                }
            }
        }

        // Get synonyms for a word (both directions)
        public List<string> GetSynonyms(string word)
        {
            if (_synonymDictionary.ContainsKey(word))
            {
                return _synonymDictionary[word].Synonyms;
            }

            return new List<string>(); // Return an empty list if the word is not found
        }
    }
}
