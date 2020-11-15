namespace SportAsso.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Dossier")]
    public partial class Dossier
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Dossier()
        {
            Document = new HashSet<Document>();
        }

        [Key]
        public int Id_Dossier { get; set; }

        public byte Paiement { get; set; }

        public int Personne_Id_Personne { get; set; }

        public int Section_Id_Section { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Document> Document { get; set; }

        public virtual Personne Personne { get; set; }

        public virtual Section Section { get; set; }
    }
}
