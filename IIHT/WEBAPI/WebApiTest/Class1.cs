using ApiSample.Controllers;
using DataAccess;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace WebApiTest
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void Add()
        {
            PurchaseOrderController controller = new PurchaseOrderController();
            PODETAIL podetail = new PODETAIL() { PONO="PO01", ITCODE = "IT01", QTY = 5 };
            controller.PostPOMASTER(new POMASTER() { PODATE = DateTime.Now, PONO ="PO01", SUPLNO = "SU01" , PODETAILs = new List<PODETAIL>() { podetail } });
            Assert.IsTrue(podetail.PONO == "PO01");
        }

        [Test]
        public void Update()
        {
            PurchaseOrderController controller = new PurchaseOrderController();
            PODETAIL podetail = new PODETAIL() { PONO = "PO01", ITCODE = "IT01", QTY = 10 };
            controller.PutPOMASTER("PO01",new POMASTER() { PODATE = DateTime.Now, PONO = "PO01", SUPLNO = "SU01", PODETAILs = new List<PODETAIL>() { podetail } });
            Assert.IsTrue(podetail.QTY == 10);
        }

        [Test]
        public void Get()
        {
            PurchaseOrderController controller = new PurchaseOrderController();
            IHttpActionResult poMasters = controller.GetPOMASTER("PO01");

            var content= poMasters as OkNegotiatedContentResult<POMASTER>;
             
            Assert.IsTrue(content.Content.PONO == "PO01");
            Assert.IsTrue(content.Content.PODETAILs.ToList()[0].QTY == 10);
        }

        [Test]
        public void GetAll()
        {
            PurchaseOrderController controller = new PurchaseOrderController();
            var poMasters = controller.GetPOMASTERs().ToList();
            Assert.IsTrue(poMasters[0].PONO == "PO01");
            Assert.IsTrue(poMasters[0].PODETAILs.ToList()[0].QTY == 10);
        }


        [Test]
        public void Delete()
        {
            PurchaseOrderController controller = new PurchaseOrderController();

            PODETAIL podetail = new PODETAIL() { PONO = "PO02", ITCODE = "IT01", QTY = 5 };
            controller.PostPOMASTER(new POMASTER() { PODATE = DateTime.Now, PONO = "PO02", SUPLNO = "SU01", PODETAILs = new List<PODETAIL>() { podetail } });

            IHttpActionResult poMasters = controller.DeletePOMASTER("PO02");
            var content = poMasters as OkNegotiatedContentResult<POMASTER>;
            Assert.IsNotNull(content);
        }
    }
}
