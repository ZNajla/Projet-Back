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
    public class DocumentStateController : ControllerBase
    {

        private readonly AuthDbContext _context;

        public DocumentStateController(AuthDbContext context)
        {
            _context = context;
        }


        // GET api/<DocumentStateController>/5
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

        
        // POST api/<DocumentStateController>
        [HttpPost("AddDocStatue/{idDoc}")]
        public async Task<object> AddDocStatue(string idDoc)
        {
            try
            {
                var doc = _context.Documents.FirstOrDefault(u => u.ID.ToString().Equals(idDoc));
                List<Detail_Processus> detProc = _context.Detail_Processus.Where(b => b.ProcessusId.ToString().Equals(doc.Types.Processus.Id)).ToList();
                if(detProc.Count != 0)
                {
                    foreach (Detail_Processus detail in detProc)
                    {
                        var documentState = new DocumentState()
                        {
                            StateDocument = State.Awaiting,
                            NumeroState = detail.Step,
                            Date = DateTime.Now,
                            Comment = "",
                            Document = doc,
                            User = detail.User,
                        };
                        var result = _context.DocumentState.Add(documentState);
                        var entries = await _context.SaveChangesAsync();
                    }
                    //SEND NOTIFICATION TO THE USER OF FIRST STEP TO VALIDAT OR CHECK THE DOCUMENT
                    //ADD CODE HERE
                    //
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Document State has been added successfully", null));
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
                List<DocumentState> docStates = _context.DocumentState.Where(b => b.DocumentId.ToString().Equals(idDoc)).ToList();
                if(docStates.Count != 0)
                {
                    if(docStatue.StepNumber == docStates.Count)
                    {
                        if(docStatue.StateDocument == State.Validated)
                        {
                            //UPDATE CURRENTSTATE DOCUMENT AS ACCEPTED
                        }
                        if(docStatue.StateDocument == State.Cancelled)
                        {
                            //UPDATE CURRENTSTATE DOCUMENT AS REJECTED
                        }
                        //UPDATE ATTRIBUTS LAST STEP DOCUMENT STATE 
                        //RETURN 
                    }
                    if(docStatue.StepNumber != docStates.Count)
                    {
                        if (docStatue.StateDocument == State.Validated)
                        {
                            //UPDATE CURRENTSTATE DOCUMENT AS ACCEPTED
                            //UPDATE ATTRIBUT THAT STEP IN DOCUMENT STATE
                            //SEND NOTIFICATION TO NEXT USER TO VALIDAT OR CHECK
                        }
                        if (docStatue.StateDocument == State.Cancelled)
                        {
                            //UPDATE ATTRIBUT THAT STEP IN DOCUMENT STATE
                            //UPDATE CURRENTSTATE DOCUMENT AS REJECTED
                        }
                        //RETURN
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
