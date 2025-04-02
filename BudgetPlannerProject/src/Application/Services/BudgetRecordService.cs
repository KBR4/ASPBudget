using Application.Dtos;
using Application.Exceptions;
using Application.Requests;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.BudgetRecordRepository;
using Infrastructure.Repositories.BudgetRepository;
using Infrastructure.Repositories.BudgetResultRepository;

namespace Application.Services
{
    public class BudgetRecordService : IBudgetRecordService
    {
        private IBudgetRecordRepository _budgetRecordRepository;
        private IBudgetRepository _budgetRepository;
        private IMapper _mapper;

        public BudgetRecordService(IBudgetRecordRepository budgetRecordRepository, IMapper mapper, IBudgetRepository budgetRepository)
        {
            _budgetRecordRepository = budgetRecordRepository;
            _mapper = mapper;
            _budgetRepository = budgetRepository;
        }

        public async Task<int> Add(CreateBudgetRecordRequest request)
        {
            var budgetRecord = new BudgetRecord()
            {
                Name = request.Name,
                CreationDate = request.CreationDate,
                SpendingDate = request.SpendingDate,
                BudgetId = request.BudgetId,
                Total = request.Total,
                Comment = request.Comment                          
            };
            var budget = await _budgetRepository.ReadById(budgetRecord.BudgetId);
            if (budget == null)
            {
                return -1;
            }
            return await _budgetRecordRepository.Create(budgetRecord);
        }

        public async Task<bool> Delete(int id)
        {
            return await _budgetRecordRepository.Delete(id);
        }

        public async Task<IEnumerable<BudgetRecordDto>> GetAll()
        {
            var budgetRecords = await _budgetRecordRepository.ReadAll();
            if (budgetRecords is null || budgetRecords.Count() == 0)
            {
                throw new NotFoundApplicationException("Users not found");
            }
            var mappedBudgetRecords = budgetRecords.Select(q => _mapper.Map<BudgetRecordDto>(q)).ToList();
            return mappedBudgetRecords;
        }

        public async Task<BudgetRecordDto?> GetById(int id)
        {
            var budgetRecord = await _budgetRecordRepository.ReadById(id);
            var mappedBudgetRecord = _mapper.Map<BudgetRecordDto>(budgetRecord);
            return mappedBudgetRecord;
        }

        public async Task<bool> Update(UpdateBudgetRecordRequest request)
        {
            var budgetRecord = new BudgetRecord()
            {
                Id = request.Id,
                Name = request.Name,
                CreationDate = request.CreationDate,
                SpendingDate = request.SpendingDate,
                BudgetId = request.BudgetId,
                Total = request.Total,
                Comment = request.Comment
            };
            var budget = await _budgetRepository.ReadById(budgetRecord.BudgetId);
            if (budget == null)
            {
                return false;
            }
            return await _budgetRecordRepository.Update(budgetRecord);
        }
    }
}
