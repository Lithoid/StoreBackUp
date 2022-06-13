using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Context;
using Entities;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private AppDataContext _context;

        public AssetController(AppDataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public List<Guid> Add(IFormCollection formCollection)
        {
            List<Guid> result = new List<Guid>();
            if (formCollection.Files.Count > 0)
            {
                foreach (IFormFile filee in formCollection.Files)
                {
                    Guid id = Guid.NewGuid();
                    string ext = Path.GetExtension(filee.FileName);
                    Asset asset = new Asset
                    {
                        Id = id,
                        FileName = id.ToString() + ext,
                        OriginalFileName = filee.FileName,
                        MimeType = filee.ContentType,
                        FileExtention = ext
                    };
                    result.Add(id);
                    using (Stream stream = new FileStream("./Assets/" + asset.FileName,
                        FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                    {
                        filee.CopyTo(stream);
                    }

                    _context.Set<Asset>().Add(asset);
                }
                if (_context.SaveChanges() > 0)
                    return result;
            }
            return result;
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid? id)
        {
            if (!id.HasValue)
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);

            Asset asset = _context.Set<Asset>().FirstOrDefault(x => x.Id == id.Value);

            if (asset == null)
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);

            Stream s = new FileStream("./Assets/" + asset.FileName, FileMode.Open, FileAccess.Read, FileShare.Write);
            return new FileStreamResult(s, asset.MimeType);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid? id)
        {
            if (!id.HasValue) return new StatusCodeResult((int)HttpStatusCode.BadRequest);

            Asset asset = _context.Set<Asset>().FirstOrDefault(x => x.Id == id.Value);

            if (asset == null) return new StatusCodeResult((int)HttpStatusCode.BadRequest);

            _context.Assets.Remove(asset);
            System.IO.File.Delete("./Assets/" + asset.FileName);
            _context.SaveChanges();

            return Redirect("/Home/index");
        }

        [HttpPut]
        public IActionResult Update(Guid? id, IFormCollection formCollection)
        {
            if (formCollection.Files.Count > 0)
            {
                foreach (IFormFile filee in formCollection.Files)
                {
                    Asset old = _context.Assets.FirstOrDefault(x => x.Id == id);
                    string ext = Path.GetExtension(filee.FileName);

                    System.IO.File.Delete("./Assets/" + old.Id);

                    old.FileName = old.Id.ToString() + ext;
                    old.FileExtention = ext;
                    old.MimeType = filee.ContentType;
                    old.OriginalFileName = old.FileName;

                    using (Stream stream = new FileStream("./Assets/" + old.FileName,
                        FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        filee.CopyToAsync(stream);
                    }

                    _context.Set<Asset>().Update(old);
                }

                _context.SaveChanges();
            }

            return Redirect("/Home/Index");
        }
    }
}