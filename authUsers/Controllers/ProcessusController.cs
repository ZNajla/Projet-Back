using Application.Models.DTO;
using Application.Models.Entitys;
using Application.Models.Enums;
using Application.Models.Request;
using Application.Models.Responce;
using Infra.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace authUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessusController : ControllerBase
    {
        private readonly AuthDbContext _context;

        private readonly UserManager<User> _userManager; 

        public ProcessusController(UserManager<User> userManager, AuthDbContext authDbContext)
        {
            _context = authDbContext;
            _userManager = userManager;
        }

        // POST api/<ProcessusController>
        [HttpPost("AddProcessus")]
        public async Task<object> AddProcessus([FromBody] AddUpdateProcessus proc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var processus = new Processus()
                    {
                        NomProcessus = proc.NomProcessus,
                        Description = proc.Description,
                    };
                    _context.Processus.Add(processus);
                    await _context.SaveChangesAsync();
                    var insertedProcess = processus.Id;
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Process has been added successfully", insertedProcess));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Something went wrong please try again", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        // GET: api/<ProcessusController>
        [HttpGet("GetAllProcessus")]
        public async Task<object> GetAllProcessus()
        {
            try
            {
                List<ProcessusDTO> allProcessus = new List<ProcessusDTO>();
                var processus = await _context.Processus.Include(b => b.Detail_Processus).ToListAsync();
                if (processus.Count != 0)
                {
                    foreach (Processus pro in processus)
                    {
                        var proc = new ProcessusDTO(pro.NomProcessus,pro.Description);
                        allProcessus.Add(proc);
                    }
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", processus));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "there is no data in the table", allProcessus));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        // GET api/<ProcessusController>
        [HttpGet("GetProcessById/{id}")]
        public async Task<object> GetProcessById(string id)
        {
            try
            {
                 var proc = await _context.Processus.Include(b => b.Detail_Processus).FirstOrDefaultAsync(u => u.Id.ToString().Equals(id));
                 if (proc != null)
                 {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", proc));  
                 }
                 return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Something went wrong please try again", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        // PUT api/<ProcessusController>/5
        [HttpPut("updateProcess/{id}")]
        public async Task<object> updateProcess(string id, [FromBody] AddUpdateProcessus process)
        {
            try
            {
                var proc = _context.Processus.FirstOrDefault(u => u.Id.ToString().Equals(id));
                if (proc != null)
                {
                    proc.NomProcessus = process.NomProcessus;
                    proc.Description = process.Description;
                    await _context.SaveChangesAsync();
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Processus has been updated", proc));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Processus does not exist", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        // DELETE api/<ProcessusController>
        [HttpDelete("DeleteProcessus/{id}")]
        public async Task<object> DeleteProcessus(string id)
        {
            try
            {
                var proc = _context.Processus.FirstOrDefault(u => u.Id.ToString().Equals(id));
                var listTypes = _context.Types.Where(x => x.Processus.Id.ToString().Equals(id)).ToList();
                if (proc != null)
                {
                    foreach (Types type in listTypes)
                    {
                        type.Processus = null;
                    }
                    _context.Processus.Remove(proc);
                    await _context.SaveChangesAsync();
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Processus has been Deleted", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Processus does not exist", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
    }
}