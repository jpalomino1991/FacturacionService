﻿using FacturacionService.Data;
using FacturacionService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacturacionService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsultaController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ConsultaController(ApplicationDbContext context)
        {
            _context = context;
        }
        public ActionResult<string> Get()
        {
            return "hola";
        }

        [HttpPost]
        [Route("AccesoAnonimo")]
        public async Task<ActionResult> AccesoAnonimo([FromBody] AccesoAnonimoModel anonimo)
        {
            if (ModelState.IsValid)
            {
                List<ComprobanteAnonimo> comprobante = await _context.ComprobanteAnonimo.FromSqlInterpolated($"taComprobanteUsuarioAnominoLeer @NumeroSerie = {anonimo.serie},@NumeroComprobante = {anonimo.numero},@MontoTotal = {anonimo.monto},@FechaComprobante = {Convert.ToDateTime(anonimo.fecha)}").ToListAsync();
                if (comprobante.Count > 0)
                {
                    return Ok(comprobante);
                }
                else
                {
                    return NotFound("No hay resultados");
                }
            }
            else
                return NotFound();
        }
    }
}
