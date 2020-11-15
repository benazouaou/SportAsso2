namespace SportAsso.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Section")]
    public partial class Section
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Section()
        {
            Creneau = new HashSet<Creneau>();
            Dossier = new HashSet<Dossier>();
        }

        [Key]
        public int Id_Section { get; set; }

        [Required]
        [StringLength(50)]
        public string Nom { get; set; }

        public decimal Prix { get; set; }

        public int Discipline_Id_Discipline { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Creneau> Creneau { get; set; }

        public virtual Discipline Discipline { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dossier> Dossier { get; set; }
    }
}
