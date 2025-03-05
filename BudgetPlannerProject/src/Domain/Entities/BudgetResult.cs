using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BudgetResult
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }
        public Budget Budget { get; set; }
        public double TotalProfit { get; set; }
        public double Surplus { get; set; }
        public bool GoingPositive { get; set; }

    }
}
