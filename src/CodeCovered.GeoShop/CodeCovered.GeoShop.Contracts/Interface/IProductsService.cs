using System.Collections.Generic;
using System.ServiceModel;
using CodeCovered.GeoShop.Contracts.Dto;

namespace CodeCovered.GeoShop.Contracts.Interface
{
    [ServiceContract]
    public interface IProductsService
    {
        [OperationContract]
        IEnumerable<BranchDto> QueryBranchesByCenterPoint(SimplePoint center, double buffer);

        [OperationContract]
        ProductDto GetProductDetails(int productId);

        [OperationContract]
        ProductDto SaveOrUpdateProduct(ProductDto productDto);

        [OperationContract]
        IEnumerable<CategoryDto> GetAllCategories();

        [OperationContract]
        void CreateCategory(string description);
    }
}