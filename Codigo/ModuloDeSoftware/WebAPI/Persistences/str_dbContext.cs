using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Persistence
{
    public partial class str_dbContext : DbContext
    {
        public str_dbContext()
        {
        }

        public str_dbContext(DbContextOptions<str_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bloco> Bloco { get; set; }
        public virtual DbSet<Codigoinfravermelho> Codigoinfravermelho { get; set; }
        public virtual DbSet<Equipamento> Equipamento { get; set; }
        public virtual DbSet<Hardwaredesala> Hardwaredesala { get; set; }
        public virtual DbSet<Horariosala> Horariosala { get; set; }
        public virtual DbSet<Monitoramento> Monitoramento { get; set; }
        public virtual DbSet<Operacao> Operacao { get; set; }
        public virtual DbSet<Organizacao> Organizacao { get; set; }
        public virtual DbSet<Planejamento> Planejamento { get; set; }
        public virtual DbSet<Sala> Sala { get; set; }
        public virtual DbSet<Salaparticular> Salaparticular { get; set; }
        public virtual DbSet<Tipohardware> Tipohardware { get; set; }
        public virtual DbSet<Tipousuario> Tipousuario { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Usuarioorganizacao> Usuarioorganizacao { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           /* if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=1234;database=str_db");
            }*/
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bloco>(entity =>
            {
                entity.ToTable("bloco", "str_db");

                entity.HasIndex(e => e.Organizacao)
                    .HasName("fk_Bloco_Organizacao1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Organizacao)
                    .HasColumnName("organizacao")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasColumnName("titulo")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.OrganizacaoNavigation)
                    .WithMany(p => p.Bloco)
                    .HasForeignKey(d => d.Organizacao)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Bloco_Organizacao1");
            });

            modelBuilder.Entity<Codigoinfravermelho>(entity =>
            {
                entity.ToTable("codigoinfravermelho", "str_db");

                entity.HasIndex(e => e.Equipamento)
                    .HasName("fk_CodigoInfravermelho_Equipamento1_idx");

                entity.HasIndex(e => e.Operacao)
                    .HasName("fk_CodigoInfravermelho_Operacao1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasColumnName("codigo")
                    .HasColumnType("mediumtext");

                entity.Property(e => e.Equipamento)
                    .HasColumnName("equipamento")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Operacao)
                    .HasColumnName("operacao")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.EquipamentoNavigation)
                    .WithMany(p => p.Codigoinfravermelho)
                    .HasForeignKey(d => d.Equipamento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_CodigoInfravermelho_Equipamento1");

                entity.HasOne(d => d.OperacaoNavigation)
                    .WithMany(p => p.Codigoinfravermelho)
                    .HasForeignKey(d => d.Operacao)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_CodigoInfravermelho_Operacao1");
            });

            modelBuilder.Entity<Equipamento>(entity =>
            {
                entity.ToTable("equipamento", "str_db");

                entity.HasIndex(e => e.HardwareDeSala)
                    .HasName("fk_Equipamento_HardwareDeSala_idx");

                entity.HasIndex(e => e.Sala)
                    .HasName("fk_Equipamento_Sala1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Descricao)
                    .HasColumnName("descricao")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.HardwareDeSala)
                    .HasColumnName("hardwareDeSala")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Marca)
                    .IsRequired()
                    .HasColumnName("marca")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Modelo)
                    .IsRequired()
                    .HasColumnName("modelo")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Sala)
                    .HasColumnName("sala")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.TipoEquipamento)
                    .IsRequired()
                    .HasColumnName("tipoEquipamento")
                    .HasColumnType("enum('CONDICIONADOR','LUZES')")
                    .HasDefaultValueSql("_utf8mb4\\'CONDICIONADOR\\'");

                entity.HasOne(d => d.HardwareDeSalaNavigation)
                    .WithMany(p => p.Equipamento)
                    .HasForeignKey(d => d.HardwareDeSala)
                    .HasConstraintName("fk_Equipamento_HardwareDeSala");

                entity.HasOne(d => d.SalaNavigation)
                    .WithMany(p => p.Equipamento)
                    .HasForeignKey(d => d.Sala)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Equipamento_Sala1");
            });

            modelBuilder.Entity<Hardwaredesala>(entity =>
            {
                entity.ToTable("hardwaredesala", "str_db");

                entity.HasIndex(e => e.Sala)
                    .HasName("fk_Hardware_Sala1_idx");

                entity.HasIndex(e => e.TipoHardware)
                    .HasName("fk_HardwareDeSala_TipoHardware1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Ip)
                    .HasColumnName("ip")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Mac)
                    .IsRequired()
                    .HasColumnName("mac")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Sala).HasColumnType("int(10) unsigned");

                entity.Property(e => e.TipoHardware)
                    .HasColumnName("tipoHardware")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Uuid)
                    .HasColumnName("uuid")
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.HasOne(d => d.SalaNavigation)
                    .WithMany(p => p.Hardwaredesala)
                    .HasForeignKey(d => d.Sala)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Hardware_Sala1");

                entity.HasOne(d => d.TipoHardwareNavigation)
                    .WithMany(p => p.Hardwaredesala)
                    .HasForeignKey(d => d.TipoHardware)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_HardwareDeSala_TipoHardware1");
            });

            modelBuilder.Entity<Horariosala>(entity =>
            {
                entity.ToTable("horariosala", "str_db");

                entity.HasIndex(e => e.Planejamento)
                    .HasName("fk_HorarioSala_Planejamento1_idx");

                entity.HasIndex(e => e.Sala)
                    .HasName("fk_HorarioSala_Sala1_idx");

                entity.HasIndex(e => e.Usuario)
                    .HasName("fk_HorarioSala_Usuario1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Data).HasColumnName("data");

                entity.Property(e => e.HorarioFim).HasColumnName("horarioFim");

                entity.Property(e => e.HorarioInicio).HasColumnName("horarioInicio");

                entity.Property(e => e.Objetivo)
                    .IsRequired()
                    .HasColumnName("objetivo")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Planejamento)
                    .HasColumnName("planejamento")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Sala)
                    .HasColumnName("sala")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Situacao)
                    .IsRequired()
                    .HasColumnName("situacao")
                    .HasColumnType("enum('PENDENTE','APROVADA','REPROVADA','CANCELADA')")
                    .HasDefaultValueSql("APROVADA");

                entity.Property(e => e.Usuario)
                    .HasColumnName("usuario")
                    .HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.PlanejamentoNavigation)
                    .WithMany(p => p.Horariosala)
                    .HasForeignKey(d => d.Planejamento)
                    .HasConstraintName("fk_HorarioSala_Planejamento1");

                entity.HasOne(d => d.SalaNavigation)
                    .WithMany(p => p.Horariosala)
                    .HasForeignKey(d => d.Sala)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_HorarioSala_Sala1");

                entity.HasOne(d => d.UsuarioNavigation)
                    .WithMany(p => p.Horariosala)
                    .HasForeignKey(d => d.Usuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_HorarioSala_Usuario1");
            });

            modelBuilder.Entity<Monitoramento>(entity =>
            {
                entity.ToTable("monitoramento", "str_db");

                entity.HasIndex(e => e.Equipamento)
                    .HasName("fk_Equipamento_Id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Equipamento)
                    .HasColumnName("equipamento")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Estado)
                    .HasColumnName("estado")
                    .HasColumnType("tinyint(4)");

                entity.HasOne(d => d.EquipamentoNavigation)
                    .WithMany(p => p.Monitoramento)
                    .HasForeignKey(d => d.Equipamento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Equipamento_Id");
            });

            modelBuilder.Entity<Operacao>(entity =>
            {
                entity.ToTable("operacao", "str_db");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Organizacao>(entity =>
            {
                entity.ToTable("organizacao", "str_db");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Cnpj)
                    .IsRequired()
                    .HasColumnName("cnpj")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.RazaoSocial)
                    .IsRequired()
                    .HasColumnName("razaoSocial")
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Planejamento>(entity =>
            {
                entity.ToTable("planejamento", "str_db");

                entity.HasIndex(e => e.Sala)
                    .HasName("fk_Planejamento_Sala1_idx");

                entity.HasIndex(e => e.Usuario)
                    .HasName("fk_Planejamento_Usuario1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.DataFim)
                    .HasColumnName("dataFim")
                    .HasColumnType("date");

                entity.Property(e => e.DataInicio)
                    .HasColumnName("dataInicio")
                    .HasColumnType("date");

                entity.Property(e => e.DiaSemana)
                    .IsRequired()
                    .HasColumnName("diaSemana")
                    .HasColumnType("enum('SEG','TER','QUA','QUI','SEX','SAB','DOM')");

                entity.Property(e => e.HorarioFim).HasColumnName("horarioFim");

                entity.Property(e => e.HorarioInicio).HasColumnName("horarioInicio");

                entity.Property(e => e.Objetivo)
                    .IsRequired()
                    .HasColumnName("objetivo")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Sala)
                    .HasColumnName("sala")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Usuario)
                    .HasColumnName("usuario")
                    .HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.SalaNavigation)
                    .WithMany(p => p.Planejamento)
                    .HasForeignKey(d => d.Sala)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Planejamento_Sala1");

                entity.HasOne(d => d.UsuarioNavigation)
                    .WithMany(p => p.Planejamento)
                    .HasForeignKey(d => d.Usuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Planejamento_Usuario1");
            });

            modelBuilder.Entity<Sala>(entity =>
            {
                entity.ToTable("sala", "str_db");

                entity.HasIndex(e => e.Bloco)
                    .HasName("fk_Sala_Bloco1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Bloco)
                    .HasColumnName("bloco")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasColumnName("titulo")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.BlocoNavigation)
                    .WithMany(p => p.Sala)
                    .HasForeignKey(d => d.Bloco)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Sala_Bloco1");
            });

            modelBuilder.Entity<Salaparticular>(entity =>
            {
                entity.ToTable("salaparticular", "str_db");

                entity.HasIndex(e => e.Sala)
                    .HasName("fk_MinhaSala_Sala1_idx");

                entity.HasIndex(e => e.Usuario)
                    .HasName("fk_MinhaSala_Usuario1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Sala)
                    .HasColumnName("sala")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Usuario)
                    .HasColumnName("usuario")
                    .HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.SalaNavigation)
                    .WithMany(p => p.Salaparticular)
                    .HasForeignKey(d => d.Sala)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_MinhaSala_Sala1");

                entity.HasOne(d => d.UsuarioNavigation)
                    .WithMany(p => p.Salaparticular)
                    .HasForeignKey(d => d.Usuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_MinhaSala_Usuario1");
            });

            modelBuilder.Entity<Tipohardware>(entity =>
            {
                entity.ToTable("tipohardware", "str_db");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnName("descricao")
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tipousuario>(entity =>
            {
                entity.ToTable("tipousuario", "str_db");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnName("descricao")
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuario", "str_db");

                entity.HasIndex(e => e.Cpf)
                    .HasName("Cpf_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.TipoUsuario)
                    .HasName("fk_Usuario_TipoUsuario1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Cpf)
                    .IsRequired()
                    .HasColumnName("cpf")
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.DataNascimento)
                    .HasColumnName("dataNascimento")
                    .HasColumnType("date");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Senha)
                    .IsRequired()
                    .HasColumnName("senha")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoUsuario)
                    .HasColumnName("tipoUsuario")
                    .HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.TipoUsuarioNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.TipoUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Usuario_TipoUsuario1");
            });

            modelBuilder.Entity<Usuarioorganizacao>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Organizacao, e.Usuario });

                entity.ToTable("usuarioorganizacao", "str_db");

                entity.HasIndex(e => e.Organizacao)
                    .HasName("fk_Organizacao_has_Usuario_Organizacao1_idx");

                entity.HasIndex(e => e.Usuario)
                    .HasName("fk_Organizacao_has_Usuario_Usuario1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Organizacao)
                    .HasColumnName("organizacao")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Usuario)
                    .HasColumnName("usuario")
                    .HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.OrganizacaoNavigation)
                    .WithMany(p => p.Usuarioorganizacao)
                    .HasForeignKey(d => d.Organizacao)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Organizacao_has_Usuario_Organizacao1");

                entity.HasOne(d => d.UsuarioNavigation)
                    .WithMany(p => p.Usuarioorganizacao)
                    .HasForeignKey(d => d.Usuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Organizacao_has_Usuario_Usuario1");
            });
        }
    }
}
