using Application.Models.DTO;
using Application.Models.Entitys;
using Application.Models.Enums;
using Application.Models.Request;
using Application.Models.Responce;
using Infra.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace authUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {

        private readonly AuthDbContext _context;

        private readonly UserManager<User> _userManager;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public DocumentController(UserManager<User> userManager, AuthDbContext authDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _context = authDbContext;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
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
                    var types = _context.Types.FirstOrDefault(u => u.ID.ToString().Equals(doc.TypesId));
                    //var path = uploadFile(doc.file);
                    if(doc.CurrentState == 0)
                    {
                        var document = new Document()
                        {
                            Url = doc.file,
                            Reference = doc.Reference,
                            Titre = doc.Titre,
                            NbPage = doc.NbPage,
                            MotCle = doc.MotCle,
                            Version = doc.Version,
                            Date = DateTime.Now,
                            DateUpdate = DateTime.Now,
                            CurrentState = State.Awaiting,
                            CurrentNumberState = 1, //etat du document a l'etape i
                            User = user,
                            Types = types
                        };
                        var result = _context.Documents.Add(document);
                        var entries = await _context.SaveChangesAsync();
                        return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Document has been added successfully", document.ID));
                    }
                    else
                    {
                        var document = new Document()
                        {
                            Url = doc.file,
                            Reference = doc.Reference,
                            Titre = doc.Titre,
                            NbPage = doc.NbPage,
                            MotCle = doc.MotCle,
                            Version = doc.Version,
                            Date = DateTime.Now,
                            DateUpdate = DateTime.Now,
                            CurrentState = State.draft ,
                            CurrentNumberState = 1, //etat du document a l'etape i
                            User = user,
                            Types = types
                        };
                        var result = _context.Documents.Add(document);
                        var entries = await _context.SaveChangesAsync();
                        return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Document has been added successfully", document.ID));
                    }
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Something went wrong please try again", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        // GET: api/<DocumentController>
        [HttpGet("GetAllDoc")]
        public async Task<object> GetAllDoc()
        {
            try
            {
                var document = await _context.Documents.Include(b => b.User).Include(b => b.Types).Include(b => b.DocumentStates).OrderByDescending(b => b.Date).ToListAsync();
                if(document.Count != 0)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", document));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "there is no data in the table", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        // GET: api/<DocumentController>
        [HttpGet("GetDocumentByState/{state}")]
        public async Task<object> GetDocumentByState(string state)
        {
            try
            {
                List<Document> docList = new List<Document>();
                if (state == "Validated")
                {
                    docList = _context.Documents.Include(u => u.DocumentStates).Include(u => u.Types).Include(u => u.User).OrderByDescending(u => u.Date).Where(u => u.CurrentState.Equals(State.Validated)).ToList();
                }
                if (state == "In Progress")
                {
                    docList = _context.Documents.Include(u => u.DocumentStates).Include(u => u.Types).Include(u => u.User).OrderByDescending(u => u.Date).Where(u => u.CurrentState.Equals(State.In_Progress)).ToList();
                }
                if (state == "Draft")
                {
                    docList = _context.Documents.Include(u => u.DocumentStates).Include(u => u.Types).Include(u => u.User).OrderByDescending(u => u.Date).Where(u => u.CurrentState.Equals(State.draft)).ToList();
                }
                if (state == "Rejected")
                {
                    docList = _context.Documents.Include(u => u.DocumentStates).Include(u => u.Types).Include(u => u.User).OrderByDescending(u => u.Date).Where(u => u.CurrentState.Equals(State.Cancelled)).ToList();
                }
                if (state == "Awaiting")
                {
                    docList = _context.Documents.Include(u => u.DocumentStates).Include(u => u.Types).Include(u => u.User).OrderByDescending(u => u.Date).Where(u => u.CurrentState.Equals(State.Awaiting)).ToList();
                }
                if (docList.Count != 0)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", docList));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "there is no document with that state", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        // GET: api/<DocumentController>
        [HttpGet("GetValidatedDocument")]
        public async Task<object> GetValidatedDocument()
        {
            try
            {
                var docList = _context.Documents.Include(u => u.DocumentStates).Include(u => u.Types).Include(u => u.User).OrderByDescending(u => u.Date).Where(u => u.CurrentState.Equals(3)).ToList();
                if (docList.Count != 0)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", docList));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "there is no document with that state", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        // GET api/<DocumentController>
        [HttpGet("GetDocById/{id}")]
        public async Task<object> GetDocById(string id)
        {
            try
            {
                 var doc = await _context.Documents.Include(b => b.User).Include(b => b.Types).Include(b => b.DocumentStates).FirstOrDefaultAsync(u => u.ID.ToString().Equals(id));
                 if (doc != null)
                 {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", doc));
                 }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Something went wrong please try again", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        // GET api/<DocumentController>
        [HttpGet("GetDocByUserId/{id}")]
        public async Task<object> GetDocByUserId(string id)
        {
            try
            {
                var doc = await _context.Documents.Include(b => b.User).Include(b => b.Types).Include(b => b.DocumentStates).OrderByDescending(b => b.Date).Where(b => b.User.Id == id).ToListAsync();
                if (doc.Count != 0)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", doc));
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
                var doc = await _context.Documents.Include(b => b.User).Include(b => b.Types).FirstOrDefaultAsync(u => u.ID.ToString().Equals(id));
                if (doc != null)
                {
                    var types = _context.Types.FirstOrDefault(u => u.ID.ToString().Equals(document.TypesId));
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
                var doc = _context.Documents.FirstOrDefault(u => u.ID.ToString().Equals(id));
                var docState = _context.DocumentState.Where(u => u.DocumentId.ToString().Equals(id)).ToList();
                if (doc != null)
                {
                    if (System.IO.File.Exists(doc.Url))
                    {
                        System.IO.File.Delete(doc.Url);
                    }
                    _context.Documents.Remove(doc);
                    foreach (DocumentState state in docState)
                    {
                        _context.DocumentState.Remove(state);
                    }
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

        [HttpPost("Upload"), DisableRequestSizeLimit]
        public async Task<object> Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "Uploaded Files");
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", fullPath));
                }
                else
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Error Size", null));
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [HttpGet("Download/{id}")]
        public async Task<ActionResult> Download(string id)
        {
            var provider = new FileExtensionContentTypeProvider();
            var document = await _context.Documents.FirstOrDefaultAsync(x => x.ID.ToString().Equals(id));

            if(document == null)
            {
                return NotFound();
            }

            var file = Path.Combine(Directory.GetCurrentDirectory(), "Uploaded Files", document.Titre);

            string contentType;

            if(!provider.TryGetContentType(file, out contentType))
            {
                contentType = "application/octet-stream";
            }

            byte[] fileBytes;

            if (System.IO.File.Exists(file))
            {
                fileBytes = System.IO.File.ReadAllBytes(file);
            }
            else
            {
                return NotFound();
            }

            return File(fileBytes, contentType, document.Titre);
        }
    }
}
