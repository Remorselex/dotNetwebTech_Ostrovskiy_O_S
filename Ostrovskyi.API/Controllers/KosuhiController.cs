using Cloth.Domain.Entities;
using Cloth.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ostrovskyi.API.Data;

namespace Ostrovskyi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KosuhiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public KosuhiController(AppDbContext context, IWebHostEnvironment _environment)
        {
            _context = context;

        }


        // GET: api/Dishes
        [HttpGet]
        public async Task<ActionResult<ResponseData<ListModel<Kosuhi>>>> GetProductListAsync(
              string? category,
              int pageNo = 1,
              int pageSize = 3)
        {
            // Создать объект результата
            var result = new ResponseData<ListModel<Kosuhi>>();

            // Фильтрация по категории загрузка данных категории
            var data = _context.Kosuhi
            .Include(d => d.Category)
            .Where(d => String.IsNullOrEmpty(category)
            || d.Category.NormalizedName.Equals(category));

            // Подсчет общего количества страниц
            int totalPages = (int)Math.Ceiling(data.Count() / (double)pageSize);
            if (pageNo > totalPages)
                pageNo = totalPages;

            // Создание объекта ProductListModel с нужной страницей данных
            var listData = new ListModel<Kosuhi>()
            {
                Items = await data
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(),
                CurrentPage = pageNo,
                TotalPages = totalPages
            };
            // поместить данные в объект результата
            result.Data = listData;
            // Если список пустой
            if (data.Count() == 0)
            {
                result.Success = false;
                result.ErrorMessage = "Нет объектов в выбранной категории";
            }
            return result;
        }
        // GET: api/Kosuhi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Kosuhi>> GetKosuhi(int id)
        {
            var kosuhi = await _context.Kosuhi.FindAsync(id);

            if (kosuhi == null)
            {
                return NotFound();
            }

            return kosuhi;
        }

        // PUT: api/Kosuhi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKosuhi(int id, Kosuhi kosuhi)
        {
            if (id != kosuhi.Id)
            {
                return BadRequest();
            }

            _context.Entry(kosuhi).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KosuhiExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Kosuhi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Kosuhi>> PostKosuhi(Kosuhi kosuhi)
        {
            _context.Kosuhi.Add(kosuhi);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKosuhi", new { id = kosuhi.Id }, kosuhi);
        }

        // DELETE: api/Kosuhi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKosuhi(int id)
        {
            var kosuhi = await _context.Kosuhi.FindAsync(id);
            if (kosuhi == null)
            {
                return NotFound();
            }

            _context.Kosuhi.Remove(kosuhi);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KosuhiExists(int id)
        {
            return _context.Kosuhi.Any(e => e.Id == id);
        }

        [HttpPost("{id}")]

        public async Task<IActionResult> SaveImage(int id, IFormFile image, [FromServices] IWebHostEnvironment env)
        {
            // Найти объект по Id
            var kosuhi = await _context.Kosuhi.FindAsync(id);
            if (kosuhi == null)
            {
                return NotFound();
            }

            // Путь к папке wwwroot/Images
            var imagesPath = Path.Combine(env.WebRootPath, "Images");

            // получить случайное имя файла
            var randomName = Path.GetRandomFileName();

            // получить расширение в исходном файле
            var extension = Path.GetExtension(image.FileName);

            // задать в новом имени расширение как в исходном файле
            var fileName = Path.ChangeExtension(randomName, extension);

            // полный путь к файлу
            var filePath = Path.Combine(imagesPath, fileName);

            // создать файл и открыть поток для записи
            using var stream = System.IO.File.OpenWrite(filePath);

            // скопировать файл в поток
            await image.CopyToAsync(stream);

            // получить Url хоста
            var host = "https://" + Request.Host;

            // Url файла изображения
            var url = $"{host}/Images/{fileName}";

            // Сохранить url файла в объекте
            kosuhi.Image = url;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}

