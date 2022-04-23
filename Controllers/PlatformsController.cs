using Microsoft.AspNetCore.Mvc;

namespace ReadiAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository _repository;
        public PlatformsController(IPlatformRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}", Name="GetPlatformById")]
        public async Task<ActionResult<Platform>> GetPlatformById(string id)
        {
            Platform platform = await _repository.ReadByIdAsync(id);
            if (platform is not null)
            {
                return Ok(platform);
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Platform>>> GetAllPlatforms()
        {
            IEnumerable<Platform> platforms = await _repository.ReadAllAsync();
            return Ok(platforms);
        }

        [HttpPost]
        public async Task<ActionResult<Platform>> CreatePlatform(Platform platform)
        {
            await _repository.CreatePlatformAsync(platform);
            return CreatedAtRoute(nameof(GetPlatformById), new { platform.Id }, platform);
        }
    }
}