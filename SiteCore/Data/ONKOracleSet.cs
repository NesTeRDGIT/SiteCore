using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SiteCore.Data
{
    public class ONKOracleSet : DbContext
    {
        private DbMedpomManager dbMedpomManager;
        public ONKOracleSet(DbContextOptions<ONKOracleSet> options) : base(options)
        {
            this.dbMedpomManager = new DbMedpomManager(this.Database.GetConnectionString());

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ONK_R");
            modelBuilder.Entity<ONKReestr>().HasOne(x => x.N_DS1).WithMany().HasForeignKey(x => x.DS1);
            modelBuilder.Entity<ONKReestr>().HasOne(x => x.N_DS_ONK_MO).WithMany().HasForeignKey(x => x.DS_ONK_MO);
            modelBuilder.Entity<ONKReestr>().HasOne(x => x.N_DS_ONK_MO_ONK).WithMany().HasForeignKey(x => x.DS_ONK_MO_ONK);
            modelBuilder.Entity<ONKReestr>().HasOne(x => x.N_MO_BIO).WithMany().HasForeignKey(x => x.MO_BIO);
            modelBuilder.Entity<ONKReestr>().HasOne(x => x.N_MO_KT).WithMany().HasForeignKey(x => x.MO_KT);
            modelBuilder.Entity<ONKReestr>().HasMany(x => x.SL).WithOne(x=>x.ONKReestr).HasForeignKey(x => x.ONK_REESTR_ID);
            modelBuilder.Entity<ONK_REESTR_SL>().HasOne(x => x.V_SL_MINI).WithMany().HasForeignKey(x => x.SLUCH_ID);
        }

        public virtual DbSet<ONKReestr> ONKReestr { get; set; }

        public Z_SL FindZ_SL(int SLUCH_Z_ID)
        {
            return dbMedpomManager.FindZ_SL(SLUCH_Z_ID);
        }
    }

    [Table("ONK_REESTR")]
    public class ONKReestr
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ONK_REESTR_ID { get; set; }
        public string ENP { get; set; }
        public string FAM { get; set; }
        public string IM { get; set; }
        public string OT { get; set; }
        public DateTime? DR { get; set; }
        public DateTime? DDEATH { get; set; }
        public string SMO { get; set; }
        public DateTime? DATE_DS_ONK { get; set; }
        public bool? DS_ONK_DISP { get; set; }
        public string DS1 { get; set; }
        public virtual MKB_SPR N_DS1 { get; set; }
        [NotMapped]
        public string DS1_FULLNAME => string.IsNullOrEmpty(DS1) ? "" : $"{N_DS1?.NAME}({DS1})";
        public DateTime? DS1_DATE { get; set; }
        public DateTime? DATE_DN { get; set; }
        public string DS_ONK_MO { get; set; }
        public virtual F003 N_DS_ONK_MO { get; set; }
        [NotMapped]
        public string DS_ONK_MO_FULLNAME => string.IsNullOrEmpty(DS_ONK_MO) ? "" : $"{N_DS_ONK_MO?.NAM_MOK}({DS_ONK_MO})";
        public DateTime? DATE_DS_ONK_ONK { get; set; }
        public string DS_ONK_MO_ONK { get; set; }
        public virtual F003 N_DS_ONK_MO_ONK { get; set; }
        [NotMapped]
        public string DS_ONK_MO_ONK_FULLNAME => string.IsNullOrEmpty(DS_ONK_MO_ONK) ? "" : $"{N_DS_ONK_MO_ONK?.NAM_MOK}({DS_ONK_MO_ONK})";
        public DateTime? DATE_BIO { get; set; }
        public string MO_BIO { get; set; }
        public virtual F003 N_MO_BIO { get; set; }
        [NotMapped]
        public string MO_BIO_FULLNAME => string.IsNullOrEmpty(MO_BIO) ? "" : $"{N_MO_BIO?.NAM_MOK}({MO_BIO})";
        public DateTime? DATE_KT { get; set; }
        public string MO_KT { get; set; }
        public virtual F003 N_MO_KT { get; set; }
        [NotMapped]
        public string MO_KT_FULLNAME => string.IsNullOrEmpty(MO_KT) ? "" : $"{N_MO_KT?.NAM_MOK}({MO_KT})";
        public DateTime? DATE_HIM { get; set; }
         public int YEAR { get; set; }
        public int MONTH { get; set; }
        public virtual ICollection<ONK_REESTR_SL> SL { get; set; } = new List<ONK_REESTR_SL>();

    }

    [Table("ONK_REESTR_SL")]
    public class ONK_REESTR_SL
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
        public int ONK_REESTR_ID { get; set; }
        public int? SLUCH_ID { get; set; }
        public int? SLUCH_ID_MTR { get; set; }
        public int YEAR { get; set; }
        public int MONTH { get; set; }
        public int? SLUCH_Z_ID { get; set; }
        public virtual ONKReestr ONKReestr { get; set; }
        public virtual V_SLUCH_MINI V_SL_MINI { get; set; }
    }

    [Table("V_SLUCH_MINI")]
    public class V_SLUCH_MINI
    {
        [Key]
        public int SLUCH_ID { get; set; }
        public int SLUCH_Z_ID { get; set; }
        public int USL_OK { get; set; }
        public string N_USL_OK { get; set; }
        public DateTime? DATE_1 { get; set; }
        public DateTime? DATE_2 { get; set; }
        public string LPU { get; set; }
        public string LPU_NAME { get; set; }
        public int? RSLT { get; set; }
        public string N_RSLT { get; set; }
        public string SMO { get; set; }
        public string N_SMO { get; set; }
        public bool? DS_ONK { get; set; }
        public string DS1 { get; set; }
        public string N_DS1 { get; set; }
        public  string NSCHET { get; set; }
        public DateTime DSCHET { get; set; }

    }

    [Table("NSI.F003")]
    public class F003
    {
        [Key]
        public string MCOD { get; set; }
        public string NAM_MOK { get; set; }
    }
  
}
