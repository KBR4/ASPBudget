using Application.Dtos;
using Application.Exceptions;
using Application.Requests;
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

        public async Task<int> Add(CreateBudgetResultRequest request)
        {
            var budgetResult = new BudgetResult()
            {
                BudgetId = request.BudgetId,
                TotalProfit = request.TotalProfit
            };
            var budget = await _budgetRepository.ReadById(budgetResult.BudgetId);
            if (budget == null)
            {
                return -1;
            }
            return await _budgetResultRepository.Create(budgetResult);
        }

        public async Task<bool> Delete(int id)
        {
            return await _budgetResultRepository.Delete(id);
        }

        public async Task<IEnumerable<BudgetResultDto>> GetAll()
        {
            var budgetResults = await _budgetResultRepository.ReadAll();
            if (budgetResults is null || budgetResults.Count() == 0)
            {
                throw new NotFoundApplicationException("Users not found");
            }
            var mappedBudgetResults = budgetResults.Select(q => _mapper.Map<BudgetResultDto>(q)).ToList();
            return mappedBudgetResults;
        }

        public async Task<BudgetResultDto?> GetById(int id)
        {
            var budgetResult = await _budgetResultRepository.ReadById(id);
            var mappedBudgetResult = _mapper.Map<BudgetResultDto>(budgetResult);
            return mappedBudgetResult;
        }

        public async Task<bool> Update(UpdateBudgetResultRequest request)
        {
            var budgetResult = new BudgetResult()
            {
                Id = request.Id,
                BudgetId = request.BudgetId,
                TotalProfit = request.TotalProfit
            };
            var budget = await _budgetRepository.ReadById(budgetResult.BudgetId);
            if (budget == null)
            {
                return false;
            }
            return await _budgetResultRepository.Update(budgetResult);
        }
    }
}
