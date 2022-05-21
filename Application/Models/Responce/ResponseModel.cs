using Application.Models.Enums;

namespace Application.Models.Responce
{
    public class ResponseModel
    {
        public ResponseModel(ResponseCode responseCode, string responseMessage, object dataset)
        {
            ResponseCode = responseCode;
            ResponseMessage = responseMessage;
            DateSet = dataset;
        }

        public ResponseCode ResponseCode { get; set; }

        public string ResponseMessage { get; set; }

        public object DateSet { get; set; }
    }
}
