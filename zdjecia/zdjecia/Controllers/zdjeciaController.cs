using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using zdjecia.Models;

namespace zdjecia.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")] // tune to your needs
    [RoutePrefix("")]
    public class zdjeciaController : ApiController
    {
            public IEnumerable<zdjeciatb> Get()
            {
                using (zdjeciaContext dbContext = new zdjeciaContext())
                {
                    return dbContext.zdjeciatb.ToList();
                }
            }
            public zdjeciatb Get(int id)
            {
                using (zdjeciaContext dbContext = new zdjeciaContext())
                {
                    return dbContext.zdjeciatb.FirstOrDefault(e => e.id == id);
                }
            }
            public void Post(zdjeciatb zdjecie)
            {
                using (zdjeciaContext dbContext = new zdjeciaContext())
                {
                    dbContext.zdjeciatb.Add(zdjecie);
                    dbContext.SaveChanges();
                }
            }
            public void Put(int id, zdjeciatb zdjecie)
            {
                using (zdjeciaContext dbContext = new zdjeciaContext())
                {
                    var entity = dbContext.zdjeciatb.FirstOrDefault(e => e.id == id);

                    entity.id = zdjecie.id;
                    entity.nazwa = zdjecie.nazwa;
                    entity.daata = zdjecie.daata;
                    entity.sciezka = zdjecie.sciezka;

                    dbContext.SaveChanges();
                }
            }
            public void Delete(int id)
            {
                using (zdjeciaContext dbContext = new zdjeciaContext())
                {
                dbContext.zdjeciatb.Remove(dbContext.zdjeciatb.FirstOrDefault(e => e.id == id));
                dbContext.SaveChanges();
                }
            }
    }
}
