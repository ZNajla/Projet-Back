using Application.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request
{
    public class AddUpdateDocState
    {
        public int StateDocument { get; set; }

        public string Comment { get; set; }

        public int StepNumber { get; set; }

        public string DocId { get; set; }
    }
}
