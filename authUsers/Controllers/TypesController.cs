using Application.Models.Entitys;
using Application.Models.Enums;
using Application.Models.Request;
using Application.Models.Responce;
using Infra.Data;
using Microsoft.AspNetCore.Mvc;

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

        // GET: api/<TypesController>
        [HttpGet("GetAllTypes")]
        public async Task<object> GetAllTypes()
        {
            try
            {
                var types = _context.Types.Select(x => x.Nom).ToList();
                if (types.Count != 0)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", types));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "there is no data in the table", types));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        // POST api/<TypesController>
        [HttpPost("AddType")]
        public async Task<object> AddType([FromBody] AddUpdateTypes types)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var type = new Types() { Nom = types.Nom };
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

        // GET api/<TypesController>/5
        [HttpGet("GetTypeById/{id}")]
        public async Task<object> GetTypeById(string id)
        {
            try
            {
                var type = _context.Types.FirstOrDefault(u => u.ID == id);
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
                var type = _context.Types.FirstOrDefault(u => u.ID == id);
                if (type != null)
                {
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
