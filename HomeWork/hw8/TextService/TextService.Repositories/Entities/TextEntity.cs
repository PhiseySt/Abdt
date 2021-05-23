using RepositoryBase.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace TextService.Repositories.Entities
{
    [Table("Text")]
    public class TextEntity : BaseEntity
    {
        public string TextValue { get; set; }
    }
}
