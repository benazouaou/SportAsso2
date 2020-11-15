namespace SportAsso.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Personne")]
    public partial class Personne
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Personne()
        {
            Creneau = new HashSet<Creneau>();
            Dossier = new HashSet<Dossier>();
            Creneau1 = new HashSet<Creneau>();
            Role = new HashSet<Role>();
        }

        [Key]
        public int Id_Personne { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "La chaîne {0} doit comporter au moins {2} caractères.", MinimumLength = 2)]
        [Display(Name = "Nom")]
        public string Nom { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "La chaîne {0} doit comporter au moins {2} caractères.", MinimumLength = 2)]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }


        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [Display(Name = "Date de naissance")]
        public DateTime Date_Naissance { get; set; }

        [Required]
        [StringLength(70)]
        [Display(Name = "Courrier électronique")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")]
        public string E_mail { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Téléphone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"(0|(\\+33)|(0033))[1-9][0-9]{8}", ErrorMessage = "Le numéro de téléphone n'est pas valide.")]
        public string Num_Telephone { get; set; }


        [StringLength(10)]
        [Display(Name = "Sexe")]
        [RegularExpression(@"female|male")]
        public string Sexe { get; set; }



        [Required]
        [StringLength(50, ErrorMessage = "La chaîne {0} doit comporter au moins {2} caractères.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Mot_de_Passe { get; set; }


        [StringLength(50)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le mot de passe ")]
        [Compare("Mot_de_Passe", ErrorMessage = "Le mot de passe et le mot de passe de confirmation ne correspondent pas.")]
        public string Confirm_Mot_Passe { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Creneau> Creneau { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dossier> Dossier { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Creneau> Creneau1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Role> Role { get; set; }
    }
}
