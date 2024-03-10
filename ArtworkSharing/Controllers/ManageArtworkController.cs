using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Artworks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtworkSharing.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ManageArtworkController : Controller
    {
        private readonly IArtworkService _artworkService;

        public ManageArtworkController(IArtworkService artworkService)
        {
            _artworkService = artworkService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ArtworkViewModel>>> GetAllArtworks()
        {
            var artworks = await _artworkService.GetAll();
            return Ok(artworks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArtworkViewModel>> GetArtwork(Guid id)
        {
            var artwork = await _artworkService.GetOne(id);
            if (artwork == null)
            {
                return NotFound();
            }
            return Ok(artwork);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArtwork(Guid id, Artwork artworkInput)
        {
            try
            {
                await _artworkService.Update(artworkInput);
                return Ok(artworkInput);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateArtwork(Artwork artworkInput)
        {
            try
            {
                await _artworkService.Add(artworkInput);
                return CreatedAtAction(nameof(GetArtwork), new { id = artworkInput.Id }, artworkInput);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtwork(Guid id)
        {
            try
            {
                await _artworkService.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
