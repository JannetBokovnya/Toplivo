using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Toplivo.Web.Models.Toplivo
{
    public class Operation
    {
        //ID операции
        [Key]
        [Display(Name = "Код операции")]
        public int OperationID { get; set; }

        //ID топлива
        [Display(Name = "Код топлива")]
        [ForeignKey("Fuel")]
        public int FuelID { get; set; }

        //ID емкости
        [Display(Name = "Код емкости")]
        [ForeignKey("Tank")]
        public int TankID { get; set; }

        //Приход/Расход
        [Display(Name = "+Приход/-Расход")]
        public float? Inc_Exp { get; set; }

        //Дата операции
        [Display(Name = "Дата операции")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Date { get; set; }

        //ссылка по внешнему ключу FuelID на Fuel
        public virtual Fuel Fuel { get; set; }
        //ссылка по по внешнему ключу TankID на Tank
        public virtual Tank Tank { get; set; }
    }
}