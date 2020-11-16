//Класс для инициализации базы данных путем заполнения ее таблиц тестовым набором записей
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Toplivo.Web.Common;
using Toplivo.Web.Models.Toplivo;

namespace Toplivo.Web.DAL
{
    public class ToplivoDbInitializer: CreateDatabaseIfNotExists<ToplivoContext>
    {
        //Seed Метод принимает объект контекста базы данных как входной параметр и код в метод использует 
        //этот объект для добавления новых сущностей в базу данных
        protected override void Seed(ToplivoContext db)
        {
            int tanks_number = 35;
            int fuels_number = 35;
            int operations_number = 300;
            string tankType;
            string tankMaterial;
            float tankWeight;
            float tankVolume;
            string fuelType;
            float fuelDensity;

            Random randObj = new Random(1);

            //Заполнение таблицы емкостей
            string[] tank_voc = { "Цистерна_", "Ведро_", "Бак_", "Фляга_", "Цистерна_" };//словарь названий емкостей
            string[] material_voc = { "Сталь", "Платина", "Алюминий", "ПЭТ", "Чугун", "Алюминий", "Сталь" };//словарь названий видов топлива
            int count_tank_voc = tank_voc.GetLength(0);
            int count_material_voc = material_voc.GetLength(0);
            for (int tankID = 1; tankID <= tanks_number; tankID++)
            {
                tankType = tank_voc[randObj.Next(count_tank_voc)] + tankID.ToString();
                tankMaterial = material_voc[randObj.Next(count_material_voc)];
                tankWeight = 500 * (float)randObj.NextDouble();
                tankVolume = 200 * (float)randObj.NextDouble();
                db.Tanks.Add(new Tank { TankID = tankID, TankType = tankType, TankWeight = tankWeight, TankVolume = tankVolume, TankMaterial = tankMaterial });
            }
            db.SaveChanges();
            //Заполнение таблицы видов топлива
            string[] fuel_voc = { "Нефть_", "Бензин_", "Керосин_", "Мазут_", "Спирт_" };
            int count_fuel_voc = fuel_voc.GetLength(0);
            for (int fuelID = 1; fuelID <= fuels_number; fuelID++)
            {
                fuelType = fuel_voc[randObj.Next(count_fuel_voc)] + fuelID.ToString();
                fuelDensity = 2 * (float)randObj.NextDouble();
                db.Fuels.Add(new Fuel { FuelID = fuelID, FuelType = fuelType, FuelDensity = fuelDensity });
            }
            db.SaveChanges();
            //Заполнение таблицы операций
            for (int operationID = 1; operationID <= operations_number; operationID++)
            {
                int tankID = randObj.Next(1, tanks_number - 1);
                int fuelID = randObj.Next(1, fuels_number - 1);
                int inc_exp = randObj.Next(200) - 100;
                DateTime today = DateTime.Now.Date;
                DateTime operationdate = today.AddDays(-operationID);
                db.Operations.Add(new Operation { OperationID = operationID, TankID = tankID, FuelID = fuelID, Inc_Exp = inc_exp, Date = operationdate });
            }
            //сохранение изменений в базу данных, связанную с объектом контекста
            db.SaveChanges();
        }
    }
}