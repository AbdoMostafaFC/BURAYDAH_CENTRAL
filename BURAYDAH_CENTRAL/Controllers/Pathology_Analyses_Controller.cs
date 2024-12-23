using BURAYDAH_CENTRAL.DTOs;
using BURAYDAH_CENTRAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BURAYDAH_CENTRAL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class Pathology_Analyses_Controller : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public Pathology_Analyses_Controller(ApplicationDBContext Context)
        {
            _context = Context;
        }
        [HttpPost]
        [Route("api/pathology_analysis")]
        public async Task<IActionResult> CreatePathologyAnalysis([FromBody] Pathology_AnalysisDTO pathology_AnalysisDTO)
        {
            if (pathology_AnalysisDTO == null) return BadRequest("Analysis data is required.");
            var patho = new Pathology_analysis
            {
                Name = pathology_AnalysisDTO.Name,
                Result = pathology_AnalysisDTO.Result,
                PatientID = pathology_AnalysisDTO.PatientID,

            };

            _context.PathologyAnalysis.Add(patho);
            await _context.SaveChangesAsync();

            return Ok(patho);
        }
        [HttpGet]
        [Route("api/pathology_analysis/{id}")]
        public async Task<IActionResult> GetPathologyAnalysisById(int id)
        {
            var analysis = await _context.PathologyAnalysis.FirstOrDefaultAsync(pa => pa.Id == id);

            if (analysis == null) return NotFound("Analysis not found.");

            return Ok(analysis);
        }

        [HttpGet]
        [Route("api/pathology_analysis")]
        public async Task<IActionResult> GetAllPathologyAnalyses()
        {
            var analyses = await _context.PathologyAnalysis.ToListAsync();
            return Ok(analyses);
        }


        [HttpPut]
        [Route("api/pathology_analysis/{id}")]
        public async Task<IActionResult> UpdatePathologyAnalysis(int id, [FromBody] Pathology_AnalysisDTO pathology_AnalysisDTO)
        {
            if (pathology_AnalysisDTO == null) return BadRequest("Analysis data is required.");

            var analysis = await _context.PathologyAnalysis.FirstOrDefaultAsync(pa => pa.Id == id);

            if (analysis == null) return NotFound("Analysis not found.");

            // Update fields
            analysis.Name = pathology_AnalysisDTO.Name;
            analysis.Result = pathology_AnalysisDTO.Result;
            
            analysis.PatientID = pathology_AnalysisDTO.PatientID;

            await _context.SaveChangesAsync();

            return Ok(analysis);
        }


        [HttpDelete]
        [Route("api/pathology_analysis/{id}")]
        public async Task<IActionResult> DeletePathologyAnalysis(int id)
        {
            var analysis = await _context.PathologyAnalysis.FirstOrDefaultAsync(pa => pa.Id == id);

            if (analysis == null) return NotFound("Analysis not found.");

            _context.PathologyAnalysis.Remove(analysis);
            await _context.SaveChangesAsync();

            return Ok($"Analysis with ID {id} was deleted successfully.");
        }



        //get all Pathology Analysis By PatientID
        [HttpGet]
        [Route("api/patho/AllPatien_PathologyByID{id}")]
        public async Task<List<Pathology_analysis>> all(int PatientId)
        {
            var all= await _context.PathologyAnalysis.Where(p=>p.PatientID == PatientId).ToListAsync();
            return all;
        }

    }
}
