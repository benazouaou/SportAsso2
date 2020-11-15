namespace SportAsso.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Lieu")]
    public partial class Lieu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Lieu()
        {
            Creneau = new HashSet<Creneau>();
        }

        [Key]
        public int Id_Lieu { get; set; }

        [Required]
        [StringLength(150)]
        public string Adresse { get; set; }

        [Required]
        [StringLength(50)]
        public string Nom { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Creneau> Creneau { get; set; }
    }
}
