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
    public class DocumentStateController : ControllerBase
    {

        private readonly AuthDbContext _context;

        public DocumentStateController(AuthDbContext context)
        {
            _context = context;
        }

        // POST api/<DocumentStateController>
        [HttpPost("AddDocStatue/{idDoc}")]
        public async Task<object> AddDocStatue(string idDoc)
        {
            try
            {
                var doc = await _context.Documents.Include(b => b.User).Include(b => b.Types).FirstOrDefaultAsync(u => u.ID.ToString().Equals(idDoc));
                var typeDoc = await _context.Types.Include(b => b.Processus).FirstOrDefaultAsync(b => b.ID.Equals(doc.Types.ID));
                List<Detail_Processus> detProc = _context.Detail_Processus.Include(x => x.User).Where(b => b.ProcessusId.Equals(typeDoc.Processus.Id)).ToList();
                if (detProc.Count != 0)
                {
                    foreach (Detail_Processus detail in detProc)
                    {
                        var documentState = new DocumentState()
                        {
                            StateDocument = State.Awaiting,
                            NumeroState = detail.Step,
                            Action = detail.Action,
                            Date = DateTime.Now,
                            Comment = "",
                            Document = doc,
                            User = detail.User,
                        };
                        _context.DocumentState.Add(documentState);
                        await _context.SaveChangesAsync();
                    }
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Document State has been added successfully", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Something went wrong please try again", typeDoc));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        // GET api/<DocumentStateController>
        [HttpGet("GetDocumentState/{docId}")]
        public async Task<object> GetDocumentState(string docId)
        {
            try
            {
                var docState = _context.DocumentState.Where(u => u.DocumentId.ToString().Equals(docId)).ToList();
                if (docState != null)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", docState));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Something went wrong please try again", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        //PUT api/<DocumentStateController>/5
        [HttpPut("UpdateStatue/{idDoc}")]
         public async Task<object> UpdateStatue(string idDoc, [FromBody] AddUpdateDocState docStatue)
         {
            try
            {
                var doc = await _context.Documents.FirstOrDefaultAsync(u => u.ID.ToString().Equals(idDoc));
                List<DocumentState> docStates = _context.DocumentState.Where(b => b.DocumentId.ToString().Equals(idDoc)).ToList();
                if(docStates.Count != 0)
                {
                    if(docStatue.StepNumber == docStates.Count)
                    {
                        var step = docStates.Find(x => x.NumeroState == docStatue.StepNumber);
                        if (docStatue.StateDocument == State.Validated)
                        {
                            doc.DateUpdate = DateTime.Now;
                            doc.CurrentState = State.Validated;
                            doc.CurrentNumberState++;
                        }
                        if(docStatue.StateDocument == State.Cancelled)
                        {
                            doc.DateUpdate = DateTime.Now;
                            doc.CurrentState = State.Cancelled;
                            doc.CurrentNumberState++;
                        }
                        step.StateDocument = docStatue.StateDocument;
                        step.Comment = docStatue.Comment;
                        step.Date = docStatue.Date;
                        await _context.SaveChangesAsync();
                        return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Document has been "+docStatue.StateDocument, null));
                    }
                    if(docStatue.StepNumber != docStates.Count)
                    {
                        var step = docStates.Find(x => x.NumeroState == docStatue.StepNumber);
                        if(step != null)
                        {
                            if (docStatue.StateDocument == State.Validated)
                            {
                                doc.DateUpdate = DateTime.Now;
                                doc.CurrentState = State.In_Progress;
                                doc.CurrentNumberState++;
                                step.StateDocument = docStatue.StateDocument;
                                step.Comment = docStatue.Comment;
                                step.Date = docStatue.Date;
                                await _context.SaveChangesAsync();
                                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Document State In Progress ", null));
                            }
                            if (docStatue.StateDocument == State.Cancelled)
                            {
                                doc.DateUpdate = DateTime.Now;
                                doc.CurrentState = State.Cancelled;
                                doc.CurrentNumberState++;
                                step.StateDocument = docStatue.StateDocument;
                                step.Comment = docStatue.Comment;
                                step.Date = docStatue.Date;
                                await _context.SaveChangesAsync();
                                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Document has been rejected ", null));
                            }
                        }
                    } 
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