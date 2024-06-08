using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloth.Domain.Entities
{
   public class Kosuhi
    {
        [Key]
        public int Id { get; set; } // id блюда
        public string Name { get; set; } // название блюда
        public string Description { get; set; } // описание блюда

        public string? Image { get; set; } // имя файла изображения 

        // Навигационные свойства
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

    }
}
