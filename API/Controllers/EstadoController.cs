﻿using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class EstadoController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public EstadoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<EstadoDto>>> Get()
        {
            var estado = await _unitOfWork.Estados.GetAllAsync();
            return _mapper.Map<List<EstadoDto>>(estado);
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EstadoDto>> Get(int id)
        {
            var estado = await _unitOfWork.Estados.GetByIdAsync(id);
            if (estado == null) return NotFound();
            return _mapper.Map<EstadoDto>(estado);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Estado>> Post(EstadoDto estadoDto)
        {
            var estado = _mapper.Map<Estado>(estadoDto);
            _unitOfWork.Estados.Add(estado);
            await _unitOfWork.SaveAsync();
            if (estado == null) return BadRequest();
            estadoDto.Id = estado.Id;
            return CreatedAtAction(nameof(Post), new { id = estadoDto.Id }, estadoDto);
        }
        
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EstadoDto>> Put(int id, [FromBody] EstadoDto estadoDto)
        {
            if (estadoDto == null) return NotFound();
            if (estadoDto.Id == 0) estadoDto.Id = id;
            if (estadoDto.Id != id) return BadRequest();
            var estado = await _unitOfWork.Estados.GetByIdAsync(id);
            _mapper.Map(estadoDto, estado);
            //estado.FechaModificacion = DateTime.Now;
            _unitOfWork.Estados.Update(estado);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<EstadoDto>(estado);
        
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var estado = await _unitOfWork.Estados.GetByIdAsync(id);
            if (estado == null) return NotFound();
            _unitOfWork.Estados.Remove(estado);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}