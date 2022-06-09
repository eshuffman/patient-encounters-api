using AutoMapper;
using HealthAPI.Data.Model;
using HealthAPI.DTOs;
using HealthAPI.Provider.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAPI.Controllers
{
    /// <summary>
    /// The PatientsController exposes endpoints for patient-related actions.
    /// </summary>
    [ApiController]
    [Route("/patients")]
    public class PatientsController : ControllerBase
    {
        private readonly ILogger<PatientsController> _logger;
        private readonly IPatientProvider _patientProvider;
        private readonly IMapper _mapper;

        public PatientsController(
            ILogger<PatientsController> logger,
            IPatientProvider patientProvider,
            IMapper mapper
        )
        {
            _logger = logger;
            _patientProvider = patientProvider;
            _mapper = mapper;
        }

        /// <summary>
        /// Sends patient information to patients endpoint
        /// </summary>
        /// <param name="patientToCreate"></param>
        /// <returns>Created patient DTO</returns>
        [HttpPost]
        public async Task<ActionResult<PatientDTO>> CreatePatientAsync([FromBody] Patient patientToCreate)
        {
            _logger.LogInformation("Request received for CreatePatientAsync");

            var patient = await _patientProvider.CreatePatientAsync(patientToCreate);
            var patientDTO = _mapper.Map<PatientDTO>(patient);

            return Created("/patients", patientDTO);
        }

        /// <summary>
        /// Retrieves all saved patient objects from the patients endpoint
        /// </summary>
        /// <returns>List of all saved patient DTOs</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDTO>>> GetAllPatientsAsync()
        {
            _logger.LogInformation("Request received for GetAllPatientsAsync");

            var patients = await _patientProvider.GetAllPatientsAsync();
            var patientsDTOs = _mapper.Map<IEnumerable<PatientDTO>>(patients);

            return Ok(patientsDTOs);
        }

        /// <summary>
        /// Retrieves one saved patient object from patients endpoint based on inputted ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Patient DTO of requested patient</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDTO>> GetPatientByIdAsync(int id)
        {
            _logger.LogInformation($"Request received for GetMovieByIdAsync for id: {id}");

            var patient = await _patientProvider.GetPatientByIdAsync(id);
            var patientDTO = _mapper.Map<PatientDTO>(patient);

            return Ok(patientDTO);
        }

        /// <summary>
        /// Updates patient information based on patient ID, using information packet
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patientToUpdate"></param>
        /// <returns>Updated movie DTO</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<PatientDTO>> UpdatePatientAsync(
            int id, [FromBody] PatientDTO patientToUpdate)
        {
            _logger.LogInformation("Request received for UpdatePatientAsync");

            var patient = _mapper.Map<Patient>(patientToUpdate);
            var updatedPatient = await _patientProvider.UpdatePatientAsync(id, patient);
            var patientDTO = _mapper.Map<PatientDTO>(updatedPatient);

            return Ok(patientDTO);
        }

        /// <summary>
        /// Deletes patient object based on inputted ID
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>204 No Content status code.</returns>
        [HttpDelete("{patientId}")]
        public async Task<ActionResult<PatientDTO>> DeletePatientByIdAsync(int patientId)
        {
            _logger.LogInformation("Request received for DeleteMovieByIdAsync");
            try
            {
                var movie = await _patientProvider.DeletePatientByIdAsync(patientId);
                return NoContent();
            }
            catch (System.ArgumentNullException)
            {
                return NotFound($"No patient with ID: {patientId} exists in the database");
            }

        }
    }
}
