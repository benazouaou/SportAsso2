namespace SportAsso.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Creneau")]
    public partial class Creneau
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Creneau()
        {
            Personne1 = new HashSet<Personne>();
        }

        [Key]
        public int Id_Creneau { get; set; }

        public int Nombre_Places_Dispo { get; set; }

        public int Nombre_Places_Max { get; set; }

        public int Section_Id_Section { get; set; }

        public int Lieu_Id_Lieu { get; set; }

        public int Encadrant { get; set; }

        [Required]
        [StringLength(50)]
        public string Jour { get; set; }

        public TimeSpan Heure { get; set; }

        public virtual Lieu Lieu { get; set; }

        public virtual Personne Personne { get; set; }

        public virtual Section Section { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Personne> Personne1 { get; set; }
    }
}
