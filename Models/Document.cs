namespace SportAsso.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Document")]
    public partial class Document
    {
        [Key]
        public int Id_Doc { get; set; }

        [Required]
        [StringLength(150)]
        public string Type_Document { get; set; }

        public int Dossier_Id_Dossier { get; set; }

        [Required]
        public byte[] Fichier { get; set; }

        public byte? Est_Valide { get; set; }

        public virtual Dossier Dossier { get; set; }
    }
}
