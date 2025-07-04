﻿using Application.Dtos;
using Application.Requests;

namespace Application.Services
{
    public interface IBudgetService
    {
        public Task<BudgetDto?> GetById(int id);
        public Task<IEnumerable<BudgetDto>> GetAll();
        public Task<int> Add(CreateBudgetRequest request);
        public Task Update(UpdateBudgetRequest request);
        public Task Delete(int id);
        Task<IEnumerable<BudgetDto>> GetByCreatorId(int creatorId);
    }
}
