using Microsoft.AspNetCore.Mvc;
using Api_lucas.Data.Collections;
using Api_lucas.Models;
using MongoDB.Driver;
using System;

namespace Api_lucas.Controllers
{   
        [ApiController]
    [Route("[controller]")]
    public class MorteController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Morte> _MortesCollection;

        public MorteController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _MortesCollection = _mongoDB.DB.GetCollection<Morte>(typeof(Morte).Name.ToLower());
        }

        [HttpPost]
        public ActionResult SalvarMorte([FromBody] MorteDto dto)
        {
            var Morte = new Morte(dto.DataNascimento, dto.DataMorte, dto.Sexo, dto.Latitude, dto.Longitude, dto.Comorbidade);

            _MortesCollection.InsertOne(Morte);
            
            return StatusCode(201, "Morte adicionado com sucesso");
        }

        [HttpGet]
        public ActionResult ObterMorte()
        {
            var Mortes = _MortesCollection.Find(Builders<Morte>.Filter.Empty).ToList();
            
            return Ok(Mortes);
        }
        [HttpPut]
        public ActionResult AtualizarInfectados([FromBody] MorteDto dto)
        {
            var Mortes = new Morte(dto.DataNascimento, dto.DataMorte, dto.Sexo, dto.Latitude, dto.Longitude, dto.Comorbidade);
            
            _MortesCollection.UpdateOne(Builders<Morte>.Filter.Where(_ => _.DataMorte == dto.DataMorte),
                                                                             Builders<Morte>.Update.Set("sexo", dto.Sexo));
            
            return Ok("Atualizado com sucesso");
        }
        [HttpDelete("{dataNasc}")]
        public ActionResult Delete(DateTime dataMort)
        {            
            _MortesCollection.DeleteOne(Builders<Morte>.Filter.Where(_ => _.DataMorte == dataMort));
            
            return Ok("deletado com sucesso");
        }
    }
}