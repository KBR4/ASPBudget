using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class BudgetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public List<BudgetRecordDto> BudgetRecords { get; set; }
        public string Description { get; set; }
        public UserDto Creator { get; set; }
        public List<UserDto> Managers { get; set; } = new List<UserDto>();
    }
}
