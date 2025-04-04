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
            if (budgetResult == null)
            {
                throw new NotFoundApplicationException("BudgetResult wasn't created.");
            }
            return await _budgetResultRepository.Create(budgetResult);
        }

        public async Task Delete(int id)
        {
            var res = await _budgetResultRepository.Delete(id);
            if (res == false)
            {
                throw new EntityDeleteException("Error when deleting BudgetResult.");
            }
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
            if (budgetResult == null)
            {
                throw new NotFoundApplicationException("BudgetResult not found");
            }
            var mappedBudgetResult = _mapper.Map<BudgetResultDto>(budgetResult);
            return mappedBudgetResult;
        }

        public async Task Update(UpdateBudgetResultRequest request)
        {
            var budgetResult = new BudgetResult()
            {
                Id = request.Id,
                BudgetId = request.BudgetId,
                TotalProfit = request.TotalProfit
            };
            var res = await _budgetResultRepository.Update(budgetResult);
            if (res == false)
            {
                throw new EntityUpdateException("BudgetResult wasn't updated.");
            }
        }
    }
}
