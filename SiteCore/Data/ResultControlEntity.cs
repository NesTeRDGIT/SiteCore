using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SiteCore.Data
{
    public class ResultControl
    {
        public List<ResultControlVZR> VZR { get; set; }
        public List<ResultControlDET> DET { get; set; }
    }
    public class ResultControlVZR
    {
        public static List<ResultControlVZR> GetList(IDataReader reader)
        {
            var result = new List<ResultControlVZR>();
            while (reader.Read())
            {
                result.Add(Get(reader));
            }
            return result;
        }
        public static ResultControlVZR Get(IDataReader reader)
        {
            try
            {
                var item = new ResultControlVZR();
                item.NN = Convert.ToString(reader[nameof(NN)]);
                item.DIAG = Convert.ToString(reader[nameof(DIAG)]);
                item.MKB = Convert.ToString(reader[nameof(MKB)]);
                item.ST4 = Convert.ToInt32(reader[nameof(ST4)]);
                item.ST5 = Convert.ToInt32(reader[nameof(ST5)]);
                item.ST6 = Convert.ToInt32(reader[nameof(ST6)]);
                item.ST7 = Convert.ToInt32(reader[nameof(ST7)]);
                item.ST8 = Convert.ToInt32(reader[nameof(ST8)]);
                item.ST9 = Convert.ToInt32(reader[nameof(ST9)]);
                item.ST10 = Convert.ToInt32(reader[nameof(ST10)]);
                item.ST11 = Convert.ToDecimal(reader[nameof(ST11)]);
                item.ST12 = Convert.ToDecimal(reader[nameof(ST12)]);
                item.ST13 = Convert.ToInt32(reader[nameof(ST13)]);
                item.ST14 = Convert.ToInt32(reader[nameof(ST14)]);
                item.ST15 = Convert.ToInt32(reader[nameof(ST15)]);
                item.ST16 = Convert.ToInt32(reader[nameof(ST16)]);
                item.ST17 = Convert.ToInt32(reader[nameof(ST17)]);
                item.ST18 = Convert.ToInt32(reader[nameof(ST18)]);
                item.ST19 = Convert.ToInt32(reader[nameof(ST19)]);
                item.ST20 = Convert.ToInt32(reader[nameof(ST20)]);
                item.ST21 = Convert.ToInt32(reader[nameof(ST21)]);
                item.ST22 = Convert.ToInt32(reader[nameof(ST22)]); 
                item.ST23 = Convert.ToInt32(reader[nameof(ST23)]);
                item.ST24 = Convert.ToInt32(reader[nameof(ST24)]); 
                item.ST25 = Convert.ToInt32(reader[nameof(ST25)]);
                item.ST26 = Convert.ToInt32(reader[nameof(ST26)]);
                item.ST27 = Convert.ToInt32(reader[nameof(ST27)]);
                item.ST28 = Convert.ToInt32(reader[nameof(ST28)]);
                item.ST29 = Convert.ToInt32(reader[nameof(ST29)]);
                item.ST30 = Convert.ToInt32(reader[nameof(ST30)]);
                item.ST31 = Convert.ToInt32(reader[nameof(ST31)]); 
                item.ST32 = Convert.ToInt32(reader[nameof(ST32)]);
                item.ST33 = Convert.ToInt32(reader[nameof(ST33)]);
                item.ST34 = Convert.ToInt32(reader[nameof(ST34)]);
                item.ST35 = Convert.ToInt32(reader[nameof(ST35)]);
                item.ST36 = Convert.ToInt32(reader[nameof(ST36)]);
                item.ST37 = Convert.ToInt32(reader[nameof(ST37)]);
                item.ST38 = Convert.ToInt32(reader[nameof(ST38)]);
                item.ST39 = Convert.ToInt32(reader[nameof(ST39)]);
                item.ST40 = Convert.ToInt32(reader[nameof(ST40)]);
                item.ST41 = Convert.ToInt32(reader[nameof(ST41)]);
                item.ST42 = Convert.ToInt32(reader[nameof(ST42)]);
                return item;
            }
            catch (Exception e)
            {
                throw new Exception($"Ошибка получения ResultControlVZR: {e.Message}", e);
            }
        }
        public string NN { get; set; }
        public string DIAG { get; set; }
        public string MKB { get; set; }
        public int ST4 { get; set; }
        public int ST5 { get; set; }
        public int ST6 { get; set; }
        public int ST7 { get; set; }
        public int ST8 { get; set; }
        public int ST9 { get; set; }
        public int ST10 { get; set; }
        public decimal ST11 { get; set; }
        public decimal ST12 { get; set; }
        public int ST13 { get; set; }
        public int ST14 { get; set; }
        public int ST15 { get; set; }
        public int ST16 { get; set; }
        public int ST17 { get; set; }
        public int ST18 { get; set; }
        public int ST19 { get; set; }
        public int ST20 { get; set; }
        public int ST21 { get; set; }
        public int ST22 { get; set; }
        public int ST23 { get; set; }
        public int ST24 { get; set; }
        public int ST25 { get; set; }
        public int ST26 { get; set; }
        public int ST27 { get; set; }
        public int ST28 { get; set; }
        public int ST29 { get; set; }
        public int ST30 { get; set; }
        public int ST31 { get; set; }
        public int ST32 { get; set; }
        public int ST33 { get; set; }
        public int ST34 { get; set; }
        public int ST35 { get; set; }
        public int ST36 { get; set; }
        public int ST37 { get; set; }
        public int ST38 { get; set; }
        public int ST39 { get; set; }
        public int ST40 { get; set; }
        public int ST41 { get; set; }
        public int ST42 { get; set; }

    }
    public class ResultControlDET
    {
        public static List<ResultControlDET> GetList(IDataReader reader)
        {
            var result = new List<ResultControlDET>();
            while (reader.Read())
            {
                result.Add(Get(reader));
            }
            return result;
        }
        public static ResultControlDET Get(IDataReader reader)
        {
            try
            {
                var item = new ResultControlDET();
                item.NN = Convert.ToString(reader[nameof(NN)]);
                item.DIAG = Convert.ToString(reader[nameof(DIAG)]);
                item.MKB = Convert.ToString(reader[nameof(MKB)]);
                item.ST4 = Convert.ToInt32(reader[nameof(ST4)]);
                item.ST5 = Convert.ToInt32(reader[nameof(ST5)]);
                item.ST6 = Convert.ToInt32(reader[nameof(ST6)]);
                item.ST7 = Convert.ToInt32(reader[nameof(ST7)]);
                item.ST8 = Convert.ToInt32(reader[nameof(ST8)]);
                item.ST9 = Convert.ToInt32(reader[nameof(ST9)]);
                item.ST10 = Convert.ToDecimal(reader[nameof(ST10)]);
                item.ST11 = Convert.ToDecimal(reader[nameof(ST11)]);
                item.ST12 = Convert.ToInt32(reader[nameof(ST12)]);
                item.ST13 = Convert.ToInt32(reader[nameof(ST13)]);
                item.ST14 = Convert.ToInt32(reader[nameof(ST14)]);
                item.ST15 = Convert.ToInt32(reader[nameof(ST15)]);
                item.ST16 = Convert.ToInt32(reader[nameof(ST16)]);
                item.ST17 = Convert.ToInt32(reader[nameof(ST17)]);
                item.ST18 = Convert.ToInt32(reader[nameof(ST18)]);
                item.ST19 = Convert.ToInt32(reader[nameof(ST19)]);
                item.ST20 = Convert.ToInt32(reader[nameof(ST20)]);
                item.ST21 = Convert.ToInt32(reader[nameof(ST21)]);
                item.ST22 = Convert.ToInt32(reader[nameof(ST22)]);
                item.ST23 = Convert.ToInt32(reader[nameof(ST23)]);
                item.ST24 = Convert.ToInt32(reader[nameof(ST24)]);
                item.ST25 = Convert.ToInt32(reader[nameof(ST25)]);
                item.ST26 = Convert.ToInt32(reader[nameof(ST26)]);
                item.ST27 = Convert.ToInt32(reader[nameof(ST27)]);
                item.ST28 = Convert.ToInt32(reader[nameof(ST28)]);
                item.ST29 = Convert.ToInt32(reader[nameof(ST29)]);
                item.ST30 = Convert.ToInt32(reader[nameof(ST30)]);
                item.ST31 = Convert.ToInt32(reader[nameof(ST31)]);
                item.ST32 = Convert.ToInt32(reader[nameof(ST32)]);
                item.ST33 = Convert.ToInt32(reader[nameof(ST33)]);
                item.ST34 = Convert.ToInt32(reader[nameof(ST34)]);
                item.ST35 = Convert.ToInt32(reader[nameof(ST35)]);
                item.ST36 = Convert.ToInt32(reader[nameof(ST36)]);
                item.ST37 = Convert.ToInt32(reader[nameof(ST37)]);
                item.ST38 = Convert.ToInt32(reader[nameof(ST38)]);
                item.ST39 = Convert.ToInt32(reader[nameof(ST39)]);
                return item;
            }
            catch (Exception e)
            {
                throw new Exception($"Ошибка получения ResultControlVZR: {e.Message}", e);
            }
        }
        public string NN { get; set; }
        public string DIAG { get; set; }
        public string MKB { get; set; }
        public int ST4 { get; set; }
        public int ST5 { get; set; }
        public int ST6 { get; set; }
        public int ST7 { get; set; }
        public int ST8 { get; set; }
        public int ST9 { get; set; }
        public decimal ST10 { get; set; }
        public decimal ST11 { get; set; }
        public int ST12 { get; set; }
        public int ST13 { get; set; }
        public int ST14 { get; set; }
        public int ST15 { get; set; }
        public int ST16 { get; set; }
        public int ST17 { get; set; }
        public int ST18 { get; set; }
        public int ST19 { get; set; }
        public int ST20 { get; set; }
        public int ST21 { get; set; }
        public int ST22 { get; set; }
        public int ST23 { get; set; }
        public int ST24 { get; set; }
        public int ST25 { get; set; }
        public int ST26 { get; set; }
        public int ST27 { get; set; }
        public int ST28 { get; set; }
        public int ST29 { get; set; }
        public int ST30 { get; set; }
        public int ST31 { get; set; }
        public int ST32 { get; set; }
        public int ST33 { get; set; }
        public int ST34 { get; set; }
        public int ST35 { get; set; }
        public int ST36 { get; set; }
        public int ST37 { get; set; }
        public int ST38 { get; set; }
        public int ST39 { get; set; }

    }
 
}
