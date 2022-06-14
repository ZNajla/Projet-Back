using Application.Models.DTO;
using Application.Models.Entitys;
using Application.Models.Enums;
using Application.Models.Request;
using Application.Models.Responce;
using Infra.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace authUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailProcessController : ControllerBase
    {

        private readonly AuthDbContext _context;

        private readonly UserManager<User> _userManager;

        public DetailProcessController(UserManager<User> userManager, AuthDbContext authDbContext)
        {
            _context = authDbContext;
            _userManager = userManager;
        }

        // POST api/<DetailProcessController>
        [HttpPost("AddDetailProcess")]
        public async Task<object> AddDetailProcess([FromBody] List<AddUpdateDetailProcess> detailProcess)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach(AddUpdateDetailProcess detProc in detailProcess)
                    {
                        var user = _userManager.Users.FirstOrDefault(u => u.Id == detProc.User);
                        var proce = _context.Processus.FirstOrDefault(u => u.Id.ToString().Equals(detProc.ProcessusId));
                        var detProcess = new Detail_Processus() { Action = detProc.Action , Step = detProc.Step , Etat = detProc.Etat , Commentaire = detProc.Commentaire , User = user , Processus = proce};
                        _context.Detail_Processus.Add(detProcess);
                        await _context.SaveChangesAsync();
                    }
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "detail Process has been added successfully", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Something went wrong please try again", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        // GET: api/<DetailProcessController>
        [HttpGet("GetDetailProcess/{id}")]
        public async Task<object> GetDetailProcess(string id)
        {
            try
            {
                List<DetailProcessDTO> detailProcess = new List<DetailProcessDTO>();
                var alldetails = _context.Detail_Processus.Where(x => x.ProcessusId.ToString().Equals(id)).ToList();
                if(alldetails.Count != 0)
                {
                    foreach (Detail_Processus det in alldetails)
                    {
                        var user = _userManager.Users.FirstOrDefault(x => x.Id == det.UserId.ToString());
                        detailProcess.Add(new DetailProcessDTO(det.Action, det.Step, det.Etat, det.Commentaire, user.UserName , user.Email));
                    }
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK,"", detailProcess));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "The list is empty", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
           
        }

        // GET api/<DetailProcessController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<DetailProcessController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<DetailProcessController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DetailProcessController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
