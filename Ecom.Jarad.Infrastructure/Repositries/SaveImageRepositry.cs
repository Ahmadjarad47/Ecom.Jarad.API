using Ecom.Jarad.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Ecom.Jarad.Infrastructure.Repositries
{
    public class SaveImageRepositry : SaveImage
    {
        private readonly IFileProvider fileProvider;

        public SaveImageRepositry(IFileProvider fileProvider)
        {
            this.fileProvider = fileProvider;
        }
        public async Task<string> AddImage(IFormFile formFile, string src)
        {
            if (formFile is not null)
            {
                string defultName = DateTime.Now.ToFileTime().ToString() + formFile.FileName;
                string ImageName = defultName.Replace(' ', '_');
                if (!Directory.Exists("wwwroot" + $"/images/{src}"))
                {
                    Directory.CreateDirectory("wwwroot" + $"/images/{src}");
                }
                src = $"/images/{src}/{ImageName}";

                IFileInfo info = fileProvider.GetFileInfo(src);

                string root = info.PhysicalPath;

                using (FileStream stream = new(root, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

            }
            return src;
        }



        void SaveImage.DeleteImage(string formFile)
        {
            IFileInfo pichinfo = fileProvider.GetFileInfo(formFile);

            string rootpath = pichinfo.PhysicalPath;

            System.IO.File.Delete(rootpath);
        }
    }
}
