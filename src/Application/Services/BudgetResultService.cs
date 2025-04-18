using Application.Dtos;
using Application.Exceptions;
using Application.Requests;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.BudgetRepository;
using Infrastructure.Repositories.BudgetResultRepository;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class BudgetResultService : IBudgetResultService
    {
        private IBudgetResultRepository _budgetResultRepository;
        private IMapper _mapper;
        private readonly ILogger<BudgetResultService> _logger;
        public BudgetResultService(IBudgetResultRepository budgetResultRepository, IMapper mapper, ILogger<BudgetResultService> logger)
        {
            _budgetResultRepository = budgetResultRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Add(CreateBudgetResultRequest request)
        {
            var budgetResult = new BudgetResult()
            {
                BudgetId = request.BudgetId,
                TotalProfit = request.TotalProfit
            };
            var res = await _budgetResultRepository.Create(budgetResult);
            _logger.LogInformation(@"BudgetResult with ID = {0} was created.", res);
            return res;
        }

        public async Task Delete(int id)
        {
            var res = await _budgetResultRepository.Delete(id);
            if (res == false)
            {
                throw new EntityDeleteException("Error when deleting BudgetResult.");
            }
            _logger.LogInformation(@"BudgetResult with ID = {0} was deleted.", id);
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
            _logger.LogInformation(@"BudgetResult with ID = {0} was updated.", budgetResult.Id);
        }
    }
}
