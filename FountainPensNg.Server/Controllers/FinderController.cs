using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FountainPensNg.Server.Data;
using FountainPensNg.Server.Data.Models;
using FountainPensNg.Server.Data.DTO;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using FountainPensNg.Server.Helpers;
using FountainPensNg.Server.Data.Repos;

namespace FountainPensNg.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class FinderController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly FinderRepo _finderRepo;

        public FinderController(DataContext context, FinderRepo finderRepo)
        {
            _context = context;
            _finderRepo = finderRepo;
        }

        [HttpGet("{fulltext}")]
        public async Task<ActionResult<IEnumerable<SearchResultDTO>>> Get(string fulltext)
        {
            return await _finderRepo.FindAll(fulltext);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SearchResultDTO>>> Get() {
            return await _finderRepo.FindAll("");
        }
    }
}
