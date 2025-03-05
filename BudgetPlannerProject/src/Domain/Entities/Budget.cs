using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Budget
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public List<BudgetRecord> BudgetRecords { get; set; }
        public string Description { get; set; }
        public int CreatorId { get; set; }
        public User Creator { get; set; }
    }
}
