using Application.Dtos;
using Application.Exceptions;
using Application.Requests;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.BudgetRepository;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class BudgetService : IBudgetService
    {
        private IBudgetRepository _budgetRepository;
        private IMapper _mapper;
        private readonly ILogger<BudgetService> _logger;

        public BudgetService(IBudgetRepository budgetRepository, IMapper mapper, ILogger<BudgetService> logger)
        {
            _budgetRepository = budgetRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Add(CreateBudgetRequest request)
        {
            var budget = new Budget()
            {
                Name = request.Name,
                StartDate = request.StartDate,
                FinishDate = request.FinishDate,
                Description = request.Description,
                CreatorId = request.CreatorId
            };
            var res = await _budgetRepository.Create(budget);
            _logger.LogInformation(@"Budget with ID = {0} was created.", res);
            return res;
        }

        public async Task Delete(int id)
        {
            var res = await _budgetRepository.Delete(id);
            if (res == false)
            {
                throw new EntityDeleteException("Error when deleting Budget.");
            }
            _logger.LogInformation(@"Budget with ID = {0} was deleted.", id);
        }

        public async Task<IEnumerable<BudgetDto>> GetAll()
        {
            var budgets = await _budgetRepository.ReadAll();
            var mappedBudgets = budgets.Select(q => _mapper.Map<BudgetDto>(q)).ToList();
            return mappedBudgets;
        }

        public async Task<BudgetDto?> GetById(int id)
        {
            var budget = await _budgetRepository.ReadById(id);
            if (budget == null)
            {
                throw new NotFoundApplicationException("Budget not found.");
            }
            var mappedBudget = _mapper.Map<BudgetDto>(budget);
            return mappedBudget;
        }

        public async Task Update(UpdateBudgetRequest request)
        {
            var budget = new Budget()
            {
                Id = request.Id,
                Name = request.Name,
                StartDate = request.StartDate,
                FinishDate = request.FinishDate,
                Description = request.Description,
                CreatorId = request.CreatorId
            };
            var res = await _budgetRepository.Update(budget);
            if (res == false)
            {
                throw new EntityUpdateException("Budget wasn't updated.");
            }
            _logger.LogInformation(@"User with ID = {0} was updated.", budget.Id);
        }
    }
}
