using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DataAccess;

namespace ApiSample.Controllers
{
    [RoutePrefix("PurchaseOrder")]
    public class PurchaseOrderController : ApiController
    {
        private PODbEntities db = new PODbEntities();

        // GET: api/PurchaseOrder
        [Route("")]
        public IQueryable<POMASTER> GetPOMASTERs()
        {
            return db.POMASTERs;
        }

        // GET: api/PurchaseOrder/5
        [ResponseType(typeof(POMASTER))]
        [Route("Get/{id}")]
        public IHttpActionResult GetPOMASTER(string id)
        {
            POMASTER pOMASTER = db.POMASTERs.Find(id);
            if (pOMASTER == null)
            {
                return NotFound();
            }

            return Ok(pOMASTER);
        }
        
        // POST: api/PurchaseOrder
        [ResponseType(typeof(POMASTER))]
        [Route("Add")]
        public IHttpActionResult PostPOMASTER(POMASTER pOMASTER)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.POMASTERs.Add(pOMASTER);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (POMASTERExists(pOMASTER.PONO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pOMASTER.PONO }, pOMASTER);
        }

        [ResponseType(typeof(void))]
        [Route("Update/{id}")]
        public IHttpActionResult PutPOMASTER(string id, POMASTER pOMASTER)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pOMASTER.PONO)
            {
                return BadRequest();
            }

            db.Entry(pOMASTER).State = EntityState.Modified;
            db.Entry(pOMASTER.PODETAILs.FirstOrDefault()).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!POMASTERExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }


        // DELETE: api/PurchaseOrder/5
        [ResponseType(typeof(POMASTER))]
        [Route("Delete/{id}")]
        public IHttpActionResult DeletePOMASTER(string id)
        {
            POMASTER pOMASTER = db.POMASTERs.Find(id);
            if (pOMASTER == null)
            {
                return NotFound();
            }
            var lst = pOMASTER.PODETAILs.ToList();
            foreach (var podDetail in lst)
            {
                db.PODETAILs.Remove(podDetail);
            }
            
            db.POMASTERs.Remove(pOMASTER);
            db.SaveChanges();

            return Ok(pOMASTER);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool POMASTERExists(string id)
        {
            return db.POMASTERs.Count(e => e.PONO == id) > 0;
        }
    }
}