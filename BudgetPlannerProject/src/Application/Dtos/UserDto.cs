﻿namespace Application.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public List<BudgetDto> BudgetPlans { get; set; }
    }
}
