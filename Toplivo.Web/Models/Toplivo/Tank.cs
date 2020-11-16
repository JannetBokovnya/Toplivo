using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Toplivo.Web.Models.Toplivo
{
    public enum SortState
    {
        TankTypeAsc,    // Наименование емкости по возрастанию
        TankTypeDesc,   // Наименование емкости по убыванию
        TankMaterialAsc, // Материал емкости по возрастанию
        TankMaterialDesc// Материал емкости по убыванию
       
    }
    public class Tank
    {
        //ID емкости
        [Key]
        [Display(Name = "Код емкости")]
        public int TankID { get; set; }

        //Наименование емкости
        [Display(Name = "Название емкости")]
        public string TankType { get; set; }

        //Вес емкости
        [Display(Name = "Вес")]
        public float TankWeight { get; set; }

        //Объем емкости
        [Display(Name = "Объем")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Пожалуйста, введите положительное значение для объема")]
        public float TankVolume { get; set; }

        //Материал емкости
        [Display(Name = "Материал")]
        public string TankMaterial { get; set; }

        //ссылка на файл изображения емкости
        [Display(Name = "Изображение")]
        public string TankPicture { get; set; }

        //Коллекция объектов Operation, связанных с моделью
        public virtual ICollection<Operation> Operations { get; set; }
    }
}