using Ecommerce.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Ecommerce.Infrastructure.Repositories.Service
{
	public class ImageManagementService : IImageManagementService
	{
		private readonly IFileProvider fileProvider;

		public ImageManagementService(IFileProvider fileProvider)
		{
			this.fileProvider = fileProvider;
		}
		public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
		{
			List<string> SaveImageSrc = new List<string>();

			var ImageDirectory = Path.Combine("wwwroot", "Images", src);

			if (!Directory.Exists(ImageDirectory))
			{
				Directory.CreateDirectory(ImageDirectory);
			}

			foreach (var image in files)
			{
				if (image.Length > 0)
				{
					var ImageName = image.FileName;
					var ImageSrc = $"/Images/{src}/{ImageName}";
					var root =Path.Combine(ImageDirectory, ImageName);

					using (FileStream stream = new FileStream(root, FileMode.Create))
					{
						await image.CopyToAsync(stream);
					}
					SaveImageSrc.Add(ImageSrc);
				}
			}
			return SaveImageSrc;
		}

		public void DeleteImageAsync(string src)
		{
			var info = fileProvider.GetFileInfo(src);

			var root = info.PhysicalPath;
			File.Delete(root);
		}
	}
}
