using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PessoasController : ControllerBase
    {
        private IList<Pessoa> pessoas;

        public PessoasController()
        {
            pessoas = new List<Pessoa>
            {
                new Pessoa{ Id = 1, Nome = "Maria" },
                new Pessoa{ Id = 2, Nome = "João" },
                new Pessoa{ Id = 3, Nome = "José" },
                new Pessoa{ Id = 4, Nome = "Enzo" },
                new Pessoa{ Id = 5, Nome = "Camila" },
                new Pessoa{ Id = 6, Nome = "Roberto" },
                new Pessoa{ Id = 7, Nome = "José Roberto" },
                new Pessoa{ Id = 8, Nome = "Ana Maria" },
                new Pessoa{ Id = 9, Nome = "Ana" },
                new Pessoa{ Id = 10, Nome = "Alexander" },
            };
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodasPessoas()
        {
            return await Task.Run(() => Ok(pessoas));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(statusCode: 200, Type = typeof(Pessoa))]
        [ProducesResponseType(statusCode: 500, Type = typeof(Exception))]
        [ProducesResponseType(statusCode: 404)]
        public async Task<IActionResult> ObterPessoa(int id)
        {
            var pessoa =
                await Task.Run(() => pessoas.Where(p => p.Id.Equals(id)).FirstOrDefault());

            if (pessoa is null)
            {
                return NotFound();
            }

            return Ok(pessoa);
        }

        [HttpPost]
        public async Task<IActionResult> IncluirPessoa([FromBody] Pessoa pessoa)
        {
            if (!(pessoa is null))
            {
                await Task.Run(() => pessoas.Add(pessoa));

                var uri = Url.Action("ObterPessoa", new { pessoa.Id });

                return Created(uri, pessoa);
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarPessoa([FromBody] Pessoa pessoa)
        {
            if (!(pessoa is null))
            {
                var pessoaBase =
                    await Task.Run(() => pessoas.Where(p => p.Id.Equals(pessoa.Id)).FirstOrDefault());

                if (!(pessoaBase is null))
                {
                    await Task.Run(() => pessoaBase.Nome = pessoa.Nome);

                    return Ok();
                }
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarPessoa(int id)
        {
            var pessoa =
                await Task.Run(() => pessoas.Where(p => p.Id.Equals(id)).FirstOrDefault());

            if (pessoa is null)
            {
                return NotFound();
            }

            await Task.Run(() => pessoas.Remove(pessoa));

            return NoContent();
        }
    }
}
