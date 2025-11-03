

using WebAPIDemo.Data.UnitOfWork;

namespace WebAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/<ProductController>
        [HttpGet("GetAll")]
        public async Task<ActionResult<List<Product>>> Get()
        {
            var producten = await _unitOfWork.ProductRepository.GetAllAsync();
            if (producten.ToList().Count > 0) {
                return Ok(producten);
            }
            else
            {
                return NotFound("Er zijn geen resultaten gevonden");
            }
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            Product? product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound($"Product {id} kan niet worden gevonden in de database");
            }
            return Ok(product);            
        }

        [HttpGet("Search")]
        public async Task<ActionResult<List<Product>>> Search(string zoekwaarde)
        {
            var producten = await _unitOfWork.ProductRepository.GetAllAsync();
            var product = producten.Where(x=>x.Naam.Contains(zoekwaarde)).OrderBy(x=>x.Naam);
            if (product == null)
            {
                return NotFound($"Er zijn geen producten in de database waar {zoekwaarde} voorkomt in de naam");
            }
            return Ok(product);
        }
        
        [HttpPost]
        public async Task<ActionResult<Product>> ProductToevoegen(AddProductDTO productDTO)
        {
            var producten = await _unitOfWork.ProductRepository.GetAllAsync();

            if (producten == null)
                return NotFound("De tabel Producten bestaat niet in de database.");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Mapping van DTO naar model
            Product product = _mapper.Map<Product>(productDTO);

            await _unitOfWork.ProductRepository.AddAsync(product);
            try
            {
                _unitOfWork.SaveChanges();
            }
            catch (DbUpdateException dbError) 
            {
                return BadRequest(dbError);
            }
            return CreatedAtAction("GetProduct", new {id = product.Id}, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ProductWijzigen(int id, UpdateProductDTO productDTO)
        {
            if (id != productDTO.Id)
            {
                return BadRequest("De opgegeven id's komen niet overeen.");
            }

            //Nieuwe gegevens valideren
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Product opzoeken in Database
            Product? product = await _unitOfWork.ProductRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound($"Er is geen product gevonden met id {id}");
            }
            // Nieuwe gegevens updaten in product
            _mapper.Map(productDTO, product);

            //Gegevens updaten in database
            _unitOfWork.ProductRepository.Update(product);

            try
            {
                _unitOfWork.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                var producten = await _unitOfWork.ProductRepository.GetAllAsync();
                if (producten.Any(x => x.Id == id))
                {
                    return NotFound("Er is geen product met dit id gevonden");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ProductVerwijdern(int id)
        {
            var producten = await _unitOfWork.ProductRepository.GetAllAsync();
            if (producten == null)
            {
                return NotFound("De tabel producten bestaat niet.");
            }
            Product? product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound("Het product met deze id is niet gevonden.");
            }

            _unitOfWork.ProductRepository.Delete(product);
            _unitOfWork.SaveChanges();

            return Ok($"Product met id {id} is verwijderd");
        }
    }
}
