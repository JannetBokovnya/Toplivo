using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Toplivo.Web.PL
{
    public struct TransferData
    {
        public int TankPage; // номер текущей страницы для работы с табличными данными в Tanks
        public string strTankTypeFind; // строка поиска по названиям в Tanks
        public int FuelPage;
        public string strFuelTypeFind;
        public int OperationPage; // номер текущей страницы для работы с табличными данными в Operations


        public TransferData(int tankpage = 1,  string strtanktypefind = "", int fuelpage = 1, string strfueltypefind = "", int operationkpage = 1)
        {
            TankPage = tankpage;
            strTankTypeFind = strtanktypefind;
            FuelPage = fuelpage;
            strFuelTypeFind = strfueltypefind;
            OperationPage = operationkpage;
        }
    }
}