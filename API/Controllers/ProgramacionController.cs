﻿using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProgramacionController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public ProgramacionController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProgramacionDto>>> Get()
        {
            var programacion = await _unitOfWork.Programaciones.GetAllAsync();
            return _mapper.Map<List<ProgramacionDto>>(programacion);
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProgramacionDto>> Get(int id)
        {
            var programacion = await _unitOfWork.Programaciones.GetByIdAsync(id);
            if (programacion == null) return NotFound();
            return _mapper.Map<ProgramacionDto>(programacion);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Programacion>> Post(ProgramacionDto programacionDto)
        {
            var programacion = _mapper.Map<Programacion>(programacionDto);
            _unitOfWork.Programaciones.Add(programacion);
            await _unitOfWork.SaveAsync();
            if (programacion == null) return BadRequest();
            programacionDto.Id = programacion.Id;
            return CreatedAtAction(nameof(Post), new { id = programacionDto.Id }, programacionDto);
        }
        
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProgramacionDto>> Put(int id, [FromBody] ProgramacionDto programacionDto)
        {
            if (programacionDto == null) return NotFound();
            if (programacionDto.Id == 0) programacionDto.Id = id;
            if (programacionDto.Id != id) return BadRequest();
            var programacion = await _unitOfWork.Programaciones.GetByIdAsync(id);
            _mapper.Map(programacionDto, programacion);
            //programacion.FechaModificacion = DateTime.Now;
            _unitOfWork.Programaciones.Update(programacion);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<ProgramacionDto>(programacion);
        
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var programacion = await _unitOfWork.Programaciones.GetByIdAsync(id);
            if (programacion == null) return NotFound();
            _unitOfWork.Programaciones.Remove(programacion);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}