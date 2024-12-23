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
    public class PatientController : ControllerBase
    {
        private readonly ApplicationDBContext context;

        public PatientController(ApplicationDBContext Context)
        {
            context = Context;
        }
        [HttpPost]
        public async Task<IActionResult> AddPatient([FromBody] PatientDTO patientDto)
        {

            if (patientDto != null)
            {
                var IsExist=context.Patients.FirstOrDefault(p=>p.Id == patientDto.UniqueNumber);
                if (IsExist == null) { 
                
                    Patient Patient=new Patient
                    {
                        UniqueNumber=patientDto.UniqueNumber,
                        Name=patientDto.Name,
                        Age=patientDto.Age,
                        Gender=patientDto.Gender,
                        DateTime=patientDto.DateTime,
                    };
                    context.Patients.Add(Patient);
                    context.SaveChanges();
                    return Ok(new {Message="Patient Created Successflly",ID=Patient.Id });
                }
            }

            return BadRequest("invalide data to Save!!!");
        }
        //[HttpGet]
        //[Route("api/patients/{id}")]
        //public async Task<IActionResult> GetPatientById(int id)
        //{
        //    var patient = await context.Patients
        //        .Include(p => p.pathology_Analyses) // Eagerly load related PathologyAnalyses
        //        .FirstOrDefaultAsync(p => p.Id == id);

        //    if (patient == null) return NotFound("Patient not found.");

        //    return Ok(patient);
        //}


        [HttpGet]
        [Route("api/patients/{id}")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var patient = await context.Patients
                .Include(p => p.pathology_Analyses)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null) return NotFound("Patient not found.");

            var patientDto = new PatientDto
            {
                Id = patient.Id,
                 Age=patient.Age,
                 Gender = patient.Gender,
                 UniqNumber=patient.UniqueNumber,
                 CreatedTime=patient.DateTime,

                Name = patient.Name,
                PathologyAnalyses = patient.pathology_Analyses.Select(pa => new PathologyAnalysisDto
                {
                    Id = pa.Id,
                    AnalysisName = pa.Name,
                    Result=pa.Result
                    
                    
                }).ToList()
            };

            return Ok(patientDto);
        }



        [HttpPut]
        [Route("api/patients/{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] Patient updatedPatient)
        {
            if (updatedPatient == null) return BadRequest("Patient data is required.");

            
            var patient = await context.Patients
                .Include(p => p.pathology_Analyses)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null) return NotFound("Patient not found.");

          
            patient.UniqueNumber = updatedPatient.UniqueNumber;
            patient.Name = updatedPatient.Name;
            patient.Age = updatedPatient.Age;
            patient.Gender = updatedPatient.Gender;

            
            foreach (var analysis in updatedPatient.pathology_Analyses)
            {
                if (analysis.Id == 0)
                {
                   
                    patient.pathology_Analyses.Add(analysis);
                }
                else
                {
                    
                    var existingAnalysis = patient.pathology_Analyses
                        .FirstOrDefault(pa => pa.Id == analysis.Id);
                    if (existingAnalysis != null)
                    {
                        existingAnalysis.Name = analysis.Name;
                        existingAnalysis.Result = analysis.Result;
                        
                    }
                }
            }

            
            var removedAnalyses = patient.pathology_Analyses
                .Where(pa => !updatedPatient.pathology_Analyses.Any(upa => upa.Id == pa.Id))
                .ToList();
            foreach (var removedAnalysis in removedAnalyses)
            {
                context.PathologyAnalysis.Remove(removedAnalysis);
            }

           
            await context.SaveChangesAsync();

            return Ok(patient);
        }


        [HttpDelete]
        [Route("api/patients/{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            
            var patient = await context.Patients
                .Include(p => p.pathology_Analyses) 
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null) return NotFound("Patient not found.");

            
            context.Patients.Remove(patient);

           
            await context.SaveChangesAsync();

            return Ok($"Patient with ID {id} and their related analyses were deleted successfully.");
        }
        public class PatientDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Gender { get; set; }
            public int Age { get; set; }
            public DateTime CreatedTime { get; set; }
            public int UniqNumber { get; set; }
            public List<PathologyAnalysisDto> PathologyAnalyses { get; set; }
        }

        public class PathologyAnalysisDto
        {
            public int Id { get; set; }
            public string AnalysisName { get; set; }
            public string? Result { get; set; }
        }


    }
}
