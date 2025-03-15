using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BudgetService : IBudgetService
    {
        private IBudgetRepository _budgetRepository;
        private IUserRepository _userRepository;
        private IMapper _mapper;

        public BudgetService(IBudgetRepository budgetRepository, IMapper mapper, IUserRepository userRepository)
        {
            _budgetRepository = budgetRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<int> Add(BudgetDto budget)
        {
            var mappedBudget = _mapper.Map<Budget>(budget);
            if (mappedBudget == null)
            {
                return -1;
            }
            var user = await _userRepository.ReadById(budget.CreatorId);
            if (user == null)
            {
                return -1;
            }
            return await _budgetRepository.Create(mappedBudget);
        }

        public async Task<bool> Delete(int id)
        {
            return await _budgetRepository.Delete(id);
        }

        public async Task<List<BudgetDto>> GetAll()
        {
            var budgets = await _budgetRepository.ReadAll();
            var mappedBudgets = budgets.Select(q => _mapper.Map<BudgetDto>(q)).ToList();
            return mappedBudgets;
        }

        public async Task<BudgetDto?> GetById(int id)
        {
            var budget = await _budgetRepository.ReadById(id);
            var mappedBudget = _mapper.Map<BudgetDto>(budget);
            return mappedBudget;
        }

        public async Task<bool> Update(BudgetDto budget)
        {
            if (budget == null)
            {
                return false;
            }
            var mappedBudget = _mapper.Map<Budget>(budget);
            var user = await _userRepository.ReadById(budget.CreatorId);
            if (user == null)
            {
                return false;
            }
            return await _budgetRepository.Update(mappedBudget);
        }
    }
}
