﻿using NpgsqlTypes;
using System.ComponentModel.DataAnnotations.Schema;

namespace FountainPensNg.Server.Data.Models {
    public class FountainPen {
        public int Id { get; set; }
        public string Maker { get; set; } = "";
        public string ModelName { get; set; } = "";
        public string Comment { get; set; } = "";
        public string Photo { get; set; } = "";
        public string Color { get; set; } = "";
        public int Rating { get; set; }
        public string Nib { get; set; } = "";
        public virtual List<InkedUp> InkedUps { get; set; } = new List<InkedUp>();
        public string ImageObjectKey { get; set; } = "";
        public NpgsqlTsVector? FullText { get; set; } = null;
        public DateTime InsertedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; }
    }
}
