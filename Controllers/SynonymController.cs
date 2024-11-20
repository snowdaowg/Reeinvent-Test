using Microsoft.AspNetCore.Mvc;
using SynonymSearchApp.Repositories;
using SynonymSearchApp.Models;

namespace SynonymSearchApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SynonymController : ControllerBase
    {
        private readonly ISynonymRepository _synonymRepository;

        public SynonymController(ISynonymRepository synonymRepository)
        {
            _synonymRepository = synonymRepository;
        }

        // POST: api/synonym
        [HttpPost]
        public IActionResult AddSynonym([FromBody] SynonymRequest request)
        {
            // Validate input
            if (request == null || string.IsNullOrEmpty(request.Word) || request.Synonyms == null || request.Synonyms.Count == 0)
            {
                return BadRequest("Invalid data.");
            }

            // Add synonyms to the repository
            _synonymRepository.AddSynonym(request.Word, request.Synonyms);

            return Ok("Synonyms added successfully.");
        }

        // GET: api/synonym/{word}
        [HttpGet("{word}")]
        public IActionResult GetSynonyms(string word)
        {
            var synonyms = _synonymRepository.GetSynonyms(word);

            if (synonyms == null || synonyms.Count == 0)
            {
                return NotFound("No synonyms found.");
            }

            return Ok(synonyms);
        }
    }

    // Helper class to represent the request payload for adding synonyms
    public class SynonymRequest
    {
        public string Word { get; set; }
        public List<string> Synonyms { get; set; }
    }
}
