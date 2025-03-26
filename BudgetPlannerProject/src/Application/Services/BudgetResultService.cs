using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.BudgetRepository;
using Infrastructure.Repositories.BudgetResultRepository;

namespace Application.Services
{
    public class BudgetResultService : IBudgetResultService
    {
        private IBudgetResultRepository _budgetResultRepository;
        private IBudgetRepository _budgetRepository;
        private IMapper _mapper;

        public BudgetResultService(IBudgetResultRepository budgetResultRepository, IMapper mapper, IBudgetRepository budgetRepository)
        {
            _budgetResultRepository = budgetResultRepository;
            _budgetRepository = budgetRepository;
            _mapper = mapper;
        }

        public async Task<int> Add(BudgetResultDto budgetResult)
        {
            var mappedBudgetResult = _mapper.Map<BudgetResult>(budgetResult);        
            if (mappedBudgetResult == null)
            {
                return -1;
            }
            var budget = await _budgetRepository.ReadById(budgetResult.BudgetId);
            if (budget == null)
            {
                return -1;
            }
            return await _budgetResultRepository.Create(mappedBudgetResult);
        }

        public async Task<bool> Delete(int id)
        {
            return await _budgetResultRepository.Delete(id);
        }

        public async Task<IEnumerable<BudgetResultDto>> GetAll()
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
            var budget = await _budgetRepository.ReadById(budgetResult.BudgetId);
            if (budget == null)
            {
                return false;
            }
            return await _budgetResultRepository.Update(mappedBudgetResult);
        }
    }
}
