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
        private IUserRepository _userRepository;
        private IMapper _mapper;

        public BudgetService(IBudgetRepository budgetRepository, IMapper mapper, IUserRepository userRepository)
        {
            _budgetRepository = budgetRepository;
            _mapper = mapper;
            _userRepository = userRepository;
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
            var user = await _userRepository.ReadById(budget.CreatorId);
            if (user == null)
            {
                return -1;
            }
            return await _budgetRepository.Create(budget);
        }

        public async Task<bool> Delete(int id)
        {
            return await _budgetRepository.Delete(id);
        }

        public async Task<IEnumerable<BudgetDto>> GetAll()
        {
            var budgets = await _budgetRepository.ReadAll();
            if (budgets is null || budgets.Count() == 0)
            {
                throw new NotFoundApplicationException("Users not found");
            }
            var mappedBudgets = budgets.Select(q => _mapper.Map<BudgetDto>(q)).ToList();
            return mappedBudgets;
        }

        public async Task<BudgetDto?> GetById(int id)
        {
            var budget = await _budgetRepository.ReadById(id);
            var mappedBudget = _mapper.Map<BudgetDto>(budget);
            return mappedBudget;
        }

        public async Task<bool> Update(UpdateBudgetRequest request)
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
            var user = await _userRepository.ReadById(budget.CreatorId);
            if (user == null)
            {
                return false;
            }
            return await _budgetRepository.Update(budget);
        }
    }
}
