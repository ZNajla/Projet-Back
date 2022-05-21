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
                    var types = _context.Types.FirstOrDefault(u => u.ID == proc.Types);
                    var processus = new Processus()
                    {
                        NomProcessus = proc.NomProcessus,
                        Description = proc.Description,
                        Date_debut = DateTime.Now,
                        Date_fin = proc.Date_fin,
                        Types = types,
                    };
                    var result = _context.Processus.Add(processus);
                    var entries = await _context.SaveChangesAsync();
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Process has been added successfully", null));
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
                var processus = await _context.Processus.Include(b => b.Types).ToListAsync();
                if (processus.Count != 0)
                {
                    foreach (Processus pro in processus)
                    {
                        if (pro.Types == null)
                        {
                            var proc = new ProcessusDTO(pro.NomProcessus,pro.Description,pro.Date_debut,pro.Date_fin,"");
                            allProcessus.Add(proc);
                        }
                        else
                        {
                            var proc = new ProcessusDTO(pro.NomProcessus, pro.Description, pro.Date_debut, pro.Date_fin, pro.Types.Nom);
                            allProcessus.Add(proc);
                        }

                    }
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", allProcessus));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "there is no data in the table", allProcessus));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        // GET api/<ProcessusController>/5
        [HttpGet("GetProcessById/{id}")]
        public async Task<object> GetProcessById(string id)
        {
            try
            {
                var proc = await _context.Processus.Include(b => b.Types).FirstOrDefaultAsync(u => u.Id == id);
                if (proc != null)
                {
                    if (proc.Types == null)
                    {
                        var procu = new ProcessusDTO(proc.NomProcessus ,proc.Description ,proc.Date_debut,proc.Date_fin ,"");
                        return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", procu));
                    }
                    else
                    {
                        var procu = new ProcessusDTO(proc.NomProcessus, proc.Description, proc.Date_debut, proc.Date_fin, proc.Types.Nom);
                        return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", procu));
                    }

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
                var proc = _context.Processus.FirstOrDefault(u => u.Id == id);
                if (proc != null)
                {
                    var types = _context.Types.FirstOrDefault(u => u.ID == process.Types);
                    proc.NomProcessus = process.NomProcessus;
                    proc.Description = process.Description;
                    proc.Date_debut = process.Date_debut;
                    proc.Date_fin = process.Date_fin;
                    proc.Types = types;
                    await _context.SaveChangesAsync();
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Processus has been updated", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Processus does not exist", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        // DELETE api/<ProcessusController>/5
        [HttpDelete("DeleteProcessus/{id}")]
        public async Task<object> DeleteProcessus(string id)
        {
            try
            {
                var proc = _context.Processus.FirstOrDefault(u => u.Id == id);
                if (proc != null)
                {
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
