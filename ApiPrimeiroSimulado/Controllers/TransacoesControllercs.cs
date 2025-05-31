using ApiPrimeiroSimulado.Dtos.Transacao;
using ApiPrimeiroSimulado.Models;
using ApiPrimeiroSimulado.Services.Transacoes;
using Microsoft.AspNetCore.Mvc;

namespace ApiPrimeiroSimulado.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransacoesController : ControllerBase
{
    private readonly ITransacaoInterface _transacaoService;

    public TransacoesController(ITransacaoInterface transacaoService)
    {
        _transacaoService = transacaoService;
    }

    [HttpGet]
    public async Task<ActionResult<ResponseModel<IEnumerable<TransacaoResponseDto>>>> GetAll()
    {
        var response = await _transacaoService.GetAllAsync();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseModel<TransacaoResponseDto>>> GetById(int id)
    {
        var response = await _transacaoService.GetByIdAsync(id);
        if (!response.status)
            return NotFound(response);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ResponseModel<TransacaoResponseDto>>> Create([FromBody] TransacaoRequestDto dto)
    {
        var response = await _transacaoService.CreateAsync(dto);
        if (!response.status)
            return BadRequest(response);

        return CreatedAtAction(nameof(GetById), new { id = response.dados?.idTransacao }, response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ResponseModel<string>>> Update(int id, [FromBody] TransacaoRequestDto dto)
    {
        var response = await _transacaoService.UpdateAsync(id, dto);
        if (!response.status)
            return NotFound(response);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ResponseModel<string>>> Delete(int id)
    {
        var response = await _transacaoService.DeleteAsync(id);
        if (!response.status)
            return NotFound(response);
        return Ok(response);
    }
}
