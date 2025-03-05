using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class BudgetResultDto
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }
        public BudgetDto Budget { get; set; }
        public double TotalProfit { get; set; }
        public double Surplus { get; set; }
        public bool GoingPositive { get; set; }
    }
}
