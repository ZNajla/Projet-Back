using Application.Models;
using Application.Models.DTO;
using Application.Models.Entitys;
using Application.Models.Enums;
using Application.Models.Request;
using Application.Models.Responce;
using Infra.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace authUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {

        private readonly AuthDbContext _context;

        private readonly UserManager<User> _userManager;

        public DocumentController ( UserManager<User> userManager, AuthDbContext authDbContext)
        {
            _context = authDbContext;
            _userManager = userManager;
        }
        // GET: api/<DocumentController>
        [HttpGet("GetAllDoc")]
        public async Task<object> GetAllDoc()
        {
            try
            {
                List<DocumentDTO> allDocument = new List<DocumentDTO>();
                var document = await _context.Documents.Include(b => b.User).Include(b => b.Types).ToListAsync();
                if(document.Count != 0)
                {
                    foreach(Document doc in document)
                    {
                        var role = (await _userManager.GetRolesAsync(doc.User)).FirstOrDefault();
                        var user = new UserDTO(doc.User.Id, doc.User.FullName, doc.User.UserName, doc.User.Email, doc.User.PhoneNumber, doc.User.Adresse, role);
                        if(doc.Types == null)
                        {
                            var docu = new DocumentDTO(doc.ID, doc.Url, doc.Reference, doc.Titre, doc.NbPage, doc.MotCle, doc.Version, doc.Date, user,"" , doc.DateUpdate);
                            allDocument.Add(docu);
                        }
                        else
                        {
                            var docu = new DocumentDTO(doc.ID, doc.Url, doc.Reference, doc.Titre, doc.NbPage, doc.MotCle, doc.Version, doc.Date, user, doc.Types.Nom, doc.DateUpdate);
                            allDocument.Add(docu);
                        }
                        
                    }
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", allDocument));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "there is no data in the table", allDocument));

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        // POST api/<DocumentController>
        [HttpPost("AddDoc")]
        public async Task<object> AddDoc([FromBody] AddUpdateDoc doc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _userManager.Users.FirstOrDefault(u => u.Id == doc.UserId);
                    var types = _context.Types.FirstOrDefault(u => u.ID == doc.TypesId);
                    var document = new Document() { Url = doc.Url, Reference = doc.Reference,  Titre = doc.Titre,
                        NbPage = doc.NbPage, MotCle = doc.MotCle, Version = doc.Version, Date = DateTime.Now , User = user, Types = types , DateUpdate = DateTime.Now};
                    var result = _context.Documents.Add(document);
                    var entries =await _context.SaveChangesAsync();
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Document has been added successfully", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Something went wrong please try again", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        // GET api/<DocumentController>/5
        [HttpGet("GetDocById/{id}")]
        public async Task<object> GetDocById(string id)
        {
            try
            {
                var doc = await _context.Documents.Include(b => b.User).Include(b => b.Types).FirstOrDefaultAsync(u => u.ID == id);
                if (doc != null)
                {
                    var role = (await _userManager.GetRolesAsync(doc.User)).FirstOrDefault();
                    var user = new UserDTO(doc.User.Id, doc.User.FullName, doc.User.UserName, doc.User.Email, doc.User.PhoneNumber, doc.User.Adresse, role);
                    if (doc.Types == null)
                    {
                        var docu = new DocumentDTO(doc.ID, doc.Url, doc.Reference, doc.Titre, doc.NbPage, doc.MotCle, doc.Version, doc.Date, user, "" , doc.DateUpdate);
                        return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", docu));
                    }
                    else
                    {
                        var docu = new DocumentDTO(doc.ID, doc.Url, doc.Reference, doc.Titre, doc.NbPage, doc.MotCle, doc.Version, doc.Date, user, doc.Types.Nom, doc.DateUpdate);
                        return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", docu));
                    }
                    
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Something went wrong please try again", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        // PUT api/<DocumentController>/5
        [HttpPut("UpdateDoc/{id}")]
        public async Task<object> UpdateDoc(string id, [FromBody] AddUpdateDoc document)
        {
            try
            {
                var doc = await _context.Documents.Include(b => b.User).Include(b => b.Types).FirstOrDefaultAsync(u => u.ID == id);
                if (doc != null)
                {
                    var types = _context.Types.FirstOrDefault(u => u.ID == document.TypesId);
                    doc.Reference = document.Reference;
                    doc.Titre = document.Titre;
                    doc.NbPage = document.NbPage;
                    doc.MotCle = document.MotCle;
                    doc.Version = document.Version;
                    doc.DateUpdate = DateTime.Now;
                    doc.Types = types;
                    await _context.SaveChangesAsync();
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Document has been updated", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Something went wrong please try again", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        // DELETE api/<DocumentController>/5
        [HttpDelete("DeleteDoc/{id}")]
        public async Task<object> DeleteDoc(string id)
        {
            try
            {
                var doc = _context.Documents.FirstOrDefault(u => u.ID == id);
                if (doc != null)
                {
                    _context.Documents.Remove(doc);
                    await _context.SaveChangesAsync();
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Document has been Deleted", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Document does not exist", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }


    }
}
