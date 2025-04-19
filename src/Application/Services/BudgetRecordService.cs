using Application.Dtos;
using Application.Exceptions;
using Application.Requests;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.BudgetRecordRepository;
using Infrastructure.Repositories.BudgetRepository;
using Infrastructure.Repositories.BudgetResultRepository;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class BudgetRecordService : IBudgetRecordService
    {
        private IBudgetRecordRepository _budgetRecordRepository;
        private IMapper _mapper;
        private readonly ILogger<BudgetRecordService> _logger;

        public BudgetRecordService(IBudgetRecordRepository budgetRecordRepository, IMapper mapper, ILogger<BudgetRecordService> logger)
        {
            _budgetRecordRepository = budgetRecordRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Add(CreateBudgetRecordRequest request)
        {
            var budgetRecord = new BudgetRecord()
            {
                Name = request.Name,
                SpendingDate = request.SpendingDate,
                BudgetId = request.BudgetId,
                Total = request.Total,
                Comment = request.Comment                          
            };
            var res = await _budgetRecordRepository.Create(budgetRecord);
            _logger.LogInformation(@"BudgetRecord with ID = {0} was created.", res);
            return res;
        }

        public async Task Delete(int id)
        {
            var res = await _budgetRecordRepository.Delete(id);
            if (res == false)
            {
                throw new EntityDeleteException("BudgetRecord for deletion not found");
            }
            _logger.LogInformation(@"BudgetRecord with ID = {0} was deleted.", id);
        }

        public async Task<IEnumerable<BudgetRecordDto>> GetAll()
        {
            var budgetRecords = await _budgetRecordRepository.ReadAll();
            var mappedBudgetRecords = budgetRecords.Select(q => _mapper.Map<BudgetRecordDto>(q)).ToList();
            return mappedBudgetRecords;
        }

        public async Task<BudgetRecordDto?> GetById(int id)
        {
            var budgetRecord = await _budgetRecordRepository.ReadById(id);
            if (budgetRecord == null)
            {
                throw new NotFoundApplicationException("BudgetRecord not found.");
            }
            var mappedBudgetRecord = _mapper.Map<BudgetRecordDto>(budgetRecord);
            return mappedBudgetRecord;
        }

        public async Task Update(UpdateBudgetRecordRequest request)
        {
            var budgetRecord = new BudgetRecord()
            {
                Id = request.Id,
                Name = request.Name,
                SpendingDate = request.SpendingDate,
                BudgetId = request.BudgetId,
                Total = request.Total,
                Comment = request.Comment
            };
            var res = await _budgetRecordRepository.Update(budgetRecord);
            if (res == false)
            {
                throw new EntityUpdateException("BudgetRecord wasn't updated.");
            }
            _logger.LogInformation(@"BudgetRecord with ID = {0} was updated.", budgetRecord.Id);
        }
    }
}
