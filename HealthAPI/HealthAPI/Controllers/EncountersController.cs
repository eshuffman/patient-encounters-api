using AutoMapper;
using HealthAPI.Data.Model;
using HealthAPI.DTOs;
using HealthAPI.Provider.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using HealthAPI.DTOMappings;

namespace HealthAPI.Controllers
{
    /// <summary>
    /// The EncountersController exposes endpoints for patient encounter-related actions.
    /// </summary>
    [ApiController]
    [Route("/patients")]
    public class EncountersController : ControllerBase
    {
        private readonly ILogger<EncountersController> _logger;
        private readonly IEncounterProvider _encounterProvider;
        private readonly IMapper _mapper;

        public EncountersController(
            ILogger<EncountersController> logger,
            IEncounterProvider encounterProvider,
            IMapper mapper
        )
        {
            _logger = logger;
            _encounterProvider = encounterProvider;
            _mapper = mapper;
        }

        /// <summary>
        /// Sends patient encounter information to encounter endpoint
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="encounterToCreate"></param>
        /// <returns>DTO of created patient encounter</returns>
        [HttpPost("{patientId}/encounters")]
        public async Task<ActionResult<EncounterDTO>> CreateEncounterAsync(int? patientId, [FromBody] EncounterDTO encounterToCreate)
        {
            _logger.LogInformation("Request received for CreateEncounterAsync");

            var newEncounter = _mapper.MapEncounterDtoToEncounter(encounterToCreate);
            var patientEncounter = await _encounterProvider.CreateEncounterAsync(patientId, newEncounter);
            var encounterDTO = _mapper.MapEncounterToEncounterDto(patientEncounter);

            //if (encounterDTO == null)
            //{
            //    throw new NotImplementedException();
            //}
            return Created($"/{patientId}/encounters", encounterDTO);
        }

        /// <summary>
        /// Retrieves all patient encounters specific to a single patient from the database.
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>List of all encounter DTOs with patientId matching param Id</returns>
        [HttpGet("{patientId}/encounters")]
        public async Task<ActionResult<List<EncounterDTO>>> GetAllEncountersByIdAsync(int? patientId)
        {

            var encounters = await _encounterProvider.GetAllEncountersByIdAsync(patientId);
            var encountersDTOs = _mapper.MapEncountersToEncounterDtos(encounters);

            return Ok(encountersDTOs);
        }

        /// <summary>
        /// Retrieves saved patient encounter object from encounters endpoint based on inputted encounter
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>DTO of requested encounter</returns>
        [HttpGet("{patientId}/encounters/{encounterId}")]
        public async Task<ActionResult<EncounterDTO>> GetEncounterByIdAsync(int encounterId)
        {
            _logger.LogInformation($"Request received for GetEncounterByIdAsync for id: {encounterId}");

            var encounter = await _encounterProvider.GetEncounterByIdAsync(encounterId);
            var encounterDTO = _mapper.Map<EncounterDTO>(encounter);

            return Ok(encounterDTO);
        }

        /// <summary>
        /// Updates existing patient encounter based on patient ID, using information packet
        /// </summary>
        /// <param name="encounterId"></param>
        /// <param name="encounterToUpdate"></param>
        /// <returns>Updated rental DTO</returns>
        [HttpPut("{patientId}/encounters/{encounterId}")]

        public async Task<ActionResult<EncounterDTO>> UpdateEncounterAsync(
            int encounterId, [FromBody] EncounterDTO encounterToUpdate)
        {
            _logger.LogInformation("Request received for UpdateEncounterAsync");

            var encounter = _mapper.Map<Encounter>(encounterToUpdate);
            var updatedEncounter = await _encounterProvider.UpdateEncounterAsync(encounterId, encounter);
            var encounterDto = _mapper.Map<EncounterDTO>(updatedEncounter);

            return Ok(encounterDto);
        }
    }
}
