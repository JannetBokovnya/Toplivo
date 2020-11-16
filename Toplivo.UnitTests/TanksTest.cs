using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using System.Text;
using System.Threading.Tasks;
using Toplivo.Web.Services;
using Toplivo.Web.Models.Toplivo;

namespace Toplivo.UnitTests
{
    [TestClass]
    public class TanksTest
    {
        [TestMethod]
        public void Index_Contains_All_Tanks()
        {
            // Организация (arrange)
            //имитация хранилища
            Mock<IToplivoService<Tank>> mock = new Mock<IToplivoService<Tank>>(); 
            
        }
    }
}
