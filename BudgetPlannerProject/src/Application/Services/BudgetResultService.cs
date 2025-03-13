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
        private IMapper mapper;
        public BudgetResultService(IBudgetResultRepository budgetResultRepository, IMapper mapper)
        {
            this._budgetResultRepository = budgetResultRepository;
            this.mapper = mapper;
        }
        public async Task<int> Add(BudgetResultDto budgetResult)
        {
            var mappedbudgetResult = mapper.Map<BudgetResult>(budgetResult);
            if (mappedbudgetResult != null)
            {
                await _budgetResultRepository.Create(mappedbudgetResult);
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
            var mappedbudgetResults = budgetResults.Select(q => mapper.Map<BudgetResultDto>(q)).ToList();
            return mappedbudgetResults;
        }

        public async Task<BudgetResultDto?> GetById(int id)
        {
            var budgetResult = await _budgetResultRepository.ReadById(id);
            var mappedbudgetResult = mapper.Map<BudgetResultDto>(budgetResult);
            return mappedbudgetResult;
        }

        public async Task<bool> Update(BudgetResultDto budgetResult)
        {
            var mappedbudgetResult = mapper.Map<BudgetResult>(budgetResult);
            return await _budgetResultRepository.Update(mappedbudgetResult);
        }
    }
}
