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
    public class BudgetResultService : IBudgetResultService
    {
        private IBudgetResultRepository _budgetResultRepository;
        private IMapper _mapper;

        public BudgetResultService(IBudgetResultRepository budgetResultRepository, IMapper mapper)
        {
            _budgetResultRepository = budgetResultRepository;
            _mapper = mapper;
        }

        public async Task<int> Add(BudgetResultDto budgetResult)
        {
            var mappedBudgetResult = _mapper.Map<BudgetResult>(budgetResult);
            if (mappedBudgetResult != null)
            {
                await _budgetResultRepository.Create(mappedBudgetResult);
            }
            return -1;
        }

        public async Task<bool> Delete(int id)
        {
            return await _budgetResultRepository.Delete(id);
        }

        public async Task<List<BudgetResultDto>> GetAll()
        {
            var budgetResults = await _budgetResultRepository.ReadAll();
            var mappedBudgetResults = budgetResults.Select(q => _mapper.Map<BudgetResultDto>(q)).ToList();
            return mappedBudgetResults;
        }

        public async Task<BudgetResultDto?> GetById(int id)
        {
            var budgetResult = await _budgetResultRepository.ReadById(id);
            var mappedBudgetResult = _mapper.Map<BudgetResultDto>(budgetResult);
            return mappedBudgetResult;
        }

        public async Task<bool> Update(BudgetResultDto budgetResult)
        {
            if (budgetResult == null)
            {
                return false;
            }
            var mappedBudgetResult = _mapper.Map<BudgetResult>(budgetResult);
            return await _budgetResultRepository.Update(mappedBudgetResult);
        }
    }
}
