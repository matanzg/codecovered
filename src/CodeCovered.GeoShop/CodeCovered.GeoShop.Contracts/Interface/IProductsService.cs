using System.ServiceModel;
using CodeCovered.GeoShop.Contracts.Dto;

namespace CodeCovered.GeoShop.Contracts.Interface
{
    [ServiceContract]
    public interface IProductsService
    {
        [OperationContract]
        SimplePoint GetProductLocation(int productId);

        [OperationContract]
        ProductDto GetProductDetails(int productId);

        [OperationContract]
        void UpdateProduct(ProductDto productDto);
    }
}