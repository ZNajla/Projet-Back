using Application.Models.Entitys;
using Application.Models.Enums;
using Application.Models.Request;
using Application.Models.Responce;
using Infra.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace authUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypesController : ControllerBase
    {
        private readonly AuthDbContext _context;

        public TypesController(AuthDbContext authDbContext)
        {
            _context = authDbContext;
        }

        // POST api/<TypesController>
        [HttpPost("AddType")]
        public async Task<object> AddType([FromBody] AddUpdateTypes types)
        {
            try
            {
                var proc = _context.Processus.FirstOrDefault(x => x.Id.ToString().Equals(types.ProcessId));
                if (ModelState.IsValid)
                {
                    var type = new Types() { Nom = types.Nom, Processus = proc };
                    var result = _context.Types.Add(type);
                    await _context.SaveChangesAsync();
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Type has been added successfully", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Something went wrong please try again", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        // GET: api/<TypesController>
        [HttpGet("GetAllTypes")]
        public async Task<object> GetAllTypes()
        {
            try
            {
                var types = _context.Types.Include(u => u.Processus).ToList();
                if (types.Count != 0)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", types));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "there is no data in the table",null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        // GET api/<TypesController>
        [HttpGet("GetTypeById/{id}")]
        public async Task<object> GetTypeById(string id)
        {
            try
            {
                var type = _context.Types.Include(b => b.Processus).FirstOrDefault(u => u.ID.ToString().Equals(id));
                if (type != null)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", type.Nom));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Something went wrong please try again", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        // DELETE api/<TypesController>/5
        [HttpDelete("DeleteType/{id}")]
        public async Task<object> DeleteType(string id)
        {
            try
            {
                var type = _context.Types.FirstOrDefault(u => u.ID.ToString().Equals(id));
                var listDocs = _context.Documents.Where(x => x.Types.ID.ToString().Equals(id)).ToList();
                if (type != null)
                {
                    foreach (Document doc in listDocs)
                    {
                        doc.Types = null;
                    }
                    _context.Types.Remove(type);
                    await _context.SaveChangesAsync();
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Type has been Deleted", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Something went wrong please try again", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
    }
}