using AutoMapper;
using Ecom.Core.Dtos;
using Ecom.Core.Entites;
using Ecom.Core.Interfaces;
using Ecom.Core.Sharing;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace Ecom.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileProvider _fileProvider;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext context, IFileProvider fileProvider, IMapper mapper) : base(context)
        {
            _context = context;
            _fileProvider = fileProvider;
            _mapper = mapper;
        }

        public async Task<ReturnProductDto> GetAllAsync(ProductParams productParams)
        {
            var result_ = new ReturnProductDto();
            var query = await _context.Products
                .Include(x=>x.Category)
                .AsNoTracking().ToListAsync();

            //Search by CategoryId
            if (productParams.CategoryId.HasValue)
                query = query.Where(x => x.CategoryId == productParams.CategoryId.Value).ToList();

            //Search by Name
            if (!string.IsNullOrEmpty(productParams.Search))
                query = query.Where(x => x.Name.ToLower().Contains(productParams.Search)).ToList();

            //Sorting
            if(!string.IsNullOrEmpty(productParams.Sort))
            {
                query = productParams.Sort switch
                {
                    "PriceAsc" => query.OrderBy(x => x.Price).ToList(),
                    "PriceDesc" => query.OrderByDescending(x => x.Price).ToList(),
                    _ => query.OrderBy(x => x.Name).ToList(),
                };
            }
            result_.TotalItems = query.Count;

            //Paging
            query = query.Skip((productParams.PageSize) * (productParams.PageNumber - 1)).Take(productParams.PageSize).ToList();

            result_.ProductsDto = _mapper.Map<List<ProductDto>>(query);
            return result_;
        }

        public async Task<bool> AddAsync(CreateProductDto dto)
        {
            var source = "";
            if (dto.Image is not null)
            {
                var directoryPath = "/images/products/";
                //unique file
                var productName = $"{Guid.NewGuid()}"+dto.Image.FileName;
                if(!Directory.Exists("wwwroot" + directoryPath))
                {
                    Directory.CreateDirectory("wwwroot"+directoryPath);
                }
                source = directoryPath + productName;
                var picInfo = _fileProvider.GetFileInfo(source);
                var rootPath = picInfo.PhysicalPath;
                using(var filestream = new FileStream(rootPath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(filestream);
                }
            }
            //Create new product
            var res = _mapper.Map<Product>(dto);
            res.ProductPicture = source;
            await _context.Products.AddAsync(res);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(int id, UpdateProductDto dto)
        {
            var currentProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(x=>x.Id == id);
            if (currentProduct is not null) 
            {
                var source = "";
                if (dto.Image is not null)
                {
                    var directoryPath = "/images/products/";
                    //unique file
                    var productName = $"{Guid.NewGuid()}" + dto.Image.FileName;
                    if (!Directory.Exists("wwwroot" + directoryPath))
                    {
                        Directory.CreateDirectory("wwwroot" + directoryPath);
                    }
                    source = directoryPath + productName;
                    var picInfo = _fileProvider.GetFileInfo(source);
                    var rootPath = picInfo.PhysicalPath;
                    using (var filestream = new FileStream(rootPath, FileMode.Create))
                    {
                        await dto.Image.CopyToAsync(filestream);
                    }

                    //remove old picture
                    if (!string.IsNullOrEmpty(currentProduct.ProductPicture))
                    {
                        //delete old picture
                        picInfo = _fileProvider.GetFileInfo(currentProduct.ProductPicture);
                        rootPath = picInfo.PhysicalPath;
                        File.Delete(rootPath);
                    }
                }

                if (dto.CategoryId is null)
                    dto.CategoryId = currentProduct.CategoryId;

                if (dto.Price is null)
                    dto.Price = currentProduct.Price;

                if (string.IsNullOrEmpty(dto.Description))
                    dto.Description = currentProduct.Description;

                if (string.IsNullOrEmpty(dto.Name))
                    dto.Name = currentProduct.Name;

                //update product
                var res = _mapper.Map<Product>(dto);
                res.Id = id;
                res.ProductPicture = source;
                //_context.Products.Update(res);
                _context.Products.Remove(currentProduct);
                await _context.Products.AddAsync(res);
                await _context.SaveChangesAsync();
                                
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsyncWithPicture(int id)
        {
            var currentProduct = await _context.Products.FindAsync(id);
            if (currentProduct is not null)
            {
                //remove old picture
                if (!string.IsNullOrEmpty(currentProduct.ProductPicture))
                {
                    //delete old picture
                    var picInfo = _fileProvider.GetFileInfo(currentProduct.ProductPicture);
                    var rootPath = picInfo.PhysicalPath;
                    File.Delete(rootPath);
                }

                //Remove
                _context.Products.Remove(currentProduct);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }
    }
}
