﻿using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ContratoController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public ContratoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ContratoDto>>> Get()
        {
            var contrato = await _unitOfWork.Contratos.GetAllAsync();
            return _mapper.Map<List<ContratoDto>>(contrato);
        }
        [HttpGet("SelectContractIsActive")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ContratoDto>>> SelectContractIsActive()
        {
            var contrato = await _unitOfWork.Contratos.SelectContractIsActive();
            return Ok(contrato);
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ContratoDto>> Get(int id)
        {
            var contrato = await _unitOfWork.Contratos.GetByIdAsync(id);
            if (contrato == null) return NotFound();
            return _mapper.Map<ContratoDto>(contrato);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Contrato>> Post(ContratoDto contratoDto)
        {
            var contrato = _mapper.Map<Contrato>(contratoDto);
            _unitOfWork.Contratos.Add(contrato);
            await _unitOfWork.SaveAsync();
            if (contrato == null) return BadRequest();
            contratoDto.Id = contrato.Id;
            return CreatedAtAction(nameof(Post), new { id = contratoDto.Id }, contratoDto);
        }
        
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ContratoDto>> Put(int id, [FromBody] ContratoDto contratoDto)
        {
            if (contratoDto == null) return NotFound();
            if (contratoDto.Id == 0) contratoDto.Id = id;
            if (contratoDto.Id != id) return BadRequest();
            var contrato = await _unitOfWork.Contratos.GetByIdAsync(id);
            _mapper.Map(contratoDto, contrato);
            //contrato.FechaModificacion = DateTime.Now;
            _unitOfWork.Contratos.Update(contrato);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<ContratoDto>(contrato);
        
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var contrato = await _unitOfWork.Contratos.GetByIdAsync(id);
            if (contrato == null) return NotFound();
            _unitOfWork.Contratos.Remove(contrato);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}