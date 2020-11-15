namespace SportAsso.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Context_db : DbContext
    {
        public Context_db()
            : base("name=SportAsso")
        {
        }

        public virtual DbSet<Creneau> Creneau { get; set; }
        public virtual DbSet<Discipline> Discipline { get; set; }
        public virtual DbSet<Document> Document { get; set; }
        public virtual DbSet<Dossier> Dossier { get; set; }
        public virtual DbSet<Lieu> Lieu { get; set; }
        public virtual DbSet<Personne> Personne { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Section> Section { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Creneau>()
                .Property(e => e.Jour)
                .IsUnicode(false);

            modelBuilder.Entity<Creneau>()
                .HasMany(e => e.Personne1)
                .WithMany(e => e.Creneau1)
                .Map(m => m.ToTable("Inscrits"));

            modelBuilder.Entity<Discipline>()
                .Property(e => e.Nom_Discipline)
                .IsUnicode(false);

            modelBuilder.Entity<Discipline>()
                .HasMany(e => e.Section)
                .WithRequired(e => e.Discipline)
                .HasForeignKey(e => e.Discipline_Id_Discipline)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Document>()
                .Property(e => e.Type_Document)
                .IsUnicode(false);

            modelBuilder.Entity<Dossier>()
                .HasMany(e => e.Document)
                .WithRequired(e => e.Dossier)
                .HasForeignKey(e => e.Dossier_Id_Dossier)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Lieu>()
                .Property(e => e.Adresse)
                .IsUnicode(false);

            modelBuilder.Entity<Lieu>()
                .Property(e => e.Nom)
                .IsUnicode(false);

            modelBuilder.Entity<Lieu>()
                .HasMany(e => e.Creneau)
                .WithRequired(e => e.Lieu)
                .HasForeignKey(e => e.Lieu_Id_Lieu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Personne>()
                .Property(e => e.Nom)
                .IsUnicode(false);

            modelBuilder.Entity<Personne>()
                .Property(e => e.Prenom)
                .IsUnicode(false);

            modelBuilder.Entity<Personne>()
                .Property(e => e.E_mail)
                .IsUnicode(false);

            modelBuilder.Entity<Personne>()
                .Property(e => e.Num_Telephone)
                .IsUnicode(false);

            modelBuilder.Entity<Personne>()
                .Property(e => e.Sexe)
                .IsUnicode(false);

            modelBuilder.Entity<Personne>()
                .Property(e => e.Mot_de_Passe)
                .IsUnicode(false);

            modelBuilder.Entity<Personne>()
                .Property(e => e.Confirm_Mot_Passe)
                .IsUnicode(false);

            modelBuilder.Entity<Personne>()
                .HasMany(e => e.Creneau)
                .WithRequired(e => e.Personne)
                .HasForeignKey(e => e.Encadrant)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Personne>()
                .HasMany(e => e.Dossier)
                .WithRequired(e => e.Personne)
                .HasForeignKey(e => e.Personne_Id_Personne)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Personne>()
                .HasMany(e => e.Role)
                .WithMany(e => e.Personne)
                .Map(m => m.ToTable("Personne_has_Role"));

            modelBuilder.Entity<Role>()
                .Property(e => e.Nom_Role)
                .IsUnicode(false);

            modelBuilder.Entity<Section>()
                .Property(e => e.Nom)
                .IsUnicode(false);

            modelBuilder.Entity<Section>()
                .Property(e => e.Prix)
                .HasPrecision(20, 0);

            modelBuilder.Entity<Section>()
                .HasMany(e => e.Creneau)
                .WithRequired(e => e.Section)
                .HasForeignKey(e => e.Section_Id_Section)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Section>()
                .HasMany(e => e.Dossier)
                .WithRequired(e => e.Section)
                .HasForeignKey(e => e.Section_Id_Section)
                .WillCascadeOnDelete(false);
        }
    }
}
