using Application.Dtos;
using Application.Exceptions;
using Application.Requests;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.BudgetRepository;
using Infrastructure.Repositories.UserRepository;

namespace Application.Services
{
    public class BudgetService : IBudgetService
    {
        private IBudgetRepository _budgetRepository;
        private IMapper _mapper;

        public BudgetService(IBudgetRepository budgetRepository, IMapper mapper)
        {
            _budgetRepository = budgetRepository;
            _mapper = mapper;
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
            return await _budgetRepository.Create(budget);
        }

        public async Task Delete(int id)
        {
            var res = await _budgetRepository.Delete(id);
            if (res == false)
            {
                throw new EntityDeleteException("Error when deleting Budget.");
            }
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
        }
    }
}
