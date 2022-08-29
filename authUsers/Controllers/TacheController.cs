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
    public class TacheController : ControllerBase
    {
        private readonly AuthDbContext _context;

        private readonly UserManager<User> _userManager;

        public TacheController(AuthDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // POST api/<TacheController>
        [HttpPost("AddTache/{idDoc}")]
        public async Task<object> AddTache(string idDoc)
        {
            try
            {
                var document = _context.Documents.FirstOrDefault(x => x.ID.ToString().Equals(idDoc));
                if (document != null)
                {
                    List<DocumentState> docstates = _context.DocumentState.Include(x => x.User).Where(x => x.DocumentId.ToString().Equals(idDoc)).ToList();
                    if(docstates.Count >= document.CurrentNumberState)
                    {
                        var currentState = docstates.Find(x => x.StepNumber.Equals(document.CurrentNumberState));
                        if (currentState != null)
                        {
                            var tache = new Tache()
                            {
                                Action = currentState.Action,
                                Etat = State.Awaiting,
                                DateCreation = DateTime.Now,
                                User = currentState.User,
                                Document = document,
                            };
                            _context.Taches.Add(tache);
                            await _context.SaveChangesAsync();
                            return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Tache has been added", tache));
                        }
                        return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Something wrong", null));
                    }
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "No Steps to creat a task", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Check the document id", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [HttpPost("AddTache2")]
        public async Task<object> AddTache2([FromBody] AddUpdateTache tache)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var doc = await _context.Documents.FirstOrDefaultAsync(x => x.ID.ToString().Equals(tache.Document));
                    var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == tache.User);
                   var tach = new Tache()
                   {
                        Action = tache.Action,
                        Etat = State.Awaiting,
                        DateCreation = tache.DateCreation,
                        User = user,    
                        Document = doc,
                   };
                   _context.Taches.Add(tach);
                   await _context.SaveChangesAsync();
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Tache has been added", tache));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Something wrong", null));      
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        // GET api/<TacheController>/5
        [HttpGet("GetAllTaches")]
        public async Task<object> GetAllTaches()
        {
            try
            {
                var taches = await _context.Taches.Include(x => x.User).Include(x => x.Document).ToListAsync();
                if (taches.Count != 0)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", taches));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "there is no data in the table", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [HttpGet("GetTachesByUserId/{userId}")]
        public async Task<object> GetTachesByUserId(string userId)
        {
            try
            {
                var taches = await _context.Taches.Include(x => x.User).Include(x => x.Document).Include(x => x.Document.User).OrderByDescending(x => x.DateCreation).Where(x => x.User.Id == userId).ToListAsync();
                if (taches.Count != 0)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", taches));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "there is no data in the table", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [HttpGet("GetTachesById/{id}")]
        public async Task<object> GetTachesById(string id)
        {
            try
            {
                var tache = await _context.Taches.Include(x => x.User).Include(x => x.Document).Include(x => x.Document.User).FirstOrDefaultAsync(x => x.ID.ToString().Equals(id));
                if (tache != null)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", tache));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Something wrong", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
    }
}
