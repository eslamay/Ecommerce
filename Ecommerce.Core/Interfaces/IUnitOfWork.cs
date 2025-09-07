namespace Ecommerce.Core.Interfaces
{
	public interface IUnitOfWork
	{
		ICategoryRepository CategoryRepository { get; }
		IProductRepository ProductRepository { get; }
		IPhotoRepository PhotoRepository { get; }
	}
}
