﻿namespace Application.Models.Request
{
    public class AddUpdateDoc
    {
        public string Url { get; set; }

        public string Reference { get; set; }

        public string Titre { get; set; }

        public int NbPage { get; set; }

        public string MotCle { get; set; }

        public string Version { get; set; }

        public DateTime DateUpdate { get; set; }

        public string UserId { get; set; }

        public string TypesId { get; set; }
    }
}